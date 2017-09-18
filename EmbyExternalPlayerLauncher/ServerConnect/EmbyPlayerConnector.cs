/*
 *  Copyright © 2017 Andrei K.
 *  
 *  This file is part of Emby External Player Launcher.
 *
 *  Emby External Player Launcher is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Emby External Player Launcher is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Emby External Player Launcher.  If not, see <http://www.gnu.org/licenses/>.
 */

using Emby.ApiClient;
using Emby.ApiClient.Cryptography;
using Emby.ApiClient.Model;
using Emby.ApiClient.Net;
using Emby.ApiClient.WebSocket;
using EmbyExternalPlayerLauncher.ServerConnect.Logging;
using EmbyExternalPlayerLauncher.Players;
using log4net;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmbyExternalPlayerLauncher.ServerConnect
{
    public class EmbyPlayerConnector
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string EmbyAddress { get; set; }

        private string userName;
        private string password;

        private IApiClient client;
        private ConnectionManager conMgr;
        private ILogger logger = new EmbyLogger();

        private IPlayerAdapter player;
        private BaseItemDto playingItem;
        private ProgressReporter progressReporter;

        public EmbyPlayerConnector(IPlayerAdapter player, string user, string pass, string address = null)
        {
            EmbyAddress = address;
            userName = user;
            password = pass;
            this.player = player;
            InitConMgr();
        }

        public bool Connect()
        {
            log.Debug("Connecting to Emby server...");
            var conSuccess = TryConnectEmby().GetAwaiter().GetResult();
            if (!conSuccess)
                return false;
            WireEmbyEvents();
            log.InfoFormat("Connected to Emby server at {0}", EmbyAddress);
            return true;
        }

        public void Stop()
        {
            //TODO: should the progress reporter be stopped here or not?
            // it could be still active if the connector is stopped while playing
            log.Debug("Stopping Emby connector...");
            UnwireEmbyEvents();
            client?.Dispose();
            conMgr?.Dispose();
            progressReporter?.Stop();
            log.Info("Emby connector stopped.");
        }

        private void InitConMgr()
        {
            log.Debug("Initializing Emby connection information...");
            var device = new Device
            {
                DeviceName = userName + '@' + Environment.MachineName,
                DeviceId = "E2PL-" + userName + '@' + Environment.MachineName
            };

            var clientCapabilities = new ClientCapabilities
            {
                PlayableMediaTypes = new string[] { "Video" },
                SupportsContentUploading = false,
                SupportsPersistentIdentifier = false,
                SupportsSync = false,
                SupportsMediaControl = true,
                SupportedCommands = new string[] { "Play", "Playstate", "VolumeUp", "VolumeDown", "SetVolume", "ToggleMute" }
            };

            conMgr = new ConnectionManager(
                logger,
                new SimpleCredentialProvider(),
                new NetworkConnection(logger),
                new Emby.ApiClient.ServerLocator(logger),
                "Emby External Player Launcher",
                Utils.ApplicationVersion,
                device,
                clientCapabilities,
                new CryptographyProvider(),
                ClientWebSocketFactory.CreateWebSocket);

            //client = new ApiClient(
            //    logger,
            //    "http://10.23.45.2:8096",
            //    "Emby External Player Launcher",
            //    device,
            //    Utils.ApplicationVersion,
            //    new CryptographyProvider());
        }

        private async Task<bool> TryConnectEmby()
        {
            try
            {

                var conRes = await (string.IsNullOrEmpty(EmbyAddress) ? conMgr.Connect() : conMgr.Connect(EmbyAddress));

                if (conRes.State != ConnectionState.ServerSignIn)
                {
                    log.ErrorFormat("Connect failed because server replied with unexpected ConnectionState: {0}", conRes.State);
                    return false;
                }
                client = conRes.ApiClient;
                

                var authRes = await client.AuthenticateUserAsync(userName, password);
                //await client.ReportCapabilities(clientCapabilities);
                //client.OpenWebSocket(ClientWebSocketFactory.CreateWebSocket);
            }
            catch (Exception ex)
            {
                log.Error("Connecting to the Emby server failed.", ex);
                return false;
            }
            EmbyAddress = client.ServerAddress;
            return true;
        }

        private void StopPlayer()
        {
            player.Stop();
            ResetPlayingItem();
        }

        private void ResetPlayingItem()
        {
            playingItem = null;
            progressReporter.PlayerStopped -= ProgressReporter_PlayerStopped;
            progressReporter = null; //will terminate on its own along with the player
        }

        #region Events

        private void WireEmbyEvents()
        {
            client.PlayCommand += Client_PlayCommand;
            client.PlaystateCommand += Client_PlaystateCommand;
            client.GeneralCommand += Client_GeneralCommand;
            client.SetVolumeCommand += Client_SetVolumeCommand;
        }

        private void UnwireEmbyEvents()
        {
            if (client == null)
                return;

            client.PlayCommand -= Client_PlayCommand;
            client.PlaystateCommand -= Client_PlaystateCommand;
            client.GeneralCommand -= Client_GeneralCommand;
            client.SetVolumeCommand -= Client_SetVolumeCommand;
        }

        private void Client_GeneralCommand(object sender, MediaBrowser.Model.Events.GenericEventArgs<GeneralCommandEventArgs> e)
        {
            try
            {
                var eventArgs = e.Argument;
                log.InfoFormat("Received GeneralCommand: {0}", eventArgs.KnownCommandType);
                switch (eventArgs.KnownCommandType)
                {
                    case GeneralCommandType.VolumeUp:
                        if (!player.VolumeUp())
                            log.Error("Player VolumeUp command failed.");
                        break;
                    case GeneralCommandType.VolumeDown:
                        if (!player.VolumeDown())
                            log.Error("Player VolumeDown command failed.");
                        break;
                    case GeneralCommandType.ToggleMute:
                        if (!player.ToggleMute())
                            log.Error("Player ToggleMute command failed.");
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("General command failed.", ex);
            }
        }

        private void Client_SetVolumeCommand(object sender, MediaBrowser.Model.Events.GenericEventArgs<int> e)
        {
            try
            {
                log.InfoFormat("Received SetVolume command: {0}", e.Argument);
                if (!player.SetVolume(e.Argument))
                    log.Error("Setting player volume failed.");
            }
            catch (Exception ex)
            {
                log.Error("Setting player volume failed.", ex);
            }
        }

        private void Client_PlaystateCommand(object sender, MediaBrowser.Model.Events.GenericEventArgs<PlaystateRequest> e)
        {
            try
            {
                var stateReq = e.Argument;
                log.InfoFormat("Received Playstate command: {0}", stateReq.Command);
                switch (stateReq.Command)
                {
                    case PlaystateCommand.Pause:
                        if (!player.Pause())
                            log.Error("Player Pause command failed.");
                        break;
                    case PlaystateCommand.Unpause:
                        if (!player.Unpause())
                            log.Error("Player Unpause command failed.");
                        break;
                    case PlaystateCommand.Seek:
                        if (stateReq.SeekPositionTicks != null)
                            if (!player.Seek((long)stateReq.SeekPositionTicks))
                                log.Error("Player Seek command failed.");
                        break;
                    case PlaystateCommand.Stop:
                        if (!player.Stop())
                            log.Error("Player Stop command failed.");
                        break;
                    case PlaystateCommand.PlayPause:
                        if (!player.PlayPause())
                            log.Error("Player PlayPause command failed.");
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("Playstate command failed.", ex);
            }
        }

        private async void Client_PlayCommand(object sender, MediaBrowser.Model.Events.GenericEventArgs<PlayRequest> e)
        {
            try
            {
                log.Debug("Play command received.");
                if (playingItem != null)
                {
                    log.InfoFormat("{0} is already playing, stopping player.", playingItem.Name);
                    StopPlayer();
                }

                var playReq = e.Argument;
                var playingId = playReq.ItemIds[0]; //only playing 1 item is currently supported
                playingItem = await client.GetItemAsync(playingId, playReq.ControllingUserId);
              
                log.InfoFormat("Playing \"{0}\" from \"{1}\"", playingItem.Name, playingItem.Path);
                if (player.Play(playingItem.Path, playingItem.UserData.PlaybackPositionTicks))
                {
                    log.Debug("Playback started.");
                    client.ReportPlaybackStartAsync(new PlaybackStartInfo
                    {
                        CanSeek = true,
                        ItemId = playingId,
                        PlayMethod = PlayMethod.DirectPlay
                    }).GetAwaiter().GetResult();

                    progressReporter = new ProgressReporter(client, player, 1000, playingId);
                    progressReporter.PlayerStopped += ProgressReporter_PlayerStopped;
                    progressReporter.Start();
                    log.Debug("ProgressReporter started.");
                }
                else
                {
                    log.Error("Player failed to start.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Play command failed.", ex);
            }
        }

        private void ProgressReporter_PlayerStopped(object sender, EventArgs e)
        {
            ResetPlayingItem();
        }

        #endregion
    }
}

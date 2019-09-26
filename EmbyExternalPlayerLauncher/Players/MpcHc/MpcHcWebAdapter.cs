/*
 *  Copyright © 2017-2019 Andrei K.
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

using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmbyExternalPlayerLauncher.Players.MpcHc
{
    public class MpcHcWebAdapter : IPlayerAdapter
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string VariablesPath = "/variables.html";
        private const string CommandPath = "/command.html";
        private readonly static PlayerStatus PlayerStopped = new PlayerStatus
        {
            State = PlayerState.Stopped
        };

        private string mpcHcPath;
        private string mpcHcArgs;
        private int mpcHcWebPort;
        private int timeout;

        private Process mpcHcProc;
        private string baseUrl;
        private object playerStoppingLock = new object();

        //used to remember the last reported playback position
        //this is returned when the MPC-HC process terminates
        private long lastPosition;

        public MpcHcWebAdapter(string mpcHcPath, string mpcHcArgs, int mpcHcWebPort, int timeout = 2000)
        {
            this.mpcHcPath = mpcHcPath;
            this.mpcHcArgs = mpcHcArgs;
            this.mpcHcWebPort = mpcHcWebPort;
            this.timeout = timeout;
            baseUrl = "http://localhost:" + mpcHcWebPort;
        }

        #region IPlayerHandler Implementation

        public PlayerStatus GetPlayerStatus()
        {
            log.Debug("MPC-HC GetPlayerStatus");

            if (mpcHcProc == null)
            {
                log.Debug("MPC-HC already exited, returning Stopped status.");
                return new PlayerStatus
                {
                    State = PlayerState.Stopped,
                    Position = lastPosition
                };
            }
            //TODO: do something about the scenario where the user clicks Stop in MPC-HC, doesn't close the player and starts a new video from Emby?
            // it's not really the intended use case for this application however...
            if (!mpcHcProc.HasExited)
            {
                try
                {
                    using (var http = new HttpClient())
                    {
                        http.Timeout = TimeSpan.FromMilliseconds(timeout);
                        using (var respStream = http.GetStreamAsync(baseUrl + VariablesPath).GetAwaiter().GetResult())
                        {
                            var status = MpcHcWebParser.ParseStatusFromStream(respStream);
                            if (status.State != PlayerState.None)
                                lastPosition = status.Position;
                            return status;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Could not get player status.", ex);
                }
            }
            else
            {
                log.Info("MPC-HC process has exited.");
                ProcessCleanup();
            }

            log.Info("Returning stopped status (position: " + lastPosition + ") by default.");
            return new PlayerStatus
            {
                State = PlayerState.Stopped,
                Position = lastPosition
            };
        }

        public bool Pause()
        {
            log.Debug("MPC-HC Pause");
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.Pause }
            });
        }

        public bool Unpause()
        {
            log.Debug("MPC-HC Unpause");
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.Play }
            });
        }

        public bool PlayPause()
        {
            log.Debug("MPC-HC PlayPause");
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.PlayPause }
            });
        }

        public bool Play(string filePath, long embyStartTicks = 0)
        {
            log.DebugFormat("MPC-HC Play: {0}", filePath);
            try
            {
                var args = GetPlayerArgs(filePath, embyStartTicks);
                mpcHcProc = Process.Start(mpcHcPath, args);
                lastPosition = 0;
            }
            catch (Exception ex)
            {
                Task.Run(() => MessageBox.Show("Launching MPC-HC failed:\n\n" + ex.Message));
                log.Error("MPC-HC playback failed.", ex);
                return false;
            }
            return true;
        }

        public bool Seek(long embyTicks)
        {
            log.DebugFormat("MPC-HC Seek: {0}", embyTicks);
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.Seek },
                { MpcHcCmdArgs.SeekPosition, TimeSpan.FromMilliseconds(embyTicks / Utils.EmbyTicksPerMs).ToString(@"hh\:mm\:ss") }
            });
        }

        public bool Stop()
        {
            log.Debug("MPC-HC Stop");

            if (mpcHcProc == null)
            {
                log.Info("MPC-HC process not running, nothing to stop.");
                return true;
            }
            
            try
            {
                if (!mpcHcProc.CloseMainWindow())
                {
                    log.Error("CloseMainWindow failed on MPC-HC process.");
                    return false;
                }
                if (!mpcHcProc.WaitForExit(5000))
                {
                    log.ErrorFormat("MPC-HC failed to exit within {0}ms", 5000);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Task.Run(() => MessageBox.Show("There was an error accessing the player process:\n\n" + ex.Message));
                log.Error("Closing MPC-HC failed.", ex);
                return false;
            }
            ProcessCleanup();
            return true;
        }

        public bool VolumeUp()
        {
            log.Debug("MPC-HC VolumeUp");
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.VolumeUp }
            });
        }

        public bool VolumeDown()
        {
            log.Debug("MPC-HC VolumeDown");
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.VolumeDown }
            });
        }

        public bool SetVolume(int volume)
        {
            log.DebugFormat("MPC-HC SetVolume: {0}", volume);
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.SetVolume },
                { MpcHcCmdArgs.VolumeLevel, volume.ToString() }
            });
        }

        public bool ToggleMute()
        {
            log.Debug("MPC-HC ToggleMute");
            return MpcHcPostCommand(new Dictionary<string, string>
            {
                { MpcHcCommands.PostCommandName, MpcHcCommands.Mute }
            });
        }

        public bool CanSeek()
        {
            return true;
        }

#endregion

        private void ProcessCleanup()
        {
            log.Debug("Cleaning up MPC-HC process resources.");
            //lock is required due to potential concurrent access from Stop call and GetPlayerStatus call
            lock (playerStoppingLock)
            {
                mpcHcProc?.Close();
                mpcHcProc = null;
            }
        }

        private string GetPlayerArgs(string filePath, long embyStartTicks)
        {
            long startPosMs = embyStartTicks / Utils.EmbyTicksPerMs;
            return mpcHcArgs.Replace("{s}", startPosMs.ToString())
                + " /webport " + mpcHcWebPort.ToString()
                + " \"" + filePath + "\"";
        }

        private bool MpcHcPostCommand(IDictionary<string, string> data)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromMilliseconds(timeout);
                    var content = new FormUrlEncodedContent(data);
                    http.PostAsync(baseUrl + CommandPath, content).Wait();
                }
            }
            catch (Exception ex)
            {
                log.Error("Could not POST command to MPC-HC's web server.", ex);
                return false;
            }
            return true;
        }
    }
}

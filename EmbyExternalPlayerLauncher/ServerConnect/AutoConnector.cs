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

using EmbyExternalPlayerLauncher.Config;
using EmbyExternalPlayerLauncher.Players;
using log4net;
using System.Threading;
using System.Threading.Tasks;

namespace EmbyExternalPlayerLauncher.ServerConnect
{
    public class AutoConnector
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IPlayerAdapter player;
        private LauncherConfig config;

        private EmbyPlayerConnector connector;

        private Timer reconnectTimer;

        public AutoConnector(IPlayerAdapter player, LauncherConfig config)
        {
            this.player = player;
            this.config = config;
            reconnectTimer = new Timer(CheckConnection);
        }

        public string EmbyAddress { get { return connector.EmbyAddress; } }

        public bool Start()
        {
            connector = new EmbyPlayerConnector(
                player,
                config.EmbyUser,
                config.EmbyPass,
                config.EmbyAddress);

            //TODO: apparently causes a deadlock when running on the main/UI thread, investigate why
            log.Info("Connecting to Emby Server...");
            bool connected = Task.Run(() => connector.Connect()).GetAwaiter().GetResult();
            if (!connected)
                return false;
            reconnectTimer.Change(config.EmbyReconnectPeriod, config.EmbyReconnectPeriod);

            return true;
        }

        public void Stop()
        {
            reconnectTimer.Dispose();
            connector?.Stop();
        }

        private void CheckConnection(object stateInfo)
        {
            log.Debug("Running periodic websocket connection check");
            connector.CheckWebSocket();
        }
    }
}

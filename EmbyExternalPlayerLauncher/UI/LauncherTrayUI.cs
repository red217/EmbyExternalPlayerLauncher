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

using EmbyExternalPlayerLauncher.Config;
using EmbyExternalPlayerLauncher.ServerConnect;
using EmbyExternalPlayerLauncher.Players.MpcHc;
using EmbyExternalPlayerLauncher.Properties;
using log4net;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmbyExternalPlayerLauncher.UI
{
    public class LauncherTrayUI : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private NotifyIcon trayIcon = new NotifyIcon();
        private ContextMenuStrip menu = new ContextMenuStrip();
        private LauncherConfig config;

        private EmbyPlayerConnector connector;
        private bool configFormOpen = false;
        private bool aboutFormOpen = false;

        public LauncherTrayUI(LauncherConfig config)
        {
            this.config = config;
            CreateMenuStrip();
            CreateTrayIcon();
            CreateEmbyConnector();
        }

        private bool CreateEmbyConnector()
        {
            if (config != null)
            {
                var player = new MpcHcWebAdapter(
                    config.PlayerPath,
                    config.PlayerArgs,
                    config.MpcHcWebPort,
                    config.MpcHcWebTimeout);
                connector = new EmbyPlayerConnector(
                    player,
                    config.EmbyUser,
                    config.EmbyPass,
                    config.EmbyAddress);

                //TODO: apparently causes a deadlock when running on the main/UI thread, investigate why
                log.Info("Connecting to Emby Server...");
                bool success = Task.Run(() => connector.Connect()).GetAwaiter().GetResult();
                if (!success)
                {
                    string msg = "Could not connect to Emby. Please check your settings.";
                    trayIcon.Text = msg;
                    log.Error(msg);
                    Task.Run(() => MessageBox.Show(msg));
                    return false;
                }
                else trayIcon.Text = "Connected to " + connector.EmbyAddress;

            }
            else
            {
                string msg = "Emby External Player Launcher is not configured.";
                trayIcon.Text = msg;
                log.Warn(msg);
                Task.Run(() => MessageBox.Show("The launcher does not seem to be configured. "
                    + "Right click the tray icon to open the configuration interface.",
                    "Not Configured",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk));
                return false;
            }
            return true;
        }

        #region UI

        private void CreateTrayIcon()
        {
            trayIcon.Text = "External Player Launcher for Emby";
            trayIcon.Icon = Resources.LauncherIcon;
            trayIcon.ContextMenuStrip = menu;
            trayIcon.Visible = true;
        }

        private void CreateMenuStrip()
        {
            var configItem = new ToolStripMenuItem("Configuration");
            configItem.Click += ConfigItem_Click;
            menu.Items.Add(configItem);

            menu.Items.Add(new ToolStripSeparator());

            var aboutItem = new ToolStripMenuItem("About");
            aboutItem.Click += AboutItem_Click;
            menu.Items.Add(aboutItem);

            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItem_Click;
            menu.Items.Add(exitItem);
        }

        private void AboutItem_Click(object sender, EventArgs e)
        {
            if (aboutFormOpen)
                return;
            aboutFormOpen = true;
            new LauncherAboutForm().ShowDialog();
            aboutFormOpen = false;
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            connector?.Stop();
            Application.Exit();
        }

        private void ConfigItem_Click(object sender, EventArgs e)
        {
            if (configFormOpen)
                return;
            configFormOpen = true;
            var configForm = new LauncherConfigForm(config);
            if (configForm.ShowDialog() == DialogResult.OK)
            {
                config = configForm.Config;
                config.SaveToFile();
                connector?.Stop();
                connector = null;
                CreateEmbyConnector();
            }
            configFormOpen = false;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    trayIcon.Dispose();
                    trayIcon = null;
                    connector?.Stop();
                    connector = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PlayerTrayUI() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

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
using System;
using System.IO;
using System.Windows.Forms;

namespace EmbyExternalPlayerLauncher.UI
{
    public partial class LauncherConfigForm : Form
    {
        private LauncherConfig newConfig;

        public LauncherConfigForm(LauncherConfig config)
        {
            InitializeComponent();
            if (config != null)
                SetFromConfig(config);
            else
                SetDefaultConfig();
        }

        public LauncherConfig Config
        {
            get { return newConfig; }
        }

        private void SetFromConfig(LauncherConfig config)
        {
            textBoxPath.Text = config.PlayerPath;
            textBoxArgs.Text = string.IsNullOrEmpty(config.PlayerArgs) ? LauncherConfig.DefaultPlayerArgs : config.PlayerArgs;
            textBoxPort.Text = config.MpcHcWebPort.ToString();
            textBoxTimeout.Text = config.MpcHcWebTimeout.ToString();
            textBoxUser.Text = config.EmbyUser;
            textBoxPassword.Text = config.EmbyPass;
            textBoxAddress.Text = config.EmbyAddress;
            textBoxReconnectPeriod.Text = config.EmbyReconnectPeriod.ToString();
        }

        private void SetDefaultConfig()
        {
            textBoxPath.Text = LauncherConfig.DefaultPlayerPath;
            textBoxArgs.Text = LauncherConfig.DefaultPlayerArgs;
            textBoxPort.Text = LauncherConfig.DefaultMpcHcPort.ToString();
            textBoxTimeout.Text = LauncherConfig.DefaultMpcHcWebTimeout.ToString();
            textBoxUser.Text = LauncherConfig.DefaultEmbyUser;
            textBoxPassword.Text = LauncherConfig.DefaultEmbyPass;
            textBoxAddress.Text = LauncherConfig.DefaultEmbyAddress;
            textBoxReconnectPeriod.Text = LauncherConfig.DefaultReconnectPeriod.ToString();
        }

        private bool ValidateFormSettings()
        {
            if (textBoxPath.Text == "")
            {
                MessageBox.Show("The player path cannot be empty.", "Invalid Player Path", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (textBoxUser.Text == "")
            {
                MessageBox.Show("The user name cannot be empty.", "Invalid User Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            int port;
            if (!int.TryParse(textBoxPort.Text, out port) || port < 1 || port > 65535)
            {
                MessageBox.Show("The port must be an integer between 1 and 65535.", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            int timeout;
            if (!int.TryParse(textBoxTimeout.Text, out timeout) || timeout < 1)
            {
                MessageBox.Show("The timeout must be a positive integer.", "Invalid Timeout", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            int period;
            if (!int.TryParse(textBoxReconnectPeriod.Text, out period) || period < 1)
            {
                MessageBox.Show("The reconnect period must be a positive integer.", "Invalid Reconnect Period", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            newConfig = new LauncherConfig
            {
                EmbyAddress = textBoxAddress.Text,
                EmbyPass = textBoxPassword.Text,
                EmbyUser = textBoxUser.Text,
                MpcHcWebPort = port,
                MpcHcWebTimeout = timeout,
                PlayerArgs = textBoxArgs.Text,
                PlayerPath = textBoxPath.Text,
                EmbyReconnectPeriod = period
            };

            return true;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (textBoxPath.Text != "")
            {
                openFileDialogMpcHc.InitialDirectory = Path.GetDirectoryName(textBoxPath.Text);
                openFileDialogMpcHc.FileName = Path.GetFileName(textBoxPath.Text);
            }
            if (openFileDialogMpcHc.ShowDialog() == DialogResult.OK)
                textBoxPath.Text = openFileDialogMpcHc.FileName;
        }

        private void buttonDefaultArgs_Click(object sender, EventArgs e)
        {
            textBoxArgs.Text = LauncherConfig.DefaultPlayerArgs;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateFormSettings())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}

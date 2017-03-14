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

using System;
using System.Windows.Forms;

namespace EmbyExternalPlayerLauncher.UI
{
    public partial class LauncherAboutForm : Form
    {
        public LauncherAboutForm()
        {
            InitializeComponent();
        }

        private void LauncherAboutForm_Load(object sender, EventArgs e)
        {
            labelVersion.Text = Utils.ApplicationVersion;
        }

        private void richTextBoxLicense_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", e.LinkText);
        }
    }
}

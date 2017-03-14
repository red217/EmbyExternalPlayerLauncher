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

namespace EmbyExternalPlayerLauncher.Players.MpcHc
{
    public static class MpcHcCommands
    {
        /// <summary>
        /// The parameter name for MPC-HC's web commands
        /// </summary>
        public const string PostCommandName = "wm_command";

        public const string Seek = "-1";
        public const string SetVolume = "-2";
        public const string Play = "887";
        public const string Pause = "888";
        public const string PlayPause = "889";
        public const string VolumeUp = "907";
        public const string VolumeDown = "908";
        public const string Mute = "909";
    }
}

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

namespace EmbyExternalPlayerLauncher.Players.MpcHc
{
    /// <summary>
    /// MpcHcStatus contains all information available from the web UI, though it is not currently used
    /// </summary>
    public class MpcHcStatus : PlayerStatus
    {
        public string File { get; set; }
        public string FilePathArg { get; set; }
        public string FilePath { get; set; }
        public string FileDirArg { get; set; }
        public string FileDir { get; set; }
        public string StateString { get; set; }
        public string PositionString { get; set; }
        public long Duration { get; set; }
        public string DurationString { get; set; }
        public float PlaybackRate { get; set; }
        public string Size { get; set; }
        public int ReloadTime { get; set; }
        public string Version { get; set; }
    }
}

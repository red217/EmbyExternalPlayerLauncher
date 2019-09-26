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

namespace EmbyExternalPlayerLauncher.Players
{
    public interface IPlayerAdapter
    {
        bool Play(string filePath, long embyStartTicks = 0);
        bool Pause();
        bool Unpause();
        bool PlayPause();        
        bool Stop();
        bool Seek(long embyTicks);
        bool VolumeUp();
        bool VolumeDown();
        bool SetVolume(int volume);
        bool ToggleMute();
        bool CanSeek();
        PlayerStatus GetPlayerStatus();
    }
}

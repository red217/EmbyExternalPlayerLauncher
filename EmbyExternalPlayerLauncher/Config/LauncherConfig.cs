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

using log4net;
using Newtonsoft.Json;
using System;
using System.IO;

namespace EmbyExternalPlayerLauncher.Config
{
    public class LauncherConfig
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string ConfigFolder = @"\Emby External Player Launcher";
        private const string ConfigFile = @"\e2pl.json";
        private static string fullFolderPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + ConfigFolder;

        public string PlayerPath { get; set; }
        public string PlayerArgs { get; set; }
        public string EmbyUser { get; set; }
        public string EmbyPass { get; set; }
        public int MpcHcWebPort { get; set; }
        public int MpcHcWebTimeout { get; set; }
        public string EmbyAddress { get; set; }

        public static LauncherConfig ReadFromFile()
        {
            try
            {
                log.DebugFormat("Reading configuration file {0}", fullFolderPath + ConfigFile);
                return JsonConvert.DeserializeObject<LauncherConfig>(File.ReadAllText(fullFolderPath + ConfigFile));
            }
            catch (Exception ex)
            {
                log.Error("Cannot read configuration file.", ex);
            }
            return null;
        }

        public void SaveToFile()
        {
            try
            {
                if (!Directory.Exists(fullFolderPath))
                {
                    log.Info("Configuration directory does not exist, attempting to create it.");
                    Directory.CreateDirectory(fullFolderPath);
                }
                log.DebugFormat("Writing configuration file {0}", fullFolderPath + ConfigFile);
                File.WriteAllText(fullFolderPath + ConfigFile, JsonConvert.SerializeObject(this));
            }
            catch (Exception ex)
            {
                log.Error("Cannot write configuration file.", ex);
            }
        }
    }
}

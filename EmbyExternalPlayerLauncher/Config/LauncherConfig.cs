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

        //default values are here for ease of access & organizational purpose
        public const string DefaultPlayerPath = "";
        public const string DefaultPlayerArgs = "/play /fullscreen /start {s} /close";
        public const string DefaultEmbyUser = "";
        public const string DefaultEmbyPass = "";
        public const string DefaultEmbyAddress = "";
        public const int DefaultMpcHcPort = 13579;
        public const int DefaultMpcHcWebTimeout = 2000;
        public const int DefaultReconnectPeriod = 60000;

        private const string ConfigFolder = @"\Emby External Player Launcher";
        private const string ConfigFile = @"\e2pl.json";
        private static string fullFolderPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + ConfigFolder;

        public string PlayerPath { get; set; } = DefaultPlayerPath;
        public string PlayerArgs { get; set; } = DefaultPlayerArgs;
        public string EmbyUser { get; set; } = DefaultEmbyUser;
        public string EmbyPass { get; set; } = DefaultEmbyPass;
        public int MpcHcWebPort { get; set; } = DefaultMpcHcPort;
        public int MpcHcWebTimeout { get; set; } = DefaultMpcHcWebTimeout;
        public string EmbyAddress { get; set; } = DefaultEmbyAddress;
        public int EmbyReconnectPeriod { get; set; } = DefaultReconnectPeriod;

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

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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmbyExternalPlayerLauncher.Players.MpcHc
{
    public static class MpcHcWebParser
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static MpcHcStatus ParseStatusFromStream(Stream stream)
        {
            var status = new MpcHcStatus();
            log.Debug("Reading MPC-HC variables to dictionary.");
            var mpcHcVars = ReadDictFromHtml(stream);
            if (log.IsDebugEnabled)
                log.DebugFormat("MPC-HC parsed variables:\n{0}", DictToString(mpcHcVars));

            status.File = GetStringOrDefault(mpcHcVars, MpcHcVarNames.File, "");
            status.FilePathArg = GetStringOrDefault(mpcHcVars, MpcHcVarNames.FilePathArg, "");
            status.FilePath = GetStringOrDefault(mpcHcVars, MpcHcVarNames.FilePath, "");
            status.FileDirArg = GetStringOrDefault(mpcHcVars, MpcHcVarNames.FileDirArg, "");
            status.FileDir = GetStringOrDefault(mpcHcVars, MpcHcVarNames.FileDir, "");
            status.State = ParseStateOrDefault(mpcHcVars, PlayerState.Stopped);
            status.StateString = GetStringOrDefault(mpcHcVars, MpcHcVarNames.StateString, "Stopped");
            status.Position = ParseIntOrDefault(mpcHcVars, MpcHcVarNames.Position, 0);
            status.PositionString = GetStringOrDefault(mpcHcVars, MpcHcVarNames.PositionString, "00:00:00");
            status.Duration = ParseIntOrDefault(mpcHcVars, MpcHcVarNames.Duration, 0);
            status.DurationString = GetStringOrDefault(mpcHcVars, MpcHcVarNames.DurationString, "00:00:00");
            status.VolumeLevel = ParseIntOrDefault(mpcHcVars, MpcHcVarNames.VolumeLevel, 100);
            status.Muted = ParseBoolOrDefault(mpcHcVars, MpcHcVarNames.Muted, false);
            status.PlaybackRate = ParseFloatOrDefault(mpcHcVars, MpcHcVarNames.PlaybackRate, 1f);
            status.Size = GetStringOrDefault(mpcHcVars, MpcHcVarNames.Size, "");
            status.ReloadTime = ParseIntOrDefault(mpcHcVars, MpcHcVarNames.ReloadTime, 0);
            status.Version = GetStringOrDefault(mpcHcVars, MpcHcVarNames.Version, "");

            return status;
        }

        private static string DictToString(IDictionary<string, string> dict)
        {
            var stringBuilder = new StringBuilder();
            foreach (var kv in dict)
                stringBuilder
                    .Append(kv.Key)
                    .Append(": ")
                    .AppendLine(kv.Value);
            return stringBuilder.ToString();
        }

        private static IDictionary<string, string> ReadDictFromHtml(Stream stream)
        {
            var dict = new Dictionary<string, string>();

            using (var sr = new StreamReader(stream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (!line.StartsWith("<p id=\""))
                        continue;
                    string key = GetKeyName(line);
                    string val = GetValue(line);
                    if (key == null || val == null)
                        continue;
                    dict[key] = val;
                }
            }

            return dict;
        }

        private static string GetValue(string line)
        {
            return SubstringBetween(line, '>', '<');
        }

        private static string GetKeyName(string line)
        {
            return SubstringBetween(line, '"', '"');
        }

        private static string SubstringBetween(string s, char c1, char c2)
        {
            int startPos = s.IndexOf(c1);
            if (startPos == -1 || startPos == s.Length - 1)
                return null;
            int endPos = s.IndexOf(c2, startPos + 1);
            if (endPos == -1)
                return null;
            return s.Substring(startPos + 1, endPos - startPos - 1);
        }

        private static string GetStringOrDefault(IDictionary<string, string> dict, string key, string defaultValue)
        {
            string ret;
            if (dict.TryGetValue(key, out ret))
                return ret;
            log.WarnFormat("No key '{0}' in dictionary, using default '{1}' instead.", key, defaultValue);
            return defaultValue;
        }

        private static int ParseIntOrDefault(IDictionary<string, string> dict, string key, int defaultValue)
        {
            string aux;
            if ((aux = GetStringOrDefault(dict, key, null)) == null)
            {
                log.WarnFormat("Using default int {0} for key '{1}'", defaultValue, key);
                return defaultValue;
            }

            int ret;
            if (int.TryParse(aux, out ret))
                return ret;
            log.WarnFormat("Could not parse '{0}' into an int for key '{2}', using default {1} instead.", aux, defaultValue, key);
            return defaultValue;
        }

        private static PlayerState ParseStateOrDefault(IDictionary<string, string> dict, PlayerState defaultValue)
        {
            var aux = GetStringOrDefault(dict, MpcHcVarNames.State, "0"); //returns Stopped state if the string doesn't exist
            switch (aux)
            {
                case "-1":
                    return PlayerState.None;
                case "0":
                    return PlayerState.Stopped;
                case "1":
                    return PlayerState.Paused;
                case "2":
                    return PlayerState.Playing;
                default:
                    log.WarnFormat("Unknown value '{0}' for MPC-HC player state, using default state '{1}' instead.", aux, defaultValue);
                    return defaultValue;
            }
        }

        private static bool ParseBoolOrDefault(IDictionary<string, string> dict, string key, bool defaultValue)
        {
            string aux = GetStringOrDefault(dict, key, "2"); //return default
            switch (aux)
            {
                case "0":
                    return false;
                case "1":
                    return true;
                default:
                    log.WarnFormat("Could not parse '{0}' into a bool for key '{2}', using default '{1}' instead.", aux, defaultValue, key);
                    return defaultValue;
            }
        }

        private static float ParseFloatOrDefault(IDictionary<string, string> dict, string key, float defaultValue)
        {
            string aux;
            if ((aux = GetStringOrDefault(dict, key, null)) == null)
            {
                log.WarnFormat("Using default float {0} for key '{1}'", defaultValue, key);
                return defaultValue;
            }

            float ret;
            if (float.TryParse(aux, out ret))
                return ret;
            log.WarnFormat("Could not parse '{0}' into a float for key '{2}', using default {1} instead.", aux, defaultValue, key);
            return defaultValue;
        }
    }
}

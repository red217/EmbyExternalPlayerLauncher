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
using MediaBrowser.Model.Logging;
using System;
using System.Text;

namespace EmbyExternalPlayerLauncher.ServerConnect.Logging
{
    public class EmbyLogger : ILogger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Debug(string message, params object[] paramList)
        {
            log.DebugFormat(message, paramList);
        }

        public void Error(string message, params object[] paramList)
        {
            log.ErrorFormat(message, paramList);
        }

        public void ErrorException(string message, Exception exception, params object[] paramList)
        {
            log.Error(string.Format(message, paramList), exception);
        }

        public void Fatal(string message, params object[] paramList)
        {
            log.FatalFormat(message, paramList);
        }

        public void FatalException(string message, Exception exception, params object[] paramList)
        {
            log.Fatal(string.Format(message, paramList), exception);
        }

        public void Info(string message, params object[] paramList)
        {
            log.InfoFormat(message, paramList);
        }

        public void Log(LogSeverity severity, string message, params object[] paramList)
        {
            switch (severity)
            {
                case LogSeverity.Debug:
                    Debug(message, paramList);
                    break;
                case LogSeverity.Error:
                    Error(message, paramList);
                    break;
                case LogSeverity.Fatal:
                    Fatal(message, paramList);
                    break;
                case LogSeverity.Info:
                    Info(message, paramList);
                    break;
                case LogSeverity.Warn:
                    Warn(message, paramList);
                    break;
            }
        }

        public void LogMultiline(string message, LogSeverity severity, StringBuilder additionalContent)
        {
            Log(severity, message + "\n" + additionalContent.ToString());
        }

        public void Warn(string message, params object[] paramList)
        {
            log.WarnFormat(message, paramList);
        }
    }
}

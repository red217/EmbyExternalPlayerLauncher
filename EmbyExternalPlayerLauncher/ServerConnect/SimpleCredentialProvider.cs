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

using Emby.ApiClient;
using Emby.ApiClient.Model;
using System.Threading.Tasks;

namespace EmbyExternalPlayerLauncher.ServerConnect
{
    public class SimpleCredentialProvider : ICredentialProvider
    {
        private ServerCredentials cred;
        public Task<ServerCredentials> GetServerCredentials()
        {
            if (cred == null)
                cred = new ServerCredentials();
            return Task.FromResult(cred);
        }

        public Task SaveServerCredentials(ServerCredentials configuration)
        {
            cred = configuration;
            return Task.FromResult(true);
        }
    }
}

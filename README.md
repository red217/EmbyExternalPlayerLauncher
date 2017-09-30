Emby External Player Launcher
=============================
Emby External Player Launcher (**E2PL**) is a small .NET 4.6.1 application that registers itself as a video player to an Emby server and launches/controls MPC-HC to actually handle video playback. I wrote E2PL because I wanted to be able to watch videos using MPC-HC while also keeping the progress synchronized to my Emby library.
E2PL is not affiliated with the Emby project.

If you somehow ended up reading about E2PL but do not know what Emby is, check out the project's website:
https://emby.media/

License
-------
Emby External Player Launcher is licensed under the GNU General Public License v3. You should have received a full copy of the license text, please see `LICENSE.txt`.

E2PL source code is freely available on GitHub:
https://github.com/Red217/EmbyExternalPlayerLauncher

Third-party libraries
---------------------
E2PL makes use of third-party libraries, please see `CREDITS.txt`

Setup
-----
### Requirements
* Windows system with .NET Framework 4.6.1 installed
* MPC-HC should be installed
* A functional Emby server installation somewhere on the local network
* Direct access to the video files - either on the same system or over the network using shared files/folders combined with the Emby server's *Shared network folder* path substitution feature

### Installing E2PL
No specific installation is required, E2PL simply runs from any folder you save it to.

1. Get a version of E2PL - either download the provided .zip archive or build it yourself from source
2. Extract/copy E2PL to the folder you wish to save it to
3. Run `Emby External Player Launcher.exe` to start the application - note that E2PL runs in the background and you can find its icon in the system tray
4. If you're starting it for the first time, you will see a message asking you to configure E2PL, you can do this by right clicking the tray icon and choosing the Configuration option
5. Set the path to `mpc-hc.exe`, your local Emby user name and your local Emby password (*The current version of E2PL is only intended to be used on a single machine setup or on your local network and the Emby server credentials are saved in its configuration file for automated login, __do not use it on a machine you do not trust__*)
6. *(Optional)* If you want E2PL to start up with Windows so it's always available, create a shortcut to the .exe in the Startup folder (to quickly open the Startup folder press **Win+R**, type `shell:startup` and hit **Enter**)

Note that E2PL requires network access to contact the Emby server if it is not running on the same machine. MPC-HC is also controlled through its built-in web interface, which is accessed exclusively through localhost and the configured port (13579 by default). If you have any firewall blocking access to either of these, you will need to add rules accordingly in order to allow traffic to pass.

If you encounter any issues, read any error messages that appear and have a look at the E2PL log file which may provide more information. It's saved under `e2pl.log` in your E2PL folder.

The configuration file is saved under `%appdata%\Emby External Player Launcher\e2pl.json`

### Playing a file
You can play files from the Emby server's web interface.

1. Make sure E2PL is configured and is running (see above)
2. Open the Emby web interface in the browser of your choice, or use the Emby app of your choice
2. Select "Emby External Player Launcher" as your player in Emby (there's an icon in the top-right of the web interface to do this)
3. Play the file of your choice, MPC-HC will start up and play the file automatically

Viewing progress will be updated automatically in your Emby library and you can also use Emby as a basic remote control for MPC-HC: pause/unpause, volume control and seeking are supported. Stop will stop playback by closing MPC-HC.
MPC-HC will be started and stopped automatically as needed. If you want automatic progress reporting to Emby to function correctly, you should make sure that MPC-HC is not already running when you start playback from Emby - **allow E2PL to manage the player by itself**.

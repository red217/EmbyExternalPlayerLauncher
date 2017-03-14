# This short script creates a zip archive of the E2PL version 
# in the Release folder, including all required files for distribution
#
# You may do with this file as you wish

$projDir = "EmbyExternalPlayerLauncher"
$releaseDir = $projDir + "\bin\Release"

if (!(Test-Path -PathType Container dist))
{
	New-Item -ItemType Directory dist
}

$fileVer = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($releaseDir + "\Emby External Player Launcher.exe").FileVersion
$versionDir = "dist\v" + $fileVer
if (Test-Path -PathType Container $versionDir)
{
	Remove-Item $versionDir\*.*
}
else
{
	New-Item -ItemType Directory $versionDir
}

cp $releaseDir\*.dll $versionDir\
cp "$releaseDir\Emby External Player Launcher.exe" $versionDir\
cp $releaseDir\log4net.xml $versionDir\
cp *.md $versionDir

Compress-Archive -Force -Path $versionDir\* -DestinationPath "dist\EmbyExternalPlayerLauncher-v$fileVer.zip"
echo Done
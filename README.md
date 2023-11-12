
<p align="center">
  <img width="300" height="300" src="https://raw.githubusercontent.com/Duinrahaic/VRCPictureToClipboard/master/icon.png">
 </p>
 <h1 align="center"> VRCPictureToClipboard </h5>



While this program runs in the background, it automatically takes the latest picture from VRChat and copies it to the clipboard.

Also while you are here please upvote this [feature](https://feedback.vrchat.com/feature-requests/p/picture-to-clipboard) to get this feature included into VRChat.

## Usage

After starting the program, it lives in the system tray.
Clicking on the tray icon gives opens a menu with a few different options:

- `Start/Attach to SteamVR`: If the program was opened with SteamVR not running, this option will launch it, or attach it to a running instance that was manually started.
- `Register with SteamVR`: "Installs" the software in SteamVR. This will put it into the list of possible startup overlay apps, so you can have SteamVR launch it automatically.
- `Unregister from SteamVR`: "Uninstalls" the software from SteamVR, so it can no longer be automatically started.
- `Pause`: Will pause/unpause the clipboard copying without closing the app.
- `Exit`


## Steps to auto-launch with SteamVR

1. Launch SteamVR
2. Launch VRCPictureToClipboard
3. In the tray, right click the icon and select `Register with SteamVR`
4. In SteamVR, go to `Settings` -> `Startup/Shutdown` -> `Choose Startup Overlay Apps` and make sure `VRCPictureToClipboard` is checked on.

Now whenever you launch SteamVR, VRCPictureToClipboard will automatically launch as well.


## Contributions 

[jangxx](https://github.com/jangxx): For enhancing the program to work as a tray application, adding SteamVR integration, and for the logo.

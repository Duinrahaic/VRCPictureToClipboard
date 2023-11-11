# VRCPictureToClipboard

While this program runs in the background, it will automatically takes the latest picture from VRChat and copy it to the clipboard.

Also while you are here please upvote this feature [VRChat Feedback](https://feedback.vrchat.com/feature-requests/p/picture-to-clipboard) to get this feature in VRChat.

# Run as a service:
## Install
1. Download/Compile the latest release.
2. Using File Explorer, Create a folder to keep the application in. 
3. Open Command Prompt in administrator. 
4. Enter 'sc.exe create VRCPictureToClipboard binPath= "[PATH]\VRCPictureToClipboard.exe" start= auto' (Replace the [PATH] with the location to the application)

If you see the message `[SC] CreateService SUCCESS` then you have successfully installed the service.

## Uninstall
1. Open Command Prompt in administrator. 
2. Enter 'sc.exe delete "VRCPictureToClipboard"'

If you see the message `[SC] DeleteService SUCCESS` then you have successfully uninstalled the service.


Enjoy. :)
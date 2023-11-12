using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Valve.VR;
using System.IO;
using System.Diagnostics;

namespace VRCPictureToClipboard
{
    internal class OVRIntegration
    {
        public static string APPLICATION_KEY = "com.jangxx.vrc-picture-clipboard";

        private CVRSystem? cVR;
        private bool initialized = false;

        public bool Initialized
        {
            get { return initialized; }
        }

        public bool TryInit()
        {
            if (cVR != null)
            {
                this.initialized = false;
                return false;
            }

            EVRInitError error = EVRInitError.None;
            cVR = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Overlay);

            if (error != EVRInitError.None)
            {
                this.initialized = false;
                return false;
            }
            else
            {
                this.initialized = true;
                return true;
            }
        }

        public void Shutdown()
        {
            if (cVR != null)
            {
                OpenVR.Shutdown();
                cVR = null;
            }
        }

        public bool IsInstalled()
        {
            if (cVR == null || !initialized)
            {
                return false;
            }

            return OpenVR.Applications.IsApplicationInstalled(APPLICATION_KEY);
        }

        public void InstallManifest()
        {
            if (cVR == null || !initialized)
            {
                return;
            }

            var executablePath = Application.ExecutablePath;
            var executableDir = Path.GetDirectoryName(executablePath);

            MessageBox.Show(executablePath);

            EVRApplicationError error = OpenVR.Applications.AddApplicationManifest(Path.Join(executableDir, "manifest.vrmanifest"), false);

            if (error != EVRApplicationError.None)
            {
                MessageBox.Show("Error while registering with SteamVR: " + error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                MessageBox.Show("Successfully registered with SteamVR", "Success");
            }
        }

        public void UninstallManifest()
        {
            if (cVR == null || !initialized)
            {
                return;
            }

            StringBuilder sb = new StringBuilder("", 512);
            EVRApplicationError error = EVRApplicationError.None;

            OpenVR.Applications.GetApplicationPropertyString(APPLICATION_KEY, EVRApplicationProperty.WorkingDirectory_String, sb, 512, ref error);

            var manifestPath = Path.Join(sb.ToString(), "manifest.vrmanifest");
            error = OpenVR.Applications.RemoveApplicationManifest(manifestPath);

            if (error != EVRApplicationError.None)
            {
                MessageBox.Show("Error while unregistering from SteamVR: " + error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Successfully unregistered from SteamVR", "Success");
            }
        }
    }
}

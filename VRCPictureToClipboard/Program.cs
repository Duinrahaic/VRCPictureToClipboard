using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace VRCPictureToClipboard
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            // only allow running one copy of this app at the same time
            bool mutexSuccess = false;
            var globalMutex = new Mutex(true, @"Local\VRCPictureToClipboard.exe", out mutexSuccess);

            if (!mutexSuccess)
            {
                Debug.Print("App is already running. Quitting...");
                globalMutex.Close();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new VRCPictureClipboardApplicationContext());

            globalMutex.Close();
        }
    }
}
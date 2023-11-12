
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRCPictureToClipboard
{
    public class Watcher
    {
        private string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "VRChat");
        private FileSystemWatcher watcher;
        private bool paused = false;

        public Watcher() 
        {
            watcher = new FileSystemWatcher();
            watcher.Path = Path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.png";
            watcher.Changed += (s, e) => this.Watcher_Changed(e);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
        }

        private void Watcher_Changed(FileSystemEventArgs e)
        {
            Debug.Print($"New image taken: {e.Name}");

            if (this.Paused) return;

            Thread t = new Thread(() =>
            {
                try
                {
                    Clipboard.SetImage(Image.FromFile(e.FullPath));
                }
                catch { } // ignore all excpetions
            });

            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        public void Dispose()
        {
            watcher.Dispose();
        }

        public void SetPaused(bool paused)
        {
            this.paused = paused;
        }

        public bool Paused
        {
            get { return paused; }
        }
    }
}

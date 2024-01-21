
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;

namespace VRCPictureToClipboard
{
    public class Watcher
    {
        private string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "VRChat");
        private FileSystemWatcher watcher;
        private bool paused = false;
        private bool QRMode = false;

        public Watcher() 
        {
            watcher = new FileSystemWatcher();
            watcher.Path = Path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.png";
            watcher.Changed += (s, e) => this.Watcher_Changed(e);
            watcher.Changed += (s, e) => this.ReadBarcode(e);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
        }

        private void Watcher_Changed(FileSystemEventArgs e)
        {
            Debug.Print($"New image taken: {e.Name}");

            if (this.Paused) return;
            if (this.QRMode) return;

            Thread t = new Thread(() =>
            {
                try
                {
                    
                    var image = Image.FromFile(e.FullPath);
                    // Clipboard.SetImage(image);
                    var data = new DataObject();
                    data.SetData(DataFormats.Bitmap, image);
                    data.SetFileDropList(new StringCollection { e.FullPath });
                    Clipboard.SetDataObject(data, true);
                }
                catch { } // ignore all excpetions
            });

            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            
        }
        
        private void ReadBarcode(FileSystemEventArgs e)
        {
            Debug.Print($"New image taken: {e.Name}");

            if (this.Paused) return;
            if (this.QRMode == false) return;

            Thread t = new Thread(() =>
            {
                try
                {
                    string output = QRReader.ReadQRCode(e.FullPath);
                    if(string.IsNullOrEmpty(output))
                        return;
                    bool isUri = Uri.IsWellFormedUriString(output, UriKind.RelativeOrAbsolute); // Only URLs are copied to clipboard
                    if (isUri)
                    {
                        Clipboard.SetText(output);
                    }
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

        public void SetQRMode(bool enable)
        {
            this.QRMode = enable;
        }
        
        public bool QRModeEnabled
        {
            get { return QRMode; }
        }

        public bool Paused
        {
            get { return paused; }
        }
    }
}

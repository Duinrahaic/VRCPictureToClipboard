
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRCPictureToClipboard
{
    public class Watcher : IHostedService, IDisposable
    {
        private ILogger<Watcher> _logger;
        private string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "VRChat");
        private FileSystemWatcher watcher;

        public Watcher(ILogger<Watcher> logger) 
        {
            _logger = logger;
            _logger.LogInformation("VRCPictureToClipboard Started");
            _logger.LogInformation($"Watching {Path}");
            watcher = new FileSystemWatcher();
            watcher.Path = Path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.png";
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
        }
        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"New image taken: {e.Name}");
            Thread t = new Thread((ThreadStart)(() => {
                Clipboard.SetImage(Image.FromFile(e.FullPath));
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            watcher.Dispose();
        }

    }
}

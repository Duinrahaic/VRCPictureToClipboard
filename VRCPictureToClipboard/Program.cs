using System.IO;

internal class Program
{
    static string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "VRChat");

     private static void Main(string[] args)
    {
        Console.WriteLine("VRCPictureToClipboard Started");
        Console.WriteLine($"Watching {Path}");
        FileSystemWatcher watcher;
        watcher = new FileSystemWatcher();
        watcher.Path = Path;
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        watcher.Filter = "*.png";
        watcher.Changed += Watcher_Changed;
        watcher.EnableRaisingEvents = true;
        watcher.IncludeSubdirectories = true;
        Thread.Sleep(-1);
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
}
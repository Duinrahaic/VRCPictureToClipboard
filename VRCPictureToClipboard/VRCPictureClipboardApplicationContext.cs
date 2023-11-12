using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRCPictureToClipboard
{
    internal class VRCPictureClipboardApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Watcher watcher;
        private OVRIntegration ovr;

        public VRCPictureClipboardApplicationContext()
        {
            var iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VRCPictureToClipboard.icon.ico");

            if (iconStream == null)
            {
                throw new Exception("Trayicon could not be loaded");
            }

            var icon = new Icon(iconStream);

            trayIcon = new NotifyIcon()
            {
                Icon = icon,
                ContextMenuStrip = new ContextMenuStrip(),
                Visible = true,
                Text = "VRC Picture To Clipboard",
            };

            watcher = new Watcher();

            ovr = new OVRIntegration();
            ovr.TryInit();

            ovr.CloseRequested += CloseRequestedHandler;

            setupTrayMenu();
        }

        private void setupTrayMenu()
        {
            trayIcon.ContextMenuStrip.Items.Clear();

            if (ovr.Initialized)
            {
                if (ovr.IsInstalled())
                {
                    trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Unregister from SteamVR", null, UnregisterSteamVR));
                } else
                {
                    trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Register with SteamVR", null, RegisterSteamVR));
                }
            } else
            {
                trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Start/Attach to SteamVR", null, AttachSteamVR));
            }

            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Pause", null, Pause));
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, Exit));
        }

        private void CloseRequestedHandler(object? sender, EventArgs args)
        {
            Exit(null, new EventArgs());
        }

        private void AttachSteamVR(object? sender, EventArgs e)
        {
            ovr.TryInit(true);
            setupTrayMenu();
        }

        private void RegisterSteamVR(object? sender, EventArgs e)
        {
            ovr.InstallManifest();
            setupTrayMenu();
        }

        private void UnregisterSteamVR(object? sender, EventArgs e)
        {
            ovr.UninstallManifest();
            setupTrayMenu();
        }

        private void Pause(object? sender, EventArgs e)
        {
            watcher.SetPaused(!watcher.Paused);

            ((ToolStripMenuItem)sender!).Checked = watcher.Paused;
        }

        private void Exit(object? sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}

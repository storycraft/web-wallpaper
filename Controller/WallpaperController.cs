using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace web_wallpaper.Controller
{
    public class WallpaperController
    {

        public WebWallpaper Wallpaper { get; }

        public NotifyIcon Icon { get; }
        public ControlMenu Menu { get; }

        public WallpaperController(WebWallpaper wallpaper)
        {
            Wallpaper = wallpaper;

            Icon = new NotifyIcon();
            Menu = new ControlMenu();

            Shown = false;

            Icon.Icon = SystemIcons.Application;
            Menu.Hook(Icon);
            Icon.ContextMenu.Popup += OnMenuOpen;

            SetupMenu();
        }

        protected void SetupMenu()
        {
            Menu.ExitItem.Click += ExitItem_Click;
            Menu.SetURLItem.Click += SetURLItem_Click;
            Menu.DevToolsItem.Click += DevToolsItem_Click;

            Menu.MouseMovementItem.Click += MouseMovementItem_Click;
            Menu.MouseInteractionItem.Click += MouseInteractionItem_Click;
        }

        protected void OnMenuOpen(object sender, EventArgs e)
        {
            Menu.MouseMovementItem.Checked = Wallpaper.WallpaperManager.MouseMovementEnabled;
            Menu.MouseInteractionItem.Checked = Wallpaper.WallpaperManager.MouseInteractionEnabled;
        }

        protected void MouseMovementItem_Click(object sender, EventArgs e)
        {
            Menu.MouseMovementItem.Checked = Wallpaper.WallpaperManager.MouseMovementEnabled = !Menu.MouseMovementItem.Checked;
        }

        protected void MouseInteractionItem_Click(object sender, EventArgs e)
        {
            Menu.MouseInteractionItem.Checked = Wallpaper.WallpaperManager.MouseInteractionEnabled = !Menu.MouseInteractionItem.Checked;
        }

        protected void DevToolsItem_Click(object sender, EventArgs e)
        {
            Wallpaper.WallpaperManager.Window.ShowDevTools();
        }

        protected void SetURLItem_Click(object sender, EventArgs e)
        {
            using (var form = new SetURLForm(Wallpaper.WallpaperManager))
            {
                form.ShowDialog();
            }
        }

        protected void ExitItem_Click(object sender, EventArgs e)
        {
            Wallpaper.Stop();
        }

        public bool Shown { get; private set; }

        public void ShowTrayIcon()
        {
            if (Shown)
            {
                return;
            }

            Icon.Visible = true;

            Shown = true;
        }

        public void HideTrayIcon()
        {
            if(!Shown)
            {
                return;
            }

            Icon.Visible = false;

            Shown = false;
        }

       
    }

}

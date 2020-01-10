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

            SetupMenu();
        }

        protected void SetupMenu()
        {
            Menu.ExitItem.Click += ExitItem_Click;
        }

        private void ExitItem_Click(object sender, EventArgs e)
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

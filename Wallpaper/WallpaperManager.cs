using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_wallpaper.Wallpaper
{
    public class WallpaperManager : IDisposable
    {

        public WebWallpaper Wallpaper { get; }

        public WallpaperWindow Window { get; private set; }

        public bool Initalized { get; private set; }
        public WallpaperManager(WebWallpaper wallpaper)
        {
            Wallpaper = wallpaper;
        }

        public void Initalize()
        {
            Window = new WallpaperWindow();
            Initalized = true;
        }

        public void ShowWindow()
        {
            if (!Initalized)
            {
                return;
            }

            Window.Show();
        }

        public void HideWindow()
        {
            if (!Initalized)
            {
                return;
            }

            Window.Hide();
        }

        public void Dispose()
        {
            Window?.Dispose();
            Initalized = false;
        }

    }
}

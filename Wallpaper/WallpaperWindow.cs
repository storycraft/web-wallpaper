using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace web_wallpaper.Wallpaper
{
    public class WallpaperWindow : IDisposable
    {
        public ChromiumWebBrowser Browser { get; }

        public Form Form { get; }

        public WallpaperWindow()
        {
            Form = new Form();

            Browser = new ChromiumWebBrowser("https://www.google.com", null);

            InitalizeWindow();
        }

        protected void InitalizeWindow()
        {
            Browser.Dock = DockStyle.Fill;
            Form.Controls.Add(Browser);

            Form.ShowIcon = false;
            Form.Text = "WebWallpaper - wallpaper";

            Form.FormBorderStyle = FormBorderStyle.None;
        }

        public bool Shown { get; private set; }

        public void Show()
        {
            Form.Show();
            Shown = true;
        }

        public void Hide()
        {
            Form.Hide();
            Shown = false;
        }

        public void FillDisplay()
        {
            Form.DesktopBounds = Screen.PrimaryScreen.Bounds;
        }

        public void ShowDevTools()
        {
            Browser.ShowDevTools();
        }

        public void Dispose()
        {

        }
    }
}

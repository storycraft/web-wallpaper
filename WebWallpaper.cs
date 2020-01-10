using CefSharp;
using CefSharp.WinForms;
using StoryWallpaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using web_wallpaper.Controller;
using web_wallpaper.Threads;
using web_wallpaper.Wallpaper;

namespace web_wallpaper
{
    public class WebWallpaper : IDisposable
    {

        private static CefSettings Settings { get; } = new CefSettings
        {
            PersistUserPreferences = true,
            PersistSessionCookies = true
        };

        public WallpaperController Controller { get; }

        public WallpaperManager WallpaperManager { get; }

        private WallpaperThread renderThread;
        private InputThread inputThread;
        public WebWallpaper()
        {
            Controller = new WallpaperController(this);
            WallpaperManager = new WallpaperManager(this);

            renderThread = new WallpaperThread(RenderTask);
            inputThread = new InputThread(InputTask);
        }

        public bool Started { get; private set; }

        public void Start()
        {
            if (Started)
            {
                throw new Exception("WebWallpaper already started");
            }
            Started = true;

            renderThread.Start();
            inputThread.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Controller.ShowTrayIcon();

            Application.Run();
        }

        public void Stop()
        {
            if (!Started)
            {
                throw new Exception("WebWallpaper is not started");
            }
            Started = false;

            renderThread.Stop();
            inputThread.Stop();

            Application.Exit();
        }

        private IntPtr wallpaperHandle = IntPtr.Zero;

        protected void RenderTask()
        {
            try
            {
                Cef.EnableHighDPISupport();

                Application.SetCompatibleTextRenderingDefault(false);

                Cef.Initialize(Settings);
                WallpaperManager.Initalize();

                WallpaperManager.ShowWindow();

                DesktopTool.AppendToWallpaperArea((wallpaperHandle = WallpaperManager.Window.Form.Handle));

                WallpaperManager.Window.FillDisplay();

                Application.Run();

                OnRenderQuit();
            } catch (ThreadAbortException)
            {
                OnRenderQuit();
            }
        }

        protected void OnRenderQuit()
        {
            if (WallpaperManager.Initalized)
            {
                Cef.Shutdown();
            }

            if (wallpaperHandle != IntPtr.Zero)
            {
                DesktopTool.RemoveFromWallpaperArea(wallpaperHandle);
                DesktopTool.UpdateWallpaper();

                wallpaperHandle = IntPtr.Zero;
            }

            Application.Exit();
        }

        protected void InputTask()
        {

        }

        public void Dispose()
        {
            WallpaperManager.Dispose();
        }

    }
}

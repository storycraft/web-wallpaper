using CefSharp;
using CefSharp.WinForms;
using StoryWallpaper;
using StoryWallpaper.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using web_wallpaper.Controller;
using web_wallpaper.Input;
using web_wallpaper.Log;
using web_wallpaper.Threads;
using web_wallpaper.Util;
using web_wallpaper.Wallpaper;

namespace web_wallpaper
{
    public class WebWallpaper : IDisposable
    {

        private static CefSettings Settings { get; } = new CefSettings
        {
            PersistUserPreferences = true,
            PersistSessionCookies = true,
        };

        static WebWallpaper()
        {
            Settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");
        }

        public WallpaperController Controller { get; }

        public WallpaperManager WallpaperManager { get; }

        private WallpaperThread renderThread;
        private InputThread inputThread;

        public IInputHandler Input { get; }
        public WebWallpaper()
        {
            Controller = new WallpaperController(this);
            WallpaperManager = new WallpaperManager(this);
            Input = WallpaperManager;

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

            Logger.Log("Webwallpaper started");

            Application.Run();
        }

        public void Stop()
        {
            if (!Started)
            {
                throw new Exception("WebWallpaper is not started");
            }
            Logger.Log("Stopping...");
            
            renderThread.Stop();
            inputThread.Stop();
            renderThread.Wait();

            Controller.HideTrayIcon();

            Application.Exit();

            Started = false;
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

                Logger.Log("Render task started");

                Application.Run();
            } catch (ThreadAbortException)
            {
                OnRenderQuit();
                Logger.Log("Render task finished");
            }
        }

        protected void OnRenderQuit()
        {
            if (WallpaperManager.Initalized)
            {
                Cef.Shutdown();
            }

            WallpaperManager.HideWindow();

            if (wallpaperHandle != IntPtr.Zero)
            {
                DesktopTool.RemoveFromWallpaperArea(wallpaperHandle);
                DesktopTool.UpdateWallpaper();

                wallpaperHandle = IntPtr.Zero;
            }

            Application.Exit();
        }

        private uint mouseHook = 0;
        private uint keyboardHook = 0;

        protected void InputTask()
        {
            try
            {
                mouseHook = Win32Util.SetWindowsHookEx(Win32Util.HookType.WH_MOUSE_LL, OnMouseInput, IntPtr.Zero, 0);
                keyboardHook = Win32Util.SetWindowsHookEx(Win32Util.HookType.WH_KEYBOARD_LL, OnKeyboardInput, IntPtr.Zero, 0);

                Logger.Log("Input task started");

                Application.Run();
            }
            catch (ThreadAbortException)
            {
                OnInputQuit();

                Logger.Log("Input task finished");
            }

        }

        private uint OnMouseInput(uint code, IntPtr wParam, IntPtr lParam)
        {
            if (code != 0)
            {
                return Win32Util.CallNextHookEx(mouseHook, code, wParam, lParam);
            }

            Input.OnWinMouseInput(code, wParam, lParam);

            return Win32Util.CallNextHookEx(mouseHook, code, wParam, lParam);
        }

        private uint OnKeyboardInput(uint code, IntPtr wParam, IntPtr lParam)
        {
            if (code != 0)
            {
                return Win32Util.CallNextHookEx(keyboardHook, code, wParam, lParam);
            }

            Input.OnWinKeyboardInput(code, wParam, lParam);

            return Win32Util.CallNextHookEx(keyboardHook, code, wParam, lParam);
        }

        protected void OnInputQuit()
        {
            if (mouseHook != 0)
            {
                Win32Util.UnhookWindowsHookEx(mouseHook);
                mouseHook = 0;
            }

            if (keyboardHook != 0)
            {
                Win32Util.UnhookWindowsHookEx(keyboardHook);
                keyboardHook = 0;
            }
        }

        public void Dispose()
        {
            WallpaperManager.Dispose();
        }

    }
}

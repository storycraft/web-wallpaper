using CefSharp.WinForms;
using StoryWallpaper.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using web_wallpaper.Input;
using web_wallpaper.Util;

namespace web_wallpaper.Wallpaper
{
    public class WallpaperManager : IDisposable, IInputHandler
    {

        public WebWallpaper Wallpaper { get; }
        
        public WallpaperWindow Window { get; private set; }
        public IntPtr WindowHandle { get; private set; }

        public bool Initalized { get; private set; }

        public bool MouseMovementEnabled { get; set; }
        public bool MouseInteractionEnabled { get; set; }
        public WallpaperManager(WebWallpaper wallpaper)
        {
            Wallpaper = wallpaper;

            MouseMovementEnabled = true;
            MouseInteractionEnabled = false;
        }

        public void Initalize()
        {
            Window = new WallpaperWindow();
            WindowHandle = Window.Form.Handle;
            Initalized = true;
        }

        public string URL
        {
            get
            {
                return Window.Browser.Address;
            }

            set
            {
                Window.Browser.Load(value);
            }
        }

        public void ShowWindow()
        {
            if (!Initalized)
                return;

            Window.Show();
        }

        public void HideWindow()
        {
            if (!Initalized)
                return;

            Window.Hide();
        }

        public void Dispose()
        {
            Window?.Dispose();
            Initalized = false;
        }

        public void OnWinMouseInput(uint code, IntPtr wParam, IntPtr lParam)
        {
            if (!Initalized || !(MouseInteractionEnabled || MouseMovementEnabled))
                return;

            Win32Util.MouseHookStructEx inputEx = Marshal.PtrToStructure<Win32Util.MouseHookStructEx>(lParam);
            Win32Util.MOUSEHOOKSTRUCT input = inputEx.mouseHookStruct;
            uint type = (uint) wParam.ToInt32();

            Window.Browser.Invoke(new Action(() => {
                IntPtr handle = Win32Util.GetForegroundWindow();

                if (!HandleUtil.DesktopAreaHandle.Equals(handle))
                    return;

                CefSharp.IBrowserHost host = Window.Browser.GetBrowser().GetHost();

                if (MouseMovementEnabled)
                {
                    switch (type)
                    {
                        case Win32Util.WM_MOUSEMOVE:
                            host.SendMouseMoveEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), false);
                            break;

                        default: break;
                    }
                }

                if (MouseInteractionEnabled)
                {
                    switch (type)
                    {
                        case Win32Util.WM_LBUTTONDOWN:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Left, false, 1);
                            break;

                        case Win32Util.WM_LBUTTONUP:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Left, true, 1);
                            break;

                        case Win32Util.WM_MBUTTONDOWN:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Middle, false, 1);
                            break;

                        case Win32Util.WM_MBUTTONUP:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Middle, true, 1);
                            break;

                        case Win32Util.WM_RBUTTONDOWN:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Right, false, 1);
                            break;

                        case Win32Util.WM_RBUTTONUP:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Right, true, 1);
                            break;

                        case Win32Util.WM_MOUSEWHEEL:
                            int deltaY = (int)((inputEx.MouseData & 0xffff0000) >> 4);
                            host.SendMouseWheelEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), 0, deltaY);
                            break;

                        default: break;
                    }
                }

                
            }));
        }

        public void OnWinKeyboardInput(uint code, IntPtr wParam, IntPtr lParam)
        {
            if (!Initalized)
                return;


        }
    }
}

using CefSharp;
using CefSharp.WinForms;
using StoryWallpaper;
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
    public class WallpaperManager : IDisposable, IInputHandler, ILifeSpanHandler
    {

        public WebWallpaper Wallpaper { get; }
        
        public WallpaperWindow Window { get; private set; }
        public IntPtr WindowHandle { get; private set; }

        public bool Initalized { get; private set; }

        public bool MouseMovementEnabled { get; set; }
        public bool MouseInteractionEnabled { get; set; }

        public bool KeyboardEnabled { get; set; }

        public bool PopupRedirect { get; set; }

        public bool HandlerEnabled { get; set; }

        public bool RenderingEnabled
        {
            get
            {
                if (Window?.Browser != null)
                    return Window.Browser.Visible;

                return false;
            }

            set
            {
                if (Window.Form.InvokeRequired)
                {
                    Window.Form.Invoke(new Action(() => Window.Form.Visible = value));
                }
                else
                {
                    Window.Form.Visible = value;
                }
                
                Window.Browser.GetBrowserHost()?.WasHidden(value);

                if (!value)
                {
                    DesktopTool.UpdateWallpaper();
                }
            }
        }

        public WallpaperManager(WebWallpaper wallpaper)
        {
            Wallpaper = wallpaper;

            MouseMovementEnabled = true;
            MouseInteractionEnabled = false;
            KeyboardEnabled = false;
            PopupRedirect = true;
            HandlerEnabled = false;
        }

        public void Initalize()
        {
            Window = new WallpaperWindow();
            WindowHandle = Window.Form.Handle;
            Initalized = true;

            Window.Browser.LifeSpanHandler = this;
            Window.Browser.FocusHandler = null;

            Window.Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
        }

        protected void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var host = Window.Browser.GetBrowserHost();

            host.SetZoomLevel(0.0);
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
        }

        public void OnWinMouseInput(uint code, IntPtr wParam, IntPtr lParam)
        {
            if (!Initalized || !(MouseInteractionEnabled || MouseMovementEnabled))
                return;

            Win32Util.MOUSEHOOKSTRUCT input = Marshal.PtrToStructure<Win32Util.MOUSEHOOKSTRUCT>(lParam);
            uint type = (uint) wParam.ToInt32();

            if (Window.Browser.IsDisposed)
            {
                return;
            }

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
                            host.SendFocusEvent(true);
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Left, false, 1);
                            break;

                        case Win32Util.WM_LBUTTONUP:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Left, true, 1);
                            break;

                        case Win32Util.WM_MBUTTONDOWN:
                            host.SendFocusEvent(true);
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Middle, false, 1);
                            break;

                        case Win32Util.WM_MBUTTONUP:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Middle, true, 1);
                            break;

                        case Win32Util.WM_RBUTTONDOWN:
                            host.SendFocusEvent(true);
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Right, false, 1);
                            break;

                        case Win32Util.WM_RBUTTONUP:
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Right, true, 1);
                            break;

                        case Win32Util.WM_MOUSEWHEEL:
                            int deltaY = input.mouseData >> 16;
                            host.SendMouseWheelEvent(new CefSharp.MouseEvent(input.pt.X, input.pt.Y, CefSharp.CefEventFlags.None), 0, deltaY);
                            break;

                        default: break;
                    }
                }

                
            }));
        }

        public void OnWinKeyboardInput(uint code, IntPtr wParam, IntPtr lParam)
        {
            if (!Initalized || !KeyboardEnabled)
                return;

            int keyCode = Marshal.ReadInt32(lParam);
            uint type = (uint) wParam.ToInt32();

            if (Window.Browser.IsDisposed)
            {
                return;
            }

            Window.Browser.Invoke(new Action(() => {
                IntPtr handle = Win32Util.GetForegroundWindow();

                if (!HandleUtil.DesktopAreaHandle.Equals(handle))
                    return;

                CefSharp.IBrowserHost host = Window.Browser.GetBrowser().GetHost();

                switch (type)
                {
                    case Win32Util.WM_KEYDOWN:
                        host.SendKeyEvent(new KeyEvent()
                        {
                            WindowsKeyCode = keyCode,
                            FocusOnEditableField = true,
                            Type = KeyEventType.KeyDown,
                            IsSystemKey = false
                        });
                        break;

                    case Win32Util.WM_KEYUP:
                        host.SendKeyEvent(new KeyEvent()
                        {
                            WindowsKeyCode = keyCode,
                            FocusOnEditableField = true,
                            Type = KeyEventType.KeyUp,
                            IsSystemKey = false
                        });
                        break;

                    default: break;
                }
            }));
        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            if (PopupRedirect)
            {
                Window.Browser.Load(targetUrl);

                newBrowser = null;

                return true;
            }

            ChromiumWebBrowser newB = new ChromiumWebBrowser();

            newB.SetAsPopup();

            newBrowser = newB;

            return false;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }
    }
}

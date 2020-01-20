using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_wallpaper.Input
{
    public interface IInputHandler
    {
        void OnWinMouseInput(uint code, IntPtr wParam, IntPtr lParam);

        void OnWinKeyboardInput(uint code, IntPtr wParam, IntPtr lParam);
    }
}

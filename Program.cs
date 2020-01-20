using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace web_wallpaper
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Console.WriteLine("Webwallpaper by storycraft");
            Console.WriteLine("==========================");

            using (var program = new WebWallpaper())
            {
                program.Start();
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_wallpaper.Log
{
    public static class Logger
    {
        public static string Prefix
        {
            get
            {
                return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            }
        }

        public static void Log(string message)
        {
            Console.WriteLine(Prefix + " [log] " + message);
        }

        public static void Warn(string message)
        {
            Console.WriteLine(Prefix + " [warning] " + message);
        }

        public static void Error(string message)
        {
            Console.WriteLine(Prefix + " [ERR] " + message);
        }

    }
}

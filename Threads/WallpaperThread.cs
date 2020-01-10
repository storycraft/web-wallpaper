using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace web_wallpaper.Threads
{
    public class WallpaperThread : StdThread
    {
        public WallpaperThread(ThreadStart task) : base(task)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace web_wallpaper.Threads
{
    public class InputThread : StdThread
    {
        public InputThread(ThreadStart task) : base(task)
        {
        }

    }
}

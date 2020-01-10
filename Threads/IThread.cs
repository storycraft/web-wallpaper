using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_wallpaper.Threads
{
    public interface IThread
    {

        void Start();
        void Stop();
        void Wait();

        bool Started { get; }

        int Id { get; }

    }
}

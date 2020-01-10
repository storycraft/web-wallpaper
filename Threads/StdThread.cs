using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace web_wallpaper.Threads
{
    public class StdThread : IThread
    {
        private readonly Thread thread;

        public delegate void StdThreadEventArgs();

        public event StdThreadEventArgs Starting;
        public event StdThreadEventArgs Stopping;

        public StdThread(ThreadStart task)
        {
            thread = new Thread(task);
        }

        public void Start()
        {
            if (Started)
            {
                Stop();
            }

            Starting?.Invoke();

            thread.Start();
        }

        public void Stop()
        {
            if (!Started)
            {
                return;
            }

            Stopping?.Invoke();

            thread.Abort();
        }

        public void Wait()
        {
            thread.Join();
        }

        public bool Started
        {
            get
            {
                return thread.IsAlive;
            }
        }

        public int Id
        {
            get
            {
                return thread.ManagedThreadId;
            }
        }

    }
}

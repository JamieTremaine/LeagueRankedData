using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueTierLevels
{
    class ApiRateLimiter
    {
        private int m_Time;
        private System.Timers.Timer m_Timer;
        private readonly Semaphore m_ApiAccess = new Semaphore(0, 1);
        private bool m_paused;

        public ApiRateLimiter(ApiType type)
        {
            switch (type)
            {
                case ApiType.DEFAULT:
                    Personal();
                    break;
                case ApiType.PERSONAL:
                    Personal();
                    break;
                case ApiType.DEVELOPMENT:
                    Development();
                    break;
                case ApiType.PRODUCTION:
                    Production();
                    break;
                default:
                    break;
            }

            this.m_ApiAccess.Release();
            this.m_Timer.Elapsed += Unlock;
            this.m_paused = false;
        }

        public void Unlock(Object source, System.Timers.ElapsedEventArgs e)
        {
            m_ApiAccess.Release();
            m_Timer.Stop();
        }

        public void ApiCalled()
        {          
            m_Timer.Start();
        }

        public void TakeSemaphore()
        {
            m_ApiAccess.WaitOne();
        }
        public bool TakeSemaphore(int timeout)
        {
            return m_ApiAccess.WaitOne(timeout);
        }

        public void Pause(int duration)
        {
            TakeSemaphore(0); // try take semaphore if free
            Thread.Sleep(duration * 100);  //the thread that has the semaphore will sleep causing all api calls to block
        }


        private void Personal()
        {
            /* Just hard coding in timings
             * If anyone knows where to get rate info dynamically let me know c:
             * Current personal limit: 20 every second
             *                          100 every 2 minutes 
             * (60 * 2) / 100 * 1200
             */
            this.m_Time = 1200;
            this.m_Timer = new System.Timers.Timer(1200);
        }

        private void Development()
        {
            /* Doesnt actually say on https://developer.riotgames.com/docs/portal
             * rate limits for development keys. So im just assuming it's the same
             * as personal keys.            
             * Current development limit: 20 every second
             *                            100 every 2 minutes 
             */
            this.m_Time = 1200;
            this.m_Timer = new System.Timers.Timer(1200);
        }

        private void Production()
        {
            /* Current development limit: 500 every 10 second
             *                            30,000 every 10 minutes 
             */
            this.m_Time = 20;
            this.m_Timer = new System.Timers.Timer(20);
        }
    }
}

using System.Diagnostics;
using System.Threading;
using System;
 
public class Clock
{
        public bool QuitRequested;
        public delegate void TickHandler();
        public event TickHandler Tick;
 
        public float DeltaTime;
        public int DeltaTimeMili;
        public int LogRate;
        public bool Log;
 
        private AutoResetEvent timerEvent = new AutoResetEvent(true);
 
        public Clock(float ticksPerSecond, bool log, int logRate)
        {
            DeltaTime = 1 / ticksPerSecond;
            DeltaTimeMili = (int)(DeltaTime * 1000);
            LogRate = logRate;
            Log = log;
            Thread gameBackgroundThread = new Thread(StartBackgroundloop);
            gameBackgroundThread.Priority = ThreadPriority.AboveNormal;
            gameBackgroundThread.IsBackground = true;
            gameBackgroundThread.Start();
        }
 
        public void StartBackgroundloop()
        {
 
 
            double nextTick = DeltaTimeMili;
            int count = 0;
            double frameStart;
            double totalFrameTime = 0;
            double maxFrameTime = 0;
 
            Stopwatch timer = Stopwatch.StartNew();
            timer.Start();
 
            while (!QuitRequested)
            {
                frameStart = timer.Elapsed.TotalMilliseconds;
                Tick?.Invoke();
                totalFrameTime += timer.Elapsed.TotalMilliseconds - frameStart;
                maxFrameTime = Math.Max(maxFrameTime, timer.Elapsed.TotalMilliseconds - frameStart);
                if (timer.Elapsed.TotalMilliseconds < nextTick)
                {
                    timerEvent.WaitOne((int)(nextTick - timer.Elapsed.TotalMilliseconds));
                }
                nextTick += DeltaTimeMili;
 
 
                //framerate displayer
                if (Log)
                {
                    count++;
                    if (count % LogRate == 0)
                    {
                        count = 0;
                        Console.WriteLine("*-----------------------*\n"+timer.Elapsed.TotalMilliseconds.ToString("0.000") + " => Logging performance\n\t"+ totalFrameTime.ToString("0.000") + " => Cumultative delta times since last log\n\t" + maxFrameTime.ToString("0.000") + " => Max delta time since last log"+ "\n*-----------------------*");
                        totalFrameTime = 0;
                        maxFrameTime = 0;
                    }
 
                }
            }
        }
    }

using System;
using System.Diagnostics;
using System.Threading;

namespace CSGL.libGame.loop
{
    public abstract class Fixed : ILoop
    {
        private long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }
        public static long MILLI = 1000;        // 10^3
        public static long MICRO = 1000000;     // 10^6
        public static long NANO = 1000000000;	// 10^9


        protected int TARGET_FPS = 60;
        protected double GAME_HERTZ = 20.0;

        protected const int MAX_UPDATES_BEFORE_RENDER = 5;
        protected double lastUpdateTime;
        protected double lastRenderTime;

        protected bool running = true;
        protected bool paused = false;

        /*
	     * Set the target renders per second.
	     */
        public void setTargetFPS(int fps)
        {
            TARGET_FPS = fps;
        }

        /*
	     * Set the target updates per second.
	     */
        public void setTargetHz(int hertz)
        {
            GAME_HERTZ = hertz;
        }

        public void pause(bool pause)
        {
            this.paused = pause;
        }

        public void stop()
        {
            running = false;
        }

        public void start()
        {
            init();
            
            double TIME_BETWEEN_RENDERS = NANO / TARGET_FPS;
            double TIME_BETWEEN_UPDATES = NANO / GAME_HERTZ;
            double OPTIMAL_TIMING = TIME_BETWEEN_UPDATES * GAME_HERTZ;
            lastUpdateTime = nanoTime();
            lastRenderTime = nanoTime();

            while (running)
            {
                double current = nanoTime();
                int updateCount = 0;

                long deltaTime = (long)(current - lastUpdateTime);
                while (current - lastUpdateTime > TIME_BETWEEN_UPDATES && updateCount < MAX_UPDATES_BEFORE_RENDER)
                {
                    double timing = deltaTime / OPTIMAL_TIMING;
                    if (!paused) update(timing);
                    lastUpdateTime += TIME_BETWEEN_UPDATES;
                    updateCount++;
                }

                if (current - lastUpdateTime > TIME_BETWEEN_UPDATES)
                    lastUpdateTime = current - TIME_BETWEEN_UPDATES;

                double interpolation = Math.Min(1.0d, (current - lastUpdateTime) / TIME_BETWEEN_UPDATES);
                if (!paused) render(interpolation);

                lastRenderTime = current;
                while (current - lastRenderTime < TIME_BETWEEN_RENDERS && current - lastUpdateTime < TIME_BETWEEN_UPDATES)
                {
                    Thread.Yield();
                    current = nanoTime();
                }

            }

            cleanUp();
        }

        public abstract void init();
        public abstract void cleanUp();
        public abstract void update(double time);
        public abstract void render(double interpolation);
    }
}

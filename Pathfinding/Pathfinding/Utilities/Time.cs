using Microsoft.Xna.Framework;

namespace MonogameLearning.Utilities
{
    class Time
    {
        private static Time instance;
        private float deltaTime, currentTime, prevTime;

        private Time()
        {
            currentTime = 0;
            prevTime = 0;
            deltaTime = 0;
        }

        public static Time Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Time();
                }
                return instance;
            }
        }

        public float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }

        public float TotalTime
        {
            get
            {
                return currentTime;
            }
        }

        public void tick(ref GameTime time)
        {
            currentTime = (float)time.TotalGameTime.TotalMilliseconds / 1000f;
            deltaTime = currentTime - prevTime;
            prevTime = currentTime;
        }
    }
}

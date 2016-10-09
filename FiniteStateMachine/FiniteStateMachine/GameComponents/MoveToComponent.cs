using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using MonogameLearning.Utilities;

namespace MonogameLearning.GameComponents
{
    class MoveToComponent : Component
    {
        protected bool arrived;
        protected Vector3 destination;
        private float minDistance;
        private float minDistanceSquared;
        protected float speed, currentSpeed;

        public MoveToComponent() : base()
        {
            minDistance = 1;
            minDistanceSquared = 1;
            destination = Vector3.Zero;
            speed = 1;
            arrived = true;
            currentSpeed = 0;
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = value;
            }
        }

        public float CurrentSpeed
        {
            get { return currentSpeed; }
        }

        public float MinimumDistance
        {
            get { return minDistance; }
            set
            {
                minDistance = value;
                minDistanceSquared = value * value;
            }
        }

        public float MinDistSquared
        {
            get { return minDistanceSquared; }
        }

        public virtual Vector3 Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                arrived = false;
                owner.transform.lookAt(destination);
                currentSpeed = speed;
            }
        }

        public bool Arrived
        {
            get { return arrived; }
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(!arrived)
            {
                move();
                checkForArrival();
            }
        }

        public void abortMovement()
        {
            arrived = true;
        }

        protected virtual void move()
        {
            owner.transform.Translate(owner.transform.Forward * currentSpeed * Time.DeltaTime);
        }

        protected void checkForArrival()
        {
            Vector3 offset = destination - owner.transform.Position;
            if (offset.LengthSquared() < minDistanceSquared)
            {
                arrived = true;
                currentSpeed = 0;
            }
        }
    }
}

using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Utilities;

namespace MonogameLearning.GameComponents
{
    class MoveToComponent : Component
    {
        
        //public bool snapRotation = true;
        protected bool arrived;
        protected Vector3 destination;
        private float minDistance;
        private float minDistanceSquared;
        protected float speed, currentSpeed;
        /*
        private Quaternion startRot, endRot;
        private float steerDuration = 2;
        private float rotTimer;*/

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
                /*
                //need to rotate to look at destination
                if(snapRotation)
                    owner.transform.lookAt(destination);
                else
                {
                    startRot = owner.transform.Rotation;
                    Vector3 newDir = destination - owner.transform.Position;
                    newDir.Normalize();
                    endRot = startRot.LookRotation(Vector3.Forward, newDir, Vector3.Up);
                    rotTimer = 0;
                }*/
                //then that allows it to simply move forward
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
                
                checkDistance();
            }
        }

        protected virtual void move()
        {
            owner.transform.Translate(owner.transform.Forward * currentSpeed * Time.Instance.DeltaTime);
        }

        private void checkDistance()
        {
            Vector3 offset = destination - owner.transform.Position;
            if (offset.LengthSquared() < minDistanceSquared)
            {
                arrived = true;
                currentSpeed = 0;
            }
        }

        /*private void steerToDestination()
        {
            /*if(rotTimer < 1)
            {
                rotTimer += Time.Instance.DeltaTime/steerDuration;
                //owner.transform.Rotation = Quaternion.Slerp(startRot, endRot, rotTimer);
                owner.transform.Rotation = Quaternion.Lerp(owner.transform.Rotation, calculateLookRotation(), 0.01f);
            }
            owner.transform.Rotation = Quaternion.Lerp(owner.transform.Rotation, calculateLookRotation(), Time.Instance.DeltaTime);
        }

        private Quaternion calculateLookRotation()
        {
            Vector3 newDir = destination - owner.transform.Position;
            newDir.Normalize();
            return startRot.LookRotation(Vector3.Forward, newDir, Vector3.Up);
        }*/
    }
}

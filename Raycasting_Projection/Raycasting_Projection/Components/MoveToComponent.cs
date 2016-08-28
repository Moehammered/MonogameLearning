using System;
using Microsoft.Xna.Framework;
using MovingTextDemo;

namespace Raycasting_Projection.Components
{
    class MoveToComponent : Component
    {
        public float speed;

        private float minDistance;
        private float minDistanceSquared;
        private Vector3 destination;
        private bool arrived;

        public MoveToComponent() : base()
        {
            minDistance = 1;
            minDistanceSquared = 1;
            destination = Vector3.Zero;
            speed = 1;
            arrived = true;
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

        public Vector3 Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                arrived = false;
                //need to rotate to look at destination
                owner.transform.lookAt(destination);
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
                //first move
                owner.transform.Translate(owner.transform.Forward * speed * Time.Instance.DeltaTime);
                checkDistance();
            }
        }

        private void checkDistance()
        {
            Vector3 offset = destination - owner.transform.Position;
            if(offset.LengthSquared() < minDistanceSquared)
                arrived = true;
        }
    }
}

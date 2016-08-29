using System;
using Microsoft.Xna.Framework;
using MovingTextDemo;
using Raycasting_Projection.Utilities;

namespace Raycasting_Projection.Components
{
    class MoveToComponent : Component
    {
        public float speed;
        public bool snapRotation = true;

        private float minDistance;
        private float minDistanceSquared;
        private Vector3 destination;
        private bool arrived;
        private Quaternion startRot, endRot;
        private float steerDuration = 2;
        private float rotTimer;

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
                if(snapRotation)
                    owner.transform.lookAt(destination);
                else
                {
                    startRot = owner.transform.Rotation;
                    Vector3 newDir = destination - owner.transform.Position;
                    newDir.Normalize();
                    endRot = startRot.LookRotation(Vector3.Forward, newDir, Vector3.Up);
                    rotTimer = 0;
                }
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
                //then if we aren't snapping rotation, steer towards destination
                if (!snapRotation)
                    steerToDestination();
                //move
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

        private void steerToDestination()
        {
            /*if(rotTimer < 1)
            {
                rotTimer += Time.Instance.DeltaTime/steerDuration;
                //owner.transform.Rotation = Quaternion.Slerp(startRot, endRot, rotTimer);
                owner.transform.Rotation = Quaternion.Lerp(owner.transform.Rotation, calculateLookRotation(), 0.01f);
            }*/
            owner.transform.Rotation = Quaternion.Lerp(owner.transform.Rotation, calculateLookRotation(), 0.025f);
        }

        private Quaternion calculateLookRotation()
        {
            Vector3 newDir = destination - owner.transform.Position;
            newDir.Normalize();
            return startRot.LookRotation(Vector3.Forward, newDir, Vector3.Up);
        }
    }
}

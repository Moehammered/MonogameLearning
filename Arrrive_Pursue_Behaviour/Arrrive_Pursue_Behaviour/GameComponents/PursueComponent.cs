using MonogameLearning.BaseComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonogameLearning.GameComponents;
using MonogameLearning.Utilities;

namespace Arrrive_Pursue_Behaviour.GameComponents
{
    class PursueComponent : Component
    {
        public float speed;
        
        private GameObject pursueTarget;
        private MoveToComponent mover;
        private float refreshRate, refreshTimer;
        private float stopDistance, stopDistSqr;
        private Vector3 destination;

        public PursueComponent() : base()
        {
            refreshRate = 0.5f;
            stopDistance = 1;
            stopDistSqr = 1;
        }

        public float RefreshRate
        {
            get { return refreshRate; }
            set
            {
                refreshRate = (value > 0) ? value : 0;
            }
        }

        public float StopDistance
        {
            get { return stopDistance; }
            set
            {
                stopDistance = value;
                stopDistSqr = stopDistance * stopDistance;
            }
        }

        public float StopDistanceSquared
        {
            get { return stopDistSqr; }
        }

        public GameObject Target
        {
            get { return pursueTarget; }
            set
            {
                pursueTarget = value;
                if(pursueTarget != null)
                {
                    mover = pursueTarget.GetComponent<MoveToComponent>();
                    if(mover != null)
                    {
                        getTargetTrajectory();
                    }
                }
            }
        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if(mover != null)
            {
                refreshTimer -= Time.Instance.DeltaTime;
                if (refreshTimer < 0)
                {
                    getTargetTrajectory();

                    refreshTimer = refreshRate;
                }
                if (!hasArrived())
                {
                    //check if we need to refresh trajectory
                    
                    //move towards target location
                    performSteerRotation();
                    owner.transform.Translate(owner.transform.Forward * speed * Time.Instance.DeltaTime);
                }
            }
        }

        private void getTargetTrajectory()
        {
            Vector3 targetDir = mover.Owner.transform.Forward;
            float targetSpeed = mover.CurrentSpeed;
            destination = mover.Owner.transform.Position + targetDir * targetSpeed;
            //Console.WriteLine("Curr Dest: " + destination);
        }

        private bool hasArrived()
        {
            return (destination - owner.transform.Position).LengthSquared() < stopDistSqr;
        }

        private void performSteerRotation()
        {
            Vector3 newDir = destination - owner.transform.Position;
            newDir.Normalize();
            Quaternion desiredRot, currentRot;
            currentRot = owner.transform.Rotation;
            desiredRot = Quaternion.Identity.LookRotation(Vector3.Forward, newDir, Vector3.Up);
            owner.transform.Rotation = Quaternion.Lerp(currentRot, desiredRot, Time.Instance.DeltaTime);
        }

        private void OnCollision()
        {
            Console.WriteLine("Pursue Collided");
        }

        private void OnCollisionExit()
        {
            Console.WriteLine("Pursue not colliding!");
        }
    }
}

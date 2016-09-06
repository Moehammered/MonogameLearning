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
    class PursueComponent : ArriveAtComponent
    {
        private GameObject pursueTarget;
        private MoveToComponent mover;
        private float refreshRate, refreshTimer, pursueCooldown;

        public PursueComponent() : base()
        {
            refreshRate = 0.5f;
            refreshTimer = refreshRate;
            steerDuration = 0.5f;
        }

        public float RefreshRate
        {
            get { return refreshRate; }
            set
            {
                refreshRate = (value > 0) ? value : 0;
            }
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

        protected override void move()
        {
            if (mover != null)
            {
                refreshTimer -= Time.Instance.DeltaTime;
                if (refreshTimer < 0)
                {
                    getTargetTrajectory();

                    refreshTimer = refreshRate;
                }
                base.move();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(!arrived)
            {
                move();
            }
            else if(pursueCooldown > 0)
            {
                pursueCooldown -= Time.Instance.DeltaTime;
                if(pursueCooldown < 0)
                {
                    arrived = false;
                    currentSpeed = speed;
                }
            }
        }

        private void getTargetTrajectory()
        {
            Vector3 targetDir = mover.Owner.transform.Forward;
            float targetSpeed = mover.CurrentSpeed;
            Vector3 gradient = mover.Owner.transform.Position - owner.transform.Position;
            float timeToTarget = gradient.Length() / (currentSpeed + 0.01f); //+0.01f so there is no divide by 0 when pursuer stops
            Destination = mover.Owner.transform.Position + (targetDir * targetSpeed) * timeToTarget;
        }

        private void OnCollision()
        {
            //Console.WriteLine("Pursue Collided");
            currentSpeed = 0;
            arrived = true;
        }

        private void OnCollisionExit()
        {
            //Console.WriteLine("Pursue not colliding!");
            //start a cooldown to let the pursuer chase again
            pursueCooldown = 1;
        }
    }
}

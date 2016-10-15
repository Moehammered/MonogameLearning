using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.GameComponents;
using System;

namespace MazeEscape.GameComponents
{
    class Hazard : Component
    {
        public GameObject player;
        private float wakeupDistance, wakeupSqr;
        private PursueComponent purComp;
        private FirstPersonController playerControl;

        public Hazard() : base()
        {
            WakeUpDistance = 6;
        }

        public float WakeUpDistance
        {
            get { return wakeupDistance; }
            set
            {
                wakeupDistance = value;
                wakeupSqr = value * value;
            }
        }

        public override void Initialize()
        {
            purComp = owner.AddComponent<PursueComponent>();
            purComp.arrivalSpeed = 0.1f;
            purComp.Speed = 0.8f;
            purComp.steerDuration = 0.2f;
            if(player != null)
                playerControl = player.GetComponent<FirstPersonController>();
        }

        public override void Update(GameTime gameTime)
        {
            if (playerControl != null)
            {
                startPursuit();
               /* if (playerControl.hitGoal)
                    purComp.Target = null;*/
            }
            else
            {
                purComp.Target = null;
            }
        }

        private void startPursuit()
        {
            if(player != null)
            {
                Vector3 gradient = player.transform.Position - owner.transform.Position;
                if(gradient.LengthSquared() < wakeupSqr)
                    purComp.Target = player;
                else
                    purComp.Target = null;
            }
        }

        public override void Destroy()
        {
        }
    }
}

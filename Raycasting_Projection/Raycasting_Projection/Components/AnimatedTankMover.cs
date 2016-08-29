using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Cameras_and_Primitives;
using Raycasting_Projection.Utilities;
using MovingTextDemo;

namespace Raycasting_Projection.Components
{
    class AnimatedTankMover : Component
    {
        private MoveToComponent mover;
        private AnimatedTank tankModel;
        private Raycast raycaster;

        public AnimatedTankMover() : base()
        {

        }

        public Vector3 Destination
        {
            get { return mover.Destination; }
            set
            {
                mover.Destination = value;
            }
        }

        public override void Initialize()
        {
            mover = owner.GetComponent<MoveToComponent>();
            tankModel = owner.GetComponent<AnimatedTank>();
            raycaster = new Raycast(GameInstance.GraphicsDevice);
            mover.MinimumDistance = 2;
            mover.speed = 4;
            mover.snapRotation = false;
        }

        public override void Update(GameTime gameTime)
        {
            animateTank();
        }
        
        private void animateTank()
        {
            if(!mover.Arrived)
            {
                tankModel.WheelRotation += Time.Instance.DeltaTime * mover.speed;
                tankModel.TurretRotation += Time.Instance.DeltaTime * mover.speed;
            }
        }
    }
}

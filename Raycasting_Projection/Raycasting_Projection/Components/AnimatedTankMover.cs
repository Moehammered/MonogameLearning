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
        public BoundingBox pickingVolume;
        private MoveToComponent mover;
        private AnimatedTank tankModel;
        private Raycast raycaster;
        
        //here for now, once an input class is made will be replaced
        private MouseState prevState;

        public AnimatedTankMover() : base()
        {

        }

        public override void Initialize()
        {
            mover = owner.GetComponent<MoveToComponent>();
            tankModel = owner.GetComponent<AnimatedTank>();
            raycaster = new Raycast(GameInstance.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            checkInput();
            animateTank();

            prevState = Mouse.GetState();
        }

        private void checkInput()
        {
            if(Mouse.GetState().LeftButton == ButtonState.Pressed && prevState.LeftButton != ButtonState.Pressed)
            {
                raycaster.setupMatrices(Camera.mainCamera.World, Camera.mainCamera.View, Camera.mainCamera.Projection);
                RaycastResult info;
                if(raycaster.cast(Mouse.GetState().Position.ToVector2(), pickingVolume, out info))
                {
                    //move the tank
                    mover.Destination = info.contactPoint;
                }
            }
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

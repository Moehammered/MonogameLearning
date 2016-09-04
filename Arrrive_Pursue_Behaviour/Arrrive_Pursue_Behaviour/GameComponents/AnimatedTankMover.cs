using Arrrive_Pursue_Behaviour.GameComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using MonogameLearning.Utilities;

namespace MonogameLearning.GameComponents
{
    class AnimatedTankMover : Component
    {
        private ArriveAtComponent mover;
        private AnimatedTank tankModel;
        private Raycast raycaster;

        public AnimatedTankMover() : base()
        {

        }

        public override void Initialize()
        {
            mover = owner.GetComponent<ArriveAtComponent>();
            tankModel = owner.GetComponent<AnimatedTank>();
            raycaster = new Raycast(GameInstance.GraphicsDevice);
            mover.MinimumDistance = 2;
            mover.Speed = 4;
            mover.steerDuration = 1.5f;
        }

        public override void Update(GameTime gameTime)
        {
            animateTank();
        }
        
        private void animateTank()
        {
            if(!mover.Arrived)
            {
                tankModel.WheelRotation += Time.Instance.DeltaTime * mover.CurrentSpeed;
                tankModel.TurretRotation += Time.Instance.DeltaTime * mover.CurrentSpeed;
            }
        }
    }
}

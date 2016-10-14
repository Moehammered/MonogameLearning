using Microsoft.Xna.Framework;
using MonogameLearning.GameComponents;
using MonogameLearning.Utilities;

namespace MonogameLearning.GameComponents
{
    class ArriveAtComponent : MoveToComponent
    {
        public float arrivalSpeed;
        public float steerDuration;
        private float slowdownRadius;
        private float slowdownRadiusSqr;
        private Quaternion startRot;

        public ArriveAtComponent() : base()
        {
            slowdownRadius = 1;
            slowdownRadiusSqr = 1;
            arrivalSpeed = 1;
            steerDuration = 2;
        }

        public float SlowdownRadius
        {
            get { return slowdownRadius; }
            set
            {
                slowdownRadius = value;
                slowdownRadiusSqr = slowdownRadius * slowdownRadius;
            }
        }

        public float SlowdownRadiusSquared
        {
            get { return slowdownRadiusSqr; }
        }

        public override Vector3 Destination
        {
            get { return base.Destination; }
            set
            {
                destination = value;
                arrived = false;
                currentSpeed = speed;
                startRot = owner.transform.Rotation;
                checkForArrival();
            }
        }

        protected override void move()
        {
            performSteerRotation();
            if (nearDistination())
                currentSpeed = arrivalSpeed;
            else
                currentSpeed = speed;

            owner.transform.Translate(owner.transform.Forward * currentSpeed * Time.DeltaTime);
        }

        private bool nearDistination()
        {
            Vector3 delta = destination - owner.transform.Position;
            return delta.LengthSquared() < slowdownRadiusSqr;
        }

        private void performSteerRotation()
        {
            Vector3 newDir = destination - owner.transform.Position;
            newDir.Normalize();
            Quaternion desiredRot, currentRot;
            currentRot = owner.transform.Rotation;
            desiredRot = Quaternion.Identity.LookRotation(Vector3.Forward, newDir, Vector3.Up);
            owner.transform.Rotation = Quaternion.Lerp(currentRot, desiredRot, Time.DeltaTime/steerDuration);
        }
    }
}

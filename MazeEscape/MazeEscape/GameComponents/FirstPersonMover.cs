using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.Utilities;

namespace MazeEscape.GameComponents
{
    class FirstPersonMover : Component
    {
        public float lookSensitivity, speed, jumpingForce;
        public float pitchFreedom;
        private float yaw, pitch, roll;
        private float jumpVelocity;

        public FirstPersonMover()
        {
            yaw = pitch = roll = 0;
            speed = 1;
            lookSensitivity = 2;
            jumpingForce = 9;
            jumpVelocity = 0;
            pitchFreedom = 60;
        }

        public override void Initialize()
        {

        }

        public void move(Vector3 direction)
        {
            direction.Y = 0; //remove vertical movement for now
            owner.transform.Translate(direction * speed * Time.DeltaTime);
        }

        public void look(float horizontalDelta, float verticalDelta, float tiltDelta)
        {
            yaw += horizontalDelta * lookSensitivity * Time.DeltaTime;
            pitch += verticalDelta * lookSensitivity * Time.DeltaTime;
            roll += tiltDelta * lookSensitivity * Time.DeltaTime;
            pitch = MathHelper.Clamp(pitch, -pitchFreedom, pitchFreedom);
            owner.transform.Rotate(pitch, yaw, roll);
        }

        public void jump()
        {
            if (jumpVelocity <= 0)
                jumpVelocity = jumpingForce;
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}

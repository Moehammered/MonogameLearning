using Microsoft.Xna.Framework;

namespace Raycasting_Projection.Utilities
{
    class Transform
    {
        private Vector3 position, up, forward, right;
        private Quaternion rotation;

        public Transform()
        {
            position = Vector3.Zero;
            up = Vector3.Up;
            forward = Vector3.Forward;
            right = Vector3.Right;
            rotation = Quaternion.Identity;
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                up = Vector3.Transform(Vector3.Up, rotation);
                forward = Vector3.Transform(Vector3.Forward, rotation);
                right = Vector3.Transform(Vector3.Right, rotation);
            }
        }

        public Vector3 Up
        {
            get { return up; }
        }

        public Vector3 Forward
        {
            get { return forward; }
        }

        public Vector3 Right
        {
            get { return right; }
        }

        public void Rotate(Vector3 axis, float angle)
        {
            Rotation = Quaternion.CreateFromAxisAngle(axis, MathHelper.ToRadians(angle));
        }

        public void Rotate(float x, float y, float z)
        {
            Rotation = Quaternion.CreateFromYawPitchRoll(y, x, z);
        }

        public void Translate(float x, float y, float z)
        {
            position.X += x;
            position.Y += y;
            position.Z += z;
        }

        public void Translate(Vector3 delta)
        {
            position += delta;
        }
    }
}

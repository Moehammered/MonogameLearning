using Microsoft.Xna.Framework;

namespace Cameras_and_Primitives
{
    class Camera
    {
        private Vector3 position, up, forward, right;
        private Quaternion rotation;
        //world seems to be becoming redundant - might be needed when transform component is made.
        private Matrix world, view, projection;
        private float FOV, aspect, near, far;

        /// <summary>
        /// Setup a camera to have 45 degree field of view in perspective with widescreen aspect ratio.
        /// Positioned at the centre of the world.
        /// </summary>
        public Camera()
        {
            FOV = MathHelper.ToRadians(45f);
            aspect = 1920f / 1080f; //widescreen aspect ratio
            near = 0.1f;
            far = 1000f;
            position = Vector3.Zero;
            up = Vector3.Up;
            forward = Vector3.Forward;
            right = Vector3.Right;
            rotation = Quaternion.Identity;
            setupMatrices();
        }

        /// <summary>
        /// Setup a camera to have 45 degree field of view in perspective with widescreen aspect ratio.
        /// Specifying the position of the camera
        /// </summary>
        /// <param name="position"></param>
        public Camera(Vector3 position)
        {
            FOV = MathHelper.ToRadians(45f);
            aspect = 1920f / 1080f; //widescreen aspect ratio
            near = 0.1f;
            far = 1000f;
            this.position = position;
            up = Vector3.Up;
            forward = Vector3.Forward;
            right = Vector3.Right;
            rotation = Quaternion.Identity;
            setupMatrices();
        }

        public Matrix World
        {
            get { return world; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                view = Matrix.CreateLookAt(position, position + forward, up);
            }
        }

        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                //updates direction vectors of this camera
                up = Vector3.Transform(Vector3.Up, rotation);
                forward = Vector3.Transform(Vector3.Forward, rotation);
                right = Vector3.Transform(Vector3.Right, rotation);
                //updates the view of the camera now
                view = Matrix.CreateLookAt(position, position + forward, up);
            }
        }

        public Vector3 Forward
        {
            get { return forward; }
        }

        public Vector3 Up
        {
            get { return up; }
        }

        public Vector3 Right
        {
            get { return right; }
        }

        /// <summary>
        /// The field of view of the camera in degrees
        /// </summary>
        public float FieldOfView
        {
            get { return MathHelper.ToDegrees(FOV); }
            set { FOV = MathHelper.ToRadians(value); }
        }

        public float NearPlane
        {
            get { return near; }
        }

        public float FarPlane
        {
            get { return far; }
        }

        public float AspectRatio
        {
            get { return aspect; }
        }

        private void setupMatrices()
        {
            world = Matrix.CreateTranslation(Vector3.Zero);
            view = Matrix.CreateLookAt(position, position + Vector3.Forward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FOV, aspect, near, far);
        }

        /// <summary>
        /// Updates the camera's orientation to be looking at the supplied position.
        /// INCOMPLETE -- Needs to be able to update rotation, and direction vectors
        /// </summary>
        /// <param name="target"></param>
        public void lookAt(Vector3 target)
        {
            view = Matrix.CreateLookAt(position, target, Vector3.Up);
        }

        /// <summary>
        /// Can be called to update the camera's projection matrix. Must be called for changes to FOV, Aspect Ratio, or Near and Far plane to take effect.
        /// </summary>
        public void updateProjection()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(FOV, aspect, near, far);
        }

        public void setAspectRatio(float width, float height)
        {
            aspect = width / height;
        }

        public void setClippingPlanes(float near, float far)
        {
            this.near = near;
            this.far = far;
        }
    }
}

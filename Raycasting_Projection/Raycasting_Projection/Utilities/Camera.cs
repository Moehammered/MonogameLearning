using Microsoft.Xna.Framework;
using Raycasting_Projection.Utilities;

namespace Cameras_and_Primitives
{
    class Camera
    {
        private Transform transform;
        //world seems to be becoming redundant - might be needed when transform component is made.
        private Matrix world, view, projection;
        private float FOV, aspect, near, far;
        private bool viewNeedsUpdate = false;

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
            transform = new Transform();
            
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
            transform = new Transform();
            transform.Position = position;

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
            get { return transform.Position; }
            set
            {
                transform.Position = value;
                viewNeedsUpdate = true;
                //view = Matrix.CreateLookAt(position, position + forward, up);
            }
        }

        public Quaternion Rotation
        {
            get { return transform.Rotation; }
            set
            {
                transform.Rotation = value;
                viewNeedsUpdate = true;
                ////updates the view of the camera now
                //view = Matrix.CreateLookAt(position, position + forward, up);
            }
        }

        public Vector3 Forward
        {
            get { return transform.Forward; }
        }

        public Vector3 Up
        {
            get { return transform.Up; }
        }

        public Vector3 Right
        {
            get { return transform.Right; }
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
            view = Matrix.CreateLookAt(transform.Position, transform.Position + transform.Forward, transform.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FOV, aspect, near, far);
        }

        /// <summary>
        /// Updates the camera's orientation to be looking at the supplied position.
        /// INCOMPLETE -- Needs to be able to update rotation, and direction vectors
        /// </summary>
        /// <param name="target"></param>
        public void lookAt(Vector3 target)
        {
            view = Matrix.CreateLookAt(transform.Position, target, transform.Up);
        }

        /// <summary>
        /// Can be called to update the camera's projection matrix. Must be called for changes to FOV, Aspect Ratio, or Near and Far plane to take effect.
        /// </summary>
        public void updateProjection()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(FOV, aspect, near, far);
        }

        /// <summary>
        /// Will perform any updates that have been queried or required by the camera due to a change in view or position
        /// </summary>
        public void update()
        {
            if(viewNeedsUpdate)
            {
                //updates the view of the camera now
                view = Matrix.CreateLookAt(transform.Position, transform.Position + transform.Forward, transform.Up);
                viewNeedsUpdate = false;
            }
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

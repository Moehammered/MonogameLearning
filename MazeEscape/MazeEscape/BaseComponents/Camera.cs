using System;
using Microsoft.Xna.Framework;

namespace MonogameLearning.BaseComponents
{
    class Camera : Component
    {
        public static Camera mainCamera;
        //world seems to be becoming redundant - might be needed when transform component is made.
        private Matrix world, view, projection;
        private float FOV, aspect, near, far;
        
        /// <summary>
        /// Setup a camera to have 45 degree field of view in perspective with widescreen aspect ratio.
        /// Positioned at the centre of the world.
        /// </summary>
        public Camera() : base()
        {
            FOV = MathHelper.ToRadians(45f);
            aspect = 1920f / 1080f; //widescreen aspect ratio
            near = 0.1f;
            far = 1000f;
        }

        public override void Initialize()
        {
            if (mainCamera == null)
                mainCamera = this;

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
            view = Matrix.CreateLookAt(Owner.transform.Position, 
                Owner.transform.Position + Owner.transform.Forward, 
                Owner.transform.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FOV, aspect, near, far);
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

        public override void Update(GameTime gameTime)
        {
            view = Matrix.CreateLookAt(Owner.transform.Position,
                Owner.transform.Position + Owner.transform.Forward,
                Owner.transform.Up);
        }

        public override void Destroy()
        {
            if (mainCamera == this)
                mainCamera = null;
        }
    }
}

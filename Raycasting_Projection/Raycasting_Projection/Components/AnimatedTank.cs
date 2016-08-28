using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cameras_and_Primitives;

namespace Raycasting_Projection.Components
{
    class AnimatedTank : RenderComponent
    {
        #region Tank Data
        private Model model;
        private ModelBone[] wheelBones;
        private ModelBone[] steerBones;
        private ModelBone[] turretBones;

        private Matrix[] wheelTransforms;
        private Matrix[] steerTransforms;
        private Matrix[] turretTransforms;
        private Matrix[] boneTransforms;

        private const float ORIENTATION_FIX = 180;
        private const int LBWHEEL = 0, RBWHEEL = 1, LFWHEEL = 2, RFWHEEL = 3;
        private const int LSTEER = 0, RSTEER = 1;
        private const int TURRET = 0, CANNON = 1, HATCH = 2;
        #endregion
        #region Bone Rotations
        private float wheelRotationValue;
        private float steerRotationValue;
        private float turretRotationValue;
        private float cannonRotationValue;
        private float hatchRotationValue;
        #endregion
        //used to tell component to update the matrices of the transforms
        private bool updateWheels = false, updateSteer = false, updateTurret = false;

        public float WheelRotation
        {
            get { return wheelRotationValue; }
            set
            {
                wheelRotationValue = value;
                updateWheels = true;
            }
        }

        public float SteerRotation
        {
            get { return steerRotationValue; }
            set
            {
                steerRotationValue = value;
                updateSteer = true;
            }
        }

        public float TurretRotation
        {
            get { return turretRotationValue; }
            set
            {
                turretRotationValue = value;
                updateTurret = true;
            }
        }
        
        public float CannonRotation
        {
            get { return cannonRotationValue; }
            set
            {
                cannonRotationValue = value;
                updateWheels = true;
            }
        }
        
        public float HatchRotation
        {
            get { return hatchRotationValue; }
            set
            {
                hatchRotationValue = value;
                updateWheels = true;
            }
        }

        private void cacheBones()
        {
            //cache the wheel bones
            wheelBones = new ModelBone[4];
            wheelBones[LBWHEEL] = model.Bones["l_back_wheel_geo"];
            wheelBones[RBWHEEL] = model.Bones["r_back_wheel_geo"];
            wheelBones[LFWHEEL] = model.Bones["l_front_wheel_geo"];
            wheelBones[RFWHEEL] = model.Bones["r_front_wheel_geo"];
            //cache the steering bones
            steerBones = new ModelBone[2];
            steerBones[LSTEER] = model.Bones["l_steer_geo"];
            steerBones[RSTEER] = model.Bones["r_steer_geo"];
            //cache the turret bones
            turretBones = new ModelBone[3];
            turretBones[TURRET] = model.Bones["turret_geo"];
            turretBones[CANNON] = model.Bones["canon_geo"];
            turretBones[HATCH] = model.Bones["hatch_geo"];
        }

        private void cacheTransforms()
        {
            wheelTransforms = new Matrix[wheelBones.Length];
            wheelTransforms[LBWHEEL] = wheelBones[LBWHEEL].Transform;
            wheelTransforms[RBWHEEL] = wheelBones[RBWHEEL].Transform;
            wheelTransforms[LFWHEEL] = wheelBones[LFWHEEL].Transform;
            wheelTransforms[RFWHEEL] = wheelBones[RFWHEEL].Transform;

            steerTransforms = new Matrix[steerBones.Length];
            steerTransforms[LSTEER] = steerBones[LSTEER].Transform;
            steerTransforms[RSTEER] = steerBones[RSTEER].Transform;

            turretTransforms = new Matrix[turretBones.Length];
            turretTransforms[TURRET] = turretBones[TURRET].Transform;
            turretTransforms[CANNON] = turretBones[CANNON].Transform;
            turretTransforms[HATCH] = turretBones[HATCH].Transform;
        }

        private void updateTransforms()
        {
            model.Root.Transform = Matrix.CreateScale(owner.transform.Scale)
                * Matrix.CreateFromQuaternion(owner.transform.Rotation)
                * Matrix.CreateFromAxisAngle(owner.transform.Up, MathHelper.ToRadians(ORIENTATION_FIX))
                * Matrix.CreateTranslation(owner.transform.Position);
            
            // Apply matrices to the relevant bones.
            if (updateWheels)
            {
                updateWheels = false;
                Matrix wheelRotation = Matrix.CreateRotationX(wheelRotationValue);
                wheelBones[LBWHEEL].Transform = wheelRotation * wheelTransforms[LBWHEEL];
                wheelBones[RBWHEEL].Transform = wheelRotation * wheelTransforms[RBWHEEL];
                wheelBones[LFWHEEL].Transform = wheelRotation * wheelTransforms[LFWHEEL];
                wheelBones[RFWHEEL].Transform = wheelRotation * wheelTransforms[RFWHEEL];
            }
            if (updateSteer)
            {
                updateSteer = false;
                Matrix steerRotation = Matrix.CreateRotationY(steerRotationValue);
                steerBones[LSTEER].Transform = steerRotation * steerTransforms[LSTEER];
                steerBones[RSTEER].Transform = steerRotation * steerTransforms[RSTEER];
            }
            if (updateTurret)
            {
                updateTurret = false;
                Matrix turretRotation = Matrix.CreateRotationY(turretRotationValue);
                Matrix cannonRotation = Matrix.CreateRotationX(cannonRotationValue);
                Matrix hatchRotation = Matrix.CreateRotationX(hatchRotationValue);
                turretBones[TURRET].Transform = turretRotation * turretTransforms[TURRET];
                turretBones[CANNON].Transform = cannonRotation * turretTransforms[CANNON];
                turretBones[HATCH].Transform = hatchRotation * turretTransforms[HATCH];
            }
            // Look up combined bone matrices for the entire model.
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        }
        
        public override void Initialize()
        {
            model = GameInstance.Content.Load<Model>("Tank/tank");
            cacheBones();
            cacheTransforms();
            boneTransforms = new Matrix[model.Bones.Count];
            updateTransforms();
        }

        public override void Update(GameTime gameTime)
        {
            updateTransforms();
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the model.
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = Camera.mainCamera.View;
                    effect.Projection = Camera.mainCamera.Projection;

                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
    }
}

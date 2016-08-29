using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingTextDemo;
using Raycasting_Projection.Utilities;
using Cameras_and_Primitives;

namespace Raycasting_Projection.Components
{
    class PlayerController : Component
    {
        public AnimatedTankMover tankMover;
        public BoundingBox pickingVolume;
        public float moveSpeed;
        public float lookSensitivity;
        public float jumpForce;
        public float groundHeight;

        private float jumpVelocity;
        private KeyboardState prevKeyState;
        private MouseState prevMouseState;
        private Vector3 currRot;
        private Vector3 currPos;
        private Vector2 oldMousePos;
        private Raycast raycaster;

        public PlayerController() : base()
        {
            moveSpeed = 20;
            lookSensitivity = 10;
            groundHeight = 3;
            jumpVelocity = 0;
            jumpForce = 10;
        }

        public override void Initialize()
        {
            currPos = owner.transform.Position;
            raycaster = new Raycast(GameInstance.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            movePlayer();
            rotatePlayer();
            applyGravity();
            findTankPosition();
            //track inputs so single frame presses can be queried
            prevKeyState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        private void findTankPosition()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed)
            {
                raycaster.setupMatrices(Camera.mainCamera.World, Camera.mainCamera.View, Camera.mainCamera.Projection);
                RaycastResult info;
                if (raycaster.cast(Mouse.GetState().Position.ToVector2(), pickingVolume, out info))
                {
                    //move the tank
                    tankMover.Destination = info.contactPoint;
                }
            }
        }

        private void movePlayer()
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector3 normalisedMovement = Vector3.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
            {
                jumpVelocity = jumpForce;
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                //camPos.Z -= moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement += owner.transform.Forward;
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                //camPos.Z += moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement -= owner.transform.Forward;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                //camPos.X -= moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement -= owner.transform.Right;
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                //camPos.X += moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement += owner.transform.Right;
            }
            //if movement has occured at all
            if (normalisedMovement.LengthSquared() > 0)
            {
                normalisedMovement.Y = 0; //remove any verticallity
                //create a normalised direction
                normalisedMovement.Normalize();
                //apply the movement
                currPos += normalisedMovement * moveSpeed * Time.Instance.DeltaTime;
                //now apply any jumping velocity if there is any
                currPos.Y += jumpVelocity * Time.Instance.DeltaTime;
                //correct our position if we've fallen below the ground
                if (currPos.Y < groundHeight)
                    currPos.Y = groundHeight;
                //update camera's position
                owner.transform.Position = currPos;
            }
            else if (jumpVelocity != 0) //if we stand still and jump
            {
                currPos.Y += jumpVelocity * Time.Instance.DeltaTime;
                if (currPos.Y < groundHeight)
                    currPos.Y = groundHeight;
                //update camera's position
                owner.transform.Position = currPos;
            }
        }

        private void rotatePlayer()
        {
            Vector2 delta = Mouse.GetState().Position.ToVector2() - oldMousePos;
            //apply the rotation angle change(degrees)
            currRot.X -= lookSensitivity * delta.Y * Time.Instance.DeltaTime;
            currRot.Y -= lookSensitivity * delta.X * Time.Instance.DeltaTime;

            //if the mouse moves enough (for example, if mouse is left alone, don't want to update rotation)
            if (currRot.LengthSquared() > 2)
            {
                //create the relevant angles in radians
                //float yaw = MathHelper.ToRadians(currRot.Y);
                //float pitch = MathHelper.ToRadians(currRot.X);

                owner.transform.Rotate(currRot.X, currRot.Y, 0);
                //camera.transform.Rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);
            }
            oldMousePos = Mouse.GetState().Position.ToVector2();
        }

        private void applyGravity()
        {
            if (currPos.Y > groundHeight)
                jumpVelocity -= Time.Instance.DeltaTime * 10;
            else
                jumpVelocity = 0;
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cameras_and_Primitives;
using MovingTextDemo;
using Raycasting_Projection.Utilities;
using Raycasting_Projection.GameObjects;
using Raycasting_Projection.Components;

namespace Raycasting_Projection
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Window Properties and Utilities
        int screenWidth = 1280, screenHeight = 720;
        string windowTitle = "Raycasting and Projection";
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Time timer;
        KeyboardState prevState;
        Vector2 oldMousePos = Vector2.Zero;
        #endregion
        #region Meshes
        Model skybox;
        Matrix skyboxWorld;
        Vector3 skyboxOffset;
        #endregion
        #region Camera Settings
        Vector3 camPos, camRot;
        float moveSpeed = 20f, lookSensitivity = 10;
        float groundHeight = 3, jumpVelocity = 0, jumpForce = 10;
        #endregion
        #region GameObjects
        GameObject camera;
        GameObject plane;
        BoundingBox planeCollider;
        GameObject tank;
        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //configure the game window
            this.Window.Title = windowTitle;
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //create the camera gameObject
            camera = new GameObject(this);
            camera.transform.Position = new Vector3(0, groundHeight, 10);
            camera.AddComponent<Camera>();
            camPos = camera.transform.Position;
            camRot = Vector3.Zero;

            //create the ground plane gameObject
            plane = new GameObject(this);
            plane.transform.Position = Vector3.Zero;
            plane.transform.Scale = new Vector3(500, 500, 1);
            plane.transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Left, MathHelper.ToRadians(90));
            MeshRendererComponent planeRend = plane.AddComponent<MeshRendererComponent>();
            planeRend.Mesh = PrimitiveShape.CreateXYPlane();
            //create the plane's bounding box
            Vector3 planeMinPoint, planeMaxPoint;
            planeMinPoint = plane.transform.Position - new Vector3(plane.transform.Scale.X / 2, 0, plane.transform.Scale.Y / 2);
            planeMaxPoint = plane.transform.Position + new Vector3(plane.transform.Scale.X / 2, 0, plane.transform.Scale.Y / 2);
            planeCollider = new BoundingBox(planeMinPoint, planeMaxPoint);

            //setup the game utlities
            timer = Time.Instance;
            oldMousePos = Mouse.GetState().Position.ToVector2();
            skyboxOffset = Vector3.Down / 8f;

            //Create the tank gameObject
            tank = new GameObject(this);
            tank.transform.Position = new Vector3(0, 0, 1);
            tank.transform.Scale = new Vector3(0.01f, 0.01f, 0.01f);
            tank.AddComponent<AnimatedTank>();
            MoveToComponent tankMove = tank.AddComponent<MoveToComponent>();
            tankMove.MinimumDistance = 1;
            tankMove.speed = 2;
            AnimatedTankMover tankMover = tank.AddComponent<AnimatedTankMover>();
            tankMover.pickingVolume = planeCollider;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MeshRendererComponent planeRend = plane.GetComponent<MeshRendererComponent>();
            planeRend.Material.Texture = Content.Load<Texture2D>("Ground Model/3SNe");
            planeRend.Material.TextureEnabled = true;
            skybox = Content.Load<Model>("Skybox Model/skybox");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            timer.tick(ref gameTime);
            // TODO: Add your update logic here
            moveCamera();
            rotateCamera();

            applyGravity();
            //store the keyboard state to check for single press instead of held down press
            prevState = Keyboard.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            drawSkybox();

            base.Draw(gameTime);
        }

        #region Camera Movement

        private void moveCamera()
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector3 normalisedMovement = Vector3.Zero;

            checkJumpKey();
            if (keyState.IsKeyDown(Keys.W))
            {
                //camPos.Z -= moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement += camera.transform.Forward;
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                //camPos.Z += moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement -= camera.transform.Forward;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                //camPos.X -= moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement -= camera.transform.Right;
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                //camPos.X += moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement += camera.transform.Right;
            }
            //if movement has occured at all
            if (normalisedMovement.LengthSquared() > 0)
            {
                normalisedMovement.Y = 0; //remove any verticallity
                //create a normalised direction
                normalisedMovement.Normalize();
                //apply the movement
                camPos += normalisedMovement * moveSpeed * timer.DeltaTime;
                //now apply any jumping velocity if there is any
                camPos.Y += jumpVelocity * timer.DeltaTime;
                //correct our position if we've fallen below the ground
                if (camPos.Y < groundHeight)
                    camPos.Y = groundHeight;
                //update camera's position
                camera.transform.Position = camPos;
            }
            else if (jumpVelocity != 0) //if we stand still and jump
            {
                camPos.Y += jumpVelocity * timer.DeltaTime;
                if (camPos.Y < groundHeight)
                    camPos.Y = groundHeight;
                //update camera's position
                camera.transform.Position = camPos;
            }
        }

        private void checkJumpKey()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
            {
                jumpVelocity = jumpForce;
            }
        }

        private void rotateCamera()
        {
            Vector2 delta = Mouse.GetState().Position.ToVector2() - oldMousePos;
            //apply the rotation angle change(degrees)
            camRot.X -= lookSensitivity * delta.Y * timer.DeltaTime;
            camRot.Y -= lookSensitivity * delta.X * timer.DeltaTime;

            //if the mouse moves enough (for example, if mouse is left alone, don't want to update rotation)
            if (camRot.LengthSquared() > 2)
            {
                //create the relevant angles in radians
                float yaw = MathHelper.ToRadians(camRot.Y);
                float pitch = MathHelper.ToRadians(camRot.X);
                //this method requires the camRot to be normalised into radians
                //camera.Rotation = Quaternion.CreateFromAxisAngle(camRot, 1f);
                camera.transform.Rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);
            }
            oldMousePos = Mouse.GetState().Position.ToVector2();
        }

        private void applyGravity()
        {
            if (camPos.Y > groundHeight)
                jumpVelocity -= timer.DeltaTime * 10;
            else
                jumpVelocity = 0;
        }

        #endregion

        private void drawSkybox()
        {
            skyboxWorld = Matrix.CreateTranslation(camera.transform.Position + skyboxOffset);
            //change how the texture is drawn on the model to remove visible seems
            SamplerState sampler = new SamplerState();
            sampler.AddressU = TextureAddressMode.Clamp;
            sampler.AddressV = TextureAddressMode.Clamp;
            GraphicsDevice.SamplerStates[0] = sampler;
            //Disable depth buffering
            DepthStencilState depthStencil = new DepthStencilState();
            depthStencil.DepthBufferEnable = false;
            GraphicsDevice.DepthStencilState = depthStencil;

            skybox.Draw(skyboxWorld, Camera.mainCamera.View, Camera.mainCamera.Projection);
            //re-enable depth buffering
            depthStencil = new DepthStencilState();
            depthStencil.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = depthStencil;
        }
    }
}

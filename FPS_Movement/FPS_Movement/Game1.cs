﻿using Cameras_and_Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MovingTextDemo;

namespace FPS_Movement
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model skybox;
        Time timer;
        //camera settings
        Camera camera;
        Vector3 camPos, camRot;
        float moveSpeed = 20f, lookSensitivity = 10;
        GameObjects.Plane plane;
        Vector2 oldMousePos = Vector2.Zero;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            camera = new Camera(new Vector3(0, 2, 10));
            camPos = camera.Position;
            camRot = Vector3.Zero;
            plane = new GameObjects.Plane(this);
            plane.position.X = 0;
            plane.position.Y = 0;
            plane.scale = new Vector3(200, 200, 1);
            plane.rotation = Quaternion.CreateFromAxisAngle(Vector3.Left, MathHelper.ToRadians(90));
            timer = Time.Instance;
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
            plane.Texture = Content.Load<Texture2D>("Ground Model/3SNe");
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
            plane.draw(camera.View, camera.Projection);

            base.Draw(gameTime);
        }

        private void moveCamera()
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector3 normalisedMovement = Vector3.Zero;

            if(keyState.IsKeyDown(Keys.W))
            {
                //camPos.Z -= moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement += camera.Forward;
            }
            else if(keyState.IsKeyDown(Keys.S))
            {
                //camPos.Z += moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement -= camera.Forward;
            }
            if(keyState.IsKeyDown(Keys.A))
            {
                //camPos.X -= moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement -= camera.Right;
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                //camPos.X += moveSpeed * timer.DeltaTime;
                //camera.Position = camPos;
                normalisedMovement += camera.Right;
            }
            //if movement has occured at all
            if (normalisedMovement.LengthSquared() > 0)
            {
                normalisedMovement.Y = 0; //remove any verticallity
                //create a normalised direction
                normalisedMovement.Normalize();
                //apply the movement
                camPos += normalisedMovement * moveSpeed * timer.DeltaTime;
                //update camera's position
                camera.Position = camPos;
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
                camera.Rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0);
            }
            oldMousePos = Mouse.GetState().Position.ToVector2();
        }

        private void drawSkybox()
        {
            Matrix world = camera.World * Matrix.CreateTranslation(camera.Position);
            SamplerState sampler = new SamplerState();
            sampler.AddressU = TextureAddressMode.Clamp;
            sampler.AddressV = TextureAddressMode.Clamp;
            GraphicsDevice.SamplerStates[0] = sampler;

            DepthStencilState depthStencil = new DepthStencilState();
            depthStencil.DepthBufferEnable = false;
            GraphicsDevice.DepthStencilState = depthStencil;

            skybox.Draw(world, camera.View, camera.Projection);

            depthStencil = new DepthStencilState();
            depthStencil.DepthBufferEnable = false;
            GraphicsDevice.DepthStencilState = depthStencil;
        }
    }
}

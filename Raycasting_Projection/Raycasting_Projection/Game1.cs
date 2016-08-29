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
        #endregion
        #region Meshes
        Model skybox;
        Matrix skyboxWorld;
        Vector3 skyboxOffset;
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

            //create the camera gameObject
            camera = new GameObject(this);
            camera.transform.Position = new Vector3(0, 3, 10);
            PlayerController pController = camera.AddComponent<PlayerController>();
            pController.groundHeight = 3;
            camera.AddComponent<Camera>();
            pController.pickingVolume = planeCollider;
            //setup the game utlities
            timer = Time.Instance;
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
            pController.tankMover = tankMover;

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

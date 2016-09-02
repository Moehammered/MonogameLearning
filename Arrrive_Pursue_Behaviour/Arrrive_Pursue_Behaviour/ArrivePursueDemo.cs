using Arrrive_Pursue_Behaviour.BaseComponents;
using Arrrive_Pursue_Behaviour.GameComponents;
using Arrrive_Pursue_Behaviour.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.BaseComponents;
using MonogameLearning.GameComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Arrrive_Pursue_Behaviour
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    
    public class ArrivePursueDemo : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #region Window Properties and Utilities
        private int screenWidth = 1280, screenHeight = 720;
        private string windowTitle = "Arrive and Pursue Behaviour";
        private Time timer;
        #endregion
        #region GameObjects
        private GameObject player;
        private GameObject ground;
        private BoundingBox groundCollider;
        private GameObject arriveTank, pursueTank;
        #endregion
        #region Skybox
        private Model skybox;
        private Matrix skyboxWorld;
        private Vector3 skyboxOffset;
        #endregion
        CollisionDetector collisionSystem;
        BoundingBox arriveTankBox, pursueTankBox;
        List<GameObject> collisionCache;

        public ArrivePursueDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = windowTitle;
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            collisionCache = new List<GameObject>(2);
            collisionSystem = new CollisionDetector();
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
            Services.AddService<CollisionDetector>(collisionSystem);
            createGroundGameObject();
            createPlayerGameObject();
            //setup a tank gameobject with arrival behaviour
            arriveTank = createTankGameObject(1, 10);
            ArriveAtComponent arriver = arriveTank.AddComponent<ArriveAtComponent>();
            arriver.MinimumDistance = 1;
            arriver.Speed = 6;
            arriver.SlowdownRadius = 3;
            arriver.arrivalSpeed = 2;
            //setup a tank gameobject with pursue behaviour
            pursueTank = createTankGameObject(0, -10);
            pursueTank.RemoveComponent<AnimatedTankMover>();
            
            PursueComponent purs = pursueTank.AddComponent<PursueComponent>();
            purs.Target = arriveTank;
            purs.speed = 4;
            purs.RefreshRate = 0.1f;
            //give the player's controller a reference to the arrival tank
            PlayerController pControl = player.GetComponent<PlayerController>();
            pControl.tankMover = arriveTank.GetComponent<AnimatedTankMover>();

            timer = Time.Instance;

            //initialises all components in the GameComponentCollection
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
            Model tankModel = Content.Load<Model>("Tank/tank");
            BoundingBox tankBound = new BoundingBox();
            foreach(ModelMesh mesh in tankModel.Meshes)
            {
                BoundingBox bound = BoundingBox.CreateFromSphere(mesh.BoundingSphere);
                tankBound = BoundingBox.CreateMerged(tankBound, bound);
            }
            arriveTank.GetComponent<BoxCollider>().UnScaledBounds = tankBound;
            pursueTank.GetComponent<BoxCollider>().UnScaledBounds = tankBound;
            arriveTankBox = tankBound;
            arriveTankBox.Min *= arriveTank.transform.Scale;
            arriveTankBox.Max *= arriveTank.transform.Scale;
            pursueTankBox = tankBound;
            pursueTankBox.Min *= pursueTank.transform.Scale;
            pursueTankBox.Max *= pursueTank.transform.Scale;
            Console.WriteLine("Tank bounds: " + tankBound);
            Console.WriteLine("Arrive bounds: " + arriveTankBox);
            Console.WriteLine("Pursue bounds: " + pursueTankBox);
            // TODO: use this.Content to load your game content here
            setupSkybox();
            MeshRendererComponent groundRend = ground.GetComponent<MeshRendererComponent>();
            groundRend.Material.Texture = Content.Load<Texture2D>("Ground Model/3SNe");
            groundRend.Material.TextureEnabled = true;
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

            // TODO: Add your update logic here
            timer.tick(ref gameTime);

            collisionSystem.checkCollisions();
            /*
            //collision testing experiment
            BoundingBox temp1, temp2;
            temp1 = arriveTankBox;
            temp2 = pursueTankBox;
            //adjust the bounding box to follow
            //arrive tank's bounding box adjustment
            temp1.Min += arriveTank.transform.Position;
            temp1.Max += arriveTank.transform.Position;
            //pursue tank's bounding box adjustment
            temp2.Min += pursueTank.transform.Position;
            temp2.Max += pursueTank.transform.Position;

            if (temp2.Intersects(temp1))
            {
                //Console.WriteLine("Pursuer collided!");
                if (!collisionCache.Contains(pursueTank))
                {
                    string methodName = "OnCollision";
                    PursueComponent pt = pursueTank.GetComponent<PursueComponent>();
                    collisionCache.Add(pursueTank);
                    collisionCache.Add(arriveTank);
                    pursueTank.BroadcastMessage(methodName);
                    pt.speed = 0;
                }
            }
            else
            {
                if(collisionCache.Contains(pursueTank))
                {
                    pursueTank.GetComponent<PursueComponent>().speed = 2;
                    pursueTank.BroadcastMessage("OnCollisionExit");
                    collisionCache.Remove(pursueTank);
                    collisionCache.Remove(arriveTank);
                }
            }*/
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

            base.Draw(gameTime);
        }

        #region Skybox Setup and Rendering
        private void setupSkybox()
        {
            skybox = Content.Load<Model>("Skybox Model/skybox");
            skyboxOffset = Vector3.Down / 8f;
        }

        private void drawSkybox()
        {
            skyboxWorld = Matrix.CreateTranslation(player.transform.Position + skyboxOffset);
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
        #endregion

        #region GameObject Creation and Setup
        private void createGroundGameObject()
        {
            ground = new GameObject(this);
            ground.transform.Position = Vector3.Zero;
            ground.transform.Scale = new Vector3(500, 500, 1);
            ground.transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Left, MathHelper.ToRadians(90));
            MeshRendererComponent groundRend = ground.AddComponent<MeshRendererComponent>();
            groundRend.Mesh = PrimitiveShape.CreateXYPlane();
            //create the plane's bounding box
            Vector3 groundMinPoint, groundMaxPoint;
            groundMinPoint = ground.transform.Position - new Vector3(ground.transform.Scale.X / 2, 0, ground.transform.Scale.Y / 2);
            groundMaxPoint = ground.transform.Position + new Vector3(ground.transform.Scale.X / 2, 0, ground.transform.Scale.Y / 2);
            groundCollider = new BoundingBox(groundMinPoint, groundMaxPoint);
        }

        private void createPlayerGameObject()
        {
            player = new GameObject(this);
            player.transform.Position = new Vector3(0, 3, 10);
            PlayerController pController = player.AddComponent<PlayerController>();
            pController.groundHeight = 3;
            player.AddComponent<Camera>();
            pController.pickingVolume = groundCollider;
        }

        private GameObject createTankGameObject(float x, float z)
        {
            GameObject newTank = new GameObject(this);

            newTank.transform.Position = new Vector3(x, 0, z);
            newTank.transform.Scale = new Vector3(0.01f, 0.01f, 0.01f);
            newTank.AddComponent<AnimatedTank>();
            newTank.AddComponent<BoxCollider>();
            AnimatedTankMover tankMover = newTank.AddComponent<AnimatedTankMover>();
            /*ArriveAtComponent tankArriver = tank.AddComponent<ArriveAtComponent>();
            tankArriver.MinimumDistance = 1;
            tankArriver.speed = 4;
            tankArriver.SlowdownRadius = 4;
            tankArriver.arrivalSpeed = 1.5f;*/

            return newTank;
        }
        #endregion
    }
}

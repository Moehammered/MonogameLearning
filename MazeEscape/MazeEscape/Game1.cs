using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Utilities;
using MazeEscape.GameUtilities;
using MazeEscape.GameComponents;
using Microsoft.Xna.Framework.Audio;

namespace MazeEscape
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        #region Game Properties
        private int screenWidth = 1280, screenHeight = 720;
        private Time timer;
        private GameObject camera;
        private Vector3 playerStartPoint, goalPoint;
        private FirstPersonController player;
        private GameObject goal;
        private LevelLoader levelLoader;
        private GameObject[,] levelTiles;
        private CollisionDetector collisionService;
        private SoundEffect bgSoundData, moveSoundData, winSoundData, deadSoundData;
        private SoundEffectInstance bgInstance, moveSoundInst, winSoundInst, deadSoundInst;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            levelLoader = new LevelLoader();
            collisionService = new CollisionDetector();
            // TODO: Add your initialization logic here
            Services.AddService<CollisionDetector>(collisionService);
            timer = Time.Instance;

            createPlayer();
            initialiseLevel();
            createGoal();
            
            base.Initialize();
            collisionService.addDynamicCollider(camera.GetComponent<BoxCollider>());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");
            loadSounds();
            setSoundInstances();
            bgInstance.Play();
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

            // TODO: Add your update logic here
            timer.tick(ref gameTime);
            //collisionService.checkCollisions();
            if(player != null && player.hitGoal)
            {
                player.Owner.RemoveComponent<FirstPersonController>();
                player = null;
            }
            testObjectCreation();
            base.Update(gameTime);
            collisionService.sweepDynamics();
            GameObject.ProcessDestroyQueue();
            //keep track of pressed buttons this frame before going to next
            Input.recordInputs();
        }

        private void testObjectCreation()
        {
            if (Input.IsKeyPressed(Keys.I))
            {
                GameObject go = new GameObject(this);
                MeshRendererComponent rend = go.AddComponent<MeshRendererComponent>();
                rend.Mesh = PrimitiveShape.CreateCube();
                rend.colour = Color.Purple;
                go.transform.Position = playerStartPoint + Vector3.Forward;
                rend.Initialize();
            }
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
            renderHUD();
        }

        private void renderHUD()
        {
            spriteBatch.Begin();
            if(player == null)
            {
                spriteBatch.DrawString(font, "You made it!", new Vector2(screenWidth / 3, screenHeight / 2), Color.White);
            }
            else if(player.dead)
            {
                spriteBatch.DrawString(font, "You died!", new Vector2(screenWidth / 3, screenHeight / 2), Color.RosyBrown);
            }
            else
            {
                spriteBatch.DrawString(font, "Find the green\nAvoid the red!", new Vector2(screenWidth / 4, screenHeight / 10), Color.RosyBrown);
                spriteBatch.DrawString(font, "Points: " + player.points, new Vector2(screenWidth - screenWidth / 4, screenHeight / 10), Color.Yellow);
            }
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }

        #region Sound Initialisation
        private void loadSounds()
        {
            moveSoundData = Content.Load<SoundEffect>("Sounds/footsteps");
            bgSoundData = Content.Load<SoundEffect>("Sounds/bg-loop");
            deadSoundData = Content.Load<SoundEffect>("Sounds/dead");
            winSoundData = Content.Load<SoundEffect>("Sounds/win");
        }

        private void setSoundInstances()
        {
            moveSoundInst = moveSoundData.CreateInstance();
            player.moveSound = moveSoundInst;
            deadSoundInst = deadSoundData.CreateInstance();
            player.deathSound = deadSoundInst;
            winSoundInst = winSoundData.CreateInstance();
            player.winSound = winSoundInst;
            bgInstance = bgSoundData.CreateInstance();
            bgInstance.Volume = 0.25f;
            bgInstance.IsLooped = true;
        }
        #endregion

        #region Level Construction
        private void initialiseLevel()
        {
            levelLoader.loadLevelFiles();
            LevelData level;
            if (levelLoader.loadLevel("Level0.txt", out level))
            {
                Console.WriteLine("Constructing level");
                levelTiles = constructLevel(level);

                camera.transform.Position = playerStartPoint;
            }
        }

        private GameObject[,] constructLevel(LevelData level)
        {
            GameObject[,] tiles = new GameObject[level.columns, level.rows];
            BoundingBox box = new BoundingBox(new Vector3(-0.5f, -0.4f, -0.5f),new Vector3(0.5f, 0.4f, 0.5f));
            for (int x = 0; x < level.columns; x++)
            {
                for(int z = 0; z < level.rows; z++)
                {
                    GameObject tile = new GameObject(this);
                    tile.transform.Position = new Vector3(x, 0, z);
                    BoxCollider collider = tile.AddComponent<BoxCollider>();
                    collider.UnScaledBounds = box;
                    MeshRendererComponent renderer = tile.AddComponent<MeshRendererComponent>();
                    renderer.Mesh = PrimitiveShape.CreateCube();
                    
                    switch(level.getData(x, z))
                    {
                        case 0: //wall tile
                            renderer.colour = Color.Black;
                            tile.transform.Scale = new Vector3(1, 5, 1);
                            collider.UnScaledBounds = box;
                            break;
                        case 1: //floor tile
                            renderer.colour = Color.White;
                            break;
                        case 2: //spawn hazard
                            renderer.colour = Color.White;
                            //hazard.transform.Position = tile.transform.Position + new Vector3(0, 1, 0);
                            createHazard(tile.transform.Position + new Vector3(0, 1, 0));
                            break;
                        case 3: //goal point
                            renderer.colour = Color.Green;
                            goalPoint = tile.transform.Position;
                            goalPoint.Y += 1;
                            break;
                        case 4://start point
                            renderer.colour = Color.Gray;
                            playerStartPoint = tile.transform.Position;
                            playerStartPoint.Y += 1;
                            break;
                        case 9://points collectable
                            renderer.colour = Color.Orange;
                            createPointPickup(tile.transform.Position + new Vector3(0, 1, 0));
                            break;
                        default: //decorative tile
                            renderer.colour = Color.Orange;
                            break;
                    }
                    tiles[x, z] = tile;
                }
            }

            return tiles;
        }

        private GameObject[] constructWalls(LevelData level, int height = 4)
        {
            GameObject[] walls = new GameObject[(level.columns * 2 + level.rows * 2)*height];
            for(int x = 0; x < level.columns*2; x++)
            {
                for(int y = 1; y < height; y++)
                {
                    GameObject wall = new GameObject(this);
                    MeshRendererComponent rend=wall.AddComponent<MeshRendererComponent>();
                    rend.Mesh = PrimitiveShape.CreateCube();
                    rend.colour = Color.Black;
                    if(x < level.columns)
                        wall.transform.Position = new Vector3(x, y, 0);
                    else
                        wall.transform.Position = new Vector3(x-level.columns, y, level.rows);
                    walls[x*y] = wall;
                }
            }

            return walls;
        }
        #endregion

        #region GameObject creation
        private void createPlayer()
        {
            camera = new GameObject(this);
            camera.AddComponent<Camera>();
            camera.AddComponent<FirstPersonMover>();
            player = camera.AddComponent<FirstPersonController>();
            BoxCollider playCol = camera.AddComponent<BoxCollider>();
            playCol.UnScaledBounds = new BoundingBox(-Vector3.One / 4, Vector3.One / 4);
        }

        private void createGoal()
        {
            goal = new GameObject(this);
            goal.name = "goal";
            goal.transform.Position = goalPoint;
            MeshRendererComponent rend = goal.AddComponent<MeshRendererComponent>();
            rend.colour = Color.Green;
            rend.Mesh = PrimitiveShape.CreateCube();
            BoxCollider collider = goal.AddComponent<BoxCollider>();
            Vector3 minPoint, maxPoint;
            minPoint = new Vector3(-0.5f, -0.5f, -0.5f);
            maxPoint = minPoint * -1;
            collider.UnScaledBounds = new BoundingBox(minPoint, maxPoint);
        }

        private void createHazard(Vector3 position)
        {
            //create the hazard
            GameObject hazard = new GameObject(this);
            hazard.transform.Scale = Vector3.One / 4;
            hazard.transform.Position = position;
            hazard.name = "hazard";
            MeshRendererComponent hazRend = hazard.AddComponent<MeshRendererComponent>();
            hazRend.Mesh = PrimitiveShape.CreateCube();
            hazRend.colour = Color.Red;
            Vector3 corner = new Vector3(-0.5f, -0.5f, -0.5f);
            hazard.AddComponent<BoxCollider>().UnScaledBounds = new BoundingBox(corner, -corner);
            Hazard ai = hazard.AddComponent<Hazard>();
            ai.player = camera;
            ai.WakeUpDistance = 7;
        }

        private void createPointPickup(Vector3 position)
        {
            GameObject pickup = new GameObject(this);
            pickup.transform.Scale = Vector3.One / 5;
            pickup.transform.Position = position;
            pickup.name = "pickup";
            MeshRendererComponent hazRend = pickup.AddComponent<MeshRendererComponent>();
            hazRend.Mesh = PrimitiveShape.CreateCube();
            hazRend.colour = Color.Yellow;
            Vector3 corner = new Vector3(-0.5f, -0.5f, -0.5f);
            pickup.AddComponent<BoxCollider>().UnScaledBounds = new BoundingBox(corner, -corner);
        }
        #endregion
    }

}

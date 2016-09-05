using System;
using MazeEscape.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Utilities;
using System.IO;
using MazeEscape.GameUtilities;
using MazeEscape.GameComponents;

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
        private int screenWidth = 1280, screenHeight = 720;
        private Time timer;
        private GameObject camera;
        private FirstPersonController player;
        private GameObject groundPlane;
        private GameObject goal;
        private LevelLoader levelLoader;
        private GameObject[,] levelTiles;
        private Random random;
        private CollisionDetector collisionService;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            levelLoader = new LevelLoader();
            random = new Random();
            collisionService = new CollisionDetector();
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
            Services.AddService<CollisionDetector>(collisionService);
            timer = Time.Instance;
            levelLoader.loadLevelFiles();
            LevelData level;
            if(levelLoader.loadLevel("Level2.txt", out level))
            {
                Console.WriteLine("Constructing level");
                levelTiles = constructLevel(level);
                //constructWalls(level);
            }
            //Console.WriteLine("Level1 Exist? " + levelLoader.loadLevel("Level1.txt", out level));
            camera = new GameObject(this);
            //camera.transform.Position = new Vector3(0, 5, 10);
            Vector3 startPos = levelTiles[levelTiles.GetLength(0)/2, levelTiles.GetLength(1)/2].transform.Position;
            startPos.Y += 1f;
            Console.WriteLine("StartPos: " + startPos);
            camera.transform.Position = startPos;
            camera.AddComponent<Camera>();
            camera.AddComponent<FirstPersonMover>();
            player = camera.AddComponent<FirstPersonController>();

            goal = new GameObject(this);
            int randX, randZ;
            randX = random.Next(levelTiles.GetLength(0));
            randZ = random.Next(levelTiles.GetLength(1));
            Vector3 goalPos = levelTiles[randX, randZ].transform.Position;
            goalPos.Y += 1f;
            goal.transform.Position = goalPos;
            MeshRendererComponent rend = goal.AddComponent<MeshRendererComponent>();
            rend.colour = Color.Green;
            rend.Mesh = PrimitiveShape.CreateCube();
            BoxCollider collider = goal.AddComponent<BoxCollider>();
            Vector3 minPoint, maxPoint;
            minPoint = new Vector3(-0.5f, -0.5f, -0.5f);
            maxPoint = minPoint * -1;
            collider.UnScaledBounds = new BoundingBox(minPoint, maxPoint);
            BoxCollider playCol = camera.AddComponent<BoxCollider>();
            playCol.UnScaledBounds = new BoundingBox(minPoint, maxPoint);
            /*groundPlane = new GameObject(this);
            MeshRendererComponent renderer = groundPlane.AddComponent<MeshRendererComponent>();
            renderer.Mesh = PrimitiveShape.CreateXYPlane();
            renderer.colour = Color.Red;*/
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
            font = Content.Load<SpriteFont>("Arial");
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
            collisionService.checkCollisions();

            base.Update(gameTime);
            //keep track of pressed buttons this frame before going to next
            Input.recordInputs();
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
            spriteBatch.DrawString(font, "FPS: " + 1/Time.DeltaTime, new Vector2(screenWidth / 2, screenHeight / 10), Color.Red);
            spriteBatch.DrawString(font, "Reached Goal: " + player.hitGoal, new Vector2(screenWidth / 8, screenHeight / 10), Color.Red);
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }

        private GameObject[,] constructLevel(LevelData level)
        {
            GameObject[,] tiles = new GameObject[level.columns, level.rows];
            BoundingBox box = new BoundingBox(new Vector3(-0.5f, 0, -0.5f),new Vector3(0.5f, 0, 0.5f));
            for (int x = 0; x < level.columns; x++)
            {
                for(int z = 0; z < level.rows; z++)
                {
                    GameObject tile = new GameObject(this);
                    tile.transform.Position = new Vector3(x, 0, z);
                    tile.transform.Rotate(270, 0, 0);
                    BoxCollider collider = tile.AddComponent<BoxCollider>();
                    collider.UnScaledBounds = box;
                    MeshRendererComponent renderer = tile.AddComponent<MeshRendererComponent>();
                    renderer.Mesh = PrimitiveShape.CreateCube();
                    
                    switch(level.getData(x, z))
                    {
                        case 0:
                            renderer.colour = Color.Black;
                            break;
                        case 1:
                            renderer.colour = Color.White;
                            break;
                        case 2:
                            renderer.colour = Color.Blue;
                            break;
                        default:
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
    }
}

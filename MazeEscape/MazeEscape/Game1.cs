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
        private Time timer;
        private GameObject camera;
        private GameObject groundPlane;
        private LevelLoader levelLoader;
        private GameObject[,] levelTiles;
        private Random random;
        private CollisionDetector collisionService;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            camera.AddComponent<FirstPersonController>();

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
                    tile.transform.Position = new Vector3(x, 0, -z);
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
    }
}

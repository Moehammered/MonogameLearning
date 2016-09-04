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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            levelLoader = new LevelLoader();
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
            timer = Time.Instance;
            levelLoader.loadLevelFiles();
            LevelData level;
            if(levelLoader.loadLevel("Level1.txt", out level))
            {
                Console.WriteLine("Constructing level");
                constructLevel(level);
            }
            //Console.WriteLine("Level1 Exist? " + levelLoader.loadLevel("Level1.txt", out level));
            camera = new GameObject(this);
            camera.transform.Position = new Vector3(0, 5, 10);
            camera.transform.lookAt(Vector3.Zero);
            camera.AddComponent<Camera>();

            groundPlane = new GameObject(this);
            MeshRendererComponent renderer = groundPlane.AddComponent<MeshRendererComponent>();
            renderer.Mesh = PrimitiveShape.CreateXYPlane();
            renderer.colour = Color.Red;
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

        private void constructLevel(LevelData level)
        {
            for(int x = 0; x < level.columns; x++)
            {
                for(int z = 0; z < level.rows; z++)
                {
                    GameObject tile = new GameObject(this);
                    tile.transform.Position = new Vector3(x, 0, -z);
                    tile.transform.Rotate(270, 0, 0);
                    MeshRendererComponent renderer = tile.AddComponent<MeshRendererComponent>();
                    renderer.Mesh = PrimitiveShape.CreateXYPlane();

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
                }
            }
        }
    }
}

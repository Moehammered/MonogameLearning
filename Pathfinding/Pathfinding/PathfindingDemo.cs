using MazeEscape.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Utilities;
using Pathfinding.GameComponents;
using Pathfinding.Pathfinding;
using Pathfinding.Utilities;

namespace Pathfinding
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PathfindingDemo : Game
    {
        #region Window Properties
        private string windowTitle = "Pathfinding";
        private int windowWidth = 1280, windowHeight = 720;
        #endregion
        #region Utility Properties
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CollisionDetector collisionService;
        private Time timer;
        #endregion
        #region Game Properties
        private LevelBuilder levelBuilder;
        private LevelGraph levelGraph;
        private GameObject playerCube;
        #endregion
        #region Path Display Properties
        private PathRenderComponent pathDisplay;
        private SpriteFont font;
        #endregion

        public PathfindingDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            #region Window Setup
            this.Window.Title = windowTitle;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();
            #endregion
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region Utility Setup
            timer = Time.Instance;
            collisionService = new CollisionDetector();
            Services.AddService<CollisionDetector>(collisionService);
            #endregion

            #region Level Setup
            //construct level gameobjects
            levelBuilder = new LevelBuilder(this);
            levelBuilder.initialise();
            levelBuilder.loadLevel("level2.txt");
            levelBuilder.buildLevel();

            //construct the levelgraph for pathfinding
            levelGraph = new LevelGraph(levelBuilder.LoadedLevelData);
            levelGraph.buildGraph();
            #endregion

            #region Player Setup
            //setup the player gameobject to have pathfinding and display capabilities
            playerCube = new GameObject(this);
            MeshRendererComponent cubeRend = playerCube.AddComponent<MeshRendererComponent>();
            cubeRend.Mesh = PrimitiveShape.CreateCube();
            cubeRend.colour = Color.Purple;
            playerCube.transform.Position = Vector3.One;
            playerCube.AddComponent<PlayerController>();
            PathfinderComponent playerPathSearch = playerCube.AddComponent<PathfinderComponent>();
            playerPathSearch.setAlgorithm<AStarPathing>(levelGraph);
            pathDisplay = playerCube.AddComponent<PathRenderComponent>();
            #endregion

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            //main game service updates
            timer.tick(ref gameTime);
            collisionService.sweepDynamics();

            //update all game components
            base.Update(gameTime);
            Input.recordInputs();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
            renderHUD();
        }

        #region Information Display Functions
        private void renderHUD()
        {
            spriteBatch.Begin();
            displayPathInfo();
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }

        private void displayPathInfo()
        {
            if (pathDisplay != null)
            {
                spriteBatch.DrawString(font, pathDisplay.PathInfo, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, pathDisplay.PointInfo, new Vector2(windowWidth*0.75f, 0), Color.White);
            }
        }
        #endregion
    }
}

using FiniteStateMachine.FSM;
using FiniteStateMachine.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLearning.BaseComponents;
using MonogameLearning.GameComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Pathfinding;
using MonogameLearning.Utilities;

namespace FiniteStateMachine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class FiniteStateMachineDemo : Game
    {
        #region Window Properties
        private string windowTitle = "Finite State Machine - AI";
        private int windowWidth = 1280, windowHeight = 720;
        #endregion
        #region Utility Properties
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private FSMInterpreter fsmParses;
        private CollisionDetector collisionService;
        private Time timer;
        #endregion
        #region Game Properties
        private LevelBuilder levelBuilder;
        private LevelGraph levelGraph;
        private GameObject playerCube;
        private GameObject NPCCube;
        private GameObject powerUpCube;
        private NPCController npcController;
        private PlayerController playerController;
        #endregion
        #region Path Display Properties
        private PathRenderComponent playerPathDisplay, npcPathDisplay;
        private SpriteFont font;
        #endregion

        public FiniteStateMachineDemo()
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
            fsmParses = new FSMInterpreter("fsm_npc1.xml");
            fsmParses.parseFile();
            BoundingBox cubeBounds = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
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
            playerCube.name = "Player";
            MeshRendererComponent cubeRend = playerCube.AddComponent<MeshRendererComponent>();
            cubeRend.Mesh = PrimitiveShape.CreateCube();
            cubeRend.colour = Color.Purple;
            playerCube.transform.Position = Vector3.One;
            playerController = playerCube.AddComponent<PlayerController>();
            PathfinderComponent playerPathSearch = playerCube.AddComponent<PathfinderComponent>();
            playerPathSearch.setAlgorithm<AStarPathing>(levelGraph);
            playerPathDisplay = playerCube.AddComponent<PathRenderComponent>();
            playerPathDisplay.colour = Color.Purple;
            BoxCollider playerCollider = playerCube.AddComponent<BoxCollider>();
            playerCollider.UnScaledBounds = cubeBounds;
            #endregion
            #region NPC Setup
            NPCCube = new GameObject(this);
            NPCCube.name = "NPC";
            MeshRendererComponent npcRend = NPCCube.AddComponent<MeshRendererComponent>();
            npcRend.Mesh = PrimitiveShape.CreateCube();
            npcRend.colour = Color.DarkOrange;
            NPCCube.transform.Position = new Vector3(10, 1, 10);
            PathfinderComponent npcPathSearch = NPCCube.AddComponent<PathfinderComponent>();
            npcPathSearch.setAlgorithm<AStarPathing>(levelGraph);
            npcPathDisplay = NPCCube.AddComponent<PathRenderComponent>();
            npcPathDisplay.colour = Color.DarkOrange;
            npcController = NPCCube.AddComponent<NPCController>();
            npcController.player = playerCube.GetComponent<PlayerController>();
            npcController.PlayerNearDistance = 4;
            npcController.PlayerFarDistance = 8;
            BoxCollider npcCollider = NPCCube.AddComponent<BoxCollider>();
            npcCollider.UnScaledBounds = cubeBounds;
            npcController.machine = fsmParses.Machine;
            #endregion
            #region Powerup Setup
            powerUpCube = new GameObject(this);
            powerUpCube.name = "powerup";
            MeshRendererComponent powerUpRend = powerUpCube.AddComponent<MeshRendererComponent>();
            powerUpRend.Mesh = PrimitiveShape.CreateCube();
            powerUpRend.colour = Color.Gold;
            powerUpCube.transform.Scale = Vector3.One / 2f;
            powerUpCube.transform.Position = new Vector3(5, 1, 5);
            BoxCollider powerUpCollider = powerUpCube.AddComponent<BoxCollider>();
            powerUpCollider.UnScaledBounds = cubeBounds;
            powerUpCube.AddComponent<PowerUpPill>();
            #endregion

            base.Initialize();
            collisionService.addDynamicCollider(playerCollider);
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
            displayNPCInfo();
            displayPlayerInfo();
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }

        private void displayNPCInfo()
        {
            if(npcPathDisplay != null)
            {
                spriteBatch.DrawString(font, "AI: " + npcController.machine.CurrentState, (Vector2.UnitX * windowWidth * 0.7f), Color.White);
                spriteBatch.DrawString(font, npcPathDisplay.PathInfo, (Vector2.UnitX * windowWidth * 0.7f) + (Vector2.UnitY * windowHeight * 0.05f), Color.White);
                spriteBatch.DrawString(font, npcPathDisplay.PointInfo, (Vector2.UnitX * windowWidth * 0.7f) + (Vector2.UnitY * windowHeight * 0.1f), Color.White);
            }
        }

        private void displayPlayerInfo()
        {
            if (playerPathDisplay != null)
            {
                spriteBatch.DrawString(font, "Player Powered? " + playerController.isPoweredUp, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, playerPathDisplay.PathInfo, Vector2.UnitY * windowHeight * 0.025f, Color.White);
                spriteBatch.DrawString(font, playerPathDisplay.PointInfo, Vector2.UnitY * windowHeight * 0.1f, Color.White);
            }
        }
        #endregion
    }
}

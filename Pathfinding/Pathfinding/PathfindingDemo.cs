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
using System.Collections.Generic;

namespace Pathfinding
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PathfindingDemo : Game
    {
        private string windowTitle = "Pathfinding";
        private int windowWidth = 1280, windowHeight = 720;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CollisionDetector collisionService;
        private LevelBuilder levelBuilder;
        private LevelGraph levelGraph;
        private GameObject playerCube;
        private PlayerController player;
        private SpriteFont font;
        private Time timer;
        private BasicEffect lineMaterial;

        public PathfindingDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = windowTitle;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();

            levelBuilder = new LevelBuilder(this);
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
            lineMaterial = new BasicEffect(GraphicsDevice);
            collisionService = new CollisionDetector();
            Services.AddService<CollisionDetector>(collisionService);
            
            levelBuilder.initialise();
            levelBuilder.loadLevel("level2.txt");
            levelBuilder.buildLevel();

            levelGraph = new LevelGraph(levelBuilder.LoadedLevelData);
            levelGraph.buildGraph();

            playerCube = new GameObject(this);
            MeshRendererComponent cubeRend = playerCube.AddComponent<MeshRendererComponent>();
            cubeRend.Mesh = PrimitiveShape.CreateCube();
            cubeRend.colour = Color.Purple;
            playerCube.transform.Position = Vector3.One;
            player = playerCube.AddComponent<PlayerController>();
            player.levelGraph = levelGraph;

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
            font = Content.Load<SpriteFont>("Arial");
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
            collisionService.sweepDynamics();

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

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            renderPathLines();
            renderHUD();
        }

        private void renderHUD()
        {
            spriteBatch.Begin();
            displayClickedInfo();
            displayPathInfo();
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        }

        private void displayPathInfo()
        {
            if (player.debugPath != null)
            {
                string message = "Current path\n";
                for(int i = 0; i < player.debugPath.Length; i++)
                {
                    message += "Point[" + i + "]: " + player.debugPath[i].position + "\n";
                }
                spriteBatch.DrawString(font, message, new Vector2(windowWidth*0.75f, 0), Color.White);
            }
        }

        private void renderPathLines()
        {
            if(player.debugPath != null)
            {
                lineMaterial.World = Camera.mainCamera.World;
                lineMaterial.View = Camera.mainCamera.View;
                lineMaterial.Projection = Camera.mainCamera.Projection;

                foreach (EffectPass pass in lineMaterial.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                        PrimitiveType.LineStrip, player.pathBuffer, 0, player.pathBuffer.Length,
                        player.pathIndices, 0, player.pathIndices.Length - 1);
                }
            }
        }

        private void displayClickedInfo()
        {
            Vector2 node;
            string message = "Start Node: ";
            if (player.selectedNode != null && player.startNode != null)
            {
                node = new Vector2(player.startNode.position.X, player.startNode.position.Z);
                message += node;
                node.X = player.selectedNode.position.X;
                node.Y = player.selectedNode.position.Z;
                message += "\nSelected Node: " + node;
                List<GraphNode> nb = player.selectedNode.getNeightbours();
                message += "\nSelected Node Neighbours: " + nb.Count;
                foreach (GraphNode n in nb)
                    message += "\nNeighbour: " + n.position + " Cost: " + n.TravelCost;
            }
            spriteBatch.DrawString(font, message, Vector2.Zero, Color.White);
        }
    }
}

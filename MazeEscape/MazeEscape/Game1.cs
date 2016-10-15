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
using MazeEscape.Utilities;

namespace MazeEscape
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        #region Game Properties
        private int levelNumber = 1;
        private MazeScene sceneTest;
        private bool gameInitialised = false;
        private int screenWidth = 1280, screenHeight = 720;
        private Time timer;
        private CollisionDetector collisionService;
        private SoundEffect bgSoundData;
        private SoundEffectInstance bgInstance;
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
            sceneTest = new MazeScene(this, levelNumber);
            collisionService = new CollisionDetector();
            Services.AddService<CollisionDetector>(collisionService);
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
            font = Content.Load<SpriteFont>("Arial");

            bgSoundData = Content.Load<SoundEffect>("Sounds/bg-loop");
            bgInstance = bgSoundData.CreateInstance();
            bgInstance.Volume = 0.25f;
            bgInstance.IsLooped = true;
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
            //first time loading of scene done here AFTER game has it's base made ready
            if (!gameInitialised)
            {
                sceneTest.Load();
                gameInitialised = true;
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                // TODO: Add your update logic here
                timer.tick(ref gameTime);
                base.Update(gameTime);

                collisionService.checkCollisions();
                GameObject.ProcessDestroyQueue();
                //keep track of pressed buttons this frame before going to next
                Input.recordInputs();
            }
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
        }
    }

}

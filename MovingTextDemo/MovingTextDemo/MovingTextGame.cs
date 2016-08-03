using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MovingTextDemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MovingTextGame : Game
    {
        System.Random rand;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Text objects to display on screen
        Text2D timeDisplay;
        MovingText movingText;
        private Color backgroundColour;
        //time tracking between frames
        Time timer;
        //sound data
        private SoundEffect moveSoundData, ambientSoundData;
        private SoundEffectInstance movementSound, ambientSound;
        private float volume, pitch;

        public MovingTextGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rand = new System.Random();
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
            setupText();
            backgroundColour = Color.Black;
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
            movingText.setSpriteBatchReference(ref spriteBatch);
            // TODO: use this.Content to load your game content here
            movingText.Font = Content.Load<SpriteFont>("Arial");
            timeDisplay.Font = movingText.Font;

            loadSounds();
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
            //tick the time between frames to capture deltaTime
            timer.tick(ref gameTime);
            timeDisplay.Text = "Runtime: " + (int)timer.TotalTime;
            //check for inputs and act accordingly
            movingText.update(timer.DeltaTime);
            //check if any movement keys are held to change the display text and play sound
            checkForKeysHeld();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColour);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.DrawString(timeDisplay.Font, timeDisplay.Text, timeDisplay.Position, timeDisplay.Colour);
            movingText.draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private Vector2 getRandomScreenPos()
        {
            float x = (float)rand.NextDouble() * GraphicsDevice.Viewport.Width;
            float y = (float)rand.NextDouble() * GraphicsDevice.Viewport.Height;

            if (x < 0)
                x = 0;
            else if (x > GraphicsDevice.Viewport.Width - 100)
                x = GraphicsDevice.Viewport.Width - 100;
            if (y < 0)
                y = 0;
            else if (y > GraphicsDevice.Viewport.Height - 100)
                y = GraphicsDevice.Viewport.Height - 100;

            return new Vector2(x, y);
        }

        private void setupText()
        {
            movingText = new MovingText();
            movingText.Text = "Moving Text";
            movingText.Position = getRandomScreenPos();
            System.Diagnostics.Trace.WriteLine(movingText.Position);
            timeDisplay = new Text2D();
            timeDisplay.Position = new Vector2(GraphicsDevice.Viewport.Width / 2f - 100, 0);
            timeDisplay.Colour = Color.Green;
        }
        
        private void loadSounds()
        {
            volume = 1;
            pitch = 0;
            //loading sound data/file
            moveSoundData = Content.Load<SoundEffect>("spray");
            ambientSoundData = Content.Load<SoundEffect>("track");
            //create an instance of the movement sound file and set it up
            movementSound = moveSoundData.CreateInstance();
            movementSound.Volume = volume;
            movementSound.Pitch = pitch - 0.25f;
            movementSound.Pan = 0;
            //create an instance of the background sound file and set it up
            ambientSound = ambientSoundData.CreateInstance();
            ambientSound.IsLooped = true;
            ambientSound.Volume = volume / 2f;
            ambientSound.Pitch = pitch;
            ambientSound.Pan = 0;
            ambientSound.Play();
        }

        private void checkForKeysHeld()
        {
            Keys[] held = Keyboard.GetState().GetPressedKeys();
            //reset the string value
            movingText.Text = "Still text...";
            //are any keys held?
            if (held.Length > 0)
            {
                //yes so check through all of them
                foreach (Keys key in held)
                {
                    switch (key)
                    {
                        case Keys.W:
                        case Keys.S:
                        case Keys.A:
                        case Keys.D:
                            movingText.Text = "I'm moving now!";
                            if (movementSound.State != SoundState.Playing)
                                movementSound.Play();
                            break;
                        default:
                            movementSound.Stop();
                            break;
                    }
                }
            }
        }
    }
}

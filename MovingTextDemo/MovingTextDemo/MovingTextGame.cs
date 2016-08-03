using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab01
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MovingTextGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //displaying text variables
        private Vector2 textPosition;
        private SpriteFont font;
        private string displayText;
        //colour display variables
        private Color textColour;
        private Color backgroundColour;
        //text movement variables
        private float movementSpeed;
        //time tracking between frames
        private float currentTime, previousTime, deltaTime;
        //sound data
        private SoundEffect moveSoundData, ambientSoundData;
        private SoundEffectInstance movementSound, ambientSound;
        private float volume, pitch;

        public MovingTextGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            textPosition = new Vector2(0, 0);
            displayText = "Moving text!";
            textColour = Color.White;
            backgroundColour = Color.Black;
            movementSpeed = 100f;
            currentTime = 0;
            previousTime = 0;
            volume = 1;
            pitch = 0;
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
            ambientSound.Volume = volume/2f;
            ambientSound.Pitch = pitch;
            ambientSound.Pan = 0;
            ambientSound.Play();
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
            tickTime(gameTime);
            //check for inputs and act accordingly
            checkMovementKeys(deltaTime);
            //check if any movement keys are held to change the display text and play sound
            checkForKeysHeld();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void tickTime(GameTime time)
        {
            currentTime = (float)time.TotalGameTime.TotalMilliseconds/1000f;
            deltaTime = currentTime - previousTime;

            System.Diagnostics.Trace.WriteLine("Current Time: " + currentTime + " | Delta: " + deltaTime + " | Prev: " + previousTime);
            previousTime = currentTime;
        }

        private void checkForKeysHeld()
        {
            Keys[] held = Keyboard.GetState().GetPressedKeys();
            //reset the string value
            displayText = "Still text...";
            //are any keys held?
            if(held.Length > 0)
            {
                //yes so check through all of them
                foreach(Keys key in held)
                {
                    switch (key)
                    {
                        case Keys.W:
                        case Keys.S:
                        case Keys.A:
                        case Keys.D:
                            displayText = "I'm moving now!";
                            if(movementSound.State != SoundState.Playing)
                                movementSound.Play();
                        break;
                        default:
                            movementSound.Stop();
                        break;
                    }
                }
            }
        }

        private void checkMovementKeys(float deltaTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                textPosition.X += movementSpeed * deltaTime;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                textPosition.X -= movementSpeed * deltaTime;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                textPosition.Y -= movementSpeed * deltaTime;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                textPosition.Y += movementSpeed * deltaTime;
            }
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
            spriteBatch.DrawString(font, "Curr: " + currentTime, new Vector2(300, 0), Color.Red);
            spriteBatch.DrawString(font, displayText, textPosition, textColour);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cameras_and_Primitives
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        TexturedCube cube;
        StaticSquareMesh square;
        Texture2D texture;
        PrimitiveTestGame primitiveTest;
        BasicEffect be;
        float xRotation, yRotation, zPos;

        public Game1()
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
            primitiveTest = new PrimitiveTestGame(GraphicsDevice, graphics);
            //primitiveTest.initialise();

            camera = new Camera();
            camera.FieldOfView = 45;
            camera.setAspectRatio((float)graphics.PreferredBackBufferWidth, (float)graphics.PreferredBackBufferHeight);
            camera.setClippingPlanes(0.1f, 1000f);
            camera.Position = new Vector3(0, 0, 5);

            cube = new TexturedCube();
            texture = Content.Load<Texture2D>("crate");
            cube.initialise(GraphicsDevice, texture);

            square = new StaticSquareMesh(GraphicsDevice);

            xRotation = yRotation = 0;

            setupBasicEffect();
            setupRasterisation();

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
            //primitiveTest.update(gameTime);
            if(Keyboard.GetState().IsKeyDown(Keys.W))
                xRotation += gameTime.ElapsedGameTime.Milliseconds / 10f;
            else if(Keyboard.GetState().IsKeyDown(Keys.S))
                xRotation -= gameTime.ElapsedGameTime.Milliseconds / 10f;
            if (Keyboard.GetState().IsKeyDown(Keys.E))
                yRotation += gameTime.ElapsedGameTime.Milliseconds / 12f;
            else if (Keyboard.GetState().IsKeyDown(Keys.Q))
                yRotation -= gameTime.ElapsedGameTime.Milliseconds / 12f;
            if (Keyboard.GetState().IsKeyDown(Keys.I))
                zPos += gameTime.ElapsedGameTime.Milliseconds / 100f;
            else if (Keyboard.GetState().IsKeyDown(Keys.P))
                zPos -= gameTime.ElapsedGameTime.Milliseconds / 100f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            be.World = Matrix.Identity * Matrix.CreateRotationX(MathHelper.ToRadians(xRotation))
                * Matrix.CreateRotationY(MathHelper.ToRadians(yRotation)) * Matrix.CreateTranslation(0, 0, zPos);
            be.View = camera.View;
            be.Projection = camera.Projection;
            
            be.EnableDefaultLighting();
            be.LightingEnabled = true;

            renderScene();
            //renderWireframe();

            base.Draw(gameTime);
        }

        private void renderScene()
        {
            //primitiveTest.draw();

            foreach (EffectPass pass in be.CurrentTechnique.Passes)
            {
                pass.Apply();
                cube.draw();
            }
        }

        private void renderWireframe()
        {
            setupRasterisation(true);
            foreach (EffectPass pass in be.CurrentTechnique.Passes)
            {
                pass.Apply();
                cube.draw();
            }
            setupRasterisation(false);
        }

        private void setupRasterisation(bool isWireframe = false)
        {
            RasterizerState rasterState = new RasterizerState();
            rasterState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterState.FillMode = (isWireframe) ? FillMode.WireFrame : FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterState;
        }

        private void setupBasicEffect()
        {
            //instantiate what I assume is a shader object?
            be = new BasicEffect(GraphicsDevice);
            
            /*be.TextureEnabled = true;
            be.Texture = texture;
            be.LightingEnabled = true;
            be.EnableDefaultLighting();*/
            /*be.DirectionalLight0.Enabled = true;
            be.DirectionalLight0.Direction = new Vector3(-1, -1, 0);
            be.DirectionalLight0.DiffuseColor = Vector3.One;
            be.LightingEnabled = true;
            be.AmbientLightColor = Vector3.One / 4f;*/
            //setup the rendering data for this effect (shader I think...)
            //be.World = camera.World;
            //be.View = camera.View;
            //be.Projection = camera.Projection;
            //be.VertexColorEnabled = true;
        }

    }
}

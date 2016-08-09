using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MovingTextDemo;

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
        Texture2D texture;
        BasicEffect be;
        float xRotation, yRotation, scale;
        Time timer;
        //mouse movement variables
        Vector2 lastMousePos, currMousePos;
        int lastScrollPos;
        float mouseSensitivity = 10;
        
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
            camera = new Camera();
            camera.FieldOfView = 45;
            camera.setAspectRatio((float)graphics.PreferredBackBufferWidth, (float)graphics.PreferredBackBufferHeight);
            camera.setClippingPlanes(0.1f, 1000f);
            camera.Position = new Vector3(0, 0, 5);

            texture = Content.Load<Texture2D>("crate");
            cube = new TexturedCube();
            cube.initialise(GraphicsDevice, texture);

            //setup default values for rotation and scale matrices
            xRotation = yRotation = 0;
            scale = 1;

            //setup the shader to have lighting and texturing enabled
            setupBasicEffect();
            //setup backface culling
            setupRasterisation();
            //get an instance of the Time class to be able to start tracking deltaTime.
            timer = Time.Instance;
            //get the initial position of the scroll wheel
            lastScrollPos = Mouse.GetState().ScrollWheelValue;

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

            //track deltatime
            timer.tick(ref gameTime);
            //check for left mouse button or scroll wheel movement and move accordingly
            processMouse();
            //keep track of the mouse's last position
            lastMousePos = Mouse.GetState().Position.ToVector2();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            /*be.World = Matrix.Identity * Matrix.CreateRotationX(MathHelper.ToRadians(xRotation))
                * Matrix.CreateRotationY(MathHelper.ToRadians(yRotation)) * Matrix.CreateTranslation(0, 0, zPos);*/
            
            //create the matrices responsible for transforming the cube
            Matrix xRot = Matrix.CreateRotationX(MathHelper.ToRadians(xRotation));
            Matrix yRot = Matrix.CreateRotationY(MathHelper.ToRadians(yRotation));
            Matrix translation = Matrix.CreateTranslation(Vector3.Zero); //not necessary but there for completion sake.
            Matrix cubeScale = Matrix.CreateScale(scale);

            //set the current world/modelview matrix to be ready for the cube
            be.World = Matrix.Identity * xRot * yRot * cubeScale * translation;
            //render the cube
            renderScene();

            base.Draw(gameTime);
        }

        private void processMouse()
        {
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                currMousePos = Mouse.GetState().Position.ToVector2();
                Vector2 delta = currMousePos - lastMousePos;
                xRotation += delta.Y * timer.DeltaTime * mouseSensitivity;
                yRotation += delta.X * timer.DeltaTime * mouseSensitivity;
            }
            if (Mouse.GetState().ScrollWheelValue != lastScrollPos)
            {
                int scrollValue = Mouse.GetState().ScrollWheelValue;
                scale += timer.DeltaTime * MathHelper.Clamp(scrollValue - lastScrollPos, -1, 1) * mouseSensitivity;
                lastScrollPos = scrollValue;
            }
        }

        private void renderScene()
        {
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
            be.TextureEnabled = true;
            be.Texture = texture;
            be.LightingEnabled = true;
            be.EnableDefaultLighting();
            //setup the rendering data for this effect (shader I think...)
            be.World = camera.World;
            be.View = camera.View;
            be.Projection = camera.Projection;
        }

    }
}

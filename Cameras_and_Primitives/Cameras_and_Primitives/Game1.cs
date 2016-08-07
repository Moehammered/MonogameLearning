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
        PrimitiveMesh mesh;

        BasicEffect basicEffect;

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
            //create the camera
            camera = new Camera();
            camera.FieldOfView = 45;
            camera.setAspectRatio((float)graphics.PreferredBackBufferWidth, (float)graphics.PreferredBackBufferHeight);
            camera.setClippingPlanes(0.1f, 1000f);
            camera.Position = new Vector3(0, 0, 10);
            //create the mesh
            setupMesh();
            //sets up the culling property of the graphics card
            setupRasterisation();
            //create our basic effect(which I am thinking more and more is a shader or material)
            setupBasicEffect();

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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            /*foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                mesh.draw();
            }*/
            mesh.draw(basicEffect.CurrentTechnique.Passes);

            base.Draw(gameTime);
        }

        private void setupMesh()
        {
            mesh = new PrimitiveMesh(GraphicsDevice);

            //create the vertices of the mesh primitive to hold position and colour data
            VertexPositionColor[] vertices = new VertexPositionColor[3];
            //vertex winding order is Clockwise
            vertices[0] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(0.5f, 0, 0), Color.Blue);
            mesh.Vertices = vertices;

            //create the indices of the vertices to make up the primitive
            ushort[] indices = new ushort[3];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            mesh.Indices = indices;
        }

        private void setupRasterisation()
        {
            RasterizerState rasterState = new RasterizerState();
            rasterState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterState;
        }

        private void setupBasicEffect()
        {
            //instantiate what I assume is a shader object?
            basicEffect = new BasicEffect(GraphicsDevice);
            //setup the rendering data for this effect (shader I think...)
            basicEffect.World = camera.World;
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.VertexColorEnabled = true;
        }
    }
}

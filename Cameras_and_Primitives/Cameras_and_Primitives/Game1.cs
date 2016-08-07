using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cameras_and_Primitives
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public struct RenderMatrices
    {
        public Matrix world, projection, view;
    };

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //camera values
        RenderMatrices matrices;
        float FOV, aspectRatio, near, far;
        Vector3 camPosition;
        //primitive values and rendering
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        VertexPositionColor[] vertices;
        short[] indices;
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
            camPosition = new Vector3(0, 0, 10);
            FOV = 45;
            aspectRatio = (float)graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            near = 0.1f;
            far = 1000f;
            setupCamera(camPosition);
            setupVertexBuffer();
            setupIndexBuffer();
            //sets up the culling property of the graphics card
            setupRasterisation();
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
            drawPrimitive();
            base.Draw(gameTime);
        }

        private void setupCamera(Vector3 position)
        {
            matrices.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), aspectRatio, near, far);
            matrices.world = Matrix.CreateTranslation(Vector3.Zero);
            matrices.view = Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
        }

        private void setupVertexBuffer()
        {
            //create the vertices of the mesh primitive to hold position and colour data
            vertices = new VertexPositionColor[3];
            //vertex winding order is Clockwise
            vertices[0] = new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(0.5f, 0, 0), Color.Blue);
            //Create the vertexbuffer and supply the vertices to it
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);
        }

        private void setupIndexBuffer()
        {
            indices = new short[3];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), 3, BufferUsage.WriteOnly);
            indexBuffer.SetData<short>(indices);
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
            basicEffect.World = matrices.world;
            basicEffect.View = matrices.view;
            basicEffect.Projection = matrices.projection;
            basicEffect.VertexColorEnabled = true;
        }

        private void drawPrimitive()
        {
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;

            foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //no indices
                //GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 1);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cameras_and_Primitives
{
    class PrimitiveTestGame
    {
        Camera camera;
        StaticPrimitiveMesh mesh;
        StaticTriangleMesh triangle;
        StaticSquareMesh square;
        StaticHexagonMesh hexagon;
        StaticCubeMesh cube;
        BasicEffect basicEffect;
        float rot;

        GraphicsDevice GraphicsDevice;
        GraphicsDeviceManager graphics;

        public PrimitiveTestGame(GraphicsDevice device, GraphicsDeviceManager graphics)
        {
            this.GraphicsDevice = device;
            this.graphics = graphics;
            rot = 0;
        }

        public void initialise()
        {
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

            triangle = new StaticTriangleMesh(GraphicsDevice);

            square = new StaticSquareMesh(GraphicsDevice);
            square.setColour(Color.Blue);
            square.updateMesh();

            hexagon = new StaticHexagonMesh(GraphicsDevice);
            hexagon.setColour(Color.Purple);
            hexagon.Radius = 0.5f;
            hexagon.updateMesh();

            cube = new StaticCubeMesh(GraphicsDevice);
        }

        public void update(GameTime gameTime)
        {
            rot += gameTime.ElapsedGameTime.Milliseconds / 10;
        }

        public void draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            basicEffect.World = Matrix.Identity;
            // TODO: Add your drawing code here
            /*foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                mesh.draw();
            }*/
            mesh.draw(basicEffect.CurrentTechnique.Passes);
            //position and draw the triangle
            Vector3 pos = basicEffect.World.Translation;
            pos.X = -3;
            basicEffect.World *= Matrix.CreateTranslation(pos);
            triangle.draw(basicEffect.CurrentTechnique.Passes);
            //position and draw the square
            pos.Y = -3;
            basicEffect.World = Matrix.Identity;
            basicEffect.World *= Matrix.CreateTranslation(pos);
            square.draw(basicEffect.CurrentTechnique.Passes);
            //position and draw the hexagon
            pos.X = 3;
            basicEffect.World = Matrix.Identity;
            basicEffect.World *= Matrix.CreateTranslation(pos);
            hexagon.draw(basicEffect.CurrentTechnique.Passes);
        }

        private void setupMesh()
        {
            mesh = new StaticPrimitiveMesh(GraphicsDevice);

            //create the vertices of the mesh primitive to hold position and colour data
            VertexData[] vertices = new VertexData[3];
            //vertex winding order is Clockwise
            vertices[0] = new VertexData(new Vector3(-0.5f, 0, 0), Color.Red);
            vertices[1] = new VertexData(new Vector3(0, 1, 0), Color.Green);
            vertices[2] = new VertexData(new Vector3(0.5f, 0, 0), Color.Blue);
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
            rasterState.FillMode = FillMode.Solid;
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

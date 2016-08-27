using Cameras_and_Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPS_Movement.GameObjects
{
    class Plane : GameComponent
    {
        //world orientation
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        //visual representation of the plane
        private Texture2D texture;
        private StaticPrimitiveMesh mesh;
        private BasicEffect material;
        private Color colour;
        private Matrix world;

        public Plane(Game game) : base(game)
        {
            mesh = new StaticPrimitiveMesh(game.GraphicsDevice);
            material = new BasicEffect(game.GraphicsDevice);
            material.TextureEnabled = true;
            material.VertexColorEnabled = true;
            texture = new Texture2D(game.GraphicsDevice, 32, 32);
            colour = Color.White;
            position = Vector3.Zero;
            scale = Vector3.One;
            rotation = Quaternion.Identity;
            world = Matrix.Identity;
            
            constructMesh();
        }
        
        public StaticPrimitiveMesh Mesh
        {
            get { return mesh; }
        }

        public BasicEffect Material
        {
            get { return material; }
            set { material = value; }
        }

        public Color Colour
        {
            get { return colour; }
            set
            {
                colour = value;
                mesh.Vertices = makeVertices();
            }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                material.Texture = texture;
            }
        }

        public void draw(Matrix view, Matrix projection)
        {
            //material.World = Matrix.CreateTranslation(position);
            material.World = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position) * world;
            material.View = view;
            material.Projection = projection;
            foreach(EffectPass pass in material.CurrentTechnique.Passes)
            {
                pass.Apply();
                mesh.draw();
            }
        }

        #region Mesh Construction

        private void constructMesh()
        {
            mesh.Vertices = makeVertices();
            mesh.Indices = makeIndices();
        }

        private VertexData[] makeVertices()
        {
            VertexData[] verts = new VertexData[4]; //4 points on the plane
            //temp variable to make it easier to read
            VertexData dummy = new VertexData();
            //top left vertex (origin being at the center of the object)
            dummy.position = new Vector3(-0.5f, 0.5f, 0);
            dummy.colour = colour;
            dummy.uv = new Vector2(0, 0);
            dummy.normal = new Vector3(0, 0, 1);
            verts[0] = dummy;
            //top right vertex
            dummy.position.X = 0.5f;
            dummy.uv.X = 1;
            verts[1] = dummy;
            //bottom right vertex
            dummy.position.Y = -0.5f;
            dummy.uv.Y = 1;
            verts[2] = dummy;
            //bottom left vertex
            dummy.position.X = -0.5f;
            dummy.uv.X = 0;
            verts[3] = dummy;

            return verts;
        }

        private ushort[] makeIndices()
        {
            ushort[] inds = {
                0, 1, 3, 2
            };

            return inds;
        }
        #endregion
    }
}

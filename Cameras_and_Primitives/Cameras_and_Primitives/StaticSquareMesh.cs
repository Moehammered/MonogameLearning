using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    class StaticSquareMesh : StaticPrimitiveMesh
    {
        private float width, height;
        private Color[] colours;

        public StaticSquareMesh(GraphicsDevice gd) : base(gd)
        {
            width = 1;
            height = 1;
            colours = new Color[4] { Color.White, Color.White, Color.White, Color.White };
            initialiseSquare();
        }

        /// <summary>
        /// Returns the triangles vertices. They cannot be modified.
        /// </summary>
        public new VertexData[] Vertices
        {
            get
            {
                return vertices;
            }
        }

        /// <summary>
        /// Returns the indices of the vertices. They cannot be modified.
        /// </summary>
        public new ushort[] Indices
        {
            get
            {
                return indices;
            }
        }

        public float Width
        {
            get { return width; }
        }

        public float Height
        {
            get { return height; }
        }

        public Color[] Colours
        {
            get { return colours; }
        }

        public void setSize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Provide a colour for the whole triangle.
        /// </summary>
        /// <param name="colour"></param>
        public void setColour(Color colour)
        {
            for (int i = 0; i < colours.Length; i++)
            {
                colours[i] = colour;
            }
        }

        /// <summary>
        /// Provide a colour for each point of the triangle.
        /// ([0] = top left, [1] = top right, [2] = bottom right, [3] = bottom left)
        /// </summary>
        /// <param name="colours"></param>
        public void setColour(Color[] colours)
        {
            this.colours = colours;
        }

        /// <summary>
        /// Must be called for any changes to colour or size to take effect
        /// </summary>
        public void updateMesh()
        {
            //update the positions
            vertices[0].position = new Vector3(-width / 2f, height / 2f, 0);
            vertices[1].position = new Vector3(width / 2f, height / 2f, 0);
            vertices[2].position = new Vector3(width / 2f, -height / 2f, 0);
            vertices[3].position = new Vector3(-width / 2f, -height / 2f, 0);
            //update the colours
            vertices[0].colour = colours[0];
            vertices[1].colour = colours[1];
            vertices[2].colour = colours[2];
            vertices[3].colour = colours[3];

            buffer.SetData<VertexData>(vertices);
        }

        private void initialiseSquare()
        {
            VertexData[] verts = new VertexData[4];
            verts[0] = new VertexData(new Vector3(-width/2f, height / 2f, 0), colours[0]);
            verts[1] = new VertexData(new Vector3(width / 2f, height / 2f, 0), colours[1]);
            verts[2] = new VertexData(new Vector3(width / 2f, -height / 2f, 0), colours[2]);
            verts[3] = new VertexData(new Vector3(-width / 2f, -height / 2f, 0), colours[3]);

            ushort[] ind = new ushort[4] {
                3, 0, 2,    //face 1
                1           //face 2
            };

            base.Vertices = verts;
            base.Indices = ind;
        }
    }
}

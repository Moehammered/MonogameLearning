using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    /// <summary>
    /// A simple Triangle mesh using the PrimitiveMesh class as a base.
    /// Can be sized and coloured without the need to re-supply vertices.
    /// </summary>
    class TriangleMesh : PrimitiveMesh
    {
        private float width, height;
        private Color[] colours;

        public TriangleMesh(GraphicsDevice gd) : base(gd)
        {
            width = 1;
            height = 1;
            colours = new Color[3] { Color.White, Color.White, Color.White };
            initialiseTriangle();
        }

        /// <summary>
        /// Returns the triangles vertices. They cannot be modified.
        /// </summary>
        public new VertexPositionColor[] Vertices
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
            for(int i = 0; i < colours.Length; i++)
            {
                colours[i] = colour;
            }
        }

        /// <summary>
        /// Provide a colour for each point of the triangle.
        /// ([0] = top, [1] = bottom right, [2] = bottom left)
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
            vertices[0].Position = new Vector3(0, height / 2f, 0);
            vertices[1].Position = new Vector3(width / 2f, -height / 2f, 0);
            vertices[2].Position = new Vector3(-width / 2f, -height / 2f, 0);
            //update the colours
            vertices[0].Color = colours[0];
            vertices[1].Color = colours[1];
            vertices[2].Color = colours[2];

            buffer.SetData<VertexPositionColor>(vertices);
        }

        private void initialiseTriangle()
        {
            VertexPositionColor[] verts = new VertexPositionColor[3];
            verts[0] = new VertexPositionColor(new Vector3(0, height / 2f, 0), colours[0]);
            verts[1] = new VertexPositionColor(new Vector3(width / 2f, -height / 2f, 0), colours[1]);
            verts[2] = new VertexPositionColor(new Vector3(-width / 2f, -height / 2f, 0), colours[2]);

            ushort[] ind = new ushort[3] { 0, 1, 2 };

            base.Vertices = verts;
            base.Indices = ind;
        }
    }
}

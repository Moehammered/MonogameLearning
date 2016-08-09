using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    class StaticHexagonMesh : StaticPrimitiveMesh
    {
        private float radius;
        private Color[] colours;

        public StaticHexagonMesh(GraphicsDevice gd) : base(gd)
        {
            radius = 1;
            colours = new Color[6] { Color.White, Color.White, Color.White, Color.White, Color.White, Color.White };
            initialiseHexagon();
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

        public float Radius
        {
            get { return radius; }
            set { radius = Math.Abs(value); }
        }

        public Color[] Colours
        {
            get { return colours; }
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
        /// ([0] = left, [1] = top left, [2] = top, [3] = top right, [4] = right, [5] = bottom right, [6] = bottom)
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
            float angle = 180;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (angle < 0)
                    angle += 360;

                vertices[i].position.X = radius * (float)Math.Cos(MathHelper.ToRadians(angle));
                vertices[i].position.Y = radius * (float)Math.Sin(MathHelper.ToRadians(angle));
                vertices[i].colour = colours[i];
                angle -= 60;
            }
            
            buffer.SetData<VertexData>(vertices);
        }

        private void initialiseHexagon()
        {
            VertexData[] verts = new VertexData[6];
            //setup the points starting from left and circling to the right
            float angle = 180;
            for (int i = 0; i < verts.Length; i++)
            {
                if (angle < 0)
                    angle += 360;

                Vector3 point = new Vector3(
                    radius * (float)Math.Cos(MathHelper.ToRadians(angle)), 
                    radius * (float)Math.Sin(MathHelper.ToRadians(angle)), 
                    0);
                verts[i] = new VertexData(point, colours[i]);
                System.Diagnostics.Trace.WriteLine("Angle: " + angle + " Point: " + point.ToString());
                angle -= 60;
            }

            ushort[] ind = new ushort[6] {
                0, 1, 5,    //face 1 (left triangle)
                2,          //face 2 (top center triangle)
                4,          //face 3 (bottom center triangle)
                3           //face 4 (right triangle)
            };

            base.Vertices = verts;
            base.Indices = ind;
        }
    }
}

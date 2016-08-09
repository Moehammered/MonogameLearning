using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    class StaticCubeMesh : StaticPrimitiveMesh
    {
        private float width, height, depth;
        private Color[] colours;

        public StaticCubeMesh(GraphicsDevice gd) : base (gd)
        {
            width = 1;
            height = 1;
            depth = 1;
            initialiseCube();
            useTriangleList();
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

        public void setSize(float size)
        {
            this.width = size;
            this.height = size;
            this.depth = size;
        }

        public void setSize(float width, float height, float depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        /// <summary>
        /// Provide a colour for the whole cube.
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
        /// Provide a colour for each point vertex of the cube.
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
            //update the position and colour
            int corner = 0;
            Vector3 point = Vector3.Zero;
            for (int i = 0; i < vertices.Length; i++, corner++)
            {
                colours[i] = Color.White; //default colour of cube
                float x = 0, y = 0, z = 0;

                z = (i < 4) ? depth / 2f : -depth / 2f;

                switch (corner)
                {
                    case 0: //top left
                        x = -width / 2f;
                        y = height / 2f;
                        break;
                    case 1: //top right
                        x = width / 2f;
                        y = height / 2f;
                        break;
                    case 2: //bottom right
                        x = width / 2f;
                        y = -height / 2f;
                        break;
                    case 3: //bottom left
                        x = -width / 2f;
                        y = -height / 2f;
                        corner = -1; //reset corner value for back faces now
                        break;
                    default:
                        //shouldn't reach here
                        System.Diagnostics.Trace.WriteLine("Corner value invalid: " + corner);
                        break;
                }

                point.X = x;
                point.Y = y;
                point.Z = z;

                vertices[i].position = point;
                vertices[i].colour = colours[i];
            }

            buffer.SetData<VertexData>(vertices);
        }

        private void initialiseCube()
        {
            VertexData[] verts = new VertexData[8];
            colours = new Color[8];

            int corner = 0;
            Vector3 point = Vector3.Zero;
            for(int i = 0; i < verts.Length; i++, corner++)
            {
                colours[i] = Color.White; //default colour of cube
                float x = 0, y = 0, z = 0;

                z = (i < 4) ? depth/2f : -depth/2f;

                switch(corner)
                {
                    case 0: //top left
                        x = -width / 2f;
                        y = height / 2f;
                        break;
                    case 1: //top right
                        x = width / 2f;
                        y = height / 2f;
                        break;
                    case 2: //bottom right
                        x = width / 2f;
                        y = -height / 2f;
                        break;
                    case 3: //bottom left
                        x = -width / 2f;
                        y = -height / 2f;
                        corner = -1; //reset corner value for back faces now
                        break;
                    default:
                        //shouldn't reach here
                        System.Diagnostics.Trace.WriteLine("Corner value invalid: " + corner);
                        break;
                }

                point.X = x;
                point.Y = y;
                point.Z = z;

                verts[i] = new VertexData(point, Color.White);
            }

            /*
                Front
                0 - 3 = TL, TR, BR, BL
                Back
                4 - 7 = TL, TR, BR, BL
            */
            ushort[] ind = new ushort[36] {
                0, 1, 3, 1, 2, 3,   //front face
                1, 5, 2, 5, 6, 2,   //right face
                5, 4, 6, 4, 7, 6,   //back face
                4, 0, 3, 4, 3, 7,   //left face
                4, 5, 0, 5, 1, 0,   //top face
                3, 2, 6, 3, 6, 7    //bottom face
            };

            base.Vertices = verts;
            base.Indices = ind;
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    class TexturedCube
    {
        private float width, height, depth;
        public StaticPrimitiveMesh mesh;
        //Not used yet
        private Texture2D texture;

        public TexturedCube()
        {
            width = height = depth = 1;
        }

        public void initialise(GraphicsDevice gd, Texture2D texture)
        {
            mesh = new StaticPrimitiveMesh(gd);
            mesh.useTriangleList();
            VertexData[] vertices = new VertexData[36];
            Vector3[] positions = buildPoint();
            Vector3[] normals = buildNormals();
            Vector2[] uvs = buildUVs();

            Color colour = Color.White;

            for(int i = 0; i < vertices.Length; i++)
            {
                switch (i)
                {
                    case 0: //front
                        colour = Color.White;
                        break;
                    case 6: //back
                        colour = Color.Red;
                        break;
                    case 12: //right
                        colour = Color.Blue;
                        break;
                    case 18: //left
                        colour = Color.Yellow;
                        break;
                    case 24: //top
                        colour = Color.Green;
                        break;
                    case 30: //bottom
                        colour = Color.Gray;
                        break;
                }
                vertices[i] = new VertexData(positions[i], normals[i], uvs[i]);
                vertices[i].colour = colour;
            }

            mesh.Vertices = vertices;
            mesh.Indices = buildIndices();
            this.texture = texture;
        }

        public void draw()
        {
            mesh.draw();
        }

        private Vector3[] buildPoint()
        {
            //half sizes
            float halfWidth, halfHeight, halfDepth;
            halfWidth = width / 2f;
            halfHeight = height / 2f;
            halfDepth = depth / 2f;
            //create the unique points first
            //front facing verts
            Vector3 FTL, FTR, FBR, FBL;
            FTL = new Vector3(-halfWidth, halfHeight, halfDepth);
            FTR = new Vector3(halfWidth, halfHeight, halfDepth);
            FBR = new Vector3(halfWidth, -halfHeight, halfDepth);
            FBL = new Vector3(-halfWidth, -halfHeight, halfDepth);
            //back facing verts (orientated same as front)
            Vector3 BTL, BTR, BBR, BBL;
            BTL = new Vector3(-halfWidth, halfHeight, -halfDepth);
            BTR = new Vector3(halfWidth, halfHeight, -halfDepth);
            BBR = new Vector3(halfWidth, -halfHeight, -halfDepth);
            BBL = new Vector3(-halfWidth, -halfHeight, -halfDepth);
            //build the positions of each vertex now
            List<Vector3> points = new List<Vector3>(36);

            for(int i = 0; i < points.Capacity; i+=6)
            {
                switch(i)
                {
                    case 0:     //construct front face
                        points.InsertRange(points.Count, buildFace(FTL, FTR, FBR, FBL));
                        break;
                    case 6:     //construct back face
                        points.InsertRange(points.Count, buildFace(BTR, BTL, BBL, BBR));
                        break;
                    case 12:    //construct right face
                        points.InsertRange(points.Count, buildFace(FTR, BTR, BBR, FBR));
                        break;
                    case 18:    //construct left face
                        points.InsertRange(points.Count, buildFace(BTL, FTL, FBL, BBL));
                        break;
                    case 24:    //construct top face
                        points.InsertRange(points.Count, buildFace(BTL, BTR, FTR, FTL));
                        break;
                    case 30:    //construct bottom face
                        points.InsertRange(points.Count, buildFace(FBL, FBR, BBR, BBL));
                        break;
                    default:    //something went wrong
                        break;
                }
            }

            return points.ToArray();
        }

        private ushort[] buildIndices()
        {
            ushort[] indices = new ushort[36];
            for(ushort i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            return indices;
        }

        private Vector3[] buildNormals()
        {
            Vector3 frontFace = new Vector3(0, 0, 1);
            Vector3 rightFace = new Vector3(1, 0, 0);
            Vector3 topFace = new Vector3(0, 1, 0);

            Vector3[] normals = new Vector3[36];
            for(int i = 0; i < normals.Length; i++)
            {
                Vector3 normal = Vector3.One;

                //check which face we are on
                if (i < 6)
                    normal = frontFace;
                else if (i < 12)
                    normal = -frontFace;
                else if (i < 18)
                    normal = rightFace;
                else if (i < 24)
                    normal = -rightFace;
                else if (i < 30)
                    normal = topFace;
                else
                    normal = -topFace;
                normals[i] = normal;
            }

            return normals;
        }

        private Vector2[] buildUVs()
        {
            List<Vector2> uvs = new List<Vector2>(36);
            //fill in the gap.
            float xSegment = 1 / 2f;
            float ySegment = 1 / 2f;
            Vector2 uvStart = Vector2.Zero;
            Vector2 uvEnd = Vector2.Zero;

            for(int i = 0; i < uvs.Capacity; i+=6)
            {
                switch(i)
                {
                    case 0:
                        uvEnd.X = xSegment;
                        uvEnd.Y = ySegment;
                        break;
                    case 12:
                        uvStart.X = xSegment;
                        uvEnd.X = xSegment * 2;
                        break;
                    case 24:
                        uvStart.X = 0;
                        uvStart.Y = ySegment;

                        uvEnd.X = xSegment;
                        uvEnd.Y = ySegment * 2;
                        break;
                    case 30:
                        uvStart.X = xSegment;
                        uvEnd.X = xSegment * 2;
                        break;
                    default:
                        break;
                }

                uvs.InsertRange(uvs.Count, buildFaceUVS(uvStart, uvEnd));
            }

            return uvs.ToArray();
        }

        private Vector3[] buildFace(Vector3 tl, Vector3 tr, Vector3 br, Vector3 bl)
        {
            Vector3[] points = new Vector3[6];
            //triangle 1
            points[0] = tl;
            points[1] = tr;
            points[2] = bl;
            //triangle 2
            points[3] = tr;
            points[4] = br;
            points[5] = bl;

            return points;
        }

        private Vector2[] buildFaceUVS(Vector2 start, Vector2 end)
        {
            Vector2[] uvs = new Vector2[6];
            //triangle 1 - left tri
            uvs[0] = start;
            uvs[1] = new Vector2(end.X, start.Y);
            uvs[2] = new Vector2(start.X, end.Y);
            //triangle 2 - right tri
            uvs[3] = uvs[1];
            uvs[4] = end;
            uvs[5] = uvs[2];

            return uvs;
        }
    }
}

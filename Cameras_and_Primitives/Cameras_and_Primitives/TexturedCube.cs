using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    class TexturedCube
    {
        private float width, height, depth;
        private StaticPrimitiveMesh mesh;
        private Texture2D texture;

        public TexturedCube()
        {
            width = height = depth = 1;
        }

        public void initialise(GraphicsDevice gd, Texture2D texture)
        {
            mesh = new StaticPrimitiveMesh(gd);
            //buildVertices();
            //buildIndices();
            this.texture = texture;
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
                        points.InsertRange(points.Count, buildFace(BTL, FTL, FBR, BBR));
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
            for(ushort i = 0; i < 36; i++)
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
                Vector3 normal = Vector3.Zero;

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
            Vector2[] uvs = new Vector2[36];

            //fill in the gap.

            return uvs;
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
    }
}

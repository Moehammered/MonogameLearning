using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameLearning.Graphics
{
    static class PrimitiveShape
    {
        #region Static Plane Mesh (XY plane)

        public static StaticMesh CreateXYPlane()
        {
            StaticMesh mesh;
            mesh = new StaticMesh();
            mesh.Vertices = makePlaneXYVertices(Color.White);
            mesh.Indices = makePlaneXYIndices();

            return mesh;
        }

        private static VertexData[] makePlaneXYVertices(Color colour)
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

        private static ushort[] makePlaneXYIndices()
        {
            ushort[] inds = {
                0, 1, 3, 2
            };

            return inds;
        }

        #endregion

        #region Static Cube Mesh

        public static StaticMesh CreateCube()
        {
            StaticMesh mesh;
            float width, height, depth;

            mesh = new StaticMesh();
            width = height = depth = 1;
            mesh.useTriangleList();
            VertexData[] vertices = new VertexData[36];
            Vector3[] positions = buildCubePoints(width, height, depth);
            Vector3[] normals = buildCubeNormals();
            Vector2[] uvs = buildCubeUVs();
            Color colour = Color.White;

            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new VertexData(positions[i], normals[i], uvs[i], colour);
            }

            mesh.Vertices = vertices;
            mesh.Indices = buildCubeIndices();

            return mesh;
        }

        private static Vector3[] buildCubePoints(float width, float height, float depth)
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

            for (int i = 0; i < points.Capacity; i += 6)
            {
                switch (i)
                {
                    case 0:     //construct front face
                        points.InsertRange(points.Count, buildCubeFace(FTL, FTR, FBR, FBL));
                        break;
                    case 6:     //construct back face
                        points.InsertRange(points.Count, buildCubeFace(BTR, BTL, BBL, BBR));
                        break;
                    case 12:    //construct right face
                        points.InsertRange(points.Count, buildCubeFace(FTR, BTR, BBR, FBR));
                        break;
                    case 18:    //construct left face
                        points.InsertRange(points.Count, buildCubeFace(BTL, FTL, FBL, BBL));
                        break;
                    case 24:    //construct top face
                        points.InsertRange(points.Count, buildCubeFace(BTL, BTR, FTR, FTL));
                        break;
                    case 30:    //construct bottom face
                        points.InsertRange(points.Count, buildCubeFace(FBL, FBR, BBR, BBL));
                        break;
                    default:    //something went wrong
                        break;
                }
            }

            return points.ToArray();
        }

        private static ushort[] buildCubeIndices()
        {
            ushort[] indices = new ushort[36];
            for (ushort i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            return indices;
        }

        private static Vector3[] buildCubeNormals()
        {
            Vector3 frontFace = new Vector3(0, 0, 1);
            Vector3 rightFace = new Vector3(1, 0, 0);
            Vector3 topFace = new Vector3(0, 1, 0);

            Vector3[] normals = new Vector3[36];
            for (int i = 0; i < normals.Length; i++)
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

        private static Vector2[] buildCubeUVs()
        {
            List<Vector2> uvs = new List<Vector2>(36);
            //fill in the gap.
            float xSegment = 1 / 2f;
            float ySegment = 1 / 2f;
            Vector2 uvStart = Vector2.Zero;
            Vector2 uvEnd = Vector2.Zero;

            for (int i = 0; i < uvs.Capacity; i += 6)
            {
                switch (i)
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

                uvs.InsertRange(uvs.Count, buildCubeFaceUVS(uvStart, uvEnd));
            }

            return uvs.ToArray();
        }

        private static Vector3[] buildCubeFace(Vector3 tl, Vector3 tr, Vector3 br, Vector3 bl)
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

        private static Vector2[] buildCubeFaceUVS(Vector2 start, Vector2 end)
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

        #endregion
    }
}

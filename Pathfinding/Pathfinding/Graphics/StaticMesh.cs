using Microsoft.Xna.Framework.Graphics;

namespace MonogameLearning.Graphics
{
    class StaticMesh
    {
        protected VertexData[] vertices;
        protected ushort[] indices;
        protected ushort vertexCount;
        protected ushort indexCount;
        protected int triangleCount;
        protected PrimitiveType triangleType = PrimitiveType.TriangleStrip;

        public StaticMesh()
        {
            vertices = new VertexData[0];
            indices = new ushort[0];
        }

        public PrimitiveType TriangleType
        {
            get {return triangleType; }
        }

        public VertexData[] Vertices
        {
            get { return vertices; }
            set
            {
                vertices = value;
                if (value != null)
                {
                    vertexCount = (ushort)vertices.Length;
                }
                else
                {
                    vertexCount = 0;
                }
            }
        }

        /// <summary>
        /// The indices of the vertices to be used when rendering.
        /// If rendering as a triangle strip, back-face culling is flipped automatically for every 2nd triangle.
        /// I.E. You do not have to account for back face winding or use de-generative triangles to order vertices.
        /// If rendering as a triangle list, each triangle must be ordered correctly.
        /// </summary>
        public ushort[] Indices
        {
            get { return indices; }
            set
            {
                indices = value;
                if (value != null)
                {
                    indexCount = (ushort)indices.Length;
                    triangleCount = (triangleType == PrimitiveType.TriangleList) ? indexCount / 3 : indexCount - 2;
                }
                else
                {
                    indexCount = 0;
                    triangleCount = 0;
                }
            }
        }

        public ushort VertexCount
        {
            get { return vertexCount; }
        }

        public ushort IndexCount
        {
            get { return indexCount; }
        }

        public int TriangleCount
        {
            get { return triangleCount; }
        }

        public void useTriangleStrip()
        {
            triangleType = PrimitiveType.TriangleStrip;
        }

        public void useTriangleList()
        {
            triangleType = PrimitiveType.TriangleList;
        }
    }
}

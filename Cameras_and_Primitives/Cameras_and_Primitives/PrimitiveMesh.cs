using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    /// <summary>
    /// A class designed to be able to render mesh primitives via user specified vertex data.
    /// Has a vertex limit of the maximum value of an unsigned short(65535).
    /// For now it uses coloured vertices, but later will be updated to use UV's and normals too.
    /// </summary>
    class PrimitiveMesh
    {
        protected VertexBuffer buffer;
        protected IndexBuffer indexBuffer;

        protected VertexPositionColor[] vertices;
        protected ushort[] indices;
        protected ushort vertexCount;
        protected ushort indexCount;
        protected int triangleCount;

        protected GraphicsDevice gfxDevice;
        protected PrimitiveType triangleType = PrimitiveType.TriangleStrip;

        /// <summary>
        /// Instantiates a primitive mesh with no data specified
        /// </summary>
        public PrimitiveMesh(GraphicsDevice gd)
        {
            gfxDevice = gd;
            vertices = new VertexPositionColor[0];
            indices = new ushort[0];
        }

        public virtual VertexPositionColor[] Vertices
        {
            get { return vertices; }
            set
            {
                vertices = value;
                if(value != null)
                {
                    vertexCount = (ushort)vertices.Length;
                    buffer = new VertexBuffer(gfxDevice, typeof(VertexPositionColor), vertexCount, BufferUsage.WriteOnly);
                    buffer.SetData<VertexPositionColor>(vertices);
                }
                else
                {
                    vertexCount = 0;
                    buffer = null;
                }
            }
        }

        /// <summary>
        /// The indices of the vertices to be used when rendering.
        /// If rendering as a triangle strip, back-face culling is flipped automatically for every 2nd triangle.
        /// I.E. You do not have to account for back face winding or use de-generative triangles to order vertices.
        /// If rendering as a triangle list, each triangle must be ordered correctly.
        /// </summary>
        public virtual ushort[] Indices
        {
            get { return indices; }
            set
            {
                indices = value;
                if(value != null)
                {
                    indexCount = (ushort)indices.Length;
                    triangleCount = (triangleType == PrimitiveType.TriangleList) ? indexCount / 3 : indexCount - 2;
                    indexBuffer = new IndexBuffer(gfxDevice, typeof(ushort), indexCount, BufferUsage.WriteOnly);
                    indexBuffer.SetData<ushort>(indices);
                }
                else
                {
                    indexCount = 0;
                    triangleCount = 0;
                    indexBuffer = null;
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

        /// <summary>
        /// Allows the mesh to change which graphics device is used to render and store it's vertex data.
        /// </summary>
        /// <param name="gd"></param>
        public void updateGraphicsDevice(GraphicsDevice gd)
        {
            gfxDevice = gd;
        }

        public void useTriangleStrip()
        {
            triangleType = PrimitiveType.TriangleStrip;
        }

        public void useTriangleList()
        {
            triangleType = PrimitiveType.TriangleList;
        }

        /// <summary>
        /// Binds the vertex and index buffers of the primitive then draws using the graphics device referenced by this object.
        /// </summary>
        public virtual void draw()
        {
            gfxDevice.SetVertexBuffer(buffer);
            gfxDevice.Indices = indexBuffer;

            gfxDevice.DrawIndexedPrimitives(triangleType, 0, 0, triangleCount);
        }

        /// <summary>
        /// Binds the vertex and index buffers then draws. Applies effects to the rendering of the primitive.
        /// </summary>
        /// <param name="effects"></param>
        public virtual void draw(EffectPassCollection effects)
        {
            gfxDevice.SetVertexBuffer(buffer);
            gfxDevice.Indices = indexBuffer;

            foreach (EffectPass pass in effects)
            {
                pass.Apply();
                gfxDevice.DrawIndexedPrimitives(triangleType, 0, 0, triangleCount);
            }
        }
    }
}

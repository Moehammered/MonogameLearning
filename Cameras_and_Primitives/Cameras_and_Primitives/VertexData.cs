using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cameras_and_Primitives
{
    /// <summary>
    /// A vertex type decleration to be used with VertexBuffers. Can contain position, normal, uv, and colour data.
    /// </summary>
    public struct VertexData : IVertexType
    {
        //The data each vertex carries
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
        public Color colour;

        //a decleration of what is contained within the Vertex and it's formatting
        public readonly static VertexDeclaration Decleration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(32, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );

        /// <summary>
        /// Sets the position of this vertex and the colour to White. Everything else is 0.
        /// </summary>
        /// <param name="position"></param>
        public VertexData(Vector3 position)
        {
            this.position = position;
            this.normal = Vector3.Zero;
            this.uv = Vector2.Zero;
            this.colour = Color.White;
        }

        /// <summary>
        /// Set the position of this vertex and it's normal. The Colour is white, and uv's are set to Zero.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        public VertexData(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
            this.uv = Vector2.Zero;
            this.colour = Color.White;
        }

        /// <summary>
        /// Set the position, normal, and uv's of this vertex. The Colour is white.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="uv"></param>
        public VertexData(Vector3 position, Vector3 normal, Vector2 uv)
        {
            this.position = position;
            this.normal = normal;
            this.uv = uv;
            this.colour = Color.White;
        }

        /// <summary>
        /// Set the position, normal,uv, and colour of this vertex.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="uv"></param>
        /// <param name="colour"></param>
        public VertexData(Vector3 position, Vector3 normal, Vector2 uv, Color colour)
        {
            this.position = position;
            this.normal = normal;
            this.uv = uv;
            this.colour = colour;
        }

        /// <summary>
        /// Set the position and colour of the vertex. Normal and UV are zero.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="colour"></param>
        public VertexData(Vector3 position, Color colour)
        {
            this.position = position;
            this.normal = Vector3.Zero;
            this.uv = Vector2.Zero;
            this.colour = colour;
        }

        //public Vector3 Position
        //{
        //    get { return position; }
        //    set { position = value; }
        //}

        //public Vector2 Normal
        //{
        //    get { return normal; }
        //    set { normal = value; }
        //}

        //public Vector2 UV
        //{
        //    get { return uv; }
        //    set { uv = value; }
        //}

        //public Color Colour
        //{
        //    get { return colour; }
        //    set { colour = value; }
        //}

        public override string ToString()
        {
            string formatted = "Pos[" + position + "] | ";
            formatted += "Norm[" + normal + "] | ";
            formatted += "UV[" + uv + "] | ";
            formatted += "Col[" + colour + "]";
            return formatted;
        }

        //The accessor method of this interface that is used to figure out what this vertex looks like
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return Decleration; }
        }
    }
}

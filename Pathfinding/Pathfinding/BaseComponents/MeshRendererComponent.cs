using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.Graphics;

namespace MonogameLearning.BaseComponents
{
    class MeshRendererComponent : RenderComponent
    {
        public Color colour;

        protected VertexBuffer buffer;
        protected IndexBuffer indexBuffer;
        protected StaticMesh mesh;
        protected BasicEffect material;
        
        public MeshRendererComponent() : base()
        {
            colour = Color.White;
        }

        public override void Initialize()
        {
            material = new BasicEffect(GraphicsDevice);
            material.VertexColorEnabled = true;
            material.DiffuseColor = colour.ToVector3();

            //base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            if(mesh != null)
            {
                GraphicsDevice.SetVertexBuffer(buffer);
                GraphicsDevice.Indices = indexBuffer;
                material.World = Matrix.CreateScale(owner.transform.Scale)
                    * Matrix.CreateFromQuaternion(owner.transform.Rotation) 
                    * Matrix.CreateTranslation(owner.transform.Position);
                material.View = Camera.mainCamera.View;
                material.Projection = Camera.mainCamera.Projection;
                foreach (EffectPass pass in material.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawIndexedPrimitives(mesh.TriangleType, 0, 0, mesh.TriangleCount);
                }
            }
            //base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            //does nothing, might reconsider inheritance hierarchy
        }

        public StaticMesh Mesh
        {
            get { return Mesh; }
            set
            {
                if (value != null)
                {
                    mesh = value;
                    //set vertex data
                    buffer = new VertexBuffer(GraphicsDevice, typeof(VertexData), mesh.VertexCount, BufferUsage.WriteOnly);
                    buffer.SetData<VertexData>(mesh.Vertices);
                    //set index data
                    indexBuffer = new IndexBuffer(GraphicsDevice, typeof(ushort), mesh.IndexCount, BufferUsage.WriteOnly);
                    indexBuffer.SetData<ushort>(mesh.Indices);
                }
            }
        }

        public BasicEffect Material
        {
            get { return material; }
            set { if (value != null) material = value; }
        }
    }
}

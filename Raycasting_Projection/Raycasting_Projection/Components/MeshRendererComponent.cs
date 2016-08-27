﻿using Cameras_and_Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raycasting_Projection.GameObjects;

namespace Raycasting_Projection.Components
{
    class MeshRendererComponent : DrawableGameComponent
    {
        public Color colour;
        public GameObject owner;

        protected VertexBuffer buffer;
        protected IndexBuffer indexBuffer;
        protected StaticMesh mesh;
        protected BasicEffect material;
        
        public MeshRendererComponent(Game g) : base(g)
        {
            //do nothing
        }

        public override void Initialize()
        {
            colour = Color.White;
            material = new BasicEffect(GraphicsDevice);
            material.VertexColorEnabled = true;
            material.DiffuseColor = colour.ToVector3();

            base.Initialize();
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
            base.Draw(gameTime);
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
    }
}

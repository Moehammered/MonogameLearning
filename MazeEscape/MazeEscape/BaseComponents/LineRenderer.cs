﻿using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonogameLearning.BaseComponents
{
    class LineRenderer : RenderComponent
    {
        private Color colour;
        private BasicEffect material;
        private VertexPositionColor[] vertexBuffer;
        private VertexBuffer buffer;
        private IndexBuffer indexBuffer;
        private Vector3[] vertices;
        private ushort[] indices;

        public LineRenderer() : base()
        {
        }

        public Color Colour
        {
            get
            {
                return colour;
            }
            set
            {
                colour = value;
                if (material != null)
                    material.DiffuseColor = colour.ToVector3();
                else
                {
                    material = new BasicEffect(GraphicsDevice);
                    material.DiffuseColor = colour.ToVector3();
                }
            }
        }

        public Vector3[] Vertices
        {
            get { return vertices; }
            set
            {
                vertices = value;
                if(vertices != null && vertices.Length > 0)
                {
                    updateVertexBuffer();
                    updateIndices(vertices.Length);
                }
                else
                {
                    vertexBuffer = null;
                    indexBuffer = null;
                    buffer = null;
                }
            }
        }

        private void updateVertexBuffer()
        {
            buffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer = new VertexPositionColor[vertices.Length];
            for(int i = 0; i < vertexBuffer.Length; i++)
            {
                vertexBuffer[i] = new VertexPositionColor(vertices[i], Color.White);
            }
            buffer.SetData(vertexBuffer);
        }

        private void updateIndices(int size)
        {
            indices = new ushort[size];
            for (ushort i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }
            indexBuffer = new IndexBuffer(GraphicsDevice, typeof(ushort), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        public override void Initialize()
        {
            if(material == null)
                material = new BasicEffect(GraphicsDevice);
            material.VertexColorEnabled = true;
            material.DiffuseColor = colour.ToVector3();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (buffer != null && indices.Length > 1)
            {
                GraphicsDevice.SetVertexBuffer(buffer);
                GraphicsDevice.Indices = indexBuffer;

                material.World = Camera.mainCamera.World;
                material.View = Camera.mainCamera.View;
                material.Projection = Camera.mainCamera.Projection;

                foreach(EffectPass pass in material.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineStrip, 0, 0, indices.Length - 1);
                }
            }
        }

        public override void Destroy()
        {
        }
    }
}

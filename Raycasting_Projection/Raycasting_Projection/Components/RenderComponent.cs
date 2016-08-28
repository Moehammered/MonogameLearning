using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raycasting_Projection.Components
{
    abstract class RenderComponent : Component, IDrawable
    {
        protected int drawOrder;
        protected bool visible;
        
        public RenderComponent() : base()
        {
            visible = true;
            drawOrder = 0;
        }

        public int DrawOrder
        {
            get { return drawOrder; }
            set
            {
                if (value != drawOrder)
                    drawOrder = value;
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                if (value != visible)
                    visible = value;
            }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return gameInstance.GraphicsDevice; }
        }

        //Need to figure out how to implement these.
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public abstract void Draw(GameTime gameTime);
    }
}

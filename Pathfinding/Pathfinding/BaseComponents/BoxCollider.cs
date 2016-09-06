using Arrrive_Pursue_Behaviour.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrrive_Pursue_Behaviour.BaseComponents
{
    class BoxCollider : ColliderComponent
    {
        private BoundingBox unscaledBounds;
        private BoundingBox scaledBounds;
        
        public BoxCollider() : base()
        {
            unscaledBounds = new BoundingBox();
            scaledBounds = new BoundingBox();
            
        }

        public override void Initialize()
        {
            CollisionDetector det = GameInstance.Services.GetService<CollisionDetector>();
            det.addCollider(this);
        }

        public BoundingBox UnScaledBounds
        {
            get { return unscaledBounds; }
            set
            {
                unscaledBounds = value;
                scaleBounds();
            }
        }

        public BoundingBox ScaledBounds
        {
            get { return scaledBounds; }
        }

        public BoundingBox Bounds
        {
            get
            {
                BoundingBox curr = scaledBounds;
                curr.Min += owner.transform.Position;
                curr.Max += owner.transform.Position;
                return curr;
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public bool Intersects(BoundingBox box)
        {
            return Bounds.Intersects(box);
        }

        private void scaleBounds()
        {
            scaledBounds.Min = unscaledBounds.Min * owner.transform.Scale;
            scaledBounds.Max = unscaledBounds.Max * owner.transform.Scale;
        }
    }
}

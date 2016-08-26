using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raycasting_Projection.Utilities
{
    public struct RaycastResult
    {
        public float distance;
        public Vector3 contactPoint;
    }

    public class Raycast
    {
        private GraphicsDevice gd;
        private Matrix world, view, projection;

        public Raycast(GraphicsDevice gd)
        {
            this.gd = gd;
        }

        public void setupMatrices(Matrix world, Matrix view, Matrix projection)
        {
            this.world = world;
            this.view = view;
            this.projection = projection;
        }

        public bool cast(Vector2 mousePos, BoundingBox collider, out RaycastResult result)
        {
            Vector3 mouseNear, mouseFar, origin, end, dir;
            result = new RaycastResult();

            mouseNear = new Vector3(mousePos, 0);
            mouseFar = new Vector3(mousePos, 1);

            origin = gd.Viewport.Unproject(mouseNear, projection, view, world);
            end = gd.Viewport.Unproject(mouseFar, projection, view, world);
            dir = end - origin;
            dir.Normalize();

            Ray ray = new Ray(origin, dir);
            float? distance = ray.Intersects(collider);

            if(distance != null)
            {
                result.contactPoint = dir * distance.Value + origin;
                result.distance = distance.Value;
                return true;
            }

            return false;
        }
    }
}

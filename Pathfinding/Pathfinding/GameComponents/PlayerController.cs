using MonogameLearning.BaseComponents;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameLearning.Utilities;
using MazeEscape.Utilities;
using Arrrive_Pursue_Behaviour.GameComponents;

namespace Pathfinding.GameComponents
{
    class PlayerController : Component
    {
        private Raycast raycast;
        private GameObject lastClicked, currentClicked;
        private ArriveAtComponent mover;

        public override void Initialize()
        {
            raycast = new Raycast(gameInstance.GraphicsDevice);
            lastClicked = null;
            mover = owner.GetComponent<ArriveAtComponent>();
            if(mover == null)
            {
                mover = owner.AddComponent<ArriveAtComponent>();
                mover.Speed = 1;
                mover.steerDuration = 0.5f;
                mover.MinimumDistance = 0.1f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(Input.IsMousePressed(MouseButton.LEFT))
            {
                //find what we clicked on
                setClickedObject(findClickedObject());
            }
            else if(Input.IsMouseReleased(MouseButton.LEFT))
            {
                if(currentClicked != null)
                {
                    Vector3 pos = currentClicked.transform.Position;
                    pos.Y = owner.transform.Position.Y;
                    mover.Destination = pos;
                    System.Console.WriteLine("Moving to: " + pos);
                }
            }
        }

        private GameObject findClickedObject()
        {
            raycast.setupMatrices(Camera.mainCamera.World, Camera.mainCamera.View, Camera.mainCamera.Projection);
            RaycastResult result;
            List<BoxCollider> colliders = gameInstance.Services.GetService<CollisionDetector>().StaticColliders;

            if(raycast.cast(Input.MousePosition, out result, colliders))
            {
                return result.collider.Owner;
            }

            return null;
        }

        private void setClickedObject(GameObject obj)
        {
            lastClicked = currentClicked;
            currentClicked = obj;
            colourObject(lastClicked, false);
            colourObject(currentClicked, true);
        }

        private void colourObject(GameObject target, bool highlight)
        {
            if(target != null)
            {
                MeshRendererComponent renderer = target.GetComponent<MeshRendererComponent>();
                if(renderer != null)
                {
                    if (!highlight)
                        renderer.Material.DiffuseColor = renderer.colour.ToVector3();
                    else
                        renderer.Material.DiffuseColor = Color.Gray.ToVector3() * renderer.colour.ToVector3();
                }
            }
        }
    }
}

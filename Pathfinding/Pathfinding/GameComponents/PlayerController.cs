using MonogameLearning.BaseComponents;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameLearning.Utilities;
using MazeEscape.Utilities;
using Arrrive_Pursue_Behaviour.GameComponents;
using Pathfinding.Pathfinding;

namespace Pathfinding.GameComponents
{
    class PlayerController : Component
    {
        private Raycast raycast;
        private GameObject lastClicked, currentClicked;
        private PathfinderComponent pathComponent;
        private ArriveAtComponent mover;
        private Stack<GraphNode> currentPath;

        public override void Initialize()
        {
            raycast = new Raycast(gameInstance.GraphicsDevice);
            lastClicked = null;
            setupArriveComponent();
            pathComponent = owner.GetComponent<PathfinderComponent>();
            currentPath = null;
        }

        private void setupArriveComponent()
        {
            mover = owner.GetComponent<ArriveAtComponent>();
            if (mover == null)
            {
                mover = owner.AddComponent<ArriveAtComponent>();
                mover.Speed = 1;
                mover.steerDuration = 0.25f;
                mover.MinimumDistance = 0.25f;
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
                updatePath();
            }
            if(mover.Arrived)
            {
                updateDestination();
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

        private void updatePath()
        {
            if (currentClicked != null)
            {
                currentPath = pathComponent.findPath(Owner.transform.Position,
                    currentClicked.transform.Position);
            }
        }

        private void updateDestination()
        {
            if (currentPath != null) //do we have a path?
            {
                if (currentPath.Count > 0) //are there any nodes left in the path?
                {
                    System.Console.WriteLine("Nodes still on path: " + currentPath.Count);
                    //make the next node the destination
                    GraphNode next = currentPath.Pop();
                    Vector3 nextPos = next.position;
                    nextPos.Y = owner.transform.Position.Y;
                    mover.Destination = nextPos;
                    System.Console.WriteLine("Moving to: " + nextPos);
                }
            }
        }
    }
}

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
        public LevelGraph levelGraph;
        public GraphNode selectedNode, startNode;
        public BreadthSearchPathing pathfinder;
        private Raycast raycast;
        private GameObject lastClicked, currentClicked;
        private ArriveAtComponent mover;
        private Stack<GraphNode> currentPath;

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
            pathfinder = new BreadthSearchPathing(levelGraph);
            currentPath = null;
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
                    GraphNode clickedNode = levelGraph.getFromWorldPos(currentClicked.transform.Position);
                    GraphNode startNode = levelGraph.getFromWorldPos(owner.transform.Position);
                    currentPath = pathfinder.findPath(startNode, clickedNode);
                    System.Console.WriteLine("Current path: " + (currentPath != null));
                    /*if (currentPath != null)
                        currentPath.Pop();*/
                    /*Vector3 pos = currentClicked.transform.Position;
                    pos.Y = owner.transform.Position.Y;
                    mover.Destination = pos;
                    System.Console.WriteLine("Moving to: " + pos);*/
                }
            }
            if(mover.Arrived)
            {
                //System.Console.WriteLine("Arrived.");
                //do we have a path?
                if(currentPath != null)
                {
                    System.Console.WriteLine("Have a path.");
                    //are there any nodes left in the path?
                    if(currentPath.Count > 0)
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

            if(currentClicked != null)
            {
                selectedNode = levelGraph.getFromWorldPos(currentClicked.transform.Position);
                startNode = levelGraph.getFromWorldPos(owner.transform.Position);
            }
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

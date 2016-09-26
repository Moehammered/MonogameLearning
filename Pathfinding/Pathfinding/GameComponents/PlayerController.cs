using MonogameLearning.BaseComponents;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameLearning.Utilities;
using MazeEscape.Utilities;
using Arrrive_Pursue_Behaviour.GameComponents;
using Pathfinding.Pathfinding;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinding.GameComponents
{
    class PlayerController : Component
    {
        public LevelGraph levelGraph;
        //for debugging/visual purposes
        public GraphNode selectedNode, startNode;
        public GraphNode[] debugPath;
        public VertexPositionColor[] pathBuffer;
        public int[] pathIndices;
        //end debug variables
        private AStarPathing aPath;
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
                mover.steerDuration = 0.25f;
                mover.MinimumDistance = 0.25f;
            }
            aPath = new AStarPathing(levelGraph);
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
                    currentPath = aPath.findPath(startNode, clickedNode);
                    if(currentPath != null)
                    {
                        System.Console.WriteLine("Found a path!");
                        createPathDisplay();
                    }
                }
            }
            if(mover.Arrived)
            {
                //System.Console.WriteLine("Arrived.");
                //do we have a path?
                if(currentPath != null)
                {
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

        private void createPathDisplay()
        {
            debugPath = currentPath.ToArray();
            pathBuffer = new VertexPositionColor[debugPath.Length];
            pathIndices = new int[debugPath.Length];
            for (int i = 0; i < debugPath.Length; i++)
            {
                Vector3 point = debugPath[i].position;
                point.Y = 1.25f;
                pathBuffer[i] = new VertexPositionColor(point, Color.White);
                pathIndices[i] = i;
            }
        }
    }
}

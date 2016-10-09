using MonogameLearning.BaseComponents;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameLearning.Pathfinding
{
    class PathRenderComponent : Component
    {
        private LineRenderer renderer;
        private PathfinderComponent pathfinder;
        private Stack<GraphNode> currentPath;
        private Vector3[] pathPoints;
        private string pathInfo, pointInfo;

        public PathRenderComponent()
        {
        }

        public string PathInfo
        {
            get { return pathInfo; }
        }

        public string PointInfo
        {
            get { return pointInfo; }
        }

        public override void Initialize()
        {
            renderer = Owner.GetComponent<LineRenderer>();
            if (renderer == null)
                renderer = Owner.AddComponent<LineRenderer>();
            pathfinder = Owner.GetComponent<PathfinderComponent>();

            pathInfo = "";
            pointInfo = "";
        }

        public override void Update(GameTime gameTime)
        {
            if(pathfinder != null)
            {
                if(currentPath != pathfinder.CurrentPath)
                {
                    System.Console.WriteLine("Updating path display.");
                    updatePath();
                    updateDisplay();
                }
            }
        }

        private void updatePath()
        {
            currentPath = pathfinder.CurrentPath;
            if (currentPath != null)
            {
                GraphNode[] path = currentPath.ToArray();
                pathPoints = new Vector3[path.Length];
                for (int i = 0; i < path.Length; i++)
                {
                    pathPoints[i] = path[i].position + Vector3.Up;
                }
            }
            else
                pathPoints = null;
        }

        private void updateDisplay()
        {
            if(renderer != null)
            {
                renderer.Vertices = pathPoints;
            }
            updatePathInformation();
        }

        private void updatePathInformation()
        {
            if (pathPoints != null)
            {
                pathInfo = "Start Node: " + pathPoints[0];
                pathInfo += "\nDestination Node: " + pathPoints[pathPoints.Length - 1];

                pointInfo = "Current Path\n";
                for (int i = 0; i < pathPoints.Length; i++)
                {
                    pointInfo += "Point[" + i + "]: " + pathPoints[i] + "\n";
                }
            }
            else
            {
                pathInfo = "No path.";
                pointInfo = "No path.";
            }
        }
    }
}

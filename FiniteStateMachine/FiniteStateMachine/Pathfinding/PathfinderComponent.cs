using MonogameLearning.BaseComponents;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameLearning.Pathfinding
{
    class PathfinderComponent : Component
    {
        private LevelGraph graph;
        private GraphPath graphSearch;
        private Stack<GraphNode> currentPath;

        public PathfinderComponent()
        {
        }

        public Stack<GraphNode> CurrentPath
        {
            get { return currentPath; }
        }

        public Stack<GraphNode> findPath(GraphNode start, GraphNode end)
        {
            currentPath = graphSearch.findPath(start, end);

            return currentPath;
        }

        public Stack<GraphNode> findPath(Vector3 start, Vector3 end)
        {
            GraphNode startNode = graph.getFromWorldPos(start);
            GraphNode endNode = graph.getFromWorldPos(end);

            return findPath(startNode, endNode);
        }

        public void clearPath()
        {
            currentPath = null;
        }

        public void setAlgorithm<T>(LevelGraph graph) where T : GraphPath
        {
            this.graph = graph;
            graphSearch = (T)Activator.CreateInstance(typeof(T), new object[] { graph });
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}

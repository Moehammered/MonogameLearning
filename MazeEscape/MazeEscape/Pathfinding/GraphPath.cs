using System.Collections.Generic;

namespace MonogameLearning.Pathfinding
{
    abstract class GraphPath
    {
        protected LevelGraph graph;

        public GraphPath(LevelGraph graph)
        {
            this.graph = graph;
        }

        public abstract Stack<GraphNode> findPath(GraphNode start, GraphNode end);
        protected abstract Stack<GraphNode> reconstructPath(GraphNode end);
    }
}

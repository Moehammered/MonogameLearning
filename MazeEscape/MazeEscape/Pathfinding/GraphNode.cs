using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameLearning.Pathfinding
{
    class GraphNode
    {
        public Vector3 position;
        public GraphNode from;
        public bool seen = false;
        private float cost, baseCost;
        private List<GraphNode> neighbours;

        public GraphNode(int defaultCost)
        {
            baseCost = defaultCost;
            cost = baseCost;
            position = Vector3.Zero;
            neighbours = new List<GraphNode>();
            from = null;
        }

        public float TravelCost
        {
            get { return baseCost; }
        }

        public float CostSoFar
        {
            get { return cost; }
            set
            {
                if (value >= 0)
                    cost = value;
                else
                    cost = baseCost;
            }
        }

        public List<GraphNode> getNeightbours()
        {
            return neighbours;
        }

        public void addNeighbour(GraphNode node)
        {
            neighbours.Add(node);
        }

        public void addNeighbours(List<GraphNode> neighbours)
        {
            this.neighbours = neighbours;
        }
    }
}

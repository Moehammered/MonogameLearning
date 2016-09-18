using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pathfinding.Pathfinding
{
    class GraphNode
    {
        public Vector3 position;
        private int cost, baseCost;
        private List<GraphNode> neighbours;

        public GraphNode(int defaultCost)
        {
            baseCost = defaultCost;
            cost = baseCost;
            position = Vector3.Zero;
            neighbours = new List<GraphNode>();
        }

        public int Cost
        {
            get { return cost; }
            set
            {
                if (value > 0)
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

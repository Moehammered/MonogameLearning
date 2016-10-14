using System;
using System.Collections.Generic;

namespace MonogameLearning.Pathfinding
{
    class DijkstraPathing : GraphPath
    {
        private List<GraphNode> processQueue;

        public DijkstraPathing(LevelGraph graph) : base(graph)
        {
            processQueue = new List<GraphNode>();
        }

        public override Stack<GraphNode> findPath(GraphNode start, GraphNode end)
        {
            graph.resetGraph();
            start.seen = true;
            processQueue.Add(start);
            float costSoFar = 0;
            while(processQueue.Count > 0)
            {
                GraphNode current = processQueue[0];
                //find lowest costing node so far
                foreach(GraphNode c in processQueue)
                {
                    if (c.CostSoFar < current.CostSoFar)
                        current = c;
                }
                processQueue.Remove(current); //we're processing it now, remove it from list
                costSoFar = current.CostSoFar;
                List<GraphNode> neighbours = current.getNeightbours();
                foreach(GraphNode node in neighbours)
                {
                    if(!node.seen)
                    {
                        //store where we came from
                        node.from = current;
                        //mark that it's been seen now
                        node.seen = true;
                        //calculate cost so far for that node
                        node.CostSoFar = node.TravelCost + costSoFar;
                        if (node == end)
                        {
                            Console.WriteLine("Found a path!");
                            processQueue.Clear();
                            return reconstructPath(node);
                        }
                        else
                            processQueue.Add(node);
                        
                    }
                    else //do we have a lower cost of getting here?
                    {
                        if(node.TravelCost + costSoFar < node.CostSoFar)
                        {
                            //let it know we have a quicker way of getting here
                            node.CostSoFar = node.TravelCost + costSoFar;
                            node.from = current;
                        }
                    }
                }
            }
            return null;
        }

        protected override Stack<GraphNode> reconstructPath(GraphNode endNode)
        {
            Stack<GraphNode> path = new Stack<GraphNode>();
            GraphNode current = endNode;
            path.Push(current);

            while (current.from != null)
            {
                current = current.from;
                path.Push(current);
            }

            return path;
        }
    }
}

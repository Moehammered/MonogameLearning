using System;
using System.Collections.Generic;

namespace MonogameLearning.Pathfinding
{
    class BreadthSearchPathing : GraphPath
    {
        private Queue<GraphNode> processQueue;

        public BreadthSearchPathing(LevelGraph graph) : base(graph)
        {
            processQueue = new Queue<GraphNode>();
        }

        public override Stack<GraphNode> findPath(GraphNode start, GraphNode end)
        {
            //reset the levelGraph
            graph.resetGraph();
            //mark starting node as seen
            start.seen = true;
            //add the starting node to the stack
            processQueue.Enqueue(start);
            while(processQueue.Count > 0)
            {
                //get first node in the stack
                GraphNode current = processQueue.Dequeue();
                //find all unseen neighbours
                List<GraphNode> neighbours = current.getNeightbours();
                foreach(GraphNode node in neighbours)
                {
                    if(!node.seen)
                    {
                        //mark where we came from for them
                        node.from = current;
                        node.seen = true;
                        //is the goal in one of the neighbours?
                        if (node == end)
                        {
                            //reconstruct the path back to beginning
                            //found the goal
                            Console.WriteLine("Found a path!");
                            processQueue.Clear();
                            return reconstructPath(node);
                        }
                        else //if not
                        {
                            //push them to the stack
                            processQueue.Enqueue(node);
                        }
                    }
                }
            }
            //something went wrong, no path was found
            Console.WriteLine("Couldn't find a path.");
            return null;
        }

        protected override Stack<GraphNode> reconstructPath(GraphNode endNode)
        {
            Stack<GraphNode> path = new Stack<GraphNode>();
            GraphNode current = endNode;
            path.Push(current);

            while(current.from != null)
            {
                current = current.from;
                path.Push(current);
            }

            return path;
        }
    }
}

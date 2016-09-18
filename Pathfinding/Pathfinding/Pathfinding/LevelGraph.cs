﻿using MazeEscape.GameUtilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pathfinding.Pathfinding
{
    class LevelGraph
    {
        private List<GraphNode> nodeGraph;
        private GraphNode[,] gridNodes;
        private LevelData level;
        private const int GRASS_ID = 0, SAND_ID = 1, BLOCK_ID = 2;

        public LevelGraph(LevelData level)
        {
            this.level = level;
            gridNodes = new GraphNode[level.columns, level.rows];
            nodeGraph = new List<GraphNode>(level.columns * level.rows);
        }

        public List<GraphNode> NodeGraph
        {
            get { return nodeGraph; }
        }

        public void buildGraph()
        {
            buildNodes();
            for(int x = 0; x < gridNodes.GetLength(0); x++)
            {
                for(int z = 0; z < gridNodes.GetLength(1); z++)
                {
                    GraphNode current = gridNodes[x, z];
                    if(current != null)
                    {
                        //get the neighbours of this node,
                        //and add them to it's list of neighbours
                        current.addNeighbours(getNeighbours(x, z));
                        nodeGraph.Add(current);
                    }
                }
            }
        }

        private void buildNodes()
        {
            for(int x = 0; x < level.columns; x++)
            {
                for(int z = 0; z < level.rows; z++)
                {
                    int tileID = level.getData(x, z);
                    GraphNode node = constructNode(tileID);
                    if(node != null)
                    {
                        node.position = new Vector3(x, 0, z);
                    }
                    gridNodes[x, z] = node;
                }
            }
        }

        private List<GraphNode> getNeighbours(int x1, int z1)
        {
            List<GraphNode> nodes = new List<GraphNode>();

            for(int x = x1 - 1; x < x1+1; x++)
            {
                for(int z = z1 - 1; z< z1+1; z++)
                {
                    if(x1 != x && z1 != z)
                    {
                        GraphNode node = getNode(x, z);
                        if(node != null)
                        {
                            nodes.Add(node);
                        }
                    }
                }
            }

            return nodes;
        }

        private GraphNode getNode(int x, int z)
        {
            if(x >= 0 && z >= 0)
            {
                if (x < gridNodes.GetLength(0) && z < gridNodes.GetLength(1))
                    return gridNodes[x, z];
            }

            return null;
        }

        private GraphNode constructNode(int id)
        {
            switch (id)
            {
                case GRASS_ID:
                    return new GraphNode(1);
                case SAND_ID:
                    return new GraphNode(2);
                case BLOCK_ID:
                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Resets the cost of each node by setting it to 0.
        /// This causes it to make it return to it's default cost.
        /// </summary>
        public void resetGraph()
        {
            foreach(GraphNode node in nodeGraph)
            {
                node.Cost = 0;
            }
        }
    }
}

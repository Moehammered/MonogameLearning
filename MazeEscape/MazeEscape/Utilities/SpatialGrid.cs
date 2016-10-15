using MazeEscape.GameUtilities;
using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using System;
using System.Collections.Generic;

namespace MazeEscape.Utilities
{
    class SpatialGrid
    {
        private List<BoxCollider>[,] grid;
        private LevelData level;
        private int chunkSize;

        public SpatialGrid(int chunkSize, LevelData level)
        {
            int xChunkCount, zChunkCount;
            this.level = level;
            this.chunkSize = chunkSize;
            xChunkCount = (int)Math.Ceiling((double)level.columns / (double)chunkSize);
            zChunkCount = (int)Math.Ceiling((double)level.rows / (double)chunkSize);
            grid = new List<BoxCollider>[xChunkCount, zChunkCount];
        }

        public void prepareGrid()
        {
            for(int x = 0; x < grid.GetLength(0); x++)
            {
                for(int z = 0; z < grid.GetLength(1); z++)
                {
                    grid[x, z] = new List<BoxCollider>(chunkSize * chunkSize);
                }
            }
        }

        public void addCollider(BoxCollider col)
        {
            Vector3 pos = col.Owner.transform.Position;
            int[] indices = getIndex(pos);
            grid[indices[0], indices[1]].Add(col);
        }

        public List<BoxCollider> getColliders(Vector3 pos)
        {
            int[] indices = getIndex(pos);

            return grid[indices[0], indices[1]];
        }

        private int[] getIndex(Vector3 pos)
        {
            int[] indices = new int[2];
            indices[0] = (int)Math.Floor(pos.X / (float)chunkSize);
            indices[1] = (int)Math.Floor(pos.Z / (float)chunkSize);

            return indices;
        }
    }
}

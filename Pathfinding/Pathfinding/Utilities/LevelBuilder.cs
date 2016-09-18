using MazeEscape.GameUtilities;
using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using Pathfinding.GameComponents;
using System.Collections.Generic;

namespace Pathfinding.Utilities
{
    class LevelBuilder
    {
        private LevelLoader loader;
        private LevelData level;
        private List<GameObject> scene;
        private const int GRASS_ID = 0, SAND_ID = 1, BLOCK_ID = 2;
        private Game gameInstance;

        public LevelBuilder(Game inst)
        {
            loader = new LevelLoader();
            scene = new List<GameObject>();
            gameInstance = inst;
        }

        public void initialise()
        {
            loader.loadLevelFiles();
            scene.Add(createGameCamera());
        }

        public LevelData LoadedLevelData
        {
            get { return level; }
        }

        public void loadLevel(string name)
        {
            if(loader.loadLevel(name, out level))
            {
                //construct objects now
                readLevelData();
            }
            else
            {
                //construct a red cube at 0, 0, 0 for error
                scene.Add(constructLevelObject(-1));
            }
        }

        private void readLevelData()
        {
            for(int x = 0; x < level.columns; x++)
            {
                for(int z = 0; z < level.rows; z++)
                {
                    int tileID = level.getData(x, z);
                    //construct object and add it to the scene
                    GameObject go = constructLevelObject(tileID);
                    go.transform.Position = new Vector3(x, 0, z);
                    scene.Add(go);
                }
            }
        }

        private GameObject constructLevelObject(int id)
        {
            GameObject obj = null;
            Color colour = Color.White;

            switch (id)
            {
                case GRASS_ID:
                    colour = Color.Green;
                    break;
                case SAND_ID:
                    colour = Color.SandyBrown;
                    break;
                case BLOCK_ID:
                    colour = Color.Black;
                    break;
                default:
                    colour = Color.Red;
                    break;
            }

            obj = createTile(colour);

            return obj;
        }

        private GameObject createTile(Color col)
        {
            GameObject tile = new GameObject(gameInstance);

            MeshRendererComponent renderer = tile.AddComponent<MeshRendererComponent>();
            renderer.Mesh = PrimitiveShape.CreateCube();
            renderer.colour = col;
            BoxCollider collider = tile.AddComponent<BoxCollider>();
            BoundingBox bounds = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
            collider.UnScaledBounds = bounds;

            return tile;
        }

        private GameObject createGameCamera()
        {
            GameObject cam = new GameObject(gameInstance);

            cam.AddComponent<Camera>();
            cam.transform.Position = new Vector3(5, 20, 20);
            cam.transform.lookAt(new Vector3(5, 0, 0));
            
            return cam;
        }
    }
}

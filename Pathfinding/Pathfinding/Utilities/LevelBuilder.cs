using MazeEscape.GameUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using System.Collections.Generic;

namespace Pathfinding.Utilities
{
    class LevelBuilder
    {
        private LevelLoader loader;
        private LevelData level;
        private List<GameObject> scene;
        private const int GRASS_ID = 0, SAND_ID = 1, BLOCK_ID = 2,
            WATER_ID = 3, LAVA_ID = 4;
        private Game gameInstance;
        private bool loadSuccessful = false;

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
            loadSuccessful = loader.loadLevel(name, out level);
        }

        public void buildLevel()
        {
            if (loadSuccessful)
                readLevelData();
        }

        private void readLevelData()
        {
            for(int x = 0; x < level.columns; x++)
            {
                for(int z = 0; z < level.rows; z++)
                {
                    int tileID = level.getData(x, z);
                    //construct object and add it to the scene
                    GameObject go = constructLevelObject(tileID, new Vector3(x, 0, z));
                    scene.Add(go);
                }
            }
        }

        private GameObject constructLevelObject(int id, Vector3 pos)
        {
            GameObject obj = null;
            Color colour = Color.White;
            bool isBlock = false;

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
                    createObstacle(pos);
                    isBlock = true;
                    break;
                case WATER_ID:
                    colour = Color.Aqua;
                    break;
                case LAVA_ID:
                    colour = Color.OrangeRed;
                    break;
                default:
                    colour = Color.Red;
                    break;
            }

            obj = createTile(colour);
            obj.transform.Position = pos;
            if (isBlock)
                obj.RemoveComponent<BoxCollider>();
            return obj;
        }

        private void createObstacle(Vector3 pos)
        {
            GameObject obs = createTile(Color.Gray);
            obs.RemoveComponent<BoxCollider>();
            obs.transform.Position = pos;
            obs.transform.Translate(0, 1, 0);
            obs.transform.Scale = Vector3.One / 2;
            scene.Add(obs);
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

        private GameObject createTile(Color col, Texture2D texture)
        {
            GameObject tile = new GameObject(gameInstance);

            MeshRendererComponent renderer = tile.AddComponent<MeshRendererComponent>();
            renderer.Mesh = PrimitiveShape.CreateCube();
            renderer.colour = col;
            renderer.Material.Texture = texture;
            renderer.Material.TextureEnabled = true;
            BoxCollider collider = tile.AddComponent<BoxCollider>();
            BoundingBox bounds = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
            collider.UnScaledBounds = bounds;

            return tile;
        }

        private GameObject createGameCamera()
        {
            GameObject cam = new GameObject(gameInstance);

            cam.AddComponent<Camera>();
            cam.transform.Position = new Vector3(10, 25, 40);
            cam.transform.lookAt(new Vector3(10, 0, 0));
            
            return cam;
        }
    }
}

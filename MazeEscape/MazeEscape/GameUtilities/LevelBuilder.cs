using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Utilities;
using System.Collections.Generic;

namespace MazeEscape.GameUtilities
{
    class LevelBuilder
    {
        private Game GameInstance;
        private Scene scene;
        //all things which could be chosen at random if more than 1 available
        private List<Vector3> startPoints;
        private List<Vector3> goalPoints;
        //all things which will be spawned sequentially
        private Stack<Vector3> enemyPoints;
        private Stack<Vector3> collectablePoints, powerPillPoints;
        
        public LevelBuilder(Game instance, Scene scene)
        {
            GameInstance = instance;
            this.scene = scene;
        }

        public List<Vector3> StartPoints
        {
            get { return startPoints; }
        }

        public List<Vector3> GoalPoints
        {
            get { return goalPoints; }
        }

        public Stack<Vector3> EnemyPoints
        {
            get { return enemyPoints; }
        }

        public Stack<Vector3> PowerPoints
        {
            get { return powerPillPoints; }
        }

        public Stack<Vector3> CollectablePoints
        {
            get { return collectablePoints; }
        }

        #region Level Tile Creation/Parsing
        public void createLevelTiles(LevelData level)
        {
            Vector3 tilePosition = Vector3.Zero;
            Color tileColour = Color.White;
            Vector3 tileScale;
            BoundingBox tileBounds = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
            for (int x = 0; x < level.columns; x++)
            {
                tilePosition.X = x;
                for (int z = 0; z < level.rows; z++)
                {
                    tilePosition.Z = z;
                    tileScale = Vector3.One;
                    int tileID = level.getData(x, z);
                    parseTileData(tileID, tilePosition, ref tileColour, ref tileScale);
                    createTile(tilePosition, tileScale, tileColour, tileBounds);
                }
            }
        }

        private void createTile(Vector3 position, Vector3 scale, Color colour, BoundingBox bounds)
        {
            GameObject tile = new GameObject(GameInstance);
            tile.name = "obstacle";
            tile.transform.Position = position;
            tile.transform.Scale = scale;

            BoxCollider collider = tile.AddComponent<BoxCollider>();
            collider.UnScaledBounds = bounds;
            CollisionDetector col = GameInstance.Services.GetService<CollisionDetector>();
            col.addStaticCollider(collider);

            MeshRendererComponent renderer = tile.AddComponent<MeshRendererComponent>();
            renderer.Mesh = PrimitiveShape.CreateCube();
            renderer.Colour = colour;

            scene.addSceneObject(tile);
        }

        private void parseTileData(int tileID, Vector3 position, ref Color colour, ref Vector3 scale)
        {
            switch ((MazeTile)tileID)
            {
                case MazeTile.WALL:
                    colour = Color.Black;
                    scale.Y = 5;
                    break;
                case MazeTile.FLOOR:
                    colour = Color.White;
                    break;
                case MazeTile.HAZARD:
                    colour = Color.White;
                    addEnemyPoint(position + Vector3.Up);
                    break;
                case MazeTile.GOAL:
                    colour = Color.White;
                    //store it for spawning later, but still spawn a floor under
                    addGoalPoint(position + Vector3.Up);
                    break;
                case MazeTile.P_START:
                    addStartPoint(position + Vector3.Up);
                    colour = Color.Gray;
                    break;
                case MazeTile.POWER_PILL:
                    colour = Color.White;
                    addPowerPoint(position + Vector3.Up);
                    break;
                case MazeTile.COLLECTABLE:
                    colour = Color.MonoGameOrange;
                    addCollectablePoint(position + Vector3.Up);
                    break;
                default:
                    colour = Color.Orange;
                    break;
            }
        }

        private void addEnemyPoint(Vector3 pos)
        {
            if (enemyPoints == null)
                enemyPoints = new Stack<Vector3>(1);
            enemyPoints.Push(pos);
        }

        private void addCollectablePoint(Vector3 pos)
        {
            if (collectablePoints == null)
                collectablePoints = new Stack<Vector3>(1);
            collectablePoints.Push(pos);
        }

        private void addPowerPoint(Vector3 pos)
        {
            if (powerPillPoints == null)
                powerPillPoints = new Stack<Vector3>(1);
            powerPillPoints.Push(pos);
        }

        private void addGoalPoint(Vector3 pos)
        {
            if (goalPoints == null)
                goalPoints = new List<Vector3>(1);
            goalPoints.Add(pos);
        }

        private void addStartPoint(Vector3 pos)
        {
            if (startPoints == null)
                startPoints = new List<Vector3>(1);
            startPoints.Add(pos);
        }
        #endregion
    }

}

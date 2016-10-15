using MazeEscape.GameComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using MonogameLearning.Graphics;
using MonogameLearning.Utilities;
using System;
using System.Collections.Generic;

namespace MazeEscape.GameUtilities
{
    enum MazeTile
    {
        NONE = -1,
        WALL,
        FLOOR,
        HAZARD,
        GOAL,
        P_START,
        COLLECTABLE = 9
    }

    class MazeScene : Scene
    {
        private LevelLoader loader;
        private bool active;
        private int levelNumber;
        //all things which could be chosen at random if more than 1 available
        private List<Vector3> startPoints;
        private List<Vector3> goalPoints;
        //all things which will be spawned sequentially
        private Stack<Vector3> enemyPoints;
        private Stack<Vector3> collectablePoints;

        public MazeScene(Game instance, int levelNumber) : base(instance)
        {
            loader = new LevelLoader();
            loader.loadLevelFiles();
            
            active = false;
            if (levelNumber < 0)
                levelNumber = 0;
            this.levelNumber = levelNumber;
        }

        public int LevelNumber
        {
            get { return levelNumber; }
            set
            {
                if (value >= 0)
                    levelNumber = value;
                else
                    levelNumber = 0;
            }
        }

        public override void Load()
        {
            if (active)
                Unload();
            else
            {
                buildLevel();
            }
        }

        public override void Unload()
        {
        }

        private void buildLevel()
        {
            LevelData level;
            if (loader.loadLevel("Level" + levelNumber + ".txt", out level))
            {
                //found level successfully
                createLevelTiles(level);
                createPlayer();
                createGoal();
                createEnemies();
                createCollectables();
            }
            else
            {
                //return to menu I guess??
                //or display error message on screen
            }
        }

        #region Level Tile Creation/Parsing
        private void createLevelTiles(LevelData level)
        {
            Vector3 tilePosition = Vector3.Zero;
            Color tileColour = Color.White;
            Vector3 tileScale;
            BoundingBox tileBounds = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
            for(int x = 0; x < level.columns; x++)
            {
                tilePosition.X = x;
                for(int z = 0; z < level.rows; z++)
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
            tile.transform.Position = position;
            tile.transform.Scale = scale;

            BoxCollider collider = tile.AddComponent<BoxCollider>();
            collider.UnScaledBounds = bounds;

            MeshRendererComponent renderer = tile.AddComponent<MeshRendererComponent>();
            renderer.Mesh = PrimitiveShape.CreateCube();
            renderer.Colour = colour;

            gameObjects.Add(tile);
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
        #region Player Creation
        private void createPlayer()
        {
            GameObject player = new GameObject(GameInstance);
            player.name = "Player";
            player.transform.Position = getPlayerStart();
            //player important components
            player.AddComponent<Camera>();

            player.AddComponent<FirstPersonMover>();
            player.AddComponent<FirstPersonController>();

            BoxCollider playerCollider = player.AddComponent<BoxCollider>();
            playerCollider.UnScaledBounds = new BoundingBox(-Vector3.One / 4f, Vector3.One / 4f);
            CollisionDetector colServ = GameInstance.Services.GetService<CollisionDetector>();
            colServ.addDynamicCollider(playerCollider);
        }

        private Vector3 getPlayerStart()
        {
            if (startPoints != null && startPoints.Count > 0)
            {
                if (startPoints.Count == 1)
                    return startPoints[0];
                else if (startPoints.Count > 1)
                {
                    Random random = new Random();
                    return startPoints[random.Next(startPoints.Count)];
                }
            }

            return Vector3.Zero;
        }
        #endregion
        #region Goal Creation
        private void createGoal()
        {
            GameObject goal = new GameObject(GameInstance);
            goal.name = "goal";
            goal.transform.Position = getGoalPoint();
            MeshRendererComponent rend = goal.AddComponent<MeshRendererComponent>();
            rend.Colour = Color.Green;
            rend.Mesh = PrimitiveShape.CreateCube();
            BoxCollider collider = goal.AddComponent<BoxCollider>();
            collider.UnScaledBounds = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
        }

        private Vector3 getGoalPoint()
        {
            if (goalPoints != null && goalPoints.Count > 0)
            {
                if (goalPoints.Count == 1)
                    return goalPoints[0];
                else if (goalPoints.Count > 1)
                {
                    Random random = new Random();
                    return goalPoints[random.Next(goalPoints.Count)];
                }
            }

            return Vector3.One;
        }
        #endregion
        #region Enemy Creation
        private void createEnemies()
        {
            if(enemyPoints != null)
            {
                while(enemyPoints.Count > 0)
                {
                    createEnemy(enemyPoints.Pop());
                }
            }
        }

        private void createEnemy(Vector3 position)
        {
            //create the hazard
            GameObject enemy = new GameObject(GameInstance);
            enemy.transform.Scale = Vector3.One / 4;
            enemy.transform.Position = position;
            enemy.name = "enemy";
            MeshRendererComponent hazRend = enemy.AddComponent<MeshRendererComponent>();
            hazRend.Mesh = PrimitiveShape.CreateCube();
            hazRend.Colour = Color.Red;
            Vector3 corner = new Vector3(-0.5f, -0.5f, -0.5f);
            enemy.AddComponent<BoxCollider>().UnScaledBounds = new BoundingBox(corner, -corner);
            gameObjects.Add(enemy);
            /*Hazard ai = hazard.AddComponent<Hazard>();
            ai.player = camera;
            ai.WakeUpDistance = 7;*/
        }
        #endregion
        #region Collectable Creation
        private void createCollectables()
        {
            if(collectablePoints != null)
            {
                while(collectablePoints.Count > 0)
                {
                    createPointPickup(collectablePoints.Pop());
                }
            }
        }

        private void createPointPickup(Vector3 position)
        {
            GameObject pickup = new GameObject(GameInstance);
            pickup.transform.Scale = Vector3.One / 5;
            pickup.transform.Position = position;
            pickup.name = "pickup";
            MeshRendererComponent hazRend = pickup.AddComponent<MeshRendererComponent>();
            hazRend.Mesh = PrimitiveShape.CreateCube();
            hazRend.Colour = Color.Yellow;
            Vector3 corner = new Vector3(-0.5f, -0.5f, -0.5f);
            pickup.AddComponent<BoxCollider>().UnScaledBounds = new BoundingBox(corner, -corner);
        }
        #endregion
    }
}

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
        private LevelBuilder builder;
        private bool active;
        private int levelNumber;

        public MazeScene(Game instance, int levelNumber) : base(instance)
        {
            loader = new LevelLoader();
            loader.loadLevelFiles();
            builder = new LevelBuilder(GameInstance, this);

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
                builder.createLevelTiles(level);
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
            player.AddComponent<PlayerTracker>();
            PlayerHUD HUD = player.AddComponent<PlayerHUD>();
            HUD.TextColour = Color.Brown;

            BoxCollider playerCollider = player.AddComponent<BoxCollider>();
            playerCollider.UnScaledBounds = new BoundingBox(-Vector3.One / 4f, Vector3.One / 4f);
            CollisionDetector colServ = GameInstance.Services.GetService<CollisionDetector>();
            colServ.addDynamicCollider(playerCollider);
        }

        private Vector3 getPlayerStart()
        {
            if (builder.StartPoints != null && builder.StartPoints.Count > 0)
            {
                if (builder.StartPoints.Count == 1)
                    return builder.StartPoints[0];
                else if (builder.StartPoints.Count > 1)
                {
                    Random random = new Random();
                    return builder.StartPoints[random.Next(builder.StartPoints.Count)];
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
            if (builder.GoalPoints != null && builder.GoalPoints.Count > 0)
            {
                if (builder.GoalPoints.Count == 1)
                    return builder.GoalPoints[0];
                else if (builder.GoalPoints.Count > 1)
                {
                    Random random = new Random();
                    return builder.GoalPoints[random.Next(builder.GoalPoints.Count)];
                }
            }

            return Vector3.One;
        }
        #endregion
        #region Enemy Creation
        private void createEnemies()
        {
            if(builder.EnemyPoints != null)
            {
                while(builder.EnemyPoints.Count > 0)
                {
                    createEnemy(builder.EnemyPoints.Pop());
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
            if(builder.CollectablePoints != null)
            {
                while(builder.CollectablePoints.Count > 0)
                {
                    createPointPickup(builder.CollectablePoints.Pop());
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

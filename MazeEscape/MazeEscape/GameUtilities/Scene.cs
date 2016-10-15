using Microsoft.Xna.Framework;
using MonogameLearning.BaseComponents;
using System.Collections.Generic;

namespace MazeEscape.GameUtilities
{
    abstract class Scene
    {
        protected Game instance;
        protected List<GameObject> gameObjects;

        public Scene(Game instance)
        {
            this.instance = instance;
            gameObjects = new List<GameObject>(10);
        }

        public abstract void Load();
        public abstract void Unload();

        public List<GameObject> SceneObjects
        {
            get { return gameObjects; }
        }

        public Game GameInstance
        {
            get { return instance; }
        }

        public void addSceneObject(GameObject obj)
        {
            gameObjects.Add(obj);
        }

        public void removeSceneObject(GameObject target)
        {
            if (gameObjects.Remove(target))
                GameObject.DestroyTest(target);
        }

        protected void clearSceneObjects()
        {
            foreach (GameObject go in gameObjects)
                GameObject.DestroyTest(go);
            gameObjects.Clear();
        }
    }
}

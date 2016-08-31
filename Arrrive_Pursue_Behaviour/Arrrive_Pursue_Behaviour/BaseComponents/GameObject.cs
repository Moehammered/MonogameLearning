using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameLearning.BaseComponents
{
    class GameObject
    {
        public Transform transform;
        private List<Component> components;
        private Game gameInstance;

        public GameObject(Game game)
        {
            transform = new Transform();
            components = new List<Component>();
            gameInstance = game;
        }

        public T AddComponent<T>() where T : Component, new ()
        {
            //leaving this here to remember this class
            //T component = (T)Activator.CreateInstance(typeof(T), gameInstance);
            T component = new T();
            component.Owner = this;
            component.GameInstance = gameInstance;

            components.Add(component);
            gameInstance.Components.Add(component);

            return component;
        }

        public void RemoveComponent<T>() where T : Component
        {
            foreach(Component comp in components)
            {
                if(comp is T)
                {
                    gameInstance.Components.Remove(comp);
                    components.Remove(comp);
                    break;
                }
            }
        }

        public T GetComponent<T>() where T : Component
        {
            if(components.Count > 0)
            {
                foreach(Component component in components)
                {
                    if (component is T)
                        return (T)component;
                }
            }

            return null;
        }

        public T[] GetComponents<T>() where T : Component
        {
            if(components.Count > 0)
            {
                List<T> found = new List<T>();
                
                foreach(Component comp in components)
                {
                    if (comp is T)
                        found.Add((T)comp);
                }
                if (found.Count > 0)
                    return found.ToArray();
            }

            return null;
        }
        
        /*public static GameObject Clone(ref GameObject prefab)
        {
            GameObject clone = null;
            
            clone = new GameObject(prefab.gameInstance);
            clone.components = new List<Component>(prefab.components.Count);
            for(int i = 0; i < prefab.components.Count; i++)
            {
                //clone.components.Add(prefab.components[i].)
            }
            return clone;
        }*/
    }
}

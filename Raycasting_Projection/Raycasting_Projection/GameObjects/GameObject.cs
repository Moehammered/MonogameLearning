using Microsoft.Xna.Framework;
using Raycasting_Projection.Utilities;
using System;
using System.Collections.Generic;

namespace Raycasting_Projection.GameObjects
{
    class GameObject
    {
        public Transform transform;
        private List<GameComponent> components;
        private Game gameInstance;

        public GameObject(Game game)
        {
            transform = new Transform();
            components = new List<GameComponent>();
            gameInstance = game;
        }

        public T AddComponent<T>() where T : GameComponent
        {
            T component = (T)Activator.CreateInstance(typeof(T), gameInstance);

            components.Add(component);
            gameInstance.Components.Add(component);

            return component;
        }

        public void RemoveComponent<T>() where T : GameComponent
        {
            foreach(GameComponent comp in components)
            {
                if((T)comp != null)
                {
                    gameInstance.Components.Remove(comp);
                    components.Remove(comp);
                    break;
                }
            }
        }

        public T GetComponent<T>() where T : GameComponent
        {
            if(components.Count > 0)
            {
                foreach(GameComponent component in components)
                {
                    if ((T)component != null)
                        return (T)component;
                }
            }

            return null;
        }

        public T[] GetComponents<T>() where T : GameComponent
        {
            List<T> found = null;

            if(components.Count > 0)
            {
                found = new List<T>();
                foreach(GameComponent comp in components)
                {
                    if ((T)comp != null)
                        found.Add((T)comp);
                }
                if (found.Count > 0)
                    return found.ToArray();
            }

            return null;
        }
        
    }
}

﻿using System;
using Microsoft.Xna.Framework;
using Raycasting_Projection.GameObjects;

namespace Raycasting_Projection.Components
{
    abstract class Component : IGameComponent, IUpdateable
    {
        protected GameObject owner;
        protected int updateOrder;
        protected bool enabled;
        protected Game gameInstance;

        public Component()
        {
            enabled = true;
            updateOrder = 0;
            owner = null;
            gameInstance = null;
        }

        public Game GameInstance
        {
            get { return gameInstance; }
            set
            {
                if (gameInstance == null)
                    gameInstance = value;
            }
        }

        public GameObject Owner
        {
            get { return owner; }
            set
            {
                if (owner == null)
                    owner = value;
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if(value != enabled)
                {
                    enabled = value;
                    //add code for when enable changes here
                }
            }
        }

        public int UpdateOrder
        {
            get { return updateOrder; }
            set
            {
                if(value != updateOrder)
                {
                    updateOrder = value;
                    //add code for when update order changes here
                }
            }
        }

        //Need to figure out how to implement these
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        /*public static bool operator ==(Component a, Component b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Component a, Component b)
        {
            return a.Equals(b);
        }*/
    }
}

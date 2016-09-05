using MonogameLearning.BaseComponents;
using System;
using System.Collections.Generic;

namespace MonogameLearning.Utilities
{
    class CollisionDetector
    {
        private List<BoxCollider> colliders;
        private List<BoxCollider> dynamicColliders;
        private List<int> collisionCache;

        public CollisionDetector()
        {
            colliders = new List<BoxCollider>();
            dynamicColliders = new List<BoxCollider>();
            collisionCache = new List<int>();
        }
        
        public void addCollider(BoxCollider col)
        {
            colliders.Add(col);
        }

        public void addDynamicCollider(BoxCollider col)
        {
            colliders.Remove(col);
            dynamicColliders.Add(col);
        }

        //new collision method. Main point will be to separate dynamic colliders and static colliders to reduce intersect calls
        public void sweepDynamics()
        {
            for(int i = 0; i < dynamicColliders.Count; i++)
            {
                for(int k = 0; k < colliders.Count; k++)
                {
                    bool collisionCached = collisionCache.Contains(colliders[k].GetHashCode());
                    if(dynamicColliders[i].Intersects(colliders[k].Bounds))
                    {
                        //collision occured
                        if (!collisionCached)
                        {
                            collisionCache.Add(colliders[k].GetHashCode());
                            dynamicColliders[i].Owner.BroadcastMessage("OnCollision", colliders[k].Owner);
                        }
                    }
                    else if(collisionCached)
                    {
                        collisionCache.Remove(colliders[k].GetHashCode());
                        dynamicColliders[i].Owner.BroadcastMessage("OnCollisionExit");
                    }
                }
            }
        }

        //old collision detection - has a crippling problem of not being able to find unique collisions
        //if the unique collision events aren't attempted it works fine, however when attempting to keep track of collisions it is very slow.
        /*
        public void checkCollisions()
        {
            
            for (int i = 0; i < colliders.Count; i++)
            {
                codes[0] = i;
                for (int j = i+1; j < colliders.Count; j++)
                {
                    codes[1] = j;
                    bool collisionCached = collisionCache.Contains(codes);
                    //bool collisionCached = false;
                    if (colliders[i].Intersects(colliders[j].Bounds))
                    {
                        //check if they're in the collision cache
                        if(!collisionCached)
                        {
                            //collider 1 is not in the list of previously collided objects
                            //register a collision message for it and add these collider pairs into the collision cache
                            collisionCache.Add(codes);
                            colliders[i].Owner.BroadcastMessage("OnCollision");
                            colliders[j].Owner.BroadcastMessage("OnCollision");
                        }
                    }
                    else
                    {
                        //check if they're in the collision cache
                        if (collisionCached)
                        {
                            //collider 1 is in the list of previously collided objects
                            //register that a collision is no longer message for it
                            collisionCache.Remove(codes);
                            colliders[i].Owner.BroadcastMessage("OnCollisionExit");
                            colliders[j].Owner.BroadcastMessage("OnCollisionExit");
                        }
                    }
                }
            }
        }*/
    }
}

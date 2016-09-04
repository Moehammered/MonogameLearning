using MonogameLearning.BaseComponents;
using System.Collections.Generic;

namespace MonogameLearning.Utilities
{
    class CollisionDetector
    {
        private List<BoxCollider> colliders;
        private List<int> collisionCache;

        public CollisionDetector()
        {
            colliders = new List<BoxCollider>();
            collisionCache = new List<int>();
        }
        
        public void addCollider(BoxCollider col)
        {
            colliders.Add(col);
        }

        public void checkCollisions()
        {
            for(int i = 0; i < colliders.Count; i++)
            {
                for(int j = i+1; j < colliders.Count; j++)
                {
                    if(colliders[i].Intersects(colliders[j].Bounds))
                    {
                        //check if they're in the collision cache
                        bool col1cached = collisionCache.Contains(colliders[i].GetHashCode());
                        bool col2cached = collisionCache.Contains(colliders[j].GetHashCode());
                        if(!col1cached)
                        {
                            //collider 1 is not in the list of previously collided objects
                            //register a collision message for it
                            collisionCache.Add(colliders[i].GetHashCode());
                            colliders[i].Owner.BroadcastMessage("OnCollision");
                        }
                        if(!col2cached)
                        {
                            //collider 2 is not in the list of previously collided objects
                            //register a collision message for it
                            collisionCache.Add(colliders[j].GetHashCode());
                            colliders[j].Owner.BroadcastMessage("OnCollision");
                        }
                    }
                    else
                    {
                        //check if they're in the collision cache
                        bool col1cached = collisionCache.Contains(colliders[i].GetHashCode());
                        bool col2cached = collisionCache.Contains(colliders[j].GetHashCode());
                        if (col1cached)
                        {
                            //collider 1 is in the list of previously collided objects
                            //register that a collision is no longer message for it
                            collisionCache.Remove(colliders[i].GetHashCode());
                            colliders[i].Owner.BroadcastMessage("OnCollisionExit");
                        }
                        if (col2cached)
                        {
                            //collider 2 is in the list of previously collided objects
                            //register that a collision is no longer message for it
                            collisionCache.Remove(colliders[j].GetHashCode());
                            colliders[j].Owner.BroadcastMessage("OnCollisionExit");
                        }
                    }
                }
            }
        }
    }
}

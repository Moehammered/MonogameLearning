using MonogameLearning.BaseComponents;
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
        
        public List<BoxCollider> StaticColliders
        {
            get { return colliders; }
        }

        public List<BoxCollider> DynamicColliders
        {
            get { return dynamicColliders; }
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
                            //can redefine how the callback is made by checking for implementation details in derived class
                            //see:http://stackoverflow.com/questions/2932421/detect-if-a-method-was-overridden-using-reflection-c
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
    }
}

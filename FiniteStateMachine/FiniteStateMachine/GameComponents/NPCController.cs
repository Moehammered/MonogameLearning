using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.GameComponents;
using MonogameLearning.Pathfinding;
using System.Collections.Generic;

namespace FiniteStateMachine.GameComponents
{
    class NPCController : Component
    {
        private ArriveAtComponent mover;
        private const int groundHeight = 1;
        private PathfinderComponent pather;
        private Stack<GraphNode> currentPath;

        public NPCController()
        {
        }

        public override void Initialize()
        {
            setupArriveComponent();
            pather = Owner.GetComponent<PathfinderComponent>();
            setRandomDestination();
        }

        private void setRandomDestination()
        {
            GraphNode randDest = pather.RandomNode;
            currentPath = pather.findPath(owner.transform.Position, randDest.position);
        }

        private void setupArriveComponent()
        {
            mover = owner.GetComponent<ArriveAtComponent>();
            if (mover == null)
            {
                mover = owner.AddComponent<ArriveAtComponent>();
                mover.Speed = 1;
                mover.steerDuration = 0.25f;
                mover.MinimumDistance = 0.25f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(currentPath != null)
            {
                if(currentPath.Count > 0 && mover.Arrived)
                {
                    mover.Destination = currentPath.Pop().position + Vector3.Up * groundHeight;
                }
                else if(mover.Arrived)
                {
                    setRandomDestination();
                }
            }
        }
    }
}

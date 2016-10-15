using MonogameLearning.BaseComponents;
using Microsoft.Xna.Framework;
using MonogameLearning.GameComponents;
using MonogameLearning.Pathfinding;
using System.Collections.Generic;
using FiniteStateMachine.FSM;
using MonogameLearning.Utilities;
using MazeEscape.GameComponents;
using System;

namespace FiniteStateMachine.GameComponents
{
    class NPCController : Component
    {
        public PlayerTracker player;
        private FSM.FiniteStateMachine machine;
        private float playerNearDistance = 2;
        private float pndSquared = 4;
        private float playerFarDistance = 4;
        private float pfdSquared = 16;
        private float repathTimer = 1;
        private float idleRepathTime = 2f, evadeRepathTime = 3f, pursueRepathTime = 3f;
        private ArriveAtComponent mover;
        private const int groundHeight = 1;
        private PathfinderComponent pather;
        private Stack<GraphNode> currentPath;
        private FSMInterpreter parser;

        public NPCController()
        {
        }

        public FSM.FiniteStateMachine Machine
        {
            get { return machine; }
            set
            {
                machine = value;
                assignMachineDelegates();
                machine.initialiseMachine();
            }
        }

        public float PlayerNearDistance
        {
            set
            {
                playerNearDistance = value;
                pndSquared = value * value;
            }
            get
            {
                return playerNearDistance;
            }
        }

        public float PlayerFarDistance
        {
            set
            {
                playerFarDistance = value;
                pfdSquared = value * value;
            }
            get
            {
                return playerFarDistance;
            }
        }

        public override void Initialize()
        {
            parser = new FSMInterpreter("fsm_npc1.xml");
            parser.parseFile();
            setupArriveComponent();
            pather = Owner.GetComponent<PathfinderComponent>();
            setRandomDestination();
            Machine = parser.Machine;
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
                mover.MinimumDistance = 0.1f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.enabled)
            {
                machine.processState(); //calls assigned delegates for the machine, see bottom of NPCController
            }
            else
                Owner.Destroy();
        }

        public void OnCollision(GameObject col)
        {
            if (col.name == "Player")
            {
                if (player.isPoweredUp)
                {
                    Console.WriteLine("Touched ya!");
                    currentPath = null;
                    mover.abortMovement();
                    this.Enabled = false;
                }
            }
        }
        
        public override void Destroy()
        {
        }

        #region State Machine Tasks
        private void moveToRandomNode()
        {
            if(repathTimer > 0)
                repathTimer -= Time.DeltaTime;
            if (currentPath != null)
            {
                if (currentPath.Count > 0 && mover.Arrived)
                {
                    if (repathTimer < 0)
                    {
                        repathTimer = idleRepathTime;
                        setRandomDestination();
                    }
                    else
                        mover.Destination = currentPath.Pop().position + Vector3.Up * groundHeight;
                }
                else if (mover.Arrived)
                {
                    setRandomDestination();
                }
            }
        }

        private void moveAwayFromPlayer()
        {
            if(repathTimer > 0)
                repathTimer -= Time.DeltaTime;
            
            if (currentPath != null)
            {
                if (currentPath.Count > 0 && mover.Arrived)
                {
                    if (repathTimer < 0)
                    {
                        repathTimer = evadeRepathTime;
                        Vector3 dir = player.Owner.transform.Position - owner.transform.Position;
                        dir.Normalize();
                        currentPath = pather.findPath(owner.transform.Position, owner.transform.Position - dir * 3);
                    }
                    else
                        mover.Destination = currentPath.Pop().position + Vector3.Up * groundHeight;
                }
                else if (mover.Arrived)
                {
                    setRandomDestination();
                }
            }
        }

        private void moveToPlayer()
        {
            if(repathTimer > 0)
                repathTimer -= Time.DeltaTime;
            
            if (currentPath != null)
            {
                if (currentPath.Count > 0 && mover.Arrived)
                {
                    if (repathTimer < 0)
                    {
                        repathTimer = pursueRepathTime;
                        currentPath = pather.findPath(owner.transform.Position, player.Owner.transform.Position);
                        mover.abortMovement();
                    }
                    else
                        mover.Destination = currentPath.Pop().position + Vector3.Up * groundHeight;
                }
                else if (mover.Arrived)
                {
                    Console.WriteLine("I gotchya!");
                    setRandomDestination();
                }
            }
        }
        #endregion
        #region State Machine Setup
        private void assignMachineDelegates()
        {
            if(machine != null)
            {
                foreach(FiniteState state in machine.States)
                {
                    assignStateTask(state);
                    assignTransitionExpressions(state);
                }
            }
        }

        private void assignStateTask(FiniteState state)
        {
            switch (state.StateName.ToLower())
            {
                case "idle":
                    state.StateTask = moveToRandomNode;
                    break;
                case "pursue":
                    state.StateTask = moveToPlayer;
                    break;
                case "evade":
                    state.StateTask = moveAwayFromPlayer;
                    break;
                default:
                    state.StateTask = moveToRandomNode;
                    break;
            }
        }

        private void assignTransitionExpressions(FiniteState state)
        {
            foreach(StateTransition t in state.Transitions)
            {
                switch(t.Condition.ToLower())
                {
                    case "player_near":
                        t.ConditionExpression = isPlayerClose;
                        break;
                    case "player_far":
                        t.ConditionExpression = isPlayerFar;
                        break;
                    case "player_poweruppill":
                        t.ConditionExpression = isPlayerPowered;
                        break;
                    case "player_poweruppill_expire":
                        t.ConditionExpression = isPlayerNotPowered;
                        break;
                    case "alwaystrue":
                        t.ConditionExpression = alwaysTrue;
                        break;
                    default:
                        t.ConditionExpression = alwaysTrue;
                        break;
                }
            }
        }
        #endregion
        #region State Machine Transition Delegates
        private bool alwaysTrue()
        {
            return true;
        }

        private bool isPlayerClose()
        {
            Vector3 playerPos = player.Owner.transform.Position;
            Vector3 gradient = playerPos - owner.transform.Position;

            return gradient.LengthSquared() < pndSquared;
        }

        private bool isPlayerFar()
        {
            Vector3 playerPos = player.Owner.transform.Position;
            Vector3 gradient = playerPos - owner.transform.Position;

            return gradient.LengthSquared() > pfdSquared;
        }

        private bool isPlayerPowered()
        {
            return player.isPoweredUp;
        }

        private bool isPlayerNotPowered()
        {
            return !player.isPoweredUp;
        }

        #endregion
    }
}

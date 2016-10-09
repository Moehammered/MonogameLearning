using System;
using System.Collections.Generic;

namespace FiniteStateMachine.FSM
{
    class FiniteState
    {
        private string name;
        private List<StateTransition> transitions;
        private Action task;

        public FiniteState(string name)
        {
            this.name = name;
            transitions = new List<StateTransition>();
        }

        public Action StateTask
        {
            set { task = value; }
        }

        public void processState()
        {
            if(task != null)
                task.Invoke();
        }

        public bool needsTransition(out string nextState)
        {
            nextState = "";
            foreach(StateTransition t in transitions)
            {
                if(t != null && t.evaluateTransition())
                {
                    nextState = t.TargetState;
                    return true;
                }
            }
            return false;
        }

        public string StateName
        {
            get { return name; }
        }

        public List<StateTransition> Transitions
        {
            get { return transitions; }
        }

        public void addTransition(StateTransition transition)
        {
            transitions.Add(transition);
        }

        public void addTransitions(ICollection<StateTransition> transitions)
        {
            this.transitions.AddRange(transitions);
        }

        public override string ToString()
        {
            string message = "State: " + name;
            foreach(StateTransition transition in transitions)
            {
                message += "\n\t\t" + transition;
            }
            return message;
        }
    }
}

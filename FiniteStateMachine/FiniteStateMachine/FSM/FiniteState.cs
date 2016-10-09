using System.Collections.Generic;

namespace FiniteStateMachine.FSM
{
    class FiniteState
    {
        private string name;
        private List<StateTransition> transitions;

        public FiniteState(string name)
        {
            this.name = name;
            transitions = new List<StateTransition>();
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

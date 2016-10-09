using System.Collections.Generic;

namespace FiniteStateMachine.FSM
{
    class FiniteStateMachine
    {
        private string name;
        private string startState;
        private List<FiniteState> states;

        public FiniteStateMachine(string name, string startState)
        {
            this.name = name;
            this.startState = startState;
            states = new List<FiniteState>();
        }

        public string StartState
        {
            get { return startState; }
        }

        public FiniteState getState(string name)
        {
            if(states != null && states.Count > 0)
            {
                foreach(FiniteState state in states)
                {
                    if (state.StateName.ToLower() == name.ToLower())
                        return state;
                }
            }

            return null;
        }

        public void addState(FiniteState state)
        {
            states.Add(state);
        }

        public void addStates(ICollection<FiniteState> states)
        {
            this.states.AddRange(states);
        }

        public override string ToString()
        {
            string message = "Machine: " + name + "\tStart State: " + startState;
            foreach(FiniteState state in states)
            {
                message += "\n\t" + state;
            }
            return message;
        }
    }
}

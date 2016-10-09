using System;
using System.Collections.Generic;

namespace FiniteStateMachine.FSM
{
    class FiniteStateMachine
    {
        private string name;
        private string startState;
        private string currStateName;
        private List<FiniteState> states;
        private FiniteState currentState;

        public FiniteStateMachine(string name, string startState)
        {
            this.name = name;
            this.startState = startState;
            states = new List<FiniteState>();
        }

        public string CurrentState
        {
            get { return currStateName; }
        }

        public void initialiseMachine()
        {
            currentState = getState(startState);
            currStateName = currentState.StateName;
        }

        public void processState()
        {
            if (currentState != null)
            {
                currentState.processState();
                string nextState = "";
                if (currentState.needsTransition(out nextState))
                {
                    currentState = getState(nextState);
                    Console.WriteLine("Changing from: " + currStateName + " to: " + nextState);
                    currStateName = nextState;
                }
            }
        }

        public string StartState
        {
            get { return startState; }
        }

        public List<FiniteState> States
        {
            get { return states; }
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

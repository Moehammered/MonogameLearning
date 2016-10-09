namespace FiniteStateMachine.FSM
{
    class StateTransition
    {
        private string target, condition;

        public StateTransition(string target, string condition)
        {
            this.target = target;
            this.condition = condition;
        }

        public bool evaluateTransition()
        {
            return false;
        }

        public string TargetState
        {
            get { return target; }
        }

        public string Condition
        {
            get { return condition; }
        }

        public override string ToString()
        {
            string message = "Transition: " + target + "\n\t\t\t";
            message += "Condition: " + condition;
            return message;
        }
    }
}

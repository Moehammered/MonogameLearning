using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace FiniteStateMachine.FSM
{
    /// <summary>
    /// An interpreter class designed to 'compile' an xml file which contains the design of state machine definitions.
    /// This interpreter contains all machines it has parsed from the file, and can then be used to give an 'FSMComponent' a machine
    /// to work with. A machine will self contain the means necessary to perform it's sequence or operations.
    /// A component is responsible for assigning delegates to the machine it owns.
    /// </summary>
    class FSMInterpreter
    {
        private XElement file;
        private List<FiniteStateMachine> machines;

        public FSMInterpreter(string filename)
        {
            file = XElement.Load(Environment.CurrentDirectory + "/Content/" + filename);
            machines = new List<FiniteStateMachine>(1);
        }

        public FiniteStateMachine Machine
        {
            get
            {
                string index = file.Attribute("activeFSM").Value;
                int parsedInd = Int32.Parse(index);
                return machines[parsedInd];
            }
        }

        public List<FiniteStateMachine> Machines
        {
            get { return machines; }
        }

        public void parseFile()
        {
            //foreach state machine in the file
            if (file != null)
            {
                Console.WriteLine(file.Elements("fsm").Count());
                foreach (XElement fsm in file.Elements("fsm"))
                {
                    FiniteStateMachine machine = new FiniteStateMachine(fsm.Attribute("name").Value, fsm.Attribute("startState").Value);
                    machine.addStates(parseStates(fsm));
                    Console.WriteLine(machine);

                    machines.Add(machine);
                }
            }
            else
                Console.WriteLine("Failed to read file.");
        }

        private List<FiniteState> parseStates(XElement parent)
        {
            List<FiniteState> states = new List<FiniteState>();

            foreach(XElement state in parent.Elements("state"))
            {
                FiniteState fs = new FiniteState(state.Attribute("name").Value);
                //process the transitions of this state -- add here
                fs.addTransitions(parseTransitions(state));
                states.Add(fs);
            }

            return states;
        }

        private List<StateTransition> parseTransitions(XElement state)
        {
            List<StateTransition> transitions = new List<StateTransition>();

            foreach(XElement transition in state.Elements("transition"))
            {
                StateTransition t = new StateTransition(transition.Attribute("toState").Value, transition.Attribute("condition").Value);
                transitions.Add(t);
            }

            return transitions;
        }
    }
}

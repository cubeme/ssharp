using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    public enum ActionSequenceStates
    {
        StateStart,
        State1,
        State2,
        State3,
        State4,
        State5,
        StateEnd
    }

    class ActionSequence : Component
    {
        public ComputingModule Module { get; private set; }

        public bool[] Conditions { get; private set; }

        public bool Abort { get; set; }

        public bool Completed() => StateMachine.State == ActionSequenceStates.StateEnd;

        public bool IsRunning() => StateMachine.State != ActionSequenceStates.StateStart && StateMachine.State != ActionSequenceStates.StateEnd;

        /// <summary>
		///   Gets the state machine that manages the state of the action sequence
		/// </summary>
		public readonly StateMachine<ActionSequenceStates> StateMachine = ActionSequenceStates.StateStart;     
        
        public ActionSequence(ComputingModule module, bool[] conditions)
        {
            Module = module;
            Conditions = conditions;
        }  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class OutgoingSequence : ActionSequence
    {

        public OutgoingSequence(ComputingModule module, bool[] conditions) : base(module, conditions) { }

        public override void Update()
        {
            StateMachine
                .Transition(
                    from: ActionSequenceStates.StateStart,
                    to: ActionSequenceStates.State1,
                    guard: Conditions[0] == true && Abort == false,
                    action: Module.OutgoingOne)
                .Transition(
                    from: ActionSequenceStates.State1,
                    to: ActionSequenceStates.State2,
                    guard: Conditions[1] && Abort == false,
                    action: Module.OutgoingTwo)
                .Transition(
                    from: ActionSequenceStates.State2,
                    to: ActionSequenceStates.State3,
                    guard: Conditions[2] && Abort == false,
                    action: Module.OutgoingThree)
                .Transition(
                    from: ActionSequenceStates.State3,
                    to: ActionSequenceStates.State4,
                    guard: Conditions[3] && Abort == false,
                    action: Module.OutgoingFour)
                .Transition(
                    from: ActionSequenceStates.State4,
                    to: ActionSequenceStates.StateEnd)
                //Abort
                .Transition(
                    from: ActionSequenceStates.StateStart,
                    to: ActionSequenceStates.StateEnd,
                    guard: Abort == true)
                .Transition(
                    from: ActionSequenceStates.State1,
                    to: ActionSequenceStates.StateEnd,
                    guard: Abort == true)
                .Transition(
                    from: ActionSequenceStates.State2,
                    to: ActionSequenceStates.StateEnd,
                    guard: Abort == true)
                .Transition(
                    from: ActionSequenceStates.State3,
                    to: ActionSequenceStates.StateEnd,
                    guard: Abort == true);
        }
    }
}

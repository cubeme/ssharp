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

        public override void Update()
        {
            StateMachine
                .Transition(
                    from: ActionSequenceStates.StateStart,
                    to: ActionSequenceStates.State1,
                    guard: Steps[0].Condition() == true && Abort == false,
                    action: () => Steps[0].RunStatement())
                .Transition(
                    from: ActionSequenceStates.State1,
                    to: ActionSequenceStates.State2,
                    guard: Steps[1].Condition() && Abort == false,
                    action: () => Steps[1].RunStatement())
                .Transition(
                    from: ActionSequenceStates.State2,
                    to: ActionSequenceStates.State3,
                    guard: Steps[2].Condition() && Abort == false,
                    action: () => Steps[2].RunStatement())
                .Transition(
                    from: ActionSequenceStates.State3,
                    to: ActionSequenceStates.State4,
                    guard: Steps[3].Condition() && Abort == false,
                    action: () => Steps[3].RunStatement())
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

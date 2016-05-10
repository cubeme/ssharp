using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;


    class RetractionSequence : ActionSequence
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
                    to: ActionSequenceStates.State5,
                    guard: Steps[4].Condition() && Abort == false,
                    action: () => Steps[4].RunStatement())
                .Transition(
                    from: ActionSequenceStates.State5,
                    to: ActionSequenceStates.StateEnd)
                .Transition(
                    from: ActionSequenceStates.State1,
                    to: ActionSequenceStates.State4,
                    guard: Steps[5].Condition() == true,
                    action: () => Steps[5].RunStatement())
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
                    guard: Abort == true)
                .Transition(
                    from: ActionSequenceStates.State4,
                    to: ActionSequenceStates.StateEnd,
                    guard: Abort == true);
        }
    }
}

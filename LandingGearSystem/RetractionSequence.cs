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
        public RetractionSequence(ComputingModule module, bool[] conditions) : base(module, conditions) { }

        public override void Update()
        {
            StateMachine
                .Transition(
                    from: ActionSequenceStates.StateStart,
                    to: ActionSequenceStates.State1,
                    guard: Conditions[0] == true && Abort == false,
                    action: Module.RetractionOne)
                .Transition(
                    from: ActionSequenceStates.State1,
                    to: ActionSequenceStates.State2,
                    guard: Conditions[1] && Abort == false,
                    action: Module.RetractionTwo)
                .Transition(
                    from: ActionSequenceStates.State2,
                    to: ActionSequenceStates.State3,
                    guard: Conditions[2] && Abort == false,
                    action: Module.RetractionThree)
                .Transition(
                    from: ActionSequenceStates.State3,
                    to: ActionSequenceStates.State4,
                    guard: Conditions[3] && Abort == false,
                    action: Module.RetractionFour)
                .Transition(
                    from: ActionSequenceStates.State4,
                    to: ActionSequenceStates.State5,
                    guard: Conditions[4] && Abort == false,
                    action: Module.RetractionFive)
                .Transition(
                    from: ActionSequenceStates.State5,
                    to: ActionSequenceStates.StateEnd)
                .Transition(
                    from: ActionSequenceStates.State1,
                    to: ActionSequenceStates.State4,
                    guard:Conditions[5] == true,
                    action: Module.RetractionFour)
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

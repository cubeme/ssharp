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
        WaitOutgoing, 
        OutgoingOne,
        OutgoingTwo,
        OutgoingThree,
        OutgoingFour,
        WaitRetract,
        RetractOne,
        RetractTwo,
        RetractThree,
        RetractFour
    }

    class ActionSequence : Component
    {
        private ComputingModule _module;

        /// <summary>
		///   Gets the state machine that manages the state of the action sequence
		/// </summary>
		public readonly StateMachine<ActionSequenceStates> StateMachine = ActionSequenceStates.WaitRetract;     
        
        public ActionSequence(ComputingModule module)
        {
            _module = module;
        }

        public override void Update()
        {//Klammern weg
            StateMachine
                .Transition(
                    from: ActionSequenceStates.WaitOutgoing,
                    to: ActionSequenceStates.OutgoingOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.One)
                .Transition(
                    from: ActionSequenceStates.OutgoingOne,
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.DoorsOpen,
                    action: _module.OutgoingTwo)
                .Transition(
                    from: ActionSequenceStates.OutgoingOne,
                    to: ActionSequenceStates.RetractFour,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: () => _module.Four())
                .Transition(
                    from: ActionSequenceStates.OutgoingTwo,
                    to: ActionSequenceStates.OutgoingThree,
                    guard: _module.GearsExtended,
                    action: () => _module.OutgoingThree())
                .Transition(
                    from: ActionSequenceStates.OutgoingTwo,
                    to: ActionSequenceStates.RetractTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up && _module.GearShockAbsorberRelaxed,
                    action: () => _module.RetractionTwo())
                .Transition(
                    from: ActionSequenceStates.OutgoingThree,
                    to: ActionSequenceStates.OutgoingFour,
                    guard: _module.HandlePosition.Value == HandlePosition.Down,
                    action: () => _module.Four())
                .Transition(
                    from: ActionSequenceStates.OutgoingThree,
                    to: ActionSequenceStates.RetractTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up && _module.GearShockAbsorberRelaxed,
                    action: () => _module.RetractionTwo())
                .Transition(
                    from: ActionSequenceStates.OutgoingFour,
                    to: ActionSequenceStates.WaitRetract,
                    guard: _module.DoorsClosed,
                    action: () => _module.Zero())
                .Transition(
                    from: ActionSequenceStates.OutgoingFour,
                    to: ActionSequenceStates.RetractOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: () => _module.One())
                .Transition(
                    from: ActionSequenceStates.WaitRetract,
                    to: ActionSequenceStates.RetractOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: () => _module.One())
                .Transition(
                    from: ActionSequenceStates.RetractOne,
                    to: ActionSequenceStates.RetractTwo,
                    guard: _module.DoorsOpen && _module.GearShockAbsorberRelaxed,
                    action: () => _module.RetractionTwo())
                .Transition(
                    from: ActionSequenceStates.RetractOne,
                    to: ActionSequenceStates.OutgoingFour,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: () => _module.Four())
                .Transition(
                    from: ActionSequenceStates.RetractTwo,
                    to: ActionSequenceStates.RetractThree,
                    guard: _module.GearsRetracted,
                    action: () => _module.RetractionThree())
                .Transition(
                    from: ActionSequenceStates.RetractTwo,
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: () => _module.OutgoingTwo())
                .Transition(
                    from: ActionSequenceStates.RetractThree,
                    to: ActionSequenceStates.RetractFour,
                    guard: _module.HandlePosition.Value == HandlePosition.Up,
                    action: () => _module.Four())
                .Transition(
                    from: ActionSequenceStates.RetractThree,
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: () => _module.OutgoingTwo())
                .Transition(
                    from: ActionSequenceStates.RetractFour,
                    to: ActionSequenceStates.WaitOutgoing,
                    guard: _module.DoorsClosed,
                    action: () => _module.Zero())
                .Transition(
                    from: ActionSequenceStates.RetractFour,
                    to: ActionSequenceStates.OutgoingOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: () => _module.One());
        }
    }
}

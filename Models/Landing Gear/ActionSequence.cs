

namespace SafetySharp.CaseStudies.LandingGear
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
        private readonly ComputingModule _module;

        /// <summary>
		///   Gets the state machine that manages the state of the action sequence
		/// </summary>
		public readonly StateMachine<ActionSequenceStates> _stateMachine = ActionSequenceStates.WaitRetract;     
        //todo: private

        public ActionSequence(ComputingModule module)
        {
            _module = module;
        }

        public override void Update()
        {
            _stateMachine
                .Transition(
                    @from: new[] {ActionSequenceStates.WaitOutgoing, ActionSequenceStates.RetractFour},
                    to: ActionSequenceStates.OutgoingOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.One)
                .Transition(
                    @from: ActionSequenceStates.OutgoingOne,
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.DoorsOpen,
                    action: _module.OutgoingTwo)
                .Transition(
                    @from: ActionSequenceStates.OutgoingOne,
                    to: ActionSequenceStates.RetractFour,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: _module.Four)
                .Transition(
                    @from: ActionSequenceStates.OutgoingTwo,
                    to: ActionSequenceStates.OutgoingThree,
                    guard: _module.GearsExtended,
                    action: _module.OutgoingThree)
                .Transition(
                    @from: new [] { ActionSequenceStates.OutgoingTwo, ActionSequenceStates.OutgoingThree},
                      to: ActionSequenceStates.RetractTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up &&
                        _module.GearShockAbsorberRelaxed,
                    action: _module.RetractionTwo)
                .Transition(
                    @from: ActionSequenceStates.OutgoingThree,
                    to: ActionSequenceStates.OutgoingFour,
                    guard: _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.Four)
                .Transition(
                    @from: ActionSequenceStates.OutgoingFour,
                    to: ActionSequenceStates.WaitRetract,
                    guard: _module.DoorsClosed,
                    action: _module.Zero)
                .Transition(
                    @from: new[] {ActionSequenceStates.OutgoingFour, ActionSequenceStates.WaitRetract},
                    to: ActionSequenceStates.RetractOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: _module.One)
                .Transition(
                    @from: ActionSequenceStates.RetractOne,
                    to: ActionSequenceStates.RetractTwo,
                    guard: _module.DoorsOpen && _module.GearShockAbsorberRelaxed,
                    action: _module.RetractionTwo)
                .Transition(
                    @from: ActionSequenceStates.RetractOne,
                    to: ActionSequenceStates.OutgoingFour,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.Four)
                .Transition(
                    @from: ActionSequenceStates.RetractTwo,
                    to: ActionSequenceStates.RetractThree,
                    guard: _module.GearsRetracted,
                    action: _module.RetractionThree)
                .Transition(
                    @from: ActionSequenceStates.RetractTwo,
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.OutgoingTwo)
                .Transition(
                    @from: ActionSequenceStates.RetractThree,
                    to: ActionSequenceStates.RetractFour,
                    guard: _module.HandlePosition.Value == HandlePosition.Up,
                    action: _module.Four)
                .Transition(
                    @from: ActionSequenceStates.RetractThree,
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.OutgoingTwo)
                .Transition(
                    @from: ActionSequenceStates.RetractFour,
                    to: ActionSequenceStates.WaitOutgoing,
                    guard: _module.DoorsClosed,
                    action: _module.Zero);
        }
    }
}

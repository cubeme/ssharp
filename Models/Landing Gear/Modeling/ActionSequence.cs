

namespace SafetySharp.CaseStudies.LandingGear.Modeling
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

    internal class ActionSequence : Component
    {
        private readonly ComputingModule _module;

        /// <summary>
        ///   Gets the state machine that manages the state of the action sequence
        /// </summary>
        private readonly StateMachine<ActionSequenceStates> _stateMachine;

        public ActionSequenceStates State => _stateMachine.State;

        public ActionSequence(ComputingModule module, ActionSequenceStates startState)
        {
            _module = module;
            _stateMachine = startState;
        }

        public bool Reset { get; private set; }

        public override void Update()
        {
            _stateMachine
                .Transition(
                    @from: ActionSequenceStates.WaitOutgoing,
                    to: ActionSequenceStates.OutgoingOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.One)
                .Transition(
                    from: ActionSequenceStates.RetractFour,
                    to: ActionSequenceStates.OutgoingOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: () =>
                    {
                        _module.One();
                        Reset = true;
                    })
                .Transition(
                    @from: ActionSequenceStates.OutgoingOne,
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.DoorsOpen,
                    action: () =>
                    {
                        _module.OutgoingTwo();
                        Reset = false;
                    })
                .Transition(
                    @from: ActionSequenceStates.OutgoingOne,
                    to: ActionSequenceStates.RetractFour,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: () =>
                    {
                        _module.Four();
                        Reset = true;
                    })
                .Transition(
                    @from: ActionSequenceStates.OutgoingTwo,
                    to: ActionSequenceStates.OutgoingThree,
                    guard: _module.GearsExtended,
                    action: () =>
                    {
                        _module.OutgoingThree();
                        Reset = false;
                    })
                .Transition(
                    @from: new [] { ActionSequenceStates.OutgoingTwo, ActionSequenceStates.OutgoingThree},
                      to: ActionSequenceStates.RetractTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up &&
                        _module.GearShockAbsorberRelaxed,
                    action: () =>
                    {
                        _module.RetractionTwo();
                        Reset = true;
                    })
                .Transition(
                    @from: ActionSequenceStates.OutgoingThree,
                    to: ActionSequenceStates.OutgoingFour,
                    guard: _module.HandlePosition.Value == HandlePosition.Down,
                    action: _module.Four)
                .Transition(
                    @from: ActionSequenceStates.OutgoingFour,
                    to: ActionSequenceStates.WaitRetract,
                    guard: _module.DoorsClosed,
                    action: () =>
                    {
                        _module.Zero();
                        Reset = false;
                    })
                .Transition(
                    @from: ActionSequenceStates.OutgoingFour,
                    to: ActionSequenceStates.RetractOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: () =>
                    {
                        _module.One();
                        Reset = true;
                    })
                .Transition(
                    from: ActionSequenceStates.WaitRetract,
                    to: ActionSequenceStates.RetractOne,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Up,
                    action: _module.One)
                .Transition(
                    @from: ActionSequenceStates.RetractOne,
                    to: ActionSequenceStates.RetractTwo,
                    guard: _module.DoorsOpen && _module.GearShockAbsorberRelaxed,
                    action: () =>
                    {
                        _module.RetractionTwo();
                        Reset = false;
                    })
                //if the gear shock absorbers are  not relaxed
                .Transition(
                    @from: ActionSequenceStates.RetractOne,
                    to: ActionSequenceStates.RetractThree,
                    guard: _module.DoorsOpen && !_module.GearShockAbsorberRelaxed,
                    action: () =>
                    {
                        Reset = false;
                    })
                .Transition(
                    @from: ActionSequenceStates.RetractOne,
                    to: ActionSequenceStates.OutgoingFour,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: () =>
                    {
                        _module.Four();
                        Reset = true;
                    })
                .Transition(
                    @from: ActionSequenceStates.RetractTwo,
                    to: ActionSequenceStates.RetractThree,
                    guard: _module.GearsRetracted,
                    action: () =>
                    {
                        _module.RetractionThree();
                        Reset = false;
                    })
                .Transition(
                    @from: new [] { ActionSequenceStates.RetractTwo, ActionSequenceStates.RetractThree},
                    to: ActionSequenceStates.OutgoingTwo,
                    guard: _module.HandleHasMoved && _module.HandlePosition.Value == HandlePosition.Down,
                    action: () =>
                    {
                        _module.OutgoingTwo();
                        Reset = true;
                    })
                .Transition(
                    @from: ActionSequenceStates.RetractThree,
                    to: ActionSequenceStates.RetractFour,
                    guard: _module.HandlePosition.Value == HandlePosition.Up,
                    action: _module.Four)
                .Transition(
                    @from: ActionSequenceStates.RetractFour,
                    to: ActionSequenceStates.WaitOutgoing,
                    guard: _module.DoorsClosed,
                    action: () =>
                    {
                        _module.Zero();
                        Reset = false;
                    })
               ;
        }
    }
}

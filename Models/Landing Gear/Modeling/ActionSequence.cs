namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes all possible states of the action sequence.
    /// </summary>
    public enum ActionSequenceStates
    {
        /// <summary>
        /// Indicates that the gears are in retracted position.
        /// </summary>
        WaitOutgoing,

        /// <summary>
        /// Indicates that the action sequence is in the first state of the outgoing sequence.
        /// </summary>
        OutgoingOne,

        /// <summary>
        /// Indicates that the action sequence is in the second state of the outgoing sequence.
        /// </summary>
        OutgoingTwo,

        /// <summary>
        /// Indicates that the action sequence is in the third state of the outgoing sequence.
        /// </summary>
        OutgoingThree,

        /// <summary>
        /// Indicates that the action sequence is in the fourth state of the outgoing sequence.
        /// </summary>
        OutgoingFour,

        /// <summary>
        /// Indicates that the gears are extended.
        /// </summary>
        WaitRetract,

        /// <summary>
        /// Indicates that the action sequence is in the first state of the retraction sequence.
        /// </summary>
        RetractOne,

        /// <summary>
        /// Indicates that the action sequence is in the second state of the retraction sequence.
        /// </summary>
        RetractTwo,

        /// <summary>
        /// Indicates that the action sequence is in the third state of the retraction sequence.
        /// </summary>
        RetractThree,

        /// <summary>
        /// Indicates that the action sequence is in the fourth state of the retraction sequence.
        /// </summary>
        RetractFour
    }

    internal class ActionSequence : Component
    {
        /// <summary>
        ///  An instance of the computing module that initializes the action sequenee.
        /// </summary>
        private readonly ComputingModule _module;

        /// <summary>
        ///   Gets the state machine that manages the state of the action sequence.
        /// </summary>
        private readonly StateMachine<ActionSequenceStates> _stateMachine;

        /// <summary>
        ///   Gets current state of the state machine managing the action sequence.
        /// </summary>
        public ActionSequenceStates State => _stateMachine.State;

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        /// <param name="module"> An instance of the computing module that initializes the action sequence. </param>
        /// <param name="startState"> The initial state of the action sequence state machine. </param>
        public ActionSequence(ComputingModule module, ActionSequenceStates startState)
        {
            _module = module;
            _stateMachine = startState;
        }

        /// <summary>
        /// Indicates whether or not the action sequence motion has been reversed.
        /// </summary>
        public bool Reset { get; private set; }

        /// <summary>
        /// Updates the ActionSequence instance.
        /// </summary>
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
                    @from: new[] { ActionSequenceStates.OutgoingTwo, ActionSequenceStates.OutgoingThree },
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
                    action: () => { Reset = false; })
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
                    @from: new[] { ActionSequenceStates.RetractTwo, ActionSequenceStates.RetractThree },
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
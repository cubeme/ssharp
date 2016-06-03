

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the state of the analogical switch.
    /// </summary>
    public enum AnalogicalSwitchStates
    {
        /// <summary>
        /// State indicating the analogical switch is open.
        /// </summary>
        Open,
        /// <summary>
        /// State indicating the analogical switch is moving to close.
        /// </summary>
        MoveClosing,
        /// <summary>
        /// State indicating the analogical switch is closed.
        /// </summary>
        Closed,
        /// <summary>
        /// State indicating the analogical switch is moving to open.
        /// </summary>
        MoveOpening
    }

    class AnalogicalSwitch : Component
    {
        /// <summary>
		///   Gets the state machine that manages the state of the analogical switch.
		/// </summary>
		private readonly StateMachine<AnalogicalSwitchStates> _stateMachine = AnalogicalSwitchStates.Open;

        /// <summary>
		///   Gets the current position of the analogical switch.
		/// </summary>
        public AnalogicalSwitchStates SwitchPosition => _stateMachine.State;

        /// <summary>
        ///  Times the movement of the analogical switch.
        /// </summary>
        private readonly Timer _timer = new Timer();

        /// <summary>
        /// Gets a value indicating whether the pilot handle has been moved.
        /// </summary>
        public extern bool HandleHasBeenMoved { get;  }

        /// <summary>
        /// Gets the value of the incoming electrical order.
        /// </summary>
        public extern bool IncomingEOrder();

        /// <summary>
        /// Gets the value of the outgoing electrical order if the switch is closed.
        /// </summary>
        public bool OutgoingEOrder() => _stateMachine == AnalogicalSwitchStates.Closed ? IncomingEOrder() : false;

        /// <summary>
        /// Updates the analogical switch.
        /// </summary>
        public override void Update()
        {
            Update(_timer);

            _stateMachine
                .Transition(
                    from: AnalogicalSwitchStates.Open,
                    to: AnalogicalSwitchStates.MoveClosing,
                    guard: HandleHasBeenMoved,
                    action: () =>
                    {
                        _timer.Start(8);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.MoveClosing,
                    to: AnalogicalSwitchStates.Closed,
                    guard: _timer.HasElapsed,
                    action: () =>
                    {
                        _timer.Start(200);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.Closed,
                    guard: HandleHasBeenMoved,
                    action: () =>
                    {
                        _timer.Start(200);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.MoveOpening,
                    guard: _timer.HasElapsed,
                    action: () =>
                    {
                        _timer.Start(12);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.MoveOpening,
                    to: AnalogicalSwitchStates.MoveClosing,
                    guard: HandleHasBeenMoved,
                    action: () =>
                    {
                        _timer.Start(8 - (2 * _timer.RemainingTime) / 3);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.MoveOpening,
                    to: AnalogicalSwitchStates.Open,
                    guard: _timer.HasElapsed);

        }
    }
}

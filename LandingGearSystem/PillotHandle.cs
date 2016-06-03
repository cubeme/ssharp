

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class PilotHandle : Component
    {
        /// <summary>
        ///  Indicates whether the pilot handle has been moved.
        /// </summary>
        public bool Moved { get; private set; }

        /// <summary>
        /// Gets the current position of the pilot handle.
        /// </summary>
        public HandlePosition PilotHandlePosition => _stateMachine.State;

        /// <summary>
		///   Gets the state machine that manages the state of the pilot handle.
		/// </summary>
		private readonly StateMachine<HandlePosition> _stateMachine = HandlePosition.Down;

        /// <summary>
        /// Is called by the pilot if the handle position has changed.
        /// </summary>
        public void HasMoved()
        {
            _stateMachine
                .Transition(
                    from: HandlePosition.Down,
                    to: HandlePosition.Up,
                    action: () => Moved = true)

                .Transition(
                    from: HandlePosition.Up,
                    to: HandlePosition.Down,
                    action: () => Moved = true);
        }
    
        public override void Update()
        {
            _stateMachine
                .Transition(
                    from: HandlePosition.Down,
                    to: HandlePosition.Down,
                    action: () => Moved = false)

                .Transition(
                    from: HandlePosition.Up,
                    to: HandlePosition.Up,
                    action: () => Moved = false);               
        }
    }
}


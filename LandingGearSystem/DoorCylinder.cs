

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class DoorCylinder : Cylinder
    {

        /// <summary>
        ///  Timer to time the movement of the door cylinder.
        /// </summary>
        private readonly Timer _timer = new Timer();

        public DoorCylinder(CylinderPosition position) : base(position) { }

        /// <summary>
		///   Gets the state machine that manages the state of the door cylinder.
		/// </summary>
		private readonly StateMachine<DoorStates> _stateMachine = DoorStates.LockedClosed;

        /// <summary>
        /// Two latching boxes locking the door cylinder in closed position.
        /// </summary>
        private readonly LatchingBox _latchingBoxClosedOne = new LatchingBox(4,3);
        private readonly LatchingBox _latchingBoxClosedTwo = new LatchingBox(4,3);

        public DoorStates DoorCylinderState => _stateMachine.State;

        /// <summary>
        /// Updates the door cylinder
        /// </summary>
        public override void Update()
        {
            Update(_timer, _latchingBoxClosedOne, _latchingBoxClosedTwo);

            //Normal motion
	        _stateMachine
		        .Transition(
			        from: new[] { DoorStates.LockedClosed, DoorStates.LockingClosed},
			        to: DoorStates.UnlockingClosed,
			        guard: ExtensionCircuitIsPressurized,
			        action: () =>
			        {
			            _latchingBoxClosedOne.Unlock();
						_latchingBoxClosedTwo.Unlock();
			        })

				.Transition(
					from: DoorStates.UnlockingClosed,
					to: DoorStates.MoveOpening,
					guard:
						ExtensionCircuitIsPressurized && _latchingBoxClosedOne.IsUnlocked &&
						_latchingBoxClosedTwo.IsUnlocked,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 : 15);
					})

				.Transition(
					from: DoorStates.MoveOpening,
					to: DoorStates.Open,
					guard: ExtensionCircuitIsPressurized && _timer.HasElapsed)

				.Transition(
					from: DoorStates.Open,
					to: DoorStates.Open,
					guard: ExtensionCircuitIsPressurized)

				.Transition(
					from: new [] { DoorStates.Open, DoorStates.OpenLoose},
                    to: DoorStates.MoveClosing,
					guard: RetractionCurcuitIsPressurized,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 : 16);
					})

				.Transition(
					from: DoorStates.Open,
					to: DoorStates.OpenLoose,
					guard: RetractionCurcuitIsPressurized == false && ExtensionCircuitIsPressurized == false)

				.Transition(
					from: DoorStates.OpenLoose,
					to: DoorStates.Open,
					guard: ExtensionCircuitIsPressurized)

				.Transition(
					from: DoorStates.MoveClosing,
					to: DoorStates.LockingClosed,
					guard: RetractionCurcuitIsPressurized && _timer.HasElapsed,
					action: () =>
					{
						_latchingBoxClosedOne.Lock();
						_latchingBoxClosedTwo.Lock();
					})

				.Transition(
					from: DoorStates.LockingClosed,
					to: DoorStates.LockedClosed,
					guard:
						RetractionCurcuitIsPressurized && _latchingBoxClosedOne.IsLocked &&
						_latchingBoxClosedTwo.IsLocked)

                .Transition(
					from: DoorStates.MoveClosing,
					to: DoorStates.MoveOpening,
					guard: ExtensionCircuitIsPressurized,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 - _timer.RemainingTime : 15 - (15 * _timer.RemainingTime) / 16);
					})

				.Transition(
					from: DoorStates.MoveOpening,
					to: DoorStates.MoveClosing,
					guard: RetractionCurcuitIsPressurized ,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 - _timer.RemainingTime : 16 - (16 * _timer.RemainingTime) / 15);
					})

                .Transition(
                    from: DoorStates.UnlockingClosed,
                    to: DoorStates.LockingClosed,
                    guard: RetractionCurcuitIsPressurized,
                    action: () =>
                    {
                        _latchingBoxClosedOne.Lock();
                        _latchingBoxClosedTwo.Lock();
                    });

        }

    }
}

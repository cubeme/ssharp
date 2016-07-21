

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class DoorCylinder : Cylinder
    {
        /// <summary>
        ///   The fault keeps the door cylinder stuck in a certain state.
        /// </summary>
        public readonly Fault DoorCylinderIsStuckFault = new PermanentFault();

        /// <summary>
        /// Two latching boxes locking the door cylinder in closed position.
        /// </summary>
        private readonly LatchingBox _latchingBoxClosedOne;
        private readonly LatchingBox _latchingBoxClosedTwo;

        public DoorCylinder(Position position)
            : base(position)
        {
            DoorCylinderIsStuckFault.Name = $"{Position}DoorCylinderIsStuck";
            _latchingBoxClosedOne = new LatchingBox(4, 3, $"{Position}DoorCylinderLatchingBoxClosedOne");
            _latchingBoxClosedTwo = new LatchingBox(4, 3, $"{Position}DoorCylinderLatchingBoxClosedTwo");
        }

        /// <summary>
		///   Gets the state machine that manages the state of the door cylinder.
		/// </summary>
		private readonly StateMachine<DoorStates> _stateMachine = DoorStates.LockedClosed;                     

        public DoorStates DoorCylinderState => _stateMachine.State;

        /// <summary>
        /// Updates the door cylinder
        /// </summary>
        public override void Update()
        {
            Update(Timer, _latchingBoxClosedOne, _latchingBoxClosedTwo);

	        _stateMachine
		        .Transition(
			        @from: new[] { DoorStates.LockedClosed, DoorStates.LockingClosed},
			        to: DoorStates.UnlockingClosed,
			        guard: ExtensionCircuitIsPressurized,
			        action: () =>
			        {
			            _latchingBoxClosedOne.Unlock();
						_latchingBoxClosedTwo.Unlock();
			        })

				.Transition(
					@from: DoorStates.UnlockingClosed,
					to: DoorStates.MoveOpening,
					guard:
						ExtensionCircuitIsPressurized && _latchingBoxClosedOne.IsUnlocked &&
						_latchingBoxClosedTwo.IsUnlocked,
					action: () =>
					{
						Timer.Start(Position == Position.Front ? 12 : 15);
					})

				.Transition(
					@from: DoorStates.MoveOpening,
					to: DoorStates.Open,
					guard: ExtensionCircuitIsPressurized && Timer.HasElapsed)

				.Transition(
					@from: DoorStates.Open,
					to: DoorStates.Open,
					guard: ExtensionCircuitIsPressurized)

				.Transition(
					@from: new [] { DoorStates.Open, DoorStates.OpenLoose},
                    to: DoorStates.MoveClosing,
					guard: RetractionCurcuitIsPressurized,
					action: () =>
					{
						Timer.Start(Position == Position.Front ? 12 : 16);
					})

				.Transition(
					@from: DoorStates.Open,
					to: DoorStates.OpenLoose,
					guard: RetractionCurcuitIsPressurized == false && ExtensionCircuitIsPressurized == false)

				.Transition(
					@from: DoorStates.OpenLoose,
					to: DoorStates.Open,
					guard: ExtensionCircuitIsPressurized)

				.Transition(
					@from: DoorStates.MoveClosing,
					to: DoorStates.LockingClosed,
					guard: RetractionCurcuitIsPressurized && Timer.HasElapsed,
					action: () =>
					{
						_latchingBoxClosedOne.Lock();
						_latchingBoxClosedTwo.Lock();
					})

				.Transition(
					@from: DoorStates.LockingClosed,
					to: DoorStates.LockedClosed,
					guard:
						RetractionCurcuitIsPressurized && _latchingBoxClosedOne.IsLocked &&
						_latchingBoxClosedTwo.IsLocked)

                .Transition(
					@from: DoorStates.MoveClosing,
					to: DoorStates.MoveOpening,
					guard: ExtensionCircuitIsPressurized,
					action: () =>
					{
						Timer.Start(Position == Position.Front ? 12 - Timer.RemainingTime : 15 - (15 * Timer.RemainingTime) / 16);
					})

				.Transition(
					@from: DoorStates.MoveOpening,
					to: DoorStates.MoveClosing,
					guard: RetractionCurcuitIsPressurized ,
					action: () =>
					{
						Timer.Start(Position == Position.Front ? 12 - Timer.RemainingTime : 16 - (16 * Timer.RemainingTime) / 15);
					})

                .Transition(
                    @from: DoorStates.UnlockingClosed,
                    to: DoorStates.LockingClosed,
                    guard: RetractionCurcuitIsPressurized,
                    action: () =>
                    {
                        _latchingBoxClosedOne.Lock();
                        _latchingBoxClosedTwo.Lock();
                    });

        }

        /// <summary>
        ///   Keeps the door cylinder stuck in one state.
        /// </summary>
        [FaultEffect(Fault = nameof(DoorCylinderIsStuckFault))]
        public class DoorCylinderIsStuckFaultEffect : DoorCylinder
        {
            public DoorCylinderIsStuckFaultEffect(Position position) : base(position) { }

            public override void Update()
            {
                Update(Timer, _latchingBoxClosedOne, _latchingBoxClosedTwo);

                //no statemachine transtiions

            }
        }

    }
}

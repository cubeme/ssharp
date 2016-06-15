

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    class GearCylinder : Cylinder
    {

        /// <summary>
        ///   The fault keeps the gear cylinder stuck in a certain state.
        /// </summary>
        public readonly Fault GearCylinderIsStuckFault = new PermanentFault();

        /// <summary>
        ///  Timer to time the movement of the gear cylinder.
        /// </summary>
        private readonly Timer _timer = new Timer();

        public GearCylinder(CylinderPosition position, GearStates startState)
            : base(position)
        {
            _stateMachine = startState;
        }

        /// <summary>
        ///   Gets the state machine that manages the state of the gear cylinder.
        /// </summary>
        private readonly StateMachine<GearStates> _stateMachine;

        /// <summary>
        /// Latching box locking the gear cylinder in extended position.
        /// </summary>
        private readonly LatchingBox _latchingBoxExtended = new LatchingBox(8, 4);

        /// <summary>
        /// Latching box locking the gear cylinder in retracted position.
        /// </summary>
        private readonly LatchingBox _latchingBoxRetracted = new LatchingBox(8, 4);

        /// <summary>
		///   Gets the current state of the gear cylinder.
		/// </summary>
        public GearStates GearCylinderState => _stateMachine.State;

        public override void Update()
        {
            Update(_timer, _latchingBoxExtended, _latchingBoxRetracted);

	        _stateMachine
		        .Transition(
			        @from: new[] { GearStates.LockedExtended, GearStates.LockingExtended},
			        to: GearStates.UnlockingExtended,
			        guard: RetractionCurcuitIsPressurized,
			        action: () =>
			        {
			            _latchingBoxExtended.Unlock();
			        })

				.Transition(
					@from: GearStates.UnlockingExtended,
					to: GearStates.MoveRetracting,
					guard: RetractionCurcuitIsPressurized && _latchingBoxExtended.IsUnlocked,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 16 : 20);
					})

				.Transition(
					@from: GearStates.MoveRetracting,
					to: GearStates.LockingRetracted,
					guard: RetractionCurcuitIsPressurized  && _timer.HasElapsed,
					action: () =>
					{
						_latchingBoxRetracted.Lock();
					})

				.Transition(
					@from: GearStates.LockingRetracted,
					to: GearStates.LockedRetracted,
					guard: RetractionCurcuitIsPressurized  && _latchingBoxRetracted.IsLocked)

				.Transition(
					@from: new[] {GearStates.LockedRetracted, GearStates.LockingRetracted },
                    to: GearStates.UnlockingRetracted,
					guard: ExtensionCircuitIsPressurized,
					action: () =>
					{
						_latchingBoxRetracted.Unlock();
					})

				.Transition(
					@from: GearStates.UnlockingRetracted,
					to: GearStates.MoveExtending,
					guard: ExtensionCircuitIsPressurized  && _latchingBoxRetracted.IsUnlocked,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 : 16);
					})

				.Transition(
					@from: GearStates.MoveExtending,
					to: GearStates.LockingExtended,
					guard: ExtensionCircuitIsPressurized  && _timer.HasElapsed,
					action: () =>
					{
						_latchingBoxExtended.Lock();
					})

				.Transition(
					@from: GearStates.LockingExtended,
					to: GearStates.LockedExtended,
					guard: ExtensionCircuitIsPressurized  && _latchingBoxExtended.IsLocked)

				.Transition(
					@from: GearStates.UnlockingExtended,
					to: GearStates.LockingExtended,
					guard: ExtensionCircuitIsPressurized,
					action: () =>
					{
						_latchingBoxExtended.Lock();
					})

				.Transition(
					@from: GearStates.MoveRetracting,
					to: GearStates.MoveExtending,
					guard: ExtensionCircuitIsPressurized,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 - (3 * _timer.RemainingTime) / 4 : 16 - (4 * _timer.RemainingTime) / 5);
					})

				.Transition(
					@from: GearStates.MoveExtending,
					to: GearStates.MoveRetracting,
					guard: RetractionCurcuitIsPressurized,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 16 - (4 * _timer.RemainingTime) / 3 : 20 - (5 * _timer.RemainingTime) / 4);
					})

                .Transition(
                    @from: GearStates.UnlockingRetracted,
                    to: GearStates.LockingRetracted,
                    guard: RetractionCurcuitIsPressurized,
                    action: () =>
                    {
                        _latchingBoxRetracted.Lock();
                    });

        }

        /// <summary>
        ///   Keeps the gear cylinder stuck in one state.
        /// </summary>
        [FaultEffect(Fault = nameof(GearCylinderIsStuckFault))]
        public class GearCylinderIsStuckFaultEffect : GearCylinder
        {
            public GearCylinderIsStuckFaultEffect(CylinderPosition position, GearStates start) : base(position, start) { }

            public override void Update()
            {
                Update(_timer, _latchingBoxExtended, _latchingBoxRetracted);

                //no statemachine transtiions

            }
        }

    }
}

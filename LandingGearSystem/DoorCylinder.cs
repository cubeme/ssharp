using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public readonly StateMachine<DoorStates> StateMachine = DoorStates.LockedClosed;

        /// <summary>
        /// Two latching boxes locking the door cylinder in closed position.
        /// </summary>
        public readonly LatchingBox _latchingBoxClosedOne = new LatchingBox(4,3);
        public readonly LatchingBox _latchingBoxClosedTwo = new LatchingBox(4,3);

        public DoorStates DoorCylinderState => StateMachine.State;

        /// <summary>
        /// Updates the door cylinder
        /// </summary>
        public override void Update()
        {
            Update(_timer, _latchingBoxClosedOne, _latchingBoxClosedTwo);

            //Normal motion
	        StateMachine
		        .Transition(
			        from: DoorStates.LockedClosed,
			        to: DoorStates.UnlockingClosed,
			        guard: CheckPressureExtensionCircuit == true,
			        action: () =>
			        {
			            _latchingBoxClosedOne.Unlock();
						_latchingBoxClosedTwo.Unlock();
			        })

				.Transition(
					from: DoorStates.UnlockingClosed,
					to: DoorStates.MoveOpening,
					guard:
						CheckPressureExtensionCircuit == true && _latchingBoxClosedOne.StateMachine == LatchingBoxState.Unlocked &&
						_latchingBoxClosedTwo.StateMachine == LatchingBoxState.Unlocked,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 : 15);
					})

				.Transition(
					from: DoorStates.MoveOpening,
					to: DoorStates.Open,
					guard: CheckPressureExtensionCircuit == true && _timer.HasElapsed)

				.Transition(
					from: DoorStates.Open,
					to: DoorStates.Open,
					guard: CheckPressureExtensionCircuit == true)

				.Transition(
					from: DoorStates.Open,
					to: DoorStates.MoveClosing,
					guard: CheckPressureRetractionCircuit == true,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 : 16);
					})

				.Transition(
					from: DoorStates.Open,
					to: DoorStates.OpenLoose,
					guard: CheckPressureRetractionCircuit == false && CheckPressureExtensionCircuit == false)

				.Transition(
					from: DoorStates.OpenLoose,
					to: DoorStates.Open,
					guard: CheckPressureExtensionCircuit == true)

				.Transition(
					from: DoorStates.OpenLoose,
					to: DoorStates.MoveClosing,
					guard: CheckPressureRetractionCircuit == true,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 : 16);
					})

				.Transition(
					from: DoorStates.MoveClosing,
					to: DoorStates.LockingClosed,
					guard: CheckPressureRetractionCircuit == true && _timer.HasElapsed,
					action: () =>
					{
						_latchingBoxClosedOne.Lock();
						_latchingBoxClosedTwo.Lock();
					})

				.Transition(
					from: DoorStates.LockingClosed,
					to: DoorStates.LockedClosed,
					guard:
						CheckPressureRetractionCircuit == true && _latchingBoxClosedOne.StateMachine == LatchingBoxState.Locked &&
						_latchingBoxClosedTwo.StateMachine == LatchingBoxState.Locked)

		        //Reverse Motion
				.Transition(
					from: DoorStates.MoveClosing,
					to: DoorStates.MoveOpening,
					guard: CheckPressureExtensionCircuit == true,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 - _timer.RemainingTime : 15 - (15 * _timer.RemainingTime) / 16);
					})

				.Transition(
					from: DoorStates.MoveOpening,
					to: DoorStates.MoveClosing,
					guard: CheckPressureRetractionCircuit == true,
					action: () =>
					{
						_timer.Start(Position == CylinderPosition.Front ? 12 - _timer.RemainingTime : 16 - (16 * _timer.RemainingTime) / 15);
					})

	             .Transition(
                    from: DoorStates.LockingClosed,
                    to: DoorStates.UnlockingClosed,
                    guard: CheckPressureExtensionCircuit == true,
                    action: () =>
                    {
                        _latchingBoxClosedOne.Unlock();
                        _latchingBoxClosedTwo.Unlock();
                    })
                    
                .Transition(
                    from: DoorStates.UnlockingClosed,
                    to: DoorStates.LockingClosed,
                    guard: CheckPressureRetractionCircuit == true,
                    action: () =>
                    {
                        _latchingBoxClosedOne.Lock();
                        _latchingBoxClosedTwo.Lock();
                    });

        }

    }
}

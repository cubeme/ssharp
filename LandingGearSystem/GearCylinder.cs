using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class GearCylinder : Cylinder
    {
        
        /// <summary>
        ///  Timer to time the movement of the gear cylinder.
        /// </summary>
        private readonly Timer _timer = new Timer();

        public GearCylinder(CylinderPosition position) : base(position) { }

        /// <summary>
		///   Gets the state machine that manages the state of the gear cylinder.
		/// </summary>
		public readonly StateMachine<GearStates> StateMachine = GearStates.LockedExtended;

        /// <summary>
        /// Latching box locking the gear cylinder in extended position.
        /// </summary>
        private LatchingBox _latchingBoxExtended = new LatchingBox(8, 4);

        /// <summary>
        /// Latching box locking the gear cylinder in retracted position.
        /// </summary>
        private LatchingBox _latchingBoxRetracted = new LatchingBox(8, 4);

        public override void Update()
        {
            Update(_timer, _latchingBoxExtended, _latchingBoxRetracted);

            StateMachine
                .Transition(
                    from: GearStates.LockedExtended,
                    to: GearStates.UnlockingExtended,
                    guard: CheckPressureRetractionCircuit() == true,
                    action: () =>
                    {
                        _latchingBoxExtended.Unlock = true;
                    })

                .Transition(
                    from: GearStates.UnlockingExtended,
                    to: GearStates.MoveRetracting,
                    guard: CheckPressureRetractionCircuit() == true && _latchingBoxExtended.StateMachine == LatchingBoxState.Unlocked,
                    action: () =>
                    {
                        _timer.SetTimeout(Position == CylinderPosition.Front ? 16 : 20);
                        _timer.Start();
                    })

                .Transition(
                    from: GearStates.MoveRetracting,
                    to: GearStates.LockingRetracted,
                    guard: CheckPressureRetractionCircuit() == true && _timer.HasElapsed,
                    action: () =>
                    {
                        _latchingBoxRetracted.Lock = true;
                    })

                .Transition(
                    from: GearStates.LockingRetracted,
                    to: GearStates.LockedRetracted,
                    guard: CheckPressureRetractionCircuit() == true && _latchingBoxRetracted.StateMachine == LatchingBoxState.Locked)

                .Transition(
                    from: GearStates.LockedRetracted,
                    to: GearStates.UnlockingRetracted,
                    guard: CheckPressureExtensionCircuit() == true,
                    action: () =>
                    {
                        _latchingBoxRetracted.Unlock = true;
                    })

                .Transition(
                    from: GearStates.UnlockingRetracted,
                    to: GearStates.MoveExtending,
                    guard: CheckPressureExtensionCircuit() == true && _latchingBoxRetracted.StateMachine == LatchingBoxState.Unlocked,
                    action: () =>
                    {
                        _timer.SetTimeout(Position == CylinderPosition.Front ? 12 : 16);
                        _timer.Start();
                    })

                .Transition(
                    from: GearStates.MoveExtending,
                    to: GearStates.LockingExtended,
                    guard: CheckPressureExtensionCircuit() == true && _timer.HasElapsed,
                    action: () =>
                    {
                        _latchingBoxExtended.Lock = true;
                    })

                .Transition(
                    from: GearStates.LockingExtended,
                    to: GearStates.LockedExtended,
                    guard: CheckPressureExtensionCircuit() == true && _latchingBoxExtended.StateMachine == LatchingBoxState.Locked)

            //Reverse Motion

                .Transition(
                    from: GearStates.UnlockingExtended,
                    to: GearStates.LockingExtended,
                    guard: CheckPressureExtensionCircuit() == true,
                    action: () =>
                    {
                        _latchingBoxExtended.Unlock = false;
                        _latchingBoxExtended.Lock = true;
                    })

                .Transition(
                    from: GearStates.LockingExtended,
                    to: GearStates.UnlockingExtended,
                    guard: CheckPressureRetractionCircuit() == true,
                    action: () =>
                    {
                        _latchingBoxExtended.Lock = false;
                        _latchingBoxExtended.Unlock = true;
                    })

                .Transition(
                    from: GearStates.MoveRetracting,
                    to: GearStates.MoveExtending,
                    guard: CheckPressureExtensionCircuit() == true,
                    action: () =>
                    {
                        _timer.SetTimeout(Position == CylinderPosition.Front ? 12 - (3 * _timer.RemainingTime) / 4 : 16 - (4 * _timer.RemainingTime) / 5);
                        _timer.Start();
                    })

                .Transition(
                    from: GearStates.MoveExtending,
                    to: GearStates.MoveRetracting,
                    guard: CheckPressureRetractionCircuit() == true,
                    action: () =>
                    {
                        _timer.SetTimeout(Position == CylinderPosition.Front ? 16 - (4 * _timer.RemainingTime) / 3 : 20 - (5 * _timer.RemainingTime) / 4);
                        _timer.Start();
                    })

                .Transition(
                    from: GearStates.LockingRetracted,
                    to: GearStates.UnlockingRetracted,
                    guard: CheckPressureExtensionCircuit() == true,
                    action: () =>
                    {
                        _latchingBoxRetracted.Lock = false;
                        _latchingBoxRetracted.Unlock = true;
                    })

                .Transition(
                    from: GearStates.UnlockingRetracted,
                    to: GearStates.LockingRetracted,
                    guard: CheckPressureRetractionCircuit() == true,
                    action: () =>
                    {
                        _latchingBoxRetracted.Unlock = false;
                        _latchingBoxRetracted.Lock = true;
                    });
      
        }

    }
}

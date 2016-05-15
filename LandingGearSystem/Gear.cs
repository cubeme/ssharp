using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the state of the gear.
    /// </summary>
    public enum GearStates
    {
        /// <summary>
        /// Position indicating the gear is locked in extended position.
        /// </summary>
        LockedExtended,
        /// <summary>
        /// State indicating the gear is being unlocked in the extended position.
        /// </summary>
        UnlockingExtended,
        /// <summary>
        /// State indicating the gear is moving to retract.
        /// </summary>
        MoveRetracting,
        /// <summary>
        /// State indicating the gear is being locked in retracted position.
        /// </summary>
        LockingRetracted,
        /// <summary>
        /// State indicating the gear is locked in retracted position.
        /// </summary>
        LockedRetracted,
        /// <summary>
        /// State indicating the gear is being unlocked in the retracted position
        /// </summary>
        UnlockingRetracted,
        /// <summary>
        /// State indicating the gear is moving to extend.
        /// </summary>
        MoveExtending,
        /// <summary>
        /// State indicating the gear is being locked in extended position.
        /// </summary>
        LockingExtended
    }

    /// <summary>
    ///  Describes the position of the gear.
    /// </summary>
    public enum GearPosition
    {
        /// <summary>
        /// Position indicating the gear is located in the front of the airplane.
        /// </summary>
        Front,
        /// <summary>
        /// Position indicating the gear is located on the left side of the airplane.
        /// </summary>
        Left,
        /// <summary>
        /// Position indicating the gear is located on the right side of the airplane.
        /// </summary>
        Right
    }

    class Gear : Component
    {
        /// <summary>
        /// Indicates the position of the gear, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public GearPosition Position { get; private set; }

        /// <summary>
		///   Gets the state machine that manages the state of the pilot handle.
		/// </summary>
		public readonly StateMachine<GearStates> StateMachine = GearStates.LockedExtended;

        /// <summary>
        /// Indicates whether the airplane is on ground or in flight.
        /// </summary>
        private Airplane _shockAbsorber = new Airplane();

        /// <summary>
        /// Gets a value indicating which state the gear cylinder is currently in.
        /// </summary>
        public extern GearStates GetGearCylinderState();

        /// <summary>
        /// Gets a value indicating whether the gear is locked in extended position.
        /// </summary>
        public bool GearIsExtended() => StateMachine.State == GearStates.LockedExtended;

        /// <summary>
        /// Gets a value indicating whether the gear is locked in retracted position.
        /// </summary>
        public bool GearIsRetracted() => StateMachine.State == GearStates.LockedRetracted;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// /// <param name="position">The position the gear is located at on the airplane.</param>
        public Gear(GearPosition position)
        {
            Position = position;
        }

        public override void Update()
        {
            Update(_shockAbsorber);

            StateMachine
                .Transition(
                    from: GearStates.LockedExtended,
                    to: GearStates.UnlockingExtended,
                    guard: GetGearCylinderState() == GearStates.UnlockingExtended)

                .Transition(
                    from: GearStates.UnlockingExtended,
                    to: GearStates.MoveRetracting,
                    guard: GetGearCylinderState() == GearStates.MoveRetracting)

                .Transition(
                    from: GearStates.MoveRetracting,
                    to: GearStates.LockingRetracted,
                    guard: GetGearCylinderState() == GearStates.LockingRetracted)

                .Transition(
                    from: GearStates.LockingRetracted,
                    to: GearStates.LockedRetracted,
                    guard: GetGearCylinderState() == GearStates.LockedRetracted)

                .Transition(
                    from: GearStates.LockedRetracted,
                    to: GearStates.UnlockingRetracted,
                    guard: GetGearCylinderState() == GearStates.UnlockingRetracted)

                .Transition(
                    from: GearStates.UnlockingRetracted,
                    to: GearStates.MoveExtending,
                    guard: GetGearCylinderState() == GearStates.MoveExtending)

                .Transition(
                    from: GearStates.MoveExtending,
                    to: GearStates.LockingExtended,
                    guard: GetGearCylinderState() == GearStates.LockingExtended)

                .Transition(
                    from: GearStates.LockingExtended,
                    to: GearStates.LockedExtended,
                    guard: GetGearCylinderState() == GearStates.LockedExtended)

                .Transition(
                    from: GearStates.UnlockingExtended,
                    to: GearStates.LockingExtended,
                    guard: GetGearCylinderState() == GearStates.LockingExtended)

                .Transition(
                    from: GearStates.LockingExtended,
                    to: GearStates.UnlockingExtended,
                    guard: GetGearCylinderState() == GearStates.UnlockingExtended)

                .Transition(
                    from: GearStates.MoveRetracting,
                    to: GearStates.MoveExtending,
                    guard: GetGearCylinderState() == GearStates.MoveExtending)

                .Transition(
                    from: GearStates.MoveExtending,
                    to: GearStates.MoveRetracting,
                    guard: GetGearCylinderState() == GearStates.MoveRetracting)

                .Transition(
                    from: GearStates.LockingRetracted,
                    to: GearStates.UnlockingRetracted,
                    guard: GetGearCylinderState() == GearStates.UnlockingRetracted)

                .Transition(
                    from: GearStates.UnlockingRetracted,
                    to: GearStates.LockingRetracted,
                    guard: GetGearCylinderState() == GearStates.LockingRetracted);

        }

    }
}

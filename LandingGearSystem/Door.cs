using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the state of the door.
    /// </summary>
    public enum DoorStates
    {
        /// <summary>
        /// State indicating the door is the in open position while pressure is being applied.
        /// </summary>
        Open,
        /// <summary>
        /// State indicating the door is the in open position while no pressure is being applied.
        /// </summary>
        OpenLoose,
        /// <summary>
        /// State indicating the door is moving to close.
        /// </summary>
        MoveClosing,
        /// <summary>
        /// State indicating the door is being locked in closed position.
        /// </summary>
        LockingClosed,
        /// <summary>
        /// State indicating the door is locked in closed position.
        /// </summary>
        LockedClosed,
        /// <summary>
        /// State indicating the door is being unlocked in closed position.
        /// </summary>
        UnlockingClosed,
        /// <summary>
        /// State indicating the door is moving to open.
        /// </summary>
        MoveOpening
    }

    /// <summary>
    ///  Describes the position of the door.
    /// </summary>
    public enum DoorPosition
    {
        /// <summary>
        /// Position indicating the door is located in the front of the airplane.
        /// </summary>
        Front,
        /// <summary>
        /// Position indicating the door is located on the left side of the airplane.
        /// </summary>
        Left,
        /// <summary>
        /// Position indicating the door is located on the right side of the airplane.
        /// </summary>
        Right
    }

    class Door : Component
    { //todo: statemachine weglassen, in update state setzen

        /// <summary>
        /// Indicates the position of the door, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public DoorPosition Position { get; private set; }

        /// <summary>
        ///   Gets the state machine that manages the state of the doors.
        /// </summary>
        public readonly StateMachine<DoorStates> StateMachine = DoorStates.LockedClosed;

        /// <summary>
        /// Gets a value indicating which state the door cylinder is currently in.
        /// </summary>
        public extern DoorStates GetDoorCylinderState { get;  }
        //bei properties kein get/set im namen


        /// <summary>
        /// Gets a value indicating whether the door is open.
        /// </summary>
        public bool DoorIsOpen => StateMachine.State == DoorStates.Open;

        /// <summary>
        /// Gets a value indicating whether the door is closed.
        /// </summary>
        public bool DoorIsClosed => StateMachine.State == DoorStates.LockedClosed;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// /// <param name="position">The position the door is located at on the airplane.</param>
        public Door(DoorPosition position)
        {
            Position = position;

        }

        public override void Update()
        {
            StateMachine
                .Transition(
                    from: DoorStates.LockedClosed,
                    to: DoorStates.UnlockingClosed,
                    guard: GetDoorCylinderState  == DoorStates.UnlockingClosed)

                .Transition(
                    from: DoorStates.UnlockingClosed,
                    to: DoorStates.MoveOpening,
                    guard: GetDoorCylinderState  == DoorStates.MoveOpening)

                .Transition(
                    from: DoorStates.MoveOpening,
                    to: DoorStates.Open,
                    guard: GetDoorCylinderState  == DoorStates.Open)

                .Transition(
                    from: DoorStates.Open,
                    to: DoorStates.Open,
                    guard: GetDoorCylinderState  == DoorStates.Open)

                .Transition(
                    from: DoorStates.Open,
                    to: DoorStates.OpenLoose,
                    guard: GetDoorCylinderState  == DoorStates.OpenLoose)

                .Transition(
                    from: DoorStates.Open,
                    to: DoorStates.MoveClosing,
                    guard: GetDoorCylinderState  == DoorStates.MoveClosing)

                .Transition(
                    from: DoorStates.OpenLoose,
                    to: DoorStates.Open,
                    guard: GetDoorCylinderState  == DoorStates.Open)

                .Transition(
                    from: DoorStates.OpenLoose,
                    to: DoorStates.MoveClosing,
                    guard: GetDoorCylinderState  == DoorStates.MoveClosing)

                .Transition(
                    from: DoorStates.MoveClosing,
                    to: DoorStates.LockingClosed,
                    guard: GetDoorCylinderState  == DoorStates.LockingClosed)

                .Transition(
                    from: DoorStates.LockingClosed,
                    to: DoorStates.LockedClosed,
                    guard: GetDoorCylinderState  == DoorStates.LockedClosed)

                .Transition(
                    from: DoorStates.MoveClosing,
                    to: DoorStates.MoveOpening,
                    guard: GetDoorCylinderState  == DoorStates.MoveOpening)

                .Transition(
                    from: DoorStates.MoveOpening,
                    to: DoorStates.MoveClosing,
                    guard: GetDoorCylinderState  == DoorStates.MoveClosing)

                .Transition(
                    from: DoorStates.LockingClosed,
                    to: DoorStates.UnlockingClosed,
                    guard: GetDoorCylinderState  == DoorStates.UnlockingClosed)

                .Transition(
                    from: DoorStates.UnlockingClosed,
                    to: DoorStates.LockingClosed,
                    guard: GetDoorCylinderState  == DoorStates.LockingClosed);
        }
    }
}

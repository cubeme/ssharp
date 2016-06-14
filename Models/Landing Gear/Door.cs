

namespace SafetySharp.CaseStudies.LandingGear
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
    {
        /// <summary>
        ///   The fault keeps the door stuck in a certain state.
        /// </summary>
        //public readonly Fault DoorIsStuckFault = new PermanentFault();

        /// <summary>
        /// Indicates the position of the door, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public DoorPosition Position { get; private set; }

        /// <summary>
        ///   Indicates the current state of the doors.
        /// </summary>
        public DoorStates State { get; private set; } = DoorStates.LockedClosed;

        /// <summary>
        /// Gets a value indicating which state the door cylinder is currently in.
        /// </summary>
        public extern DoorStates DoorCylinderState { get;  }

        /// <summary>
        /// Gets a value indicating whether the door is open.
        /// </summary>
        public bool DoorIsOpen => State == DoorStates.Open;

        /// <summary>
        /// Gets a value indicating whether the door is closed.
        /// </summary>
        public bool DoorIsClosed => State == DoorStates.LockedClosed;

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
            State = DoorCylinderState;
        }

        ///// <summary>
        /////   Keeps the door stuck in one state.
        ///// </summary>
        //[FaultEffect(Fault = nameof(DoorIsStuckFault))]
        //public class DoorIsStuckFaultEffect : Door
        //{
        //    public DoorIsStuckFaultEffect(DoorPosition position) : base(position) { }

        //    public override void Update()
        //    {

        //    }
        //}
    }
}

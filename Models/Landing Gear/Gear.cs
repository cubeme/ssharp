
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
        ///  Indicates the current state of the gears.
        /// </summary>
        public GearStates State { get; private set; }

        /// <summary>
        /// Gets a value indicating which state the gear cylinder is currently in.
        /// </summary>
        public extern GearStates GearCylinderState { get;  }

        /// <summary>
        /// Gets a value indicating whether the gear is locked in extended position.
        /// </summary>
        public bool GearIsExtended => State == GearStates.LockedExtended;

        /// <summary>
        /// Gets a value indicating whether the gear is locked in retracted position.
        /// </summary>
        public bool GearIsRetracted => State == GearStates.LockedRetracted;

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
            State = GearCylinderState;
        }

    }
}

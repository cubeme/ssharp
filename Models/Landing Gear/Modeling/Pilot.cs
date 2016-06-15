
namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the position of the pilot handle.
    /// </summary>
    public enum HandlePosition
    {
        /// <summary>
        /// Position indicating the pilot handle is switched to up.
        /// </summary>
        Down,
        /// <summary>
        /// Position indicating the pilot handle is switched to down.
        /// </summary>
        Up
    }

    class Pilot : Component
    {

        /// <summary>
        /// The instance of the pilot handle being controlled by the pilot.
        /// </summary>
        public Cockpit Cockpit { get; set;  }

        private readonly HandlePosition _move;

        /// <summary>
        /// Gets the current handle position.
        /// </summary>
        public HandlePosition Position { get; private set; }

        public Pilot(HandlePosition startPosition)
        {
            Position = startPosition;
            _move = Position == HandlePosition.Down ? HandlePosition.Up : HandlePosition.Down;
        }

        public override void Update()
        {
            Update(Cockpit);

            var oldPosition = Position;
            Position = _move;

            if (oldPosition != Position)
                return;

            //Set PilotHandle to new position.
            Cockpit.PilotHandle.Position = Position;
            Cockpit.PilotHandle.Moved();

        }
    }
}

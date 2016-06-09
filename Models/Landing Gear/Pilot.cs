
namespace SafetySharp.CaseStudies.LandingGear
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


        /// <summary>
        /// Gets the current handle position.
        /// </summary>
        public HandlePosition Position { get; private set; } = HandlePosition.Down;

        public override void Update()
        {
            Update(Cockpit);

            var oldPosition = Position;
            //Position = Choose(HandlePosition.Up, HandlePosition.Down);
            Position = HandlePosition.Up;
            //Position = HandlePosition.Down;

            if (oldPosition != Position)
                return;

            //Set PilotHandle to new position.
            Cockpit.PilotHandle.Position = Position;
            Cockpit.PilotHandle.Moved();

        }
    }
}


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

    public class Pilot : Component
    {

        /// <summary>
        /// The instance of the pilot handle being controlled by the pilot.
        /// </summary>
        public Cockpit Cockpit { get; set;  }

        [Hidden]
        public HandlePosition Move { private get; set; }

        /// <summary>
        /// Gets the current handle position.
        /// </summary>
        public HandlePosition Position { get; private set; }

        public Pilot(HandlePosition startPosition)
        {
            Position = startPosition;
        }

        public override void Update()
        {
            Update(Cockpit);

            var oldPosition = Position;
            Position = Move;

            if (oldPosition != Position)
                return;

            //Set PilotHandle to new position.
            Cockpit.PilotHandle.Position = Position;
            Cockpit.PilotHandle.Moved();

        }
    }
}

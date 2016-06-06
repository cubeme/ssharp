
namespace LandingGearSystem
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
        /// Remembers the old handle position to determine whether or not the handle has been moved.
        /// </summary>
        [Hidden]
        private HandlePosition _oldPosition;

        /// <summary>
        /// Gets the current handle position.
        /// </summary>
        public HandlePosition Position { get; private set; } = HandlePosition.Down;

        //todo: The F
        [Range(0, 500, OverflowBehavior.Clamp)]
	    public int F;

        public override void Update()
        {
            Update(Cockpit);

            _oldPosition = Position;
            //Position = Choose(HandlePosition.Up, HandlePosition.Down);
            Position = (HandlePosition.Up);
            if (F%250 == 0)
            {
                Position = Position == HandlePosition.Down ? HandlePosition.Up : HandlePosition.Down;
               // Position = Choose(HandlePosition.Up, HandlePosition.Down);
            }

            if (_oldPosition != Position)
                Cockpit.PilotHandle.HasMoved();

           //if (F < 500)
           ++F;

        }
    }
}

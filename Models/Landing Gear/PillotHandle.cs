

namespace SafetySharp.CaseStudies.LandingGear
{
    using SafetySharp.Modeling;

    class PilotHandle : Component
    {
        //todo: Removed HasMoved() and Moved() is called directly by the pilot.
        /// <summary>
        ///  Indicates whether the pilot handle has been moved.
        /// </summary>
        public extern void Moved();

        /// <summary>
        /// Gets the current position of the pilot handle.
        /// </summary>
        public HandlePosition Position { get; set; }

    }
}


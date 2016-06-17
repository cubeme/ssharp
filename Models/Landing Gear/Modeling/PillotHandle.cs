

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class PilotHandle : Component
    {
        //todo: So?
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


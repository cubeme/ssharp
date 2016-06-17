

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class Cockpit : Component
    {
        /// <summary>
        /// The instance of the pilot handle which is controlled by the pilot.
        /// </summary>
        public readonly PilotHandle PilotHandle = new PilotHandle();

        /// <summary>
        /// Cockpit light indicating the gears are locked down.
        /// </summary>
        public readonly Light GreenLight = new Light();
        /// <summary>
        /// Cockpit light indicating the gears are maneuvering.
        /// </summary>
        public readonly Light OrangeLight = new Light();
        /// <summary>
        /// Cockpit light indicating an anomaly has been detected.
        /// </summary>
        public readonly Light RedLight = new Light();

        public override void Update()
        {
            Update(PilotHandle, GreenLight, OrangeLight, RedLight);
        }
    }
}



namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    class Light : Component
    {
        //todo: Should the light have an attribute color?

        /// <summary>
        ///   The fault flips the light value.
        /// </summary>
        public readonly Fault LightFault = new PermanentFault();

        /// <summary>
        ///  Indicates whether the green, orange or red light in the pilot cockpit is on.
        /// </summary>
        public virtual bool IsOn => LightValue;

        /// <summary>
        ///  Gets a value indicating whether the gears are locked down (green), the gears are maneuvering (orange) or an anomlay has been detected (red).
        /// </summary>
        public extern bool LightValue {  get; }

        /// <summary>
        ///   Will not lock the latching box.
        /// </summary>
        [FaultEffect(Fault = nameof(LightFault))]
        public class LightFaultEffect : Light
        {
            public override bool IsOn => !LightValue;
        }

    }
}

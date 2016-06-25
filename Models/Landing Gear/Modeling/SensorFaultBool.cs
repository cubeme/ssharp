namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    class SensorFaultBool : Sensor<bool>
    {
        /// <summary>
        ///   The fault flips the boolean value provided by the sensor.
        /// </summary>
        public readonly Fault SensorFault = new PermanentFault();

        /// <summary>
        ///   Flips the boolean value provided by the sensor.
        /// </summary>
        [FaultEffect(Fault = nameof(SensorFault))]
        public class SensorFaultEffect : SensorFaultBool
        {
            public SensorFaultEffect(string type)
                : base(type)
            {}

            public override bool Value => !CheckValue;
        }

        public SensorFaultBool(string type)
            : base(type)
        {
            SensorFault.Name = $"{Type}IsFalse";
        }
    }
}

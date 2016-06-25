namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    class SensorFaultHandle : Sensor<HandlePosition>
    {
        /// <summary>
        ///   The fault flips the HandlePosition value provided by the sensor.
        /// </summary>
        public readonly Fault SensorFault = new PermanentFault();

        /// <summary>
        ///   Flips the HandlePosition value provided by the sensor.
        /// </summary>
        [FaultEffect(Fault = nameof(SensorFault))]
        public class SensorFaultEffect : SensorFaultHandle
        {
            public SensorFaultEffect(string type)
                : base(type)
            {
            }

            public override HandlePosition Value => CheckValue == HandlePosition.Down ? HandlePosition.Up : HandlePosition.Down;
        }

        public SensorFaultHandle(string type)
            : base(type)
        {
            SensorFault.Name = $"{Type}IsFalse";
        }
    }
}

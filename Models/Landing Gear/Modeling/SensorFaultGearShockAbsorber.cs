namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    class SensorFaultGearShockAbsorber : Sensor<AirplaneStates>
    {
        /// <summary>
        ///   The fault flips the AirplaneStates value provided by the sensor.
        /// </summary>
        public readonly Fault SensorFault = new PermanentFault();

        /// <summary>
        ///   Flips the AirplaneStates value provided by the sensor.
        /// </summary>
        [FaultEffect(Fault = nameof(SensorFault))]
        public class SensorFaultEffect : SensorFaultGearShockAbsorber
        {
            public SensorFaultEffect(string type)
                : base(type)
            {
            }

            public override AirplaneStates Value => CheckValue == AirplaneStates.Ground ? AirplaneStates.Flight : AirplaneStates.Ground;
        }

        public SensorFaultGearShockAbsorber(string type)
            : base(type)
        {
            SensorFault.Name = $"{Type}IsFalse";
        }
    
    }
}

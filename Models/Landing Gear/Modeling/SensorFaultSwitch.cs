namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    class SensorFaultSwitch : Sensor<AnalogicalSwitchStates>
    {
        /// <summary>
        ///   The fault flips the AnalogicalSwitchStates value provided by the sensor.
        /// </summary>
        public readonly Fault SensorFault = new PermanentFault();

        /// <summary>
        ///   Flips the AnalogicalSwitchStates value provided by the sensor.
        /// </summary>
        [FaultEffect(Fault = nameof(SensorFault))]
        public class SensorFaultEffect : SensorFaultSwitch
        {
            //todo: Works like this because only open and closed states are of interest.
            public SensorFaultEffect(string type)
                : base(type)
            {
            }

            public override AnalogicalSwitchStates Value => CheckValue == AnalogicalSwitchStates.Closed ? AnalogicalSwitchStates.Open : AnalogicalSwitchStates.Closed;
        }

        public SensorFaultSwitch(string type)
            : base(type)
        {
            SensorFault.Name = $"{Type}IsFalse";
        }
    }
}

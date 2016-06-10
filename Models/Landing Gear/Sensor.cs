namespace SafetySharp.CaseStudies.LandingGear
{
    using SafetySharp.Modeling;

    class Sensor<TSensorType> : Component
    {
        /// <summary>
        ///   The fault flips the sensor value.
        /// </summary>
        public readonly Fault FlipSensorValueFault = new TransientFault();

        /// <summary>
        ///  Indicates the current sensor value.
        /// </summary>
        public extern TSensorType CheckValue { get;  }

        /// <summary>
        ///  Gets the value recorded by the sensor.
        /// </summary>
        public virtual TSensorType Value => CheckValue;

        /// <summary>
        ///  Flips a boolean sensor value.
        /// </summary>
        [FaultEffect(Fault = nameof(FlipSensorValueFault))]
        public class FlipSensorValueFaultBoolEffect : Sensor<bool>
        {
            public override bool Value => !CheckValue;
        }

        /// <summary>
        ///   Flips a sensor value of the type "HandlePosition".
        /// </summary>
        [FaultEffect(Fault = nameof(FlipSensorValueFault))]
        public class FlipSensorValueFaultHandleEffect : Sensor<HandlePosition>
        {
            public override HandlePosition Value => CheckValue == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;
        }

        /// <summary>
        ///   Flips a sensor value of the type "AirplaneStates".
        /// </summary>
        [FaultEffect(Fault = nameof(FlipSensorValueFault))]
        public class FlipSensorValueFaultShockAbsorberEffect : Sensor<AirplaneStates>
        {
            public override AirplaneStates Value => CheckValue == AirplaneStates.Ground ? AirplaneStates.Flight : AirplaneStates.Ground;
        }

        /// <summary>
        ///   Sets the sensor value of the type "AnalogicalSwitchStates" to a specified value.
        /// </summary>
        [FaultEffect(Fault = nameof(FlipSensorValueFault))]
        public class FlipSensorValueFaultSwitchEffect : Sensor<AnalogicalSwitchStates>
        {
            public FlipSensorValueFaultSwitchEffect(AnalogicalSwitchStates stuckIn)
            {
                Value = stuckIn;
            }

            public override AnalogicalSwitchStates Value { get; }
        }
    }
}

//Needed Sensors:
//-> AnalogicalSwitch (AnalogicalSwitchStates)
//-> CircuitPressurized (bool)
//-> DoorLockedClosed (bool)
//-> DoorOpen (bool)
//-> GearLockedExtended (true)
//-> GearLockedRetracted (true)
//-> GearShockAbsorber (AirplaneStates)
//-> PilotHandle (Position)
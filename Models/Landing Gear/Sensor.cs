﻿namespace SafetySharp.CaseStudies.LandingGear
{
    using SafetySharp.Modeling;

    class Sensor<TSensorType> : Component
    {
        /// <summary>
        ///  Indicates the current sensor value.
        /// </summary>
        public extern TSensorType CheckValue { get;  }

        /// <summary>
        ///  Gets the value recorded by the sensor.
        /// </summary>
        public TSensorType Value => CheckValue;
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
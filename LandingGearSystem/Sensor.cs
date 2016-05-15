using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class Sensor<SensorType> : Component
    {
        /// <summary>
        ///  Indicates the current sensor value.
        /// </summary>
        public extern SensorType CheckValue();

        /// <summary>
        ///  Gets the value recorded by the sensor.
        /// </summary>
        public SensorType Value => CheckValue();
    }
}

///Needed Sensors:
///-> AnalogicalSwitch (AnalogicalSwitchStates)
///-> CircuitPressurized (bool)
///-> DoorLockedClosed (bool)
///-> DoorOpen (bool)
///-> GearLockedExtended (true)
///-> GearLockedRetracted (true)
///-> GearShockAbsorber (AirplaneStates)
///-> PilotHandle (HandlePosition)
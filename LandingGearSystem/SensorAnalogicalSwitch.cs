using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorAnalogicalSwitch : Component
    {
        /// <summary>
        ///  Gets the postition of the pilot handle.
        /// </summary>
        public extern AnalogicalSwitchStates CheckSwitchPosition();

        /// <summary>
        ///  Gets the value of the sensor monitoring the pilot handle.
        /// </summary>
        public AnalogicalSwitchStates SensorAnalogicalSwitchValue() => CheckSwitchPosition();
    }
}

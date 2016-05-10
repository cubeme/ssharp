using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorGearShockAbsorber : Component
    {
        /// <summary>
        ///  Gets the value of the gear shock absorber.
        /// </summary>
        public extern AirplaneStatus CheckGearShockAbsorber();

        /// <summary>
        ///  Gets the value of the sensor monitoring the gear shock absorber.
        /// </summary>
        public AirplaneStatus SensorGearShockAbsorberValue() => CheckGearShockAbsorber();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorCircuitPressurized : Component
    {
        /// <summary>
        ///  Gets the value indicating whether  the hydraulic circuit is pressurized.
        /// </summary>
        public extern bool CheckPressure();

        /// <summary>
        ///  Gets the value of the sensor monitoring whether  the hydraulic circuit is pressurized.
        /// </summary>
        public bool SensorCircuitPressurizedValue() => CheckPressure();
    }
}

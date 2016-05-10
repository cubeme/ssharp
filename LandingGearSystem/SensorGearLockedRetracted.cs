using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorGearLockedRetracted : Component
    {
        /// <summary>
        ///  Gets the value indicating whether the corresponding landing gear is locked retracted.
        /// </summary>
        public extern bool CheckGearLockedRetracted();

        /// <summary>
        ///  Gets the value of the sensor monitoring whether the corresponding landing gear is locked extended.
        /// </summary>
        public bool SensorGearLockedRetractedValue() => CheckGearLockedRetracted();

    }
}

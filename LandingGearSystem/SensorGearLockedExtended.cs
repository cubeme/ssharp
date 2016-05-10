using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorGearLockedExtended : Component
    {
        /// <summary>
        ///  Gets the value indicating whether the corresponding landing gear is locked extended.
        /// </summary>
        public extern bool CheckGearLockedExtended();

        /// <summary>
        ///  Gets the value of the sensor monitoring whether the corresponding landing gear is locked extended.
        /// </summary>
        public bool SensorGearLockedExtendedValue() => CheckGearLockedExtended();
    }
}


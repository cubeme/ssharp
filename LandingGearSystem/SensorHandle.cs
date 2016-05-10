using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorHandle : Component
    {
        /// <summary>
        ///  Gets the postition of the pilot handle.
        /// </summary>
        public extern HandlePosition CheckHandlePosition();

        /// <summary>
        ///  Gets the value of the sensor monitoring the pilot handle.
        /// </summary>
        public HandlePosition SensorHandleValue() => CheckHandlePosition();

    }
}

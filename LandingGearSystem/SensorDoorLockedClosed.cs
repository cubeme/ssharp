using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorDoorLockedClosed : Component
    {
        /// <summary>
        ///  Gets the value indicating whether the corresponding door is locked closed.
        /// </summary>
        public extern bool CheckDoorLockedClosed();

        /// <summary>
        ///  Gets the value of the sensor monitoring whether the corresponding door is locked closed.
        /// </summary>
        public bool SensorDooLockedrClosedValue() => CheckDoorLockedClosed();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SensorDoorOpen : Component
    {

        /// <summary>
        ///  Gets the value indicating whether the corresponding door is locked open.
        /// </summary>
        public extern bool CheckDoorOpen();

        /// <summary>
        ///  Gets the value of the sensor monitoring whether the corresponding door is locked open.
        /// </summary>
        public bool SensorDoorOpenValue() => CheckDoorOpen();

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetySharp.CaseStudies.LandingGear
{
    using SafetySharp.Modeling;

    class SensorFaultBool : Sensor<bool>
    {
        /// <summary>
        ///   The fault flips the boolean value provided by the sensor.
        /// </summary>
        public readonly Fault SensorFault = new PermanentFault();

        /// <summary>
        ///   Flips the boolean value provided by the sensor.
        /// </summary>
        [FaultEffect(Fault = nameof(SensorFault))]
        public class SensorFaultEffect : SensorFaultBool
        {
            public override bool Value => !CheckValue;
        }
    }
}

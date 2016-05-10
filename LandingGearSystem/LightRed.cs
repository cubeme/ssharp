using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class LightRed : Component
    {
        /// <summary>
        ///  Indicates whether the red light in the pilot cockpit is on.
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        ///  Gets a value indicating whether an anomaly has been detected.
        /// </summary>
        public extern bool AnomalyDetected();

        /// <summary>
        /// Updates the state of the red light.
        /// </summary>
        public void update()
        {
            IsOn = AnomalyDetected();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class Light : Component
    {
        /// <summary>
        ///  Indicates whether the green, orange or red light in the pilot cockpit is on.
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        ///  Gets a value indicating whether the gears are locked down (green), the gears are maneuvering (orange) or an anomlay has been detected (red).
        /// </summary>
        public extern bool LightValue { private get;  set; }

        /// <summary>
        /// Updates the state light.
        /// </summary>
        public void update()
        {
            IsOn = LightValue;
}
    }
}

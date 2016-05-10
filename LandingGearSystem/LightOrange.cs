using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class LightOrange : Component
    {
        /// <summary>
        ///  Indicates whether the orange light in the pilot cockpit is on.
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        ///  Gets a value indicating whether the gears are maneuvering.
        /// </summary>
        public extern bool AreGearsManeuvering();

        /// <summary>
        /// Updates the state of the orange light.
        /// </summary>
        public void update()
        {
            IsOn = AreGearsManeuvering();
        }
    }
}

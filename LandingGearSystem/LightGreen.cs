using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class LightGreen : Component
    {
        /// <summary>
        ///  Indicates whether the green light in the pilot cockpit is on.
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        ///  Gets a value indicating whether the gears are locked in extended or in retracted position.
        /// </summary>
        public extern bool AreGearsLockedDown();

        /// <summary>
        /// Updates the state of the green light.
        /// </summary>
        public void update()
        {
            IsOn = AreGearsLockedDown();
        }
    }
}
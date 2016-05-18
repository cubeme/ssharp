using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    public enum AirplaneStates
    {
        /// <summary>
        /// Sensor value indicating that the aircraft is on ground
        /// </summary>
        Ground,
        /// <summary>
        ///  Sensor value indicating that the aircraft is in flight
        /// </summary>
        Flight
    }

    class Airplane : Component
    {
        /// <summary>
        /// Nondeterministically chooses an airplane status.
        /// </summary>
        public AirplaneStates AirPlaneStatus { get; private set; }

        public Airplane(AirplaneStates state)
        {
            AirPlaneStatus = state;
        }
    }
}

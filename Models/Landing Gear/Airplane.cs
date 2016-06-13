﻿

namespace SafetySharp.CaseStudies.LandingGear
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
        public AirplaneStates AirPlaneStatus { get; }

        public Airplane(AirplaneStates state)
        {
            AirPlaneStatus = state;
        }
    }
}
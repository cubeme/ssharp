

namespace SafetySharp.CaseStudies.LandingGear.Modeling
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

    public class Airplane : Component
    {
        /// <summary>
        /// Indicates the current state of the airplane, i.e. in flight or on ground.
        /// </summary>
        public AirplaneStates AirPlaneStatus { get; set; }

        public Airplane(AirplaneStates state)
        {
            AirPlaneStatus = state;
        }
    }
}

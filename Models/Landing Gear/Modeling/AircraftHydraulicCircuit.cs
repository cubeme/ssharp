
namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    class AircraftHydraulicCircuit : Component
    {
        /// <summary>
        ///  Indicates the pressure of the aircraft hydraulic circuit.
        /// </summary>
        public int Pressure { get; }

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="pressure">The pressure of the aircraft hydraulic circuitk.</param>
        public AircraftHydraulicCircuit(int pressure)
        {
            Pressure = pressure;
        }
    }
}

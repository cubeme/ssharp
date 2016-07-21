

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;
   
    public class Cylinder : Component
    {

        /// <summary>
        /// Indicates the type of the cylinder, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        ///  Gets a value indicating whether the retraction circuit is pressurized.
        /// </summary>
        public extern bool RetractionCurcuitIsPressurized { get;  }

        /// <summary>
        ///  Gets a value indictaing whether the extension circuit is pressurized.
        /// </summary>
        public extern bool ExtensionCircuitIsPressurized { get; }

        /// <summary>
        ///  Timer to time the movement of the gear cylinder.
        /// </summary>
        protected readonly Timer Timer = new Timer();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// /// <param name="position">The position the cylinder is located at on the airplane.</param>
        public Cylinder(Position position)
        {
            Position = position;
        }
    }
}

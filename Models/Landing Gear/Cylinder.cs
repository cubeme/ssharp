

namespace SafetySharp.CaseStudies.LandingGear
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the position of the cylinder.
    /// </summary>
    public enum CylinderPosition
    {
        /// <summary>
        /// Position indicating the cylinder is located in the front of the airplane.
        /// </summary>
        Front,
        /// <summary>
        ///  Position indicating the cylinder is located on the right side of the airplane.
        /// </summary>
        Right,
        /// <summary>
        /// Position indicating the cylinder is located on the left side of the airplane.
        /// </summary>
        Left
    }

    class Cylinder : Component
    {

        /// <summary>
        /// Indicates the type of the cylinder, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public CylinderPosition Position { get; private set; }

        /// <summary>
        ///  Gets a value indicating whether the retraction circuit is pressurized.
        /// </summary>
        public extern bool RetractionCurcuitIsPressurized { get;  }

        /// <summary>
        ///  Gets a value indictaing whether the extension circuit is pressurized.
        /// </summary>
        public extern bool ExtensionCircuitIsPressurized { get; }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// /// <param name="position">The position the cylinder is located at on the airplane.</param>
        public Cylinder(CylinderPosition position)
        {
            Position = position;
        }
    }
}

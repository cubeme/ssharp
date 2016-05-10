using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class DigitalPart : Component
    {
        /// <summary>
        /// Value from computing module one indicating whether the general electro valve is to be stimulated.
        /// </summary>
        public extern bool GeneralEVOne();
        /// <summary>
        /// Value from computing module one indicating whether the door closure electro valve is to be stimulated.
        /// </summary>
        public extern bool CloseEVOne();
        /// <summary>
        /// Value from computing module one indicating whether the door opening electro valve is to be stimulated.
        /// </summary>
        public extern bool OpenEVOne();
        /// <summary>
        /// Value from computing module one indicating whether the gear retraction electro valve is to be stimulated.
        /// </summary>
        public extern bool RetractEVOne();
        /// <summary>
        /// Value from computing module one indicating whether the gear extension electro valve is to be stimulated.
        /// </summary>
        public extern bool ExtendEVOne();

        /// <summary>
        /// Value from computing module two indicating whether all three gears are locked down.
        /// </summary>
        public extern bool GearsLockedDownOne();
        /// <summary>
        /// Value from computing module two indicating whether all three gears are maneuvering.
        /// </summary>
        public extern bool GearsManeuveringOne();
        /// <summary>
        /// Value from computing module two indicating whether an anomaly has been detected.
        /// </summary>
        public extern bool AnomalyOne();

        /// <summary>
        /// Value from computing module two indicating whether the general electro valve is to be stimulated.
        /// </summary>
        public extern bool GeneralEVTwo();
        /// <summary>
        /// Value from computing module two indicating whether the door closure electro valve is to be stimulated.
        /// </summary>
        public extern bool CloseEVTwo();
        /// <summary>
        /// Value from computing module two indicating whether the door opening electro valve is to be stimulated.
        /// </summary>
        public extern bool OpenEVTwo();
        /// <summary>
        /// Value from computing module two indicating whether the gear retraction electro valve is to be stimulated.
        /// </summary>
        public extern bool RetractEVTwo();
        /// <summary>
        /// Value from computing module two indicating whether the gear extension electro valve is to be stimulated.
        /// </summary>
        public extern bool ExtendEVTwo();

        /// <summary>
        /// Value from computing module two indicating whether all three gears are locked down.
        /// </summary>
        public extern bool GearsLockedDownTwo();
        /// <summary>
        /// Value from computing module two indicating whether all three gears are maneuvering.
        /// </summary>
        public extern bool GearsManeuveringTwo();
        /// <summary>
        /// Value from computing module two indicating whether an anomaly has been detected.
        /// </summary>
        public extern bool AnomalyTwo();

        /// <summary>
        /// Gets a value indicating whether the general electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary>
        public bool GeneralEVComposition() => GeneralEVOne() || GeneralEVTwo();

        /// <summary>
        /// Gets a value indicating whether the door closure electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool CloseEVComposition() => CloseEVOne() || CloseEVTwo();

        /// <summary>
        /// Gets a value indicating whether the door opening electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool OpenEVComposition() => OpenEVOne() || OpenEVTwo();

        /// <summary>
        /// Gets a value indicating whether the gear retraction electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool RetractEVComposition() => RetractEVOne() || RetractEVTwo();

        /// <summary>
        /// Gets a value indicating whether the gear extension electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool ExtendEVComposition() => ExtendEVOne() || ExtendEVTwo();

        /// <summary>
        /// Gets a value indicating whether all three gears are locked down through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsLockedDownComposition() => GearsLockedDownOne() || GearsLockedDownTwo();

        /// <summary>
        /// Gets a value indicating whether all three gears are maneuvering through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsManeuveringComposition() => GearsManeuveringOne() || GearsManeuveringTwo();

        /// <summary>
        /// Gets a value indicating whether an anomaly has been detected through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool AnomalyComposition() => AnomalyOne() || AnomalyTwo();

    }
}

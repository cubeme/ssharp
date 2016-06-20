

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class PilotHandle : Component
    {
        /// <summary>
        ///   The fault keeps the handle stuck in the up position.
        /// </summary>
        public readonly Fault HandleUpFault = new PermanentFault();

        /// <summary>
        ///   The fault keeps the handle stuck in the down position.
        /// </summary>
        public readonly Fault HandleDownFault = new PermanentFault();

        //todo: So?
        /// <summary>
        ///  Indicates whether the pilot handle has been moved.
        /// </summary>
        public extern void Moved();

        /// <summary>
        /// Gets the current position of the pilot handle.
        /// </summary>
        public virtual HandlePosition Position { get; set; }

        /// <summary>
        ///   Keeps the pilot handle stuck in the up position.
        /// </summary>
        [FaultEffect(Fault = nameof(HandleUpFault))]
        public class HandleUpFaultEffect : PilotHandle
        {
            public override HandlePosition Position => HandlePosition.Up;
        }

        /// <summary>
        ///   Keeps the pilot handle stuck in the down position.
        /// </summary>
        [FaultEffect(Fault = nameof(HandleDownFault))]
        public class HandleDownFaultEffect : PilotHandle
        {
            public override HandlePosition Position => HandlePosition.Down;
        }

    }
}


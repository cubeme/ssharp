

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class PressureCircuit : Component
    {
        /// <summary>
        ///   The fault returns false for IsEnabled.
        /// </summary>
        public readonly Fault CircuitEnabledFault = new PermanentFault();

        /// <summary>
        ///   The fault returns 0 as the pressure value.
        /// </summary>
        public readonly Fault CircuitPressureFault = new PermanentFault();

        ///<summary>
        /// Indicates current pressure levell.
        /// </summary>
        private int _pressureLevel;

        ///<summary>
        /// Indicates the maximum pressure.
        /// </summary>
        private readonly int _maxPressure;

        /// <summary>
		///   Initializes a new instance.
		/// </summary>
		/// <param name="maxPressure">The maximum allowed pressure level of the tank.</param>
        public PressureCircuit(int maxPressure, string type)
        {
            _maxPressure = maxPressure;
            Range.Restrict(_pressureLevel, 0, _maxPressure, OverflowBehavior.Clamp);

            CircuitEnabledFault.Name = $"{type}CircuitIsAlwaysDisabled";
            CircuitPressureFault.Name = $"{type}CircuitPressureIsAlwaysZero";
        }

        ///<summary>
        /// Gets a value indicating whether the hydraulic circuit is currently enabled.
        /// </summary>
        public virtual bool IsEnabled => _pressureLevel >= _maxPressure;

        ///<summary>
        /// Gets the value of the pressure put into the hydraulic circuit.
        /// </summary>
        public extern int InputPressure { get;  }

	    ///<summary>
	    /// Gets the value of the pressure put into the hydraulic circuit.
	    /// </summary>
	    public virtual int Pressure => IsEnabled ? _pressureLevel : 0;

        /// <summary>
        ///   Updates the hydraulic circuit's internal state.
        /// </summary>
        public override void Update()
        {
            _pressureLevel = InputPressure > 0 ? InputPressure : 0;
        }

        /// <summary>
        ///  Returns false for IsEnabled.
        /// </summary>
        [FaultEffect(Fault = nameof(CircuitEnabledFault))]
        public class CircuitEnabledFaultEffect : PressureCircuit
        {
            public CircuitEnabledFaultEffect(int maxP, string type) : base(maxP, type) { }

            public override bool IsEnabled => false;
        }

        /// <summary>
        ///  Returns 0 as pressure value.
        /// </summary>
        [FaultEffect(Fault = nameof(CircuitPressureFault))]
        public class CircuitPressureFaultEffect : PressureCircuit
        {
            public CircuitPressureFaultEffect(int maxP, string type) : base(maxP, type) { }

            public override int Pressure => 0;
        }
    }
}

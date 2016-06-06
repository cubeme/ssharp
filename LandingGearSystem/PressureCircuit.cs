

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    public class PressureCircuit : Component
    {
        ///<summary>
        /// Indicates current pressure levell.
        /// </summary>
        public int _pressureLevel;
        //todo: private
        ///<summary>
        /// Indicates the maximum pressure.
        /// </summary>
        private readonly int _maxPressure;

        /// <summary>
		///   Initializes a new instance.
		/// </summary>
		/// <param name="maxPressure">The maximum allowed pressure level of the tank.</param>
        public PressureCircuit(int maxPressure)
        {
            _maxPressure = maxPressure;
            _pressureLevel = 0;
            Range.Restrict(_pressureLevel, 0, _maxPressure, OverflowBehavior.Clamp);
        }

        ///<summary>
        /// Gets a value indicating whether the hydraulic circuit is currently enabled.
        /// </summary>
        public bool IsEnabled => _pressureLevel >= _maxPressure;

        ///<summary>
        /// Gets the value of the pressure put into the hydraulic circuit.
        /// </summary>
        public extern int InputPressure { get;  }

	    ///<summary>
	    /// Gets the value of the pressure put into the hydraulic circuit.
	    /// </summary>
	    public int Pressure => IsEnabled ? _pressureLevel : 0;

        /// <summary>
        ///   Updates the hydraulic circuit's internal state.
        /// </summary>
        public override void Update()
        {
            //todo: So oder einfach nur 0, wenn kein Inputpressure mehr?
            if(InputPressure > 0)
                _pressureLevel = InputPressure;
            else
            {
                _pressureLevel--;
            }
        }
    }
}

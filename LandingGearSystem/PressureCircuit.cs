using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    public class PressureCircuit : Component
    {
        //todo: gets vernichten bei properties
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
        public extern int GetInputPressure { get;  }

	    ///<summary>
	    /// Gets the value of the pressure put into the hydraulic circuit.
	    /// </summary>
	    public int Pressure => IsEnabled ? _pressureLevel : 0;

        /// <summary>
        ///   Updates the hydraulic circuit's internal state.
        /// </summary>
        public override void Update()
        {
            int input = GetInputPressure;

            if (input > 0)
                _pressureLevel += input;
			else
				_pressureLevel -= 1;            
        }
    }
}

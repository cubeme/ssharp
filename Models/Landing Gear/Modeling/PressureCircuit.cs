// The MIT License (MIT)
// 
// Copyright (c) 2014-2016, Institute for Software & Systems Engineering
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class PressureCircuit : Component
    {
        /// <summary>
        ///   Indicates the maximum pressure.
        /// </summary>
        private readonly int _maxPressure;

        /// <summary>
        ///   The fault returns false for IsEnabled.
        /// </summary>
        public readonly Fault CircuitEnabledFault = new PermanentFault();

        /// <summary>
        ///   The fault returns 0 as the pressure value.
        /// </summary>
        public readonly Fault CircuitPressureFault = new PermanentFault();

        /// <summary>
        ///   Indicates current pressure levell.
        /// </summary>
        private int _pressureLevel;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="maxPressure">The maximum allowed pressure level of the tank.</param>
        /// <param name="type">Indicates the name of the faults.</param>
        public PressureCircuit(int maxPressure, string type)
        {
            _maxPressure = maxPressure;
            Range.Restrict(_pressureLevel, 0, _maxPressure, OverflowBehavior.Clamp);

            CircuitEnabledFault.Name = $"{type}CircuitIsAlwaysDisabled";
            CircuitPressureFault.Name = $"{type}CircuitPressureIsAlwaysZero";
        }

        /// <summary>
        ///   Gets a value indicating whether the hydraulic circuit is currently enabled.
        /// </summary>
        public virtual bool IsEnabled => _pressureLevel >= _maxPressure;

        /// <summary>
        ///   Gets the value of the pressure put into the hydraulic circuit.
        /// </summary>
        public extern int InputPressure { get; }

        /// <summary>
        ///   Gets the value of the pressure put into the hydraulic circuit.
        /// </summary>
        public virtual int Pressure => IsEnabled ? _pressureLevel : 0;

        /// <summary>
        ///   Updates the PressureCircuit instance.
        /// </summary>
        public override void Update()
        {
            _pressureLevel = InputPressure > 0 ? InputPressure : 0;
        }

        /// <summary>
        ///   Returns false for IsEnabled.
        /// </summary>
        [FaultEffect(Fault = nameof(CircuitEnabledFault))]
        public class CircuitEnabledFaultEffect : PressureCircuit
        {
            public CircuitEnabledFaultEffect(int maxP, string type)
                : base(maxP, type)
            {
            }

            public override bool IsEnabled => false;
        }

        /// <summary>
        ///   Returns 0 as pressure value.
        /// </summary>
        [FaultEffect(Fault = nameof(CircuitPressureFault))]
        public class CircuitPressureFaultEffect : PressureCircuit
        {
            public CircuitPressureFaultEffect(int maxP, string type)
                : base(maxP, type)
            {
            }

            public override int Pressure => 0;
        }
    }
}
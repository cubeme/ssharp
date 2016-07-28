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

    /// <summary>
    ///   Describes the state of the electro valve.
    /// </summary>
    public enum EVStates
    {
        /// <summary>
        ///   State indicating the EV is closed.
        /// </summary>
        Closed,

        /// <summary>
        ///   State indicating the EV is open.
        /// </summary>
        Open
    }

    public class ElectroValve : Component
    {
        /// <summary>
        ///   Indicates the maximum input pressure of the electro valve.
        /// </summary>
        private readonly int _maxHin;

        /// <summary>
        ///   Gets the state machine that manages the state of the electro valve.
        /// </summary>
        private readonly StateMachine<EVStates> _stateMachine = EVStates.Closed;

        /// <summary>
        ///   The fault does not close the electro valve.
        /// </summary>
        public readonly Fault EvCloseFault = new PermanentFault();

        /// <summary>
        ///   The fault does not open the electro valve.
        /// </summary>
        public readonly Fault EvOpenFault = new PermanentFault();

        /// <summary>
        ///   Indicates the output pressure of the EV.
        /// </summary>
        private int _pressureLevel;

        /// <summary>
        ///   Initilializes a new instance.
        /// </summary>
        /// <param name="maxHin"> The maximum input pressure of the electro valve. </param>
        /// <param name="type">Indicates the type of electro valve, i.e. door closing, door opening etc.</param>
        public ElectroValve(int maxHin, string type)
        {
            Range.Restrict(_pressureLevel, 0, maxHin, OverflowBehavior.Clamp);
            _maxHin = maxHin;
            EvCloseFault.Name = $"{type}CannotClose";
            EvOpenFault.Name = $"{type}CannotOpen";
        }

        /// <summary>
        ///   Gets the hydraulic output pressure of the EV.
        /// </summary>
        public int Hout => _stateMachine == EVStates.Open ? _pressureLevel : 0;

        /// <summary>
        ///   Gets the hydraulic input pressure of the EV.
        /// </summary>
        public extern int Hin { get; }

        /// <summary>
        ///   Gets the current state of the electro valve.
        /// </summary>
        public EVStates State => _stateMachine.State;

        /// <summary>
        ///   Transitions to be executed when EOrder == true.
        /// </summary>
        public virtual void Open()
        {
            _stateMachine
                .Transition(
                    @from: EVStates.Closed,
                    to: EVStates.Open);
        }

        /// <summary>
        ///   Transitions to be executed when EOrder == false.
        /// </summary>
        public virtual void Close()
        {
            _stateMachine
                .Transition(
                    @from: EVStates.Open,
                    to: EVStates.Closed);
        }

        /// <summary>
        ///   Updates the ElectroValve instance.
        /// </summary>
        public override void Update()
        {
            if (_stateMachine.State == EVStates.Open)
                _pressureLevel += Hin / 10; //Needs 1sec to fill; 1 Step = 0.1sec
            else
            {
                _pressureLevel -= _maxHin / 36; //Needs 3.6sec for pressure to go down.
            }
        }

        /// <summary>
        ///   Does not close the electro valve.
        /// </summary>
        [FaultEffect(Fault = nameof(EvCloseFault))]
        public class EvCloseFaultEffect : ElectroValve
        {
            public EvCloseFaultEffect(int pressure, string type)
                : base(pressure, type)
            {
            }

            public override void Close()
            {
            }
        }

        /// <summary>
        ///   Does not open the electro valve.
        /// </summary>
        [FaultEffect(Fault = nameof(EvOpenFault))]
        public class EvOpenFaultEffect : ElectroValve
        {
            public EvOpenFaultEffect(int pressure, string type)
                : base(pressure, type)
            {
            }

            public override void Open()
            {
            }
        }
    }
}
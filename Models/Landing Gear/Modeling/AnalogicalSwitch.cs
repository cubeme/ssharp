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
    ///   Describes the possible states of the analogical switch.
    /// </summary>
    public enum AnalogicalSwitchStates
    {
        /// <summary>
        ///   State indicating the analogical switch is open.
        /// </summary>
        Open,

        /// <summary>
        ///   State indicating the analogical switch is moving to close.
        /// </summary>
        MoveClosing,

        /// <summary>
        ///   State indicating the analogical switch is closed.
        /// </summary>
        Closed,

        /// <summary>
        ///   State indicating the analogical switch is moving to open.
        /// </summary>
        MoveOpening
    }

    public class AnalogicalSwitch : Component
    {
        /// <summary>
        ///   Gets the state machine that manages the state of the analogical switch.
        /// </summary>
        private readonly StateMachine<AnalogicalSwitchStates> _stateMachine = AnalogicalSwitchStates.Open;

        /// <summary>
        ///   Times the movement of the analogical switch.
        /// </summary>
        private readonly Timer _timer = new Timer();

        /// <summary>
        ///   The fault will not close the AnalgocialSwitch.
        /// </summary>
        public readonly Fault SwitchCloseFault = new PermanentFault();

        /// <summary>
        ///   The fault keeps the AnalgocialSwitch stuck in one state.
        /// </summary>
        public readonly Fault SwitchStuckFault = new PermanentFault();

        /// <summary>
        ///   Indicates the state of the general electro-valve.
        /// </summary>
        private bool _evState;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        public AnalogicalSwitch()
        {
            SwitchStuckFault.Name = "SwitchIsStuck";
            SwitchCloseFault.Name = "SwitchCannotClose";
        }

        /// <summary>
        ///   Gets the current position of the analogical switch.
        /// </summary>
        public AnalogicalSwitchStates SwitchPosition => _stateMachine.State;

        /// <summary>
        ///   Gets the value of the incoming electrical order.
        /// </summary>
        public extern bool IncomingEOrder();

        /// <summary>
        ///   Checks whether or not the general electr-valve is to be closed.
        /// </summary>
        public virtual void CheckEOrder()
        {
            if (_stateMachine != AnalogicalSwitchStates.Closed)
                return;

            var mustOpen = IncomingEOrder();
            var mustClose = !IncomingEOrder();

            if (mustOpen && !_evState)
            {
                OpenGeneralEV();
                _evState = true;
            }

            if (mustClose && _evState)
            {
                CloseGeneralEV();
                _evState = false;
            }
        }

        /// <summary>
        ///   Opens the general electro-valve.
        /// </summary>
        public extern void OpenGeneralEV();

        /// <summary>
        ///   Closes the genenral electro-valve.
        /// </summary>
        public extern void CloseGeneralEV();

        /// <summary>
        ///   Closes the analogcial switch.
        /// </summary>
        public virtual void Close()
        {
            _stateMachine
                .Transition(
                    @from: new[] { AnalogicalSwitchStates.Open, AnalogicalSwitchStates.MoveOpening },
                    to: AnalogicalSwitchStates.MoveClosing,
                    action: () => { _timer.Start(8 - (2 * _timer.RemainingTime) / 3); })
                .Transition(
                    @from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.Closed,
                    action: () => { _timer.Start(200); });
        }

        /// <summary>
        ///   Updates the AnalogicalSwitch instance.
        /// </summary>
        public override void Update()
        {
            Update(_timer);

            _stateMachine
                .Transition(
                    @from: AnalogicalSwitchStates.MoveClosing,
                    to: AnalogicalSwitchStates.Closed,
                    guard: _timer.HasElapsed,
                    action: () => { _timer.Start(200); })
                .Transition(
                    @from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.MoveOpening,
                    guard: _timer.HasElapsed,
                    action: () => { _timer.Start(12); })
                .Transition(
                    @from: AnalogicalSwitchStates.MoveOpening,
                    to: AnalogicalSwitchStates.Open,
                    guard: _timer.HasElapsed);

            CheckEOrder();
        }

        /// <summary>
        ///   Keeps the AnalgocialSwitch stuck in one state.
        /// </summary>
        [FaultEffect(Fault = nameof(SwitchStuckFault))]
        public class SwitchStuckFaultEffect : AnalogicalSwitch
        {
            public override void Update()
            {
                Update(_timer);

                //no transitions

                CheckEOrder();
            }
        }

        /// <summary>
        ///   Will not close the analgical switch.
        /// </summary>
        [FaultEffect(Fault = nameof(SwitchCloseFault))]
        public class SwitchCloseFaultEffect : AnalogicalSwitch
        {
            public override void Close()
            {
            }
        }
    }
}
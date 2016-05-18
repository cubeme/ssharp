using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the state of the analogical switch.
    /// </summary>
    public enum AnalogicalSwitchStates
    {
        /// <summary>
        /// State indicating the analogical switch is open.
        /// </summary>
        Open,
        /// <summary>
        /// State indicating the analogical switch is moving to close.
        /// </summary>
        MoveClosing,
        /// <summary>
        /// State indicating the analogical switch is closed.
        /// </summary>
        Closed,
        /// <summary>
        /// State indicating the analogical switch is moving to open.
        /// </summary>
        MoveOpening
    }

    class AnalogicalSwitch : Component
    {
        /// <summary>
		///   Gets the state machine that manages the state of the analogical switch.
		/// </summary>
		public readonly StateMachine<AnalogicalSwitchStates> StateMachine = AnalogicalSwitchStates.Open;

        public AnalogicalSwitchStates SwitchPosition => StateMachine.State;

        /// <summary>
        ///  Times the movement of the analogical switch.
        /// </summary>
        [Hidden]
        private Timer _timer = new Timer();

        /// <summary>
        /// Gets a value indicating whether the pilot handle has been moved.
        /// </summary>
        public extern bool GetHandleHasBeenMoved { get;  }

        /// <summary>
        /// Gets the value of the incoming electrical order.
        /// </summary>
        public extern bool IncomingEOrder();

        /// <summary>
        /// Gets the value of the outgoing electrical order if the switch is closed.
        /// </summary>
        public bool OutgoingEOrder() => StateMachine == AnalogicalSwitchStates.Closed ? IncomingEOrder() : false;

        /// <summary>
        /// Updates the analogical switch.
        /// </summary>
        public override void Update()
        {
            Update(_timer);

            StateMachine
                .Transition(
                    from: AnalogicalSwitchStates.Open,
                    to: AnalogicalSwitchStates.MoveClosing,
                    guard: GetHandleHasBeenMoved == true,
                    action: () =>
                    {
                        _timer.Start(8);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.MoveClosing,
                    to: AnalogicalSwitchStates.Closed,
                    guard: _timer.HasElapsed,
                    action: () =>
                    {
                        _timer.Start(200);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.Closed,
                    guard: GetHandleHasBeenMoved == true,
                    action: () =>
                    {
                        _timer.Start(20);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.MoveClosing,
                    guard: _timer.HasElapsed,
                    action: () =>
                    {
                        _timer.Start(12);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.MoveOpening,
                    to: AnalogicalSwitchStates.MoveClosing,
                    guard: GetHandleHasBeenMoved == true,
                    action: () =>
                    {
                        _timer.Start(8 - (2 * _timer.RemainingTime) / 3);
                    })

                .Transition(
                    from: AnalogicalSwitchStates.MoveOpening,
                    to: AnalogicalSwitchStates.Open,
                    guard: _timer.HasElapsed);

        }
    }
}

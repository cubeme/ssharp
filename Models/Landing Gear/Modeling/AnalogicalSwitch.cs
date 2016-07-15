

namespace SafetySharp.CaseStudies.LandingGear.Modeling
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

    public class AnalogicalSwitch : Component
    {

        /// <summary>
        ///   The fault keeps the AnalgocialSwitch stuck in one state.
        /// </summary>
        public readonly Fault SwitchStuckFault = new PermanentFault();

        /// <summary>
        ///   The fault will not close the AnalgocialSwitch.
        /// </summary>
        public readonly Fault SwitchCloseFault = new PermanentFault();

        public AnalogicalSwitch()
        {
            SwitchStuckFault.Name = "SwitchIsStuck";
            SwitchCloseFault.Name = "SwitchCannotClose";
        }

        /// <summary>
		///   Gets the state machine that manages the state of the analogical switch.
		/// </summary>
		private readonly StateMachine<AnalogicalSwitchStates> _stateMachine = AnalogicalSwitchStates.Open;

        /// <summary>
		///   Gets the current position of the analogical switch.
		/// </summary>
        public AnalogicalSwitchStates SwitchPosition => _stateMachine.State;

        /// <summary>
        ///  Times the movement of the analogical switch.
        /// </summary>
        private readonly Timer _timer = new Timer();

        /// <summary>
        /// Gets the value of the incoming electrical order.
        /// </summary>
        public extern bool IncomingEOrder();

        private bool _evState;

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

        public extern void OpenGeneralEV();

        public extern void CloseGeneralEV();


        /// <summary>
        /// Closes the analogcial switch.
        /// </summary>
        public virtual void Close()
        {
            _stateMachine
                .Transition(
                    @from: new[] {AnalogicalSwitchStates.Open, AnalogicalSwitchStates.MoveOpening},
                    to: AnalogicalSwitchStates.MoveClosing,
                    action: () =>
                    {
                        _timer.Start(8 - (2*_timer.RemainingTime)/3);
                    })

                .Transition(
                    @from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.Closed,
                    action: () =>
                    {
                        _timer.Start(200);
                    });
        }

        /// <summary>
        /// Updates the analogical switch.
        /// </summary>
        public override void Update()
        {
            Update(_timer);

            _stateMachine
                .Transition(
                    @from: AnalogicalSwitchStates.MoveClosing,
                    to: AnalogicalSwitchStates.Closed,
                    guard: _timer.HasElapsed,
                    action: () =>
                    {
                        _timer.Start(200);
                    })

                .Transition(
                    @from: AnalogicalSwitchStates.Closed,
                    to: AnalogicalSwitchStates.MoveOpening,
                    guard: _timer.HasElapsed,
                    action: () =>
                    {
                        _timer.Start(12);
                    })

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

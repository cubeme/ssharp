

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the state of the electro valve.
    /// </summary>
    public enum EVStates
    {
        /// <summary>
        /// State indicating the EV is closed.
        /// </summary>
        Closed,
        /// <summary>
        /// State indicating the EV is open.
        /// </summary>
        Open
    }

    class ElectroValve : Component
    {
        /// <summary>
        ///   Gets the state machine that manages the state of the gear cylinder.
        /// </summary>
        private readonly StateMachine<EVStates> _stateMachine = EVStates.Closed;

        /// <summary>
        /// Indicates the output pressure of the EV.
        /// </summary>
        private int _hout = 0;

        /// <summary>
        /// Initilializes a new instance.
        /// </summary>
        /// <param name="maxHin"> The maximum input pressure of the EV. </param>
        public ElectroValve(int maxHin)
        {
            Range.Restrict(_hout, 0, maxHin, OverflowBehavior.Clamp);
        }

        ///<summary>
        /// Gets the hydraulic output pressure of the EV.
        /// </summary>
        public int Hout => _stateMachine == EVStates.Open ? _hout : 0;

        /// <summary>
        ///   Gets the hydraulic input pressure of the EV.
        /// </summary>
        public extern int Hin { get; }

        ///<summary>
        /// Gets the electric order.
        /// </summary>
        public extern bool EOrder { get; }

        ///<summary>
        /// Transitions to be executed when EOrder == true.
        /// </summary>
        public void EOrderIsTrue()
        {
            _stateMachine
                .Transition(
                    from: EVStates.Closed,
                    to: EVStates.Open);
        }

        ///<summary>
        /// Transitions to be executed when EOrder == false.
        /// </summary>
        public void EOrderIsFalse()
        {
            _stateMachine
                .Transition(
                    from: EVStates.Open,
                    to: EVStates.Closed);
        }

        ///<summary>
        /// Updates the EV.
        /// </summary>
        public override void Update()
        {
            //todo: Mit Port oder irgendwie Methode direkt aufrufen? Hin lassen oder lieber einfach ansteigen lassen?
            if (EOrder)
                EOrderIsTrue();
            else
                EOrderIsFalse();

            if (_stateMachine.State == EVStates.Open)
                _hout += Hin/10; //Needs 1sec to fill; 1 Step = 0.1sec
            else
                _hout -= Hin / 36; //Needs 3.6sec to go down; 1 Step = 0.1sec
        }
    }
}



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
        public readonly StateMachine<EVStates> _stateMachine = EVStates.Closed;
        //todo: private

        /// <summary>
        /// Indicates the output pressure of the EV.
        /// </summary>
        public int _pressureLevel = 0;
        //todo: private

        private readonly int _maxHin;

        /// <summary>
        /// Initilializes a new instance.
        /// </summary>
        /// <param name="maxHin"> The maximum input pressure of the EV. </param>
        public ElectroValve(int maxHin)
        {
            Range.Restrict(_pressureLevel, 0, maxHin, OverflowBehavior.Clamp);
            _maxHin = maxHin;
        }

        ///<summary>
        /// Gets the hydraulic output pressure of the EV.
        /// </summary>
        public int Hout => _stateMachine == EVStates.Open ? _pressureLevel : 0;

        /// <summary>
        ///   Gets the hydraulic input pressure of the EV.
        /// </summary>
        public extern int Hin { get; }

        ///<summary>
        /// Transitions to be executed when EOrder == true.
        /// </summary>
        public void Open()
        {
            _stateMachine
                .Transition(
                    from: EVStates.Closed,
                    to: EVStates.Open);
        }

        ///<summary>
        /// Transitions to be executed when EOrder == false.
        /// </summary>
        public void Close()
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

            if (_stateMachine.State == EVStates.Open)
                _pressureLevel += Hin/10; //Needs 1sec to fill; 1 Step = 0.1sec
            else
            {
                _pressureLevel -= _maxHin/36; //Needs 3.6sec for pressure to go down.
            }               
        }
    }
}

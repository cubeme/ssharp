

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    /// States of the state machines monitoring the system health.
    /// </summary>
    public enum HealthMonitoringStates
    {
        /// <summary>
        /// State indicating that no action is currently taking place.
        /// </summary>
        Wait,
        /// <summary>
        /// In case of the pressure sensor and analogical switch monitoring: State indicating that a trigger action has taken place.
        /// </summary>
        Start,
        /// <summary>
        /// State indicating that an anomaly has been detected.
        /// </summary>
        Error,
        /// <summary>
        /// In case of the pressure sensor: State indicating that the circuit is pressurized and no stopping order has yet been given.
        /// </summary>
        Intermediate,
        /// <summary>
        /// In case of the pressure sensor and analogical switch monitoring: State indicating that the action to be executed is being finished.
        /// </summary>
        End,
        /// <summary>
        /// In case of doors motion monitoring: State indicating the doors are being opened.
        /// </summary>
        Open,
        /// <summary>
        /// In case of doors motion monitoring: State indicating the doors are being closed.
        /// </summary>
        Close,
        /// <summary>
        /// In case of gears motion monitoring: State indicating the gears are being extended.
        /// </summary>
        Extend,
        /// <summary>
        /// In case of gears motion monitoring: State indicating the geras are being retracted.
        /// </summary>
        Retract
    }

    class HealthMonitoring : Component
    {

        //Health monitoring
        //Generic monitoring -> see ComputingModule

        /// <summary>
        /// An instance of the associated computing module.
        /// </summary>
        protected readonly ComputingModule ComputingModule;

        /// <summary>
        /// Indicates whether an anomaly has been detected.
        /// </summary>
        public bool AnomalyDetected { get; protected set; }

        /// <summary>
        ///   Gets the state machine that manages the healthmonitoring of the analogical switch.
        /// </summary>
        protected readonly StateMachine<HealthMonitoringStates> StateMachine = HealthMonitoringStates.Wait;

        /// <summary>
        /// Timer used for health monitoring of the analogical switch.
        /// </summary>
        protected readonly Timer Timer = new Timer();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        public HealthMonitoring(ComputingModule module)
        {
            ComputingModule = module;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// Indicates the current position of the analogical switch.
        /// </summary>
        private Func<AnalogicalSwitchStates> _switchPosition;

        /// <summary>
        /// Indicates whether the hydraulic circuit is pressurized.
        /// </summary>
        private Func<bool> _circuitIsPressurized;

        /// <summary>
        /// Indicates whether all three doors are open.
        /// </summary>
        private Func<bool> _doorsAreOpen;

        /// <summary>
        /// Indicates whether all three doors are locked closed.
        /// </summary>
        private Func<bool> _doorsAreClosed;

        /// <summary>
        /// Indicates whether all three gears are locked retracted.
        /// </summary>
        private Func<bool> _gearsAreRetracted;

        /// <summary>
        /// Indicates whether all three gears are locked extended.
        /// </summary>
        private Func<bool> _gearsAreExtended;

        /// <summary>
        /// Indicates whether the pilot handle has been moved.
        /// </summary>
        private Func<bool> _handleHasMoved;

        /// <summary>
        /// Indicates whether the general electro valve is being stimulated.
        /// </summary>
        private Func<bool> _generalEV;

        /// <summary>
        /// Indicates whether the door opening electro valve is being stimulated.
        /// </summary>
        private Func<bool> _openEV;

        /// <summary>
        /// Indicates whether the door closure electro valve is being stimulated.
        /// </summary>
        private Func<bool> _closeEV;

        /// <summary>
        /// Indicates whether the gear extension electro valve is being stimulated.
        /// </summary>
        private Func<bool> _extendEV;

        /// <summary>
        /// Indicates whether the gear retraction electro valve is being stimulated.
        /// </summary>
        private Func<bool> _retractEV;

        /// <summary>
        /// Indicates whether an anomaly has been detected.
        /// </summary>
        public bool AnomalyDetected { get; private set; }

        /// <summary>
        ///   Gets the state machine that manages the healthmonitoring of the analogical switch.
        /// </summary>
        public readonly StateMachine<HealthMonitoringStates> StateMachineSwitch = HealthMonitoringStates.Wait;

        /// <summary>
        ///   Gets the state machine that manages the healthmonitoring of the pressure sensor.
        /// </summary>
        public readonly StateMachine<HealthMonitoringStates> StateMachinePressureSensor = HealthMonitoringStates.Wait;

        /// <summary>
        ///   Gets the state machine that manages the healthmonitoring of the doors motion.
        /// </summary>
        public readonly StateMachine<HealthMonitoringStates> StateMachineDoors = HealthMonitoringStates.Wait;

        /// <summary>
        ///   Gets the state machine that manages the healthmonitoring of the gears motion.
        /// </summary>
        public readonly StateMachine<HealthMonitoringStates> StateMachineGears = HealthMonitoringStates.Wait;

        /// <summary>
        /// Timer used for health monitoring of the pressure sensor.
        /// </summary>
        private Timer _timerPressure = new Timer();

        /// <summary>
        /// Timer used for health monitoring of the analogical switch.
        /// </summary>
        private Timer _timerSwitch = new Timer();

        /// <summary>
        /// Timer used for health monitoring of the doors motion.
        /// </summary>
        private Timer _timerDoors = new Timer();

        /// <summary>
        /// Timer used for health monitoring of the gears motion.
        /// </summary>
        private Timer _timerGears = new Timer();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="switchPosition"> Indicates the current position of the analogical switch. </param>
        /// <param name="circuitPressurized"> Indicates whether the hydraulic circuit is pressurized. </param>
        /// <param name="doorsAreOpen"> Indicates whether all three doors are open. </param>
        /// <param name="doorsAreClosed"> Indicates whether all three doors are locked closed. </param>
        /// <param name="gearsAreRetracted"> Indicates whether all three gears are locked retracted. </param>
        /// <param name="gearsAreExtended"> Indicates whether all three gears are locked extended. </param>
        /// <param name="handleHasMoved"> Indicates whether the pilot handle has been moved. </param>
        /// <param name="generalEV"> Indicates whether the general electro valve is being stimulated. </param>
        /// <param name="openEV"> Indicates whether the door opening electro valve is being stimulated. </param>
        /// <param name="closeEV"> Indicates whether the door closure electro valve is being stimulated. </param>
        /// <param name="extendEV"> Indicates whether the gear extension electro valve is being stimulated. </param>
        /// <param name="retractEV"> Indicates whether the gear retraction electro valve is being stimulatet. </param>
        public HealthMonitoring(Func<AnalogicalSwitchStates> switchPosition, Func<bool> circuitPressurized, Func<bool> doorsAreOpen, Func<bool> doorsAreClosed, Func<bool> gearsAreRetracted, Func<bool> gearsAreExtended, Func<bool> handleHasMoved, Func<bool> generalEV, Func<bool> openEV, Func<bool> closeEV, Func<bool> extendEV, Func<bool> retractEV)
        {
        _switchPosition = switchPosition;
        _circuitIsPressurized = circuitPressurized;

        _doorsAreOpen = doorsAreOpen;
        _doorsAreClosed = doorsAreClosed;

        _gearsAreRetracted = gearsAreRetracted;
        _gearsAreExtended = gearsAreRetracted;

        _handleHasMoved = handleHasMoved;
        _generalEV = generalEV;

        _openEV = openEV;
        _closeEV = closeEV;

        _extendEV = extendEV;
        _retractEV = retractEV;
    }

        public override void Update()
        {
            Update(_timerSwitch, _timerPressure, _timerDoors, _timerGears);

            //Health monitoring
            //Generic monitoring -> see ComputingModule
            //Analogical switch monitoring
            StateMachineSwitch
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Start,
                    guard: _handleHasMoved(),
                    action: () =>
                    {
                        _timerSwitch.SetTimeout(200);
                        _timerSwitch.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.End,
                    guard: _timerSwitch.HasElapsed,
                    action: () =>
                    {
                        _timerSwitch.SetTimeout(15);
                        _timerSwitch.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Error,
                    guard: _timerSwitch.RemainingTime == 19 && _switchPosition.Equals(AnalogicalSwitchStates.open),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: _switchPosition.Equals(AnalogicalSwitchStates.open) && !_handleHasMoved(),
                    action: () => _timerSwitch.Stop())
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Start,
                    guard: _handleHasMoved(),
                    action: () =>
                    {
                        _timerSwitch.SetTimeout(200);
                        _timerSwitch.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard: _timerSwitch.HasElapsed && !_handleHasMoved() && _switchPosition.Equals(AnalogicalSwitchStates.closed),
                    action: () => AnomalyDetected = true);

            //Pressure sensor monitoring
            StateMachinePressureSensor
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Start,
                    guard:  _generalEV(),
                    action: () =>
                    {
                        _timerPressure.SetTimeout(20);
                        _timerPressure.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Intermediate,
                    guard: _circuitIsPressurized())
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Error,
                    guard: _timerPressure.HasElapsed && !_circuitIsPressurized(),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Intermediate,
                    to: HealthMonitoringStates.End,
                    guard: !_generalEV(),
                    action: () =>
                    {
                        _timerPressure.SetTimeout(100);
                        _timerPressure.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: !_circuitIsPressurized())
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Start,
                    guard: _generalEV(),
                    action: () =>
                    {
                        _timerPressure.SetTimeout(20);
                        _timerPressure.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard: _timerPressure.HasElapsed && _circuitIsPressurized(),
                    action: () => AnomalyDetected = true);

            //Doors motion monitoring
            StateMachineDoors
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Open,
                    guard: _openEV(),
                    action: () =>
                    {
                        _timerDoors.SetTimeout(70);
                        _timerDoors.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Open,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerDoors.HasElapsed && _doorsAreClosed()) || (_timerDoors.HasElapsed && !_doorsAreOpen()),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Open,
                    to: HealthMonitoringStates.Wait,
                    guard: !_doorsAreClosed() && _doorsAreOpen())
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Close,
                    guard: _closeEV(),
                    action: () =>
                    {
                        _timerDoors.SetTimeout(70);
                        _timerDoors.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Close,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerDoors.HasElapsed && _doorsAreOpen()) || (_timerDoors.HasElapsed && !_doorsAreClosed()),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Close,
                    to: HealthMonitoringStates.Wait,
                    guard: _doorsAreClosed() && !_doorsAreOpen());

            //Gears motion monitoring
            StateMachineGears
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Extend,
                    guard: _extendEV(),
                    action: () =>
                    {
                        _timerGears.SetTimeout(100);
                        _timerGears.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Extend,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerGears.RemainingTime == 30 && _gearsAreRetracted()) || (_timerGears.HasElapsed && !_gearsAreExtended()),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Extend,
                    to: HealthMonitoringStates.Wait,
                    guard: _gearsAreExtended() && !_gearsAreRetracted())
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Retract,
                    guard: _retractEV(),
                    action: () =>
                    {
                        _timerGears.SetTimeout(100);
                        _timerGears.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Retract,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerGears.RemainingTime == 30 && _gearsAreExtended()) || (_timerGears.HasElapsed && !_gearsAreRetracted()),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Retract,
                    to: HealthMonitoringStates.Wait,
                    guard: _gearsAreRetracted() && !_gearsAreExtended());
        }
    }
}

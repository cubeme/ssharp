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
        /// An instance of the associated computing module.
        /// </summary>
        private ComputingModule computingModule;

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
        /// <param name="module"></param>
        public HealthMonitoring(ComputingModule module)
        {
            computingModule = module;
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
                    guard: computingModule.HandleHasMoved,
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
                    guard: _timerSwitch.RemainingTime == 19 && computingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Open,
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: computingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Open && !computingModule.HandleHasMoved,
                    action: () => _timerSwitch.Stop())
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Start,
                    guard: computingModule.HandleHasMoved,
                    action: () =>
                    {
                        _timerSwitch.SetTimeout(200);
                        _timerSwitch.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard: _timerSwitch.HasElapsed && !computingModule.HandleHasMoved && computingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Closed,
                    action: () => AnomalyDetected = true);

            //Pressure sensor monitoring
            StateMachinePressureSensor
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Start,
                    guard:  computingModule.GeneralEV,
                    action: () =>
                    {
                        _timerPressure.SetTimeout(20);
                        _timerPressure.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Intermediate,
                    guard: computingModule.CircuitPressurized.Value == true)
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Error,
                    guard: _timerPressure.HasElapsed && !computingModule.CircuitPressurized.Value == true,
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Intermediate,
                    to: HealthMonitoringStates.End,
                    guard: !computingModule.GeneralEV,
                    action: () =>
                    {
                        _timerPressure.SetTimeout(100);
                        _timerPressure.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: !computingModule.CircuitPressurized.Value == true)
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Start,
                    guard: computingModule.GeneralEV,
                    action: () =>
                    {
                        _timerPressure.SetTimeout(20);
                        _timerPressure.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard: _timerPressure.HasElapsed && computingModule.CircuitPressurized.Value == true,
                    action: () => AnomalyDetected = true);

            //Doors motion monitoring
            StateMachineDoors
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Open,
                    guard: computingModule.OpenEV,
                    action: () =>
                    {
                        _timerDoors.SetTimeout(70);
                        _timerDoors.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Open,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerDoors.HasElapsed && computingModule.DoorsClosed) || (_timerDoors.HasElapsed && !computingModule.DoorsOpen),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Open,
                    to: HealthMonitoringStates.Wait,
                    guard: !computingModule.DoorsClosed && computingModule.DoorsOpen)
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Close,
                    guard: computingModule.CloseEV,
                    action: () =>
                    {
                        _timerDoors.SetTimeout(70);
                        _timerDoors.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Close,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerDoors.HasElapsed && computingModule.DoorsOpen) || (_timerDoors.HasElapsed && !computingModule.DoorsClosed),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Close,
                    to: HealthMonitoringStates.Wait,
                    guard: computingModule.DoorsClosed && !computingModule.DoorsOpen);

            //Gears motion monitoring
            StateMachineGears
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Extend,
                    guard: computingModule.ExtendEV,
                    action: () =>
                    {
                        _timerGears.SetTimeout(100);
                        _timerGears.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Extend,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerGears.RemainingTime == 30 && computingModule.GearsRetracted) || (_timerGears.HasElapsed && !computingModule.GearsExtended),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Extend,
                    to: HealthMonitoringStates.Wait,
                    guard: computingModule.GearsExtended && !computingModule.GearsRetracted)
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Retract,
                    guard: computingModule.RetractEV,
                    action: () =>
                    {
                        _timerGears.SetTimeout(100);
                        _timerGears.Start();
                    })
                .Transition(
                    from: HealthMonitoringStates.Retract,
                    to: HealthMonitoringStates.Error,
                    guard: (_timerGears.RemainingTime == 30 && computingModule.GearsExtended) || (_timerGears.HasElapsed && !computingModule.GearsRetracted),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Retract,
                    to: HealthMonitoringStates.Wait,
                    guard: computingModule.GearsRetracted && !computingModule.GearsExtended);
        }
    }
}



namespace LandingGearSystem
{

    class HealthSwitch : HealthMonitoring
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"> The computing model which does the health monitoring. </param>
        public HealthSwitch(ComputingModule module) : base(module) { }

        public override void Update()
        {
            Update(Timer);


            //Analogical switch monitoring
            StateMachine
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Start,
                    guard: ComputingModule.HandleHasMoved,
                    action: () =>
                    {
                        Timer.Start(200);
                    })
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.End,
                    guard: Timer.HasElapsed,
                    action: () =>
                    {
                        Timer.Start(15);
                    })
                .Transition(
                    from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Error,
                    guard: Timer.RemainingTime == 19 && ComputingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Open,
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: ComputingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Open && !ComputingModule.HandleHasMoved,
                    action: () => Timer.Stop())
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Start,
                    guard: ComputingModule.HandleHasMoved,
                    action: () =>
                    {
                        Timer.Start(200);
                    })
                .Transition(
                    from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard: Timer.HasElapsed && !ComputingModule.HandleHasMoved && ComputingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Closed,
                    action: () => AnomalyDetected = true);
       
        }
    }
}


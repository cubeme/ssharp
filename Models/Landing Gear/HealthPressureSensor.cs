

namespace SafetySharp.CaseStudies.LandingGear
{

    class HealthPressureSensor : HealthMonitoring
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"> The computing model which does the health monitoring. </param>
        public HealthPressureSensor(ComputingModule module) : base(module) { }

        public override void Update()
        {
            Update( Timer);
           
            //Pressure sensor monitoring
            StateMachine
                .Transition(
                    @from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Start,
                    guard: ComputingModule.GeneralEV,
                    action: () =>
                    {
                        Timer.Start(20);
                    })
                .Transition(
                    @from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Intermediate,
                    guard: ComputingModule.CircuitPressurized.Value)
                .Transition(
                    @from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Error,
                    guard: Timer.HasElapsed && !ComputingModule.CircuitPressurized.Value ,
                    action: () => AnomalyDetected = true)
                .Transition(
                    @from: HealthMonitoringStates.Intermediate,
                    to: HealthMonitoringStates.End,
                    guard: !ComputingModule.GeneralEV,
                    action: () =>
                    {
                        Timer.Start(100);
                    })
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: !ComputingModule.CircuitPressurized.Value)
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Start,
                    guard: ComputingModule.GeneralEV,
                    action: () =>
                    {
                        Timer.Start(20);
                    })
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard: Timer.HasElapsed && ComputingModule.CircuitPressurized.Value,
                    action: () => AnomalyDetected = true);

           
        }
    }
}

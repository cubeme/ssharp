

namespace SafetySharp.CaseStudies.LandingGear
{

    class HealthDoors : HealthMonitoring
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"> The computing model which does the health monitoring. </param>
        public HealthDoors(ComputingModule module) : base(module) { }
       

        public override void Update()
        {
            Update(Timer);
          
            //Doors motion monitoring
            StateMachine
                .Transition(
                    @from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Open,
                    guard: ComputingModule.OpenEV,
                    action: () =>
                    {
                        Timer.Start(70);
                    })
                .Transition(
                    @from: HealthMonitoringStates.Open,
                    to: HealthMonitoringStates.Error,
                    guard: (Timer.HasElapsed && ComputingModule.DoorsClosed) || (Timer.HasElapsed && !ComputingModule.DoorsOpen),
                    action: () => AnomalyDetected = true)
                .Transition(
                    @from: HealthMonitoringStates.Open,
                    to: HealthMonitoringStates.Wait,
                    guard: !ComputingModule.DoorsClosed && ComputingModule.DoorsOpen)
                .Transition(
                    @from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Close,
                    guard: ComputingModule.CloseEV,
                    action: () =>
                    {
                        Timer.Start(70);
                    })
                .Transition(
                    @from: HealthMonitoringStates.Close,
                    to: HealthMonitoringStates.Error,
                    guard: (Timer.HasElapsed && ComputingModule.DoorsOpen) || (Timer.HasElapsed && !ComputingModule.DoorsClosed),
                    action: () => AnomalyDetected = true)
                .Transition(
                    @from: HealthMonitoringStates.Close,
                    to: HealthMonitoringStates.Wait,
                    guard: ComputingModule.DoorsClosed && !ComputingModule.DoorsOpen);

            
        }
    }
}

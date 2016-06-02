using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{ 
    using SafetySharp.Modeling;

    class HealthGears : HealthMonitoring
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"> The computing model which does the health monitoring. </param>
        public HealthGears(ComputingModule module) : base(module) { }

        public override void Update()
        {
            Update(Timer);

            //Gears motion monitoring
            StateMachine
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Extend,
                    guard: ComputingModule.ExtendEV,
                    action: () =>
                    {
                        Timer.Start(100);
                    })
                .Transition(
                    from: HealthMonitoringStates.Extend,
                    to: HealthMonitoringStates.Error,
                    guard: (Timer.RemainingTime == 30 && ComputingModule.GearsRetracted) || (Timer.HasElapsed && !ComputingModule.GearsExtended),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Extend,
                    to: HealthMonitoringStates.Wait,
                    guard: ComputingModule.GearsExtended && !ComputingModule.GearsRetracted)
                .Transition(
                    from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Retract,
                    guard: ComputingModule.RetractEV,
                    action: () =>
                    {
                        Timer.Start(100);
                    })
                .Transition(
                    from: HealthMonitoringStates.Retract,
                    to: HealthMonitoringStates.Error,
                    guard: (Timer.RemainingTime == 30 && ComputingModule.GearsExtended) || (Timer.HasElapsed && !ComputingModule.GearsRetracted),
                    action: () => AnomalyDetected = true)
                .Transition(
                    from: HealthMonitoringStates.Retract,
                    to: HealthMonitoringStates.Wait,
                    guard: ComputingModule.GearsRetracted && !ComputingModule.GearsExtended);
        }
    }
}

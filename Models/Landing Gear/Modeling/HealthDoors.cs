// The MIT License (MIT)
// 
// Copyright (c) 2014-2016, Institute for Software & Systems Engineering
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    internal class HealthDoors : HealthMonitoring
    {
        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="module"> The computing model which does the health monitoring. </param>
        public HealthDoors(ComputingModule module)
            : base(module)
        {
        }

        /// <summary>
        ///   Update the HealthDoors instance.
        /// </summary>
        public override void Update()
        {
            Update(Timer);

            //Doors motion monitoring
            StateMachine
                .Transition(
                    @from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Open,
                    guard: ComputingModule.OpenEV,
                    action: () => { Timer.Start(70); })
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
                    action: () => { Timer.Start(70); })
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

        /// <summary>
        ///   Resets the state machine.
        /// </summary>
        public override void Reset()
        {
            StateMachine.
                Transition(
                    from: new[] { HealthMonitoringStates.Close, HealthMonitoringStates.Open },
                    to: HealthMonitoringStates.Wait);
        }
    }
}
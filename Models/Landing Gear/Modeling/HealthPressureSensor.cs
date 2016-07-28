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
    internal class HealthPressureSensor : HealthMonitoring
    {
        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="module"> The computing model which does the health monitoring. </param>
        public HealthPressureSensor(ComputingModule module)
            : base(module)
        {
        }

        /// <summary>
        ///   Updates the HealthPressureSensor instance.
        /// </summary>
        public override void Update()
        {
            Update(Timer);

            //Pressure sensor monitoring
            StateMachine
                .Transition(
                    @from: HealthMonitoringStates.Wait,
                    to: HealthMonitoringStates.Start,
                    guard: ComputingModule.GeneralEV,
                    action: () => { Timer.Start(20); })
                .Transition(
                    @from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Intermediate,
                    guard: ComputingModule.CircuitPressurized.Value)
                .Transition(
                    @from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Error,
                    guard: Timer.HasElapsed && !ComputingModule.CircuitPressurized.Value,
                    action: () => AnomalyDetected = true)
                .Transition(
                    @from: HealthMonitoringStates.Intermediate,
                    to: HealthMonitoringStates.End,
                    guard: !ComputingModule.GeneralEV,
                    action: () => { Timer.Start(100); })
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: !ComputingModule.CircuitPressurized.Value)
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Start,
                    guard: ComputingModule.GeneralEV,
                    action: () => { Timer.Start(20); })
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard: Timer.HasElapsed && ComputingModule.CircuitPressurized.Value,
                    action: () => AnomalyDetected = true);
        }

        /// <summary>
        ///   Resets the state machine.
        /// </summary>
        public override void Reset()
        {
            StateMachine.
                Transition(
                    from: new[] { HealthMonitoringStates.Start, HealthMonitoringStates.Intermediate },
                    to: HealthMonitoringStates.Wait);
        }
    }
}
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
    internal class HealthSwitch : HealthMonitoring
    {
        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="module"> The computing model which does the health monitoring. </param>
        public HealthSwitch(ComputingModule module)
            : base(module)
        {
        }

        /// <summary>
        ///   Updates the HealthSwitch instance.
        /// </summary>
        public override void Update()
        {
            Update(Timer);

            //Analogical switch monitoring
            StateMachine
                .Transition(
                    @from: new[] { HealthMonitoringStates.Wait, HealthMonitoringStates.End, HealthMonitoringStates.Start },
                    to: HealthMonitoringStates.Start,
                    guard: ComputingModule.HandleHasMoved,
                    action: () => { Timer.Start(200); })
                .Transition(
                    @from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.End,
                    guard: Timer.HasElapsed,
                    action: () => { Timer.Start(15); })
                .Transition(
                    @from: HealthMonitoringStates.Start,
                    to: HealthMonitoringStates.Error,
                    guard: Timer.RemainingTime == 190 && ComputingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Open,
                    action: () => AnomalyDetected = true)
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Wait,
                    guard: ComputingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Open && !ComputingModule.HandleHasMoved,
                    action: () => Timer.Stop())
                .Transition(
                    @from: HealthMonitoringStates.End,
                    to: HealthMonitoringStates.Error,
                    guard:
                        Timer.HasElapsed && !ComputingModule.HandleHasMoved &&
                        ComputingModule.AnalogicalSwitch.Value == AnalogicalSwitchStates.Closed,
                    action: () => AnomalyDetected = true);
        }
    }
}
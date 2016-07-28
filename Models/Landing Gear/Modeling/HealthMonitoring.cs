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
    using SafetySharp.Modeling;

    /// <summary>
    ///   Describes the possible states of the state machines monitoring system health.
    /// </summary>
    public enum HealthMonitoringStates
    {
        /// <summary>
        ///   State indicating that no action is currently taking place.
        /// </summary>
        Wait,

        /// <summary>
        ///   In case of the pressure sensor and analogical switch monitoring: State indicating that a trigger action has taken place.
        /// </summary>
        Start,

        /// <summary>
        ///   State indicating that an anomaly has been detected.
        /// </summary>
        Error,

        /// <summary>
        ///   In case of the pressure sensor: State indicating that the circuit is pressurized and no stopping order has yet been given.
        /// </summary>
        Intermediate,

        /// <summary>
        ///   In case of the pressure sensor and analogical switch monitoring: State indicating that the action to be executed is being
        ///   finished.
        /// </summary>
        End,

        /// <summary>
        ///   In case of doors motion monitoring: State indicating the doors are being opened.
        /// </summary>
        Open,

        /// <summary>
        ///   In case of doors motion monitoring: State indicating the doors are being closed.
        /// </summary>
        Close,

        /// <summary>
        ///   In case of gears motion monitoring: State indicating the gears are being extended.
        /// </summary>
        Extend,

        /// <summary>
        ///   In case of gears motion monitoring: State indicating the geras are being retracted.
        /// </summary>
        Retract
    }

    internal class HealthMonitoring : Component
    {
        //Health monitoring
        //Generic monitoring -> see ComputingModule

        /// <summary>
        ///   An instance of the associated computing module.
        /// </summary>
        protected readonly ComputingModule ComputingModule;

        /// <summary>
        ///   Gets the state machine that manages the healthmonitoring of the analogical switch.
        /// </summary>
        protected readonly StateMachine<HealthMonitoringStates> StateMachine = HealthMonitoringStates.Wait;

        /// <summary>
        ///   Timer used for health monitoring of the analogical switch.
        /// </summary>
        protected readonly Timer Timer = new Timer();

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        public HealthMonitoring(ComputingModule module)
        {
            ComputingModule = module;
        }

        /// <summary>
        ///   Indicates whether an anomaly has been detected.
        /// </summary>
        public bool AnomalyDetected { get; protected set; }

        /// <summary>
        ///   Virtual methods to reset the state machine.
        /// </summary>
        public virtual void Reset()
        {
        }
    }
}
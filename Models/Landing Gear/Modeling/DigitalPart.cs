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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SafetySharp.Modeling;

    /// <summary>
    ///   Desscribe the mode the digital part operates in.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        ///   Any one value of the computing module has to be true to return a true value (logical OR).
        /// </summary>
        Any,

        /// <summary>
        ///   All values of the computing module has to be true to return a true value (logical AND).
        /// </summary>
        All
    }

    public class DigitalPart : Component
    {
        /// <summary>
        ///   Function delegate that can be set in accordance with the chosen mode.
        /// </summary>
        private readonly Func<IEnumerable<ComputingModule>, Func<ComputingModule, bool>, bool> _comparisonFunction;

        /// <summary>
        ///   Array with computing modules the digital part is composed of.
        /// </summary>
        public readonly ComputingModule[] ComputingModules;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="mode">Indicates the mode the digital part is operating in: Any, All, One.</param>
        /// <param name="count">Indicates how many computing modules are to be used.</param>
        /// <param name="startState">Indicates the indital state of the action sequence.</param>
        public DigitalPart(Mode mode, int count, ActionSequenceStates startState)
        {
            ComputingModules = new ComputingModule[count];
            for (var i = 0; i < count; i++)
            {
                ComputingModules[i] = new ComputingModule(startState);
            }

            if (mode == Mode.All)
                _comparisonFunction = Enumerable.All;
            else
                _comparisonFunction = Enumerable.Any;

            InitializeSensors();
        }

        /// <summary>
        ///   Initializes a new instance with only one computing module.
        /// </summary>
        /// <param name="startState">Indicates the initial state of the action sequence. </param>
        public DigitalPart(ActionSequenceStates startState)
        {
            ComputingModules = new[] { new ComputingModule(startState) };
            _comparisonFunction = Enumerable.Any;

            InitializeSensors();
        }

        /// <summary>
        ///   Updates the DigitalPart instance.
        /// </summary>
        public override void Update()
        {
            Update(ComputingModules);

            //Opens or closes motion controlling electro-valves
            if (_comparisonFunction(ComputingModules, element => element.CloseEV))
            {
                OpenCloseEV();
            }
            else
            {
                CloseCloseEV();
            }

            if (_comparisonFunction(ComputingModules, element => element.OpenEV))
            {
                OpenOpenEV();
            }
            else
            {
                CloseOpenEV();
            }

            if (_comparisonFunction(ComputingModules, element => element.RetractEV))
            {
                OpenRetractEV();
            }
            else
            {
                CloseRetractEV();
            }

            if (_comparisonFunction(ComputingModules, element => element.ExtendEV))
            {
                OpenExtendEV();
            }
            else
            {
                CloseExtendEV();
            }
        }

        /// <summary>
        ///   Gets a value indicating whether the general electro valve is to be stimulated through composition of the two computing
        ///   modules outputs.
        /// </summary>
        public bool GeneralEVComposition() => _comparisonFunction(ComputingModules, element => element.GeneralEV);

        /// <summary>
        ///   Closes the door closing electro-valve.
        /// </summary>
        public extern void CloseCloseEV();

        /// <summary>
        ///   Opens the door closing electro-valve.
        /// </summary>
        public extern void OpenCloseEV();

        /// <summary>
        ///   Closes the door opening electro-valve.
        /// </summary>
        public extern void CloseOpenEV();

        /// <summary>
        ///   Opens the door opening electro-valve.
        /// </summary>
        public extern void OpenOpenEV();

        /// <summary>
        ///   Closes the gear retraction electro-valve.
        /// </summary>
        public extern void CloseRetractEV();

        /// <summary>
        ///   Opens the gear retraction electro-valve.
        /// </summary>
        public extern void OpenRetractEV();

        /// <summary>
        ///   Closes the gear extension electro-valve.
        /// </summary>
        public extern void CloseExtendEV();

        /// <summary>
        ///   OPens the gear extension electro-valve.
        /// </summary>
        public extern void OpenExtendEV();

        /// <summary>
        ///   Gets a value indicating whether all three gears are locked down through composition of the two computing modules outputs.
        /// </summary>
        public bool GearsLockedDownComposition() => _comparisonFunction(ComputingModules, element => element.GearsLockedDown);

        /// <summary>
        ///   Gets a value indicating whether at least one gear or one door is not in locked position through composition of the two
        ///   computing modules outputs.
        /// </summary>
        public bool GearsManeuveringComposition() => _comparisonFunction(ComputingModules, element => element.GearsManeuvering);

        /// <summary>
        ///   Gets a value indicating whether an anomaly has been detected through composition of the two computing modules outputs.
        /// </summary>
        public bool AnomalyComposition() => _comparisonFunction(ComputingModules, element => element.Anomaly);

        /// <summary>
        ///   Initializes the triple sensors in the computing modules.
        /// </summary>
        private void InitializeSensors()
        {
            var sensorHandle = new TripleSensor<HandlePosition>("SensorPilotHandle");

            var sensorSwitch = new TripleSensor<AnalogicalSwitchStates>("SensorSwitch");

            var sensorFrontGearExtended = new TripleSensor<bool>("SensorFrontGearExtended");
            var sensorFrontGearRetracted = new TripleSensor<bool>("SensorFrontGearRetracted");
            var sensorFrontGearShockAbsorber = new TripleSensor<AirplaneStates>("SensorFrontGearShockAbsorber");

            var sensorLeftGearExtended = new TripleSensor<bool>("SensorLeftGearExtended");
            var sensorLeftGearRetracted = new TripleSensor<bool>("SensorLeftGearRetracted");
            var sensorLeftGearShockAbsorber = new TripleSensor<AirplaneStates>("SensorLeftGearShockAbsorber");

            var sensorRightGearExtended = new TripleSensor<bool>("SensorRightGearExtended");
            var sensorRightGearRetracted = new TripleSensor<bool>("SensorRightGearRetracted");
            var sensorRightGearShockAbsorber = new TripleSensor<AirplaneStates>("SensorRichtGearShockAbsorber");

            var sensorFrontDoorOpen = new TripleSensor<bool>("SensorFrontDoorOpen");
            var sensorFrontDoorClosed = new TripleSensor<bool>("SensorFrontDoorClosed");

            var sensorLeftDoorOpen = new TripleSensor<bool>("SensorLeftDoorOpen");
            var sensorLeftDoorClosed = new TripleSensor<bool>("SensorLeftDoorClosed");

            var sensorRightDoorOpen = new TripleSensor<bool>("SensorRightDoorOpen");
            var sensorRightDoorClosed = new TripleSensor<bool>("SensorRightDoorClosed");

            var sensorCircuitPressurized = new TripleSensor<bool>("SensorFirstPressureCircuit");

            foreach (var module in ComputingModules)
            {
                module.HandlePosition = sensorHandle;

                module.AnalogicalSwitch = sensorSwitch;

                module.FrontGearExtented = sensorFrontGearExtended;
                module.FrontGearRetracted = sensorFrontGearRetracted;
                module.FrontGearShockAbsorber = sensorFrontGearShockAbsorber;

                module.LeftGearExtented = sensorLeftGearExtended;
                module.LeftGearRetracted = sensorLeftGearRetracted;
                module.LeftGearShockAbsorber = sensorLeftGearShockAbsorber;

                module.RightGearExtented = sensorRightGearExtended;
                module.RightGearRetracted = sensorRightGearRetracted;
                module.RightGearShockAbsorber = sensorRightGearShockAbsorber;

                module.FrontDoorOpen = sensorFrontDoorOpen;
                module.FrontDoorClosed = sensorFrontDoorClosed;

                module.LeftDoorOpen = sensorLeftDoorOpen;
                module.LeftDoorClosed = sensorLeftDoorClosed;

                module.RightDoorOpen = sensorRightDoorOpen;
                module.RightDoorClosed = sensorRightDoorClosed;

                module.CircuitPressurized = sensorCircuitPressurized;
            }
        }
    }
}
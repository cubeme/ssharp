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

    public class MechanicalPartControllers : Component
    {
        /// <summary>
        ///   Instance of AircraftHydraulicCircuit.
        /// </summary>
        public readonly AircraftHydraulicCircuit AircraftHydraulicCircuit;

        /// <summary>
        ///   Instance of AnalogicalSwitch.
        /// </summary>
        public readonly AnalogicalSwitch AnalogicalSwitch = new AnalogicalSwitch();

        /// <summary>
        ///   Instance of ElectroValve representing the door closure electro-valve.
        /// </summary>
        public readonly ElectroValve CloseEV;

        /// <summary>
        ///   Instance of ElectroValve representing the gear extension electro-valve.
        /// </summary>
        public readonly ElectroValve ExtendEV;

        /// <summary>
        ///   Instance of PressureCircuit representing the extension circuit of the doors.
        /// </summary>
        public readonly PressureCircuit ExtensionCircuitDoors;

        /// <summary>
        ///   Instance of PressureCircuit representing the extension circuit of the gears.
        /// </summary>
        public readonly PressureCircuit ExtensionCircuitGears;

        /// <summary>
        ///   Instance of PressureCircuit representing the first pressure circuite.
        /// </summary>
        public readonly PressureCircuit FirstPressureCircuit;

        /// <summary>
        ///   Instance of ElectroValve representing the general electro-valve.
        /// </summary>
        public readonly ElectroValve GeneralEV;

        /// <summary>
        ///   Instance of ElectroValve representing the door opening electro-valve.
        /// </summary>
        public readonly ElectroValve OpenEV;

        /// <summary>
        ///   Instance of ElectroValve representing the gear retrction electro-valve.
        /// </summary>
        public readonly ElectroValve RetractEV;

        /// <summary>
        ///   Instance of PressureCircuit representing the retraction circuit of the doors.
        /// </summary>
        public readonly PressureCircuit RetractionCircuitDoors;

        /// <summary>
        ///   Instance of PressureCircuit representing the retraction circuit of the gears.
        /// </summary>
        public readonly PressureCircuit RetractionCircuitGears;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="limit">Indicates the maximum pressure of the pressure circuits.</param>
        public MechanicalPartControllers(int limit)
        {
            AircraftHydraulicCircuit = new AircraftHydraulicCircuit(limit);
            FirstPressureCircuit = new PressureCircuit(limit, "FirstPressureCircuit");
            RetractionCircuitDoors = new PressureCircuit(limit, "RetractionCircuitDoors");
            ExtensionCircuitDoors = new PressureCircuit(limit, "ExtensionCircuitDoors");
            RetractionCircuitGears = new PressureCircuit(limit, "RetractionCircuitGears");
            ExtensionCircuitGears = new PressureCircuit(limit, "ExtensionCircuitGears");

            GeneralEV = new ElectroValve(limit, "GeneralEV");
            OpenEV = new ElectroValve(limit, "OpenEV");
            CloseEV = new ElectroValve(limit, "CloseEV");
            ExtendEV = new ElectroValve(limit, "ExtendEV");
            RetractEV = new ElectroValve(limit, "RetractEV");
        }

        /// <summary>
        ///   Updates the MechanicalPartControllers instance.
        /// </summary>
        public override void Update()
        {
            Update(GeneralEV, OpenEV, CloseEV, ExtendEV, RetractEV, AircraftHydraulicCircuit, AnalogicalSwitch, FirstPressureCircuit,
                RetractionCircuitDoors, ExtensionCircuitDoors, RetractionCircuitGears, ExtensionCircuitGears);
        }
    }
}
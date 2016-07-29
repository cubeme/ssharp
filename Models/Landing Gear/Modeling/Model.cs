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
    ///   Auxialiary class for initialization of a model with more than one computing module.
    /// </summary>
    public class InitializeMany
    {
        public readonly ActionSequenceStates ActionStart = ActionSequenceStates.WaitRetract;
        public readonly AirplaneStates AirplaneState = AirplaneStates.Flight;
        public readonly int Count = 1;
        public readonly GearStates GearStart = GearStates.LockedExtended;
        public readonly HandlePosition HandlePosition = HandlePosition.Down;
        public readonly Mode Mode = Mode.Any;
        public readonly HandlePosition Move = HandlePosition.Down;
        public readonly int PressureLimit = 60;
    }

    /// <summary>
    ///   Auxialiary class for initialization of a model with one computing module.
    /// </summary>
    public class InitializeOne
    {
        public readonly ActionSequenceStates ActionStart = ActionSequenceStates.WaitRetract;
        public readonly AirplaneStates AirplaneState = AirplaneStates.Flight;
        public readonly GearStates GearStart = GearStates.LockedExtended;
        public readonly HandlePosition HandlePosition = HandlePosition.Down;
        public readonly HandlePosition Move = HandlePosition.Down;
        public readonly int PressureLimit = 60;
    }

    public class Model : ModelBase
    {
        /// <summary>
        ///   Instance of Airplane categorized as plant.
        /// </summary>
        [Root(RootKind.Plant)]
        public Airplane Airplane;

        /// <summary>
        ///   Instance of DigitalPart categorized as controller.
        /// </summary>
        [Root(RootKind.Controller)]
        public DigitalPart DigitalPart;

        /// <summary>
        ///   Instance of MechanicalPartControllers categorized as controller.
        /// </summary>
        [Root(RootKind.Controller)]
        public MechanicalPartControllers MechanicalPartControllers;

        /// <summary>
        ///   Instance of MechanicalPartPlants categorized as plant.
        /// </summary>
        [Root(RootKind.Plant)]
        public MechanicalPartPlants MechanicalPartPlants;

        /// <summary>
        ///   Instance of Pilot categorized as plant.
        /// </summary>
        [Root(RootKind.Plant)]
        public Pilot Pilot;

        /// <summary>
        ///   Intialize a new instance with more than one computing module.
        /// </summary>
        /// <param name="initialize">Helps initialize with initialization.</param>
        public Model(InitializeMany initialize)
        {
            Airplane = new Airplane(initialize.AirplaneState);

            DigitalPart = new DigitalPart(initialize.Mode, initialize.Count, initialize.ActionStart);

            MechanicalPartControllers = new MechanicalPartControllers(initialize.PressureLimit);

            Pilot = new Pilot(initialize.HandlePosition)
            {
                Cockpit = new Cockpit(),
                Move = initialize.Move
            };

            MechanicalPartPlants = new MechanicalPartPlants(initialize.GearStart)
            {
                Actuators = new MechanicalPartActuators(initialize.GearStart)
            };

            Bindings();
        }

        /// <summary>
        ///   Intialize a new instance with one computing module.
        /// </summary>
        /// <param name="initialize">Helps initialize with initialization.</param>
        public Model(InitializeOne initialize)
        {
            Airplane = new Airplane(initialize.AirplaneState);

            DigitalPart = new DigitalPart(initialize.ActionStart);

            MechanicalPartControllers = new MechanicalPartControllers(initialize.PressureLimit);

            Pilot = new Pilot(initialize.HandlePosition)
            {
                Cockpit = new Cockpit(),
                Move = initialize.Move
            };

            MechanicalPartPlants = new MechanicalPartPlants(initialize.GearStart)
            {
                Actuators = new MechanicalPartActuators(initialize.GearStart)
            };

            Bindings();
        }

        /// <summary>
        ///   Instance of MechanicalActuators actin as container for actuators (which are neither plants nor controllers).
        /// </summary>
        public MechanicalPartActuators MechanicalActuators => MechanicalPartPlants.Actuators;

        /// <summary>
        ///   Instance of Cockpit actin as container for pilot handel and cockpit lights (which are neither plants nor controllers).
        /// </summary>
        public Cockpit Cockpit => Pilot.Cockpit;

        /// <summary>
        ///   Sets the port bindings.
        /// </summary>
        private void Bindings()
        {
            Bind(nameof(MechanicalPartPlants.GearFront.GearCylinderState), nameof(MechanicalActuators.FrontGearCylinder.GearCylinderState));
            Bind(nameof(MechanicalPartPlants.GearRight.GearCylinderState), nameof(MechanicalActuators.RightGearCylinder.GearCylinderState));
            Bind(nameof(MechanicalPartPlants.GearLeft.GearCylinderState), nameof(MechanicalActuators.LeftGearCylinder.GearCylinderState));

            Bind(nameof(MechanicalPartPlants.DoorFront.DoorCylinderState), nameof(MechanicalActuators.FrontDoorCylinder.DoorCylinderState));
            Bind(nameof(MechanicalPartPlants.DoorRight.DoorCylinderState), nameof(MechanicalActuators.RightDoorCylinder.DoorCylinderState));
            Bind(nameof(MechanicalPartPlants.DoorLeft.DoorCylinderState), nameof(MechanicalActuators.LeftDoorCylinder.DoorCylinderState));

            Bind(nameof(MechanicalActuators.FrontGearCylinder.ExtensionCircuitIsPressurized),
                nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalActuators.RightGearCylinder.ExtensionCircuitIsPressurized),
                nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalActuators.LeftGearCylinder.ExtensionCircuitIsPressurized),
                nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));

            Bind(nameof(MechanicalActuators.FrontGearCylinder.RetractionCircuitIsPressurized),
                nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalActuators.RightGearCylinder.RetractionCircuitIsPressurized),
                nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalActuators.LeftGearCylinder.RetractionCircuitIsPressurized),
                nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));

            Bind(nameof(MechanicalActuators.FrontDoorCylinder.ExtensionCircuitIsPressurized),
                nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalActuators.RightDoorCylinder.ExtensionCircuitIsPressurized),
                nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalActuators.LeftDoorCylinder.ExtensionCircuitIsPressurized),
                nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));

            Bind(nameof(MechanicalActuators.FrontDoorCylinder.RetractionCircuitIsPressurized),
                nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalActuators.RightDoorCylinder.RetractionCircuitIsPressurized),
                nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalActuators.LeftDoorCylinder.RetractionCircuitIsPressurized),
                nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));

            Bind(nameof(MechanicalPartControllers.ExtensionCircuitGears.InputPressure), nameof(MechanicalPartControllers.ExtendEV.Hout));
            Bind(nameof(MechanicalPartControllers.RetractionCircuitGears.InputPressure), nameof(MechanicalPartControllers.RetractEV.Hout));
            Bind(nameof(MechanicalPartControllers.ExtensionCircuitDoors.InputPressure), nameof(MechanicalPartControllers.OpenEV.Hout));
            Bind(nameof(MechanicalPartControllers.RetractionCircuitDoors.InputPressure), nameof(MechanicalPartControllers.CloseEV.Hout));

            Bind(nameof(MechanicalPartControllers.ExtendEV.Hin), nameof(MechanicalPartControllers.FirstPressureCircuit.Pressure));
            Bind(nameof(MechanicalPartControllers.RetractEV.Hin), nameof(MechanicalPartControllers.FirstPressureCircuit.Pressure));
            Bind(nameof(MechanicalPartControllers.OpenEV.Hin), nameof(MechanicalPartControllers.FirstPressureCircuit.Pressure));
            Bind(nameof(MechanicalPartControllers.CloseEV.Hin), nameof(MechanicalPartControllers.FirstPressureCircuit.Pressure));

            Bind(nameof(MechanicalPartControllers.FirstPressureCircuit.InputPressure), nameof(MechanicalPartControllers.GeneralEV.Hout));

            Bind(nameof(MechanicalPartControllers.GeneralEV.Hin), nameof(MechanicalPartControllers.AircraftHydraulicCircuit.Pressure));

            Bind(nameof(Cockpit.PilotHandle.Moved), nameof(MechanicalPartControllers.AnalogicalSwitch.Close));

            Bind(nameof(Cockpit.GreenLight.LightValue), nameof(DigitalPart.GearsLockedDownComposition));
            Bind(nameof(Cockpit.OrangeLight.LightValue), nameof(DigitalPart.GearsManeuveringComposition));
            Bind(nameof(Cockpit.RedLight.LightValue), nameof(DigitalPart.AnomalyComposition));

            Bind(nameof(DigitalPart.CloseExtendEV), nameof(MechanicalPartControllers.ExtendEV.Close));
            Bind(nameof(DigitalPart.OpenExtendEV), nameof(MechanicalPartControllers.ExtendEV.Open));
            Bind(nameof(DigitalPart.CloseRetractEV), nameof(MechanicalPartControllers.RetractEV.Close));
            Bind(nameof(DigitalPart.OpenRetractEV), nameof(MechanicalPartControllers.RetractEV.Open));
            Bind(nameof(DigitalPart.CloseOpenEV), nameof(MechanicalPartControllers.OpenEV.Close));
            Bind(nameof(DigitalPart.OpenOpenEV), nameof(MechanicalPartControllers.OpenEV.Open));
            Bind(nameof(DigitalPart.CloseCloseEV), nameof(MechanicalPartControllers.CloseEV.Close));
            Bind(nameof(DigitalPart.OpenCloseEV), nameof(MechanicalPartControllers.CloseEV.Open));

            Bind(nameof(MechanicalPartControllers.AnalogicalSwitch.IncomingEOrder), nameof(DigitalPart.GeneralEVComposition));
            Bind(nameof(MechanicalPartControllers.AnalogicalSwitch.OpenGeneralEV), nameof(MechanicalPartControllers.GeneralEV.Open));
            Bind(nameof(MechanicalPartControllers.AnalogicalSwitch.CloseGeneralEV), nameof(MechanicalPartControllers.GeneralEV.Close));

            foreach (var module in DigitalPart.ComputingModules)
            {
                foreach (var sensor in module.HandlePosition.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(Cockpit.PilotHandle.Position));

                foreach (var sensor in module.AnalogicalSwitch.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartControllers.AnalogicalSwitch.SwitchPosition));

                foreach (var sensor in module.FrontGearExtented.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.GearFront.GearIsExtended));

                foreach (var sensor in module.FrontGearRetracted.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.GearFront.GearIsRetracted));

                foreach (var sensor in module.FrontGearShockAbsorber.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(Airplane.AirPlaneStatus));

                foreach (var sensor in module.LeftGearExtented.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.GearLeft.GearIsExtended));

                foreach (var sensor in module.LeftGearRetracted.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.GearLeft.GearIsRetracted));

                foreach (var sensor in module.LeftGearShockAbsorber.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(Airplane.AirPlaneStatus));

                foreach (var sensor in module.RightGearExtented.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.GearRight.GearIsExtended));

                foreach (var sensor in module.RightGearRetracted.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.GearRight.GearIsRetracted));

                foreach (var sensor in module.RightGearShockAbsorber.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(Airplane.AirPlaneStatus));

                foreach (var sensor in module.FrontDoorOpen.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.DoorFront.DoorIsOpen));

                foreach (var sensor in module.FrontDoorClosed.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.DoorFront.DoorIsClosed));

                foreach (var sensor in module.LeftDoorOpen.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.DoorLeft.DoorIsOpen));

                foreach (var sensor in module.LeftDoorClosed.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.DoorLeft.DoorIsClosed));

                foreach (var sensor in module.RightDoorOpen.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.DoorRight.DoorIsOpen));

                foreach (var sensor in module.RightDoorClosed.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartPlants.DoorRight.DoorIsClosed));

                foreach (var sensor in module.CircuitPressurized.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPartControllers.FirstPressureCircuit.IsEnabled));
            }
        }
    }
}
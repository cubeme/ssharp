using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;
    using SafetySharp.Analysis;

    class Model : ModelBase
    {
        public const int PressureLimit = 60;

        public const AirplaneStates AirplaneState = AirplaneStates.Ground;

        public const Mode Modus = Mode.Any;

        public const int Count = 2;

//--> Roles? Rootkind.Controller?
        [Root(Role.System)]
        public DigitalPart DigitalPart = new DigitalPart(Modus, Count);

        public MechanicalPart MechanicalPart = new MechanicalPart(PressureLimit);

        public PilotInterface PilotInterface = new PilotInterface();

        public Airplane Airplane = new Airplane(AirplaneState);
     
        public Model()
        {
            Bind(nameof(MechanicalPart.GearFront.GetGearCylinderState), nameof(MechanicalPart.FrontGearCylinder.GearCylinderState));
            Bind(nameof(MechanicalPart.GearRight.GetGearCylinderState), nameof(MechanicalPart.RightGearCylinder.GearCylinderState));
            Bind(nameof(MechanicalPart.GearLeft.GetGearCylinderState), nameof(MechanicalPart.LeftGearCylinder.GearCylinderState));

            Bind(nameof(MechanicalPart.DoorFront.GetDoorCylinderState), nameof(MechanicalPart.FrontDoorCylinder.DoorCylinderState));
            Bind(nameof(MechanicalPart.DoorRight.GetDoorCylinderState), nameof(MechanicalPart.RightDoorCylinder.DoorCylinderState));
            Bind(nameof(MechanicalPart.DoorLeft.GetDoorCylinderState), nameof(MechanicalPart.LeftDoorCylinder.DoorCylinderState));

            Bind(nameof(MechanicalPart.FrontGearCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPart.ExtensionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalPart.RightGearCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPart.ExtensionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalPart.LeftGearCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPart.ExtensionCircuitGears.IsEnabled));

            Bind(nameof(MechanicalPart.FrontGearCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPart.RetractionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalPart.RightGearCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPart.RetractionCircuitGears.IsEnabled));
            Bind(nameof(MechanicalPart.LeftGearCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPart.RetractionCircuitGears.IsEnabled));

            Bind(nameof(MechanicalPart.FrontDoorCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPart.ExtensionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalPart.RightDoorCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPart.ExtensionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalPart.LeftDoorCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPart.ExtensionCircuitDoors.IsEnabled));

            Bind(nameof(MechanicalPart.FrontDoorCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPart.RetractionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalPart.RightDoorCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPart.RetractionCircuitDoors.IsEnabled));
            Bind(nameof(MechanicalPart.LeftDoorCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPart.RetractionCircuitDoors.IsEnabled));

            Bind(nameof(MechanicalPart.ExtensionCircuitGears.GetInputPressure), nameof(MechanicalPart.ExtendEV.Hout));
            Bind(nameof(MechanicalPart.RetractionCircuitGears.GetInputPressure), nameof(MechanicalPart.RetractEV.Hout));
            Bind(nameof(MechanicalPart.ExtensionCircuitDoors.GetInputPressure), nameof(MechanicalPart.OpenEV.Hout));
            Bind(nameof(MechanicalPart.RetractionCircuitDoors.GetInputPressure), nameof(MechanicalPart.CloseEV.Hout));

            Bind(nameof(MechanicalPart.ExtendEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.GetPressure));
            Bind(nameof(MechanicalPart.RetractEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.GetPressure));
            Bind(nameof(MechanicalPart.OpenEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.GetPressure));
            Bind(nameof(MechanicalPart.CloseEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.GetPressure));

            Bind(nameof(MechanicalPart.FirstPressureCircuit.GetInputPressure), nameof(MechanicalPart.GeneralEV.Hout));

            Bind(nameof(MechanicalPart.GeneralEV.Hin), nameof(MechanicalPart.AircraftHydraulicCircuit.Pressure));

            Bind(nameof(PilotInterface.Handle.GetPilotHandlePosition), nameof(PilotInterface.Pilot.HandlePosition));
            Bind(nameof(MechanicalPart.AnalogicalSwitch.GetHandleHasBeenMoved), nameof(PilotInterface.Handle.Moved));

            Bind(nameof(DigitalPart.GearsLockedDownComposition), nameof(PilotInterface.GreenLight.LightValue));
            Bind(nameof(DigitalPart.GearsManeuveringComposition), nameof(PilotInterface.OrangeLight.LightValue));
            Bind(nameof(DigitalPart.AnomalyComposition), nameof(PilotInterface.RedLight.LightValue));

            Bind(nameof(MechanicalPart.ExtendEV.EOrder), nameof(DigitalPart.ExtendEVComposition));
            Bind(nameof(MechanicalPart.RetractEV.EOrder), nameof(DigitalPart.RetractEVComposition));
            Bind(nameof(MechanicalPart.OpenEV.EOrder), nameof(DigitalPart.OpenEVComposition));
            Bind(nameof(MechanicalPart.CloseEV.EOrder), nameof(DigitalPart.CloseEVComposition));

            Bind(nameof(MechanicalPart.AnalogicalSwitch.IncomingEOrder), nameof(DigitalPart.GeneralEVComposition));
            Bind(nameof(MechanicalPart.GeneralEV.EOrder), nameof(MechanicalPart.AnalogicalSwitch.OutgoingEOrder));

            foreach(ComputingModule module in DigitalPart.ComputingModules)
            {
                foreach (Sensor<HandlePosition> sensor in module.HandlePosition.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(PilotInterface.Handle.PilotHandlePosition));

                foreach (Sensor<AnalogicalSwitchStates> sensor in module.AnalogicalSwitch.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.AnalogicalSwitch.SwitchPosition));

                foreach (Sensor<bool> sensor in module.FrontGearExtented.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.GearFront.GearIsExtended));

                foreach (Sensor<bool> sensor in module.FrontGearRetracted.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.GearFront.GearIsRetracted));

                foreach (Sensor<AirplaneStates> sensor in module.FrontGearShockAbsorber.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(Airplane.AirPlaneStatus));

                foreach (Sensor<bool> sensor in module.LeftGearExtented.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.GearLeft.GearIsExtended));

                foreach (Sensor<bool> sensor in module.LeftGearRetracted.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.GearLeft.GearIsRetracted));

                foreach (Sensor<AirplaneStates> sensor in module.LeftGearShockAbsorber.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(Airplane.AirPlaneStatus));

                foreach (Sensor<bool> sensor in module.RightGearExtented.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.GearRight.GearIsExtended));

                foreach (Sensor<bool> sensor in module.RightGearRetracted.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.GearRight.GearIsRetracted));

                foreach (Sensor<AirplaneStates> sensor in module.RightGearShockAbsorber.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(Airplane.AirPlaneStatus));

                foreach (Sensor<bool> sensor in module.FrontDoorOpen.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.DoorFront.DoorIsOpen));

                foreach(Sensor <bool> sensor in module.FrontDoorClosed.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.DoorFront.DoorIsClosed));

                foreach(Sensor <bool> sensor in module.LeftDoorOpen.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.DoorLeft.DoorIsOpen));

                foreach (Sensor<bool> sensor in module.LeftDoorClosed.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.DoorLeft.DoorIsClosed));

                foreach (Sensor<bool> sensor in module.RightDoorOpen.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.DoorRight.DoorIsOpen));

                foreach (Sensor<bool> sensor in module.RightDoorClosed.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.DoorRight.DoorIsClosed));
                //--> nur FirstPressureCircuit oder alle?
                foreach (Sensor<bool> sensor in module.CircuitPressurized.Sensors)
                    Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.FirstPressureCircuit.IsEnabled));
            }


        }
    }
}

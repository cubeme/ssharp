using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class MechanicalPart : Component
    {
        //todo: Unterteilung, evtl Arrays mit doors, getEnvironmentCompenents in Module

       
        public readonly Door DoorFront = new Door(DoorPosition.Front);
        public readonly Door DoorLeft = new Door(DoorPosition.Left);
        public readonly Door DoorRight = new Door(DoorPosition.Right);
			    
        public readonly Gear GearFront = new Gear(GearPosition.Front);
        public readonly Gear GearLeft = new Gear(GearPosition.Left);
        public readonly Gear GearRight = new Gear(GearPosition.Right);
			    
        public readonly ElectroValve GeneralEV = new ElectroValve();
        public readonly ElectroValve OpenEV = new ElectroValve();
        public readonly ElectroValve CloseEV = new ElectroValve();
        public readonly ElectroValve ExtendEV = new ElectroValve();
        public readonly ElectroValve RetractEV = new ElectroValve();
			    
        public readonly AircraftHydraulicCircuit AircraftHydraulicCircuit;
			    
        public readonly DoorCylinder FrontDoorCylinder = new DoorCylinder(CylinderPosition.Front);
        public readonly DoorCylinder LeftDoorCylinder = new DoorCylinder(CylinderPosition.Left);
        public readonly DoorCylinder RightDoorCylinder = new DoorCylinder(CylinderPosition.Right);
			    
        public readonly GearCylinder FrontGearCylinder = new GearCylinder(CylinderPosition.Front);
        public readonly GearCylinder LeftGearCylinder = new GearCylinder(CylinderPosition.Left);
        public readonly GearCylinder RightGearCylinder = new GearCylinder(CylinderPosition.Right);
			    
        public readonly AnalogicalSwitch AnalogicalSwitch = new AnalogicalSwitch();
			    
        public readonly PressureCircuit FirstPressureCircuit;
        public readonly PressureCircuit RetractionCircuitDoors;
        public readonly PressureCircuit ExtensionCircuitDoors;
        public readonly PressureCircuit RetractionCircuitGears;
        public readonly PressureCircuit ExtensionCircuitGears;

        public MechanicalPart(int limit)
        {
            AircraftHydraulicCircuit = new AircraftHydraulicCircuit(limit);
            FirstPressureCircuit = new PressureCircuit(limit);
            RetractionCircuitDoors = new PressureCircuit(limit);
            ExtensionCircuitDoors = new PressureCircuit(limit);
            RetractionCircuitGears = new PressureCircuit(limit);
            ExtensionCircuitGears = new PressureCircuit(limit);
        }

        public override void Update()
        {
            Update(DoorFront, DoorLeft, DoorRight, GearFront, GearLeft, GearRight, GeneralEV, OpenEV, CloseEV, ExtendEV, RetractEV, AircraftHydraulicCircuit, FrontDoorCylinder, LeftDoorCylinder, RightDoorCylinder, FrontGearCylinder, LeftGearCylinder, RightGearCylinder, AnalogicalSwitch, FirstPressureCircuit, RetractionCircuitDoors, ExtensionCircuitDoors, RetractionCircuitGears, ExtensionCircuitGears);
        }
    }
}

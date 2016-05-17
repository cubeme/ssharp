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
       
        public Door DoorFront = new Door(DoorPosition.Front);
        public Door DoorLeft = new Door(DoorPosition.Left);
        public Door DoorRight = new Door(DoorPosition.Right);

        public Gear GearFront = new Gear(GearPosition.Front);
        public Gear GearLeft = new Gear(GearPosition.Left);
        public Gear GearRight = new Gear(GearPosition.Right);

        public ElectroValve GeneralEV = new ElectroValve();
        public ElectroValve OpenEV = new ElectroValve();
        public ElectroValve CloseEV = new ElectroValve();
        public ElectroValve ExtendEV = new ElectroValve();
        public ElectroValve RetractEV = new ElectroValve();

        public AircraftHydraulicCircuit AircraftHydraulicCircuit;

        public DoorCylinder FrontDoorCylinder = new DoorCylinder(CylinderPosition.Front);
        public DoorCylinder LeftDoorCylinder = new DoorCylinder(CylinderPosition.Left);
        public DoorCylinder RightDoorCylinder = new DoorCylinder(CylinderPosition.Right);

        public GearCylinder FrontGearCylinder = new GearCylinder(CylinderPosition.Front);
        public GearCylinder LeftGearCylinder = new GearCylinder(CylinderPosition.Left);
        public GearCylinder RightGearCylinder = new GearCylinder(CylinderPosition.Right);

        public AnalogicalSwitch AnalogicalSwitch = new AnalogicalSwitch();

        public PressureCircuit FirstPressureCircuit;
        public PressureCircuit RetractionCircuitDoors;
        public PressureCircuit ExtensionCircuitDoors;
        public PressureCircuit RetractionCircuitGears;
        public PressureCircuit ExtensionCircuitGears;

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

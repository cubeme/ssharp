﻿using System;
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

        [Root(Role.Environment)]
        public Door DoorFront = new Door(DoorPosition.Front);
        [Root(Role.Environment)]
        public Door DoorLeft = new Door(DoorPosition.Left);
        [Root(Role.Environment)]
        public Door DoorRight = new Door(DoorPosition.Right);

        [Root(Role.Environment)]
        public Gear GearFront = new Gear(GearPosition.Front);
        [Root(Role.Environment)]
        public Gear GearLeft = new Gear(GearPosition.Left);
        [Root(Role.Environment)]
        public Gear GearRight = new Gear(GearPosition.Right);

        [Root(Role.Environment)]
        public Pilot Pilot = new Pilot();

        [Root(Role.System)]
        public ComputingModule ComputingModuleOne = new ComputingModule();
        [Root(Role.System)]
        public ComputingModule ComputingModuleTwo = new ComputingModule();

        [Root(Role.System)]
        public DigitalPart DigitalPart = new DigitalPart();

        [Root(Role.System)]
        public ElectroValve GeneralEV = new ElectroValve();
        [Root(Role.System)]
        public ElectroValve OpenEV = new ElectroValve();
        [Root(Role.System)]
        public ElectroValve CloseEV = new ElectroValve();
        [Root(Role.System)]
        public ElectroValve ExtendEV = new ElectroValve();
        [Root(Role.System)]
        public ElectroValve RetractEV = new ElectroValve();

        [Root(Role.System)]
        public AircraftHydraulicCircuit AircraftHydraulicCircuit = new AircraftHydraulicCircuit(PressureLimit);

        public DoorCylinder FrontDoorCylinder = new DoorCylinder(CylinderPosition.Front);
        public DoorCylinder LeftDoorCylinder = new DoorCylinder(CylinderPosition.Left);
        public DoorCylinder RightDoorCylinder = new DoorCylinder(CylinderPosition.Right);

        public GearCylinder FrontGearCylinder = new GearCylinder(CylinderPosition.Front);
        public GearCylinder LeftGearCylinder = new GearCylinder(CylinderPosition.Left);
        public GearCylinder RightGearCylinder = new GearCylinder(CylinderPosition.Right);

        public PilotHandle PilotHandle = new PilotHandle();

        [Root(Role.System)]
        public AnalogicalSwitch AnalogicalSwitch = new AnalogicalSwitch();

        [Root(Role.System)]
        public HydraulicCircuit FirstPressureCircuit = new HydraulicCircuit(PressureLimit);
        [Root(Role.System)]
        public HydraulicCircuit RetractionCircuitDoors = new HydraulicCircuit(PressureLimit);
        [Root(Role.System)]
        public HydraulicCircuit ExtensionCircuitDoors = new HydraulicCircuit(PressureLimit);
        [Root(Role.System)]
        public HydraulicCircuit RetractionCircuitGears = new HydraulicCircuit(PressureLimit);
        [Root(Role.System)]
        public HydraulicCircuit ExtensionCircuitGears = new HydraulicCircuit(PressureLimit);

        public LightGreen GreenLight = new LightGreen();
        public LightOrange OrangelLight = new LightOrange();
        public LightRed RedLight = new LightRed();

        public Model()
        {
            //Bind...
        }
    }
}

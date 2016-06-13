namespace SafetySharp.CaseStudies.LandingGear
{
    using System.Diagnostics;
    using NUnit.Framework;
    using SafetySharp.Analysis;
    using SafetySharp.Modeling;

    [TestFixture(60, AirplaneStates.Flight, Mode.Any, 1)]
    class Model : ModelBase
	{
	    [Root(RootKind.Plant)]
        public Airplane Airplane;

	    [Root(RootKind.Controller)]
        public DigitalPart DigitalPart;

	    [Root(RootKind.Controller)]
        public MechanicalPartControllers MechanicalPartControllers;

	    [Root(RootKind.Plant)]
        public MechanicalPartPlants MechanicalPartPlants;

	    public MechanicalPartActuators MechanicalActuators => MechanicalPartPlants.Actuators;

	    public Cockpit Cockpit => Pilot.Cockpit;

	    [Root(RootKind.Plant)]
        public Pilot Pilot;

        public Model(int limit, AirplaneStates state, Mode modus, int number)
		{
		    var pressureLimit = limit;
		    var airplaneStates = state;
		    var mode = modus;
		    var count = number;

            Airplane = new Airplane(airplaneStates);

            DigitalPart = new DigitalPart(mode, count);

            MechanicalPartControllers = new MechanicalPartControllers(pressureLimit);

            Pilot = new Pilot
            {
                Cockpit = new Cockpit()
            };

            MechanicalPartPlants = new MechanicalPartPlants()
            {
                Actuators = new MechanicalPartActuators()
            };

            Bind(nameof(MechanicalPartPlants.GearFront.GearCylinderState), nameof(MechanicalActuators.FrontGearCylinder.GearCylinderState));
			Bind(nameof(MechanicalPartPlants.GearRight.GearCylinderState), nameof(MechanicalActuators.RightGearCylinder.GearCylinderState));
			Bind(nameof(MechanicalPartPlants.GearLeft.GearCylinderState), nameof(MechanicalActuators.LeftGearCylinder.GearCylinderState));

            Bind(nameof(MechanicalPartPlants.DoorFront.DoorCylinderState), nameof(MechanicalActuators.FrontDoorCylinder.DoorCylinderState));
			Bind(nameof(MechanicalPartPlants.DoorRight.DoorCylinderState), nameof(MechanicalActuators.RightDoorCylinder.DoorCylinderState));
			Bind(nameof(MechanicalPartPlants.DoorLeft.DoorCylinderState), nameof(MechanicalActuators.LeftDoorCylinder.DoorCylinderState));

			Bind(nameof(MechanicalActuators.FrontGearCylinder.ExtensionCircuitIsPressurized), nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.RightGearCylinder.ExtensionCircuitIsPressurized), nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftGearCylinder.ExtensionCircuitIsPressurized), nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));

            Bind(nameof(MechanicalActuators.FrontGearCylinder.RetractionCurcuitIsPressurized), nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.RightGearCylinder.RetractionCurcuitIsPressurized), nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftGearCylinder.RetractionCurcuitIsPressurized), nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));

			Bind(nameof(MechanicalActuators.FrontDoorCylinder.ExtensionCircuitIsPressurized), nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.RightDoorCylinder.ExtensionCircuitIsPressurized), nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftDoorCylinder.ExtensionCircuitIsPressurized), nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));

            Bind(nameof(MechanicalActuators.FrontDoorCylinder.RetractionCurcuitIsPressurized), nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.RightDoorCylinder.RetractionCurcuitIsPressurized), nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftDoorCylinder.RetractionCurcuitIsPressurized), nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));

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
			Bind(nameof(MechanicalPartControllers.AnalogicalSwitch.OpenGeneralEV),nameof(MechanicalPartControllers.GeneralEV.Open));
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
				//--> nur FirstPressureCircuit oder alle?
				foreach (var sensor in module.CircuitPressurized.Sensors)
					Bind(nameof(sensor.CheckValue), nameof(MechanicalPartControllers.FirstPressureCircuit.IsEnabled));
			}
		}

		[Test]
		public void EnumerateAllStates()
		{
			var model = new Model(60, AirplaneStates.Flight, Mode.Any, 1);

			var modelchecker = new SSharpChecker() { Configuration = { CpuCount = 1, StateCapacity = 1 << 26, StackCapacity = 1 << 22 } };

			// var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );

			var result = modelchecker.CheckInvariant(model, true);

			Assert.IsTrue(result.FormulaHolds);
		}

		[Test]
		public void Main()
		{
			var model = new Model(60, AirplaneStates.Flight, Mode.Any, 1);

			var modelchecker = new SSharpChecker() { Configuration = { StateCapacity = 1 << 16, CpuCount = 1 } };

			// var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );

			var result = modelchecker.CheckInvariant(model, !model.MechanicalPartPlants.DoorFront.DoorIsOpen);

			Assert.IsTrue(result.FormulaHolds);
		}

		[Test]
		public void Simulation()
		{
			var model = new Model(60, AirplaneStates.Flight, Mode.Any, 1);

			var simulator = new Simulator(model);
			model = (Model)simulator.Model;

			for (var i = 0; i < 300; i++)
			{
				simulator.SimulateStep();

                Debug.WriteLine($"ActionSequence State: {model.DigitalPart.ComputingModules[0]._actionSequence._stateMachine.State}");
                Debug.WriteLine($"Pilot: {model.Pilot.Position}");
				Debug.WriteLine($"DigiPart HandleHasMoved: {model.DigitalPart.ComputingModules[0].HandleHasMoved}");
				Debug.WriteLine($"OpenEV DigiPart: {model.DigitalPart.ComputingModules[0].OpenEV}");
				Debug.WriteLine($"GeneralEV DigiPart: {model.DigitalPart.ComputingModules[0].GeneralEV}");
				Debug.WriteLine($"RetractEV DigiPart: {model.DigitalPart.ComputingModules[0].RetractEV}");
				Debug.WriteLine($"GearShockAbsorberRelaxed DigiPart: {model.DigitalPart.ComputingModules[0].GearShockAbsorberRelaxed}");				
                Debug.WriteLine($"AnalogicalSwitch Timer: {model.MechanicalPartControllers.AnalogicalSwitch._timer.RemainingTime}");
                Debug.WriteLine($"GeneralEV State: {model.MechanicalPartControllers.GeneralEV._stateMachine.State}");
                Debug.WriteLine($"GeneralEV Pressure: {model.MechanicalPartControllers.GeneralEV._pressureLevel}");
                Debug.WriteLine($"FirstPressureCircuit Pressure: {model.MechanicalPartControllers.FirstPressureCircuit.Pressure}");
                Debug.WriteLine($"FirstPressureCircuit _pressureLevel: {model.MechanicalPartControllers.FirstPressureCircuit._pressureLevel}");
                Debug.WriteLine($"OpenEV State: {model.MechanicalPartControllers.OpenEV._stateMachine.State}");
                Debug.WriteLine($"OpenEV Pressure: {model.MechanicalPartControllers.OpenEV._pressureLevel}");
                Debug.WriteLine($"ExtensionCircuitDoors _pressureLevel: {model.MechanicalPartControllers.ExtensionCircuitDoors._pressureLevel}");
                Debug.WriteLine($"ExtensionCircuitDoors Pressure: {model.MechanicalPartControllers.ExtensionCircuitDoors.Pressure}");
                Debug.WriteLine($"RetractEV State: {model.MechanicalPartControllers.RetractEV._stateMachine.State}");
                Debug.WriteLine($"Retract Pressure: {model.MechanicalPartControllers.RetractEV._pressureLevel}");
                Debug.WriteLine($"RetractionCircuitGears _pressureLevel: {model.MechanicalPartControllers.RetractionCircuitGears._pressureLevel}");
                Debug.WriteLine($"RetractionCircuitGears Pressure: {model.MechanicalPartControllers.RetractionCircuitGears.Pressure}");
                Debug.WriteLine($"DoorFront: {model.MechanicalPartPlants.DoorFront.State}");
                Debug.WriteLine($"DoorLeft: {model.MechanicalPartPlants.DoorLeft.State}");
                Debug.WriteLine($"DoorRight: {model.MechanicalPartPlants.DoorRight.State}");
                Debug.WriteLine($"DoorsClosed: {model.DigitalPart.ComputingModules[0].DoorsClosed}");
                Debug.WriteLine($"DoorsOpen: {model.DigitalPart.ComputingModules[0].DoorsOpen}");
                Debug.WriteLine($"GearFront: {model.MechanicalPartPlants.GearFront.State}");
                Debug.WriteLine($"GearLeft: {model.MechanicalPartPlants.GearLeft.State}");
                Debug.WriteLine($"GearRight: {model.MechanicalPartPlants.GearRight.State}");
                Debug.WriteLine($"GearsRetracted: {model.DigitalPart.ComputingModules[0].GearsRetracted}");
                Debug.WriteLine($"================== (step: {i}) ==========================================");
			}

			// var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );
		}
	}
}
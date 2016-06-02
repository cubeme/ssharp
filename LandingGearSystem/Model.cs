namespace LandingGearSystem
{
	using System;
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

        public Model(int limit, AirplaneStates state, Mode modus, int number) : base()
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

			Bind(nameof(MechanicalActuators.FrontGearCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.RightGearCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftGearCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPartControllers.ExtensionCircuitGears.IsEnabled));

            Bind(nameof(MechanicalActuators.FrontGearCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.RightGearCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftGearCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPartControllers.RetractionCircuitGears.IsEnabled));

			Bind(nameof(MechanicalActuators.FrontDoorCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.RightDoorCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftDoorCylinder.CheckPressureExtensionCircuit), nameof(MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled));

            Bind(nameof(MechanicalActuators.FrontDoorCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.RightDoorCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));
			Bind(nameof(MechanicalActuators.LeftDoorCylinder.CheckPressureRetractionCircuit), nameof(MechanicalPartControllers.RetractionCircuitDoors.IsEnabled));

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

			Bind(nameof(MechanicalPartControllers.AnalogicalSwitch.GetHandleHasBeenMoved), nameof(Cockpit.PilotHandle.Moved));

			Bind(nameof(Cockpit.GreenLight.LightValue), nameof(DigitalPart.GearsLockedDownComposition));
			Bind(nameof(Cockpit.OrangeLight.LightValue), nameof(DigitalPart.GearsManeuveringComposition));
			Bind(nameof(Cockpit.RedLight.LightValue), nameof(DigitalPart.AnomalyComposition));

			Bind(nameof(MechanicalPartControllers.ExtendEV.EOrder), nameof(DigitalPart.ExtendEVComposition));
			Bind(nameof(MechanicalPartControllers.RetractEV.EOrder), nameof(DigitalPart.RetractEVComposition));
			Bind(nameof(MechanicalPartControllers.OpenEV.EOrder), nameof(DigitalPart.OpenEVComposition));
			Bind(nameof(MechanicalPartControllers.CloseEV.EOrder), nameof(DigitalPart.CloseEVComposition));

			Bind(nameof(MechanicalPartControllers.AnalogicalSwitch.IncomingEOrder), nameof(DigitalPart.GeneralEVComposition));
			Bind(nameof(MechanicalPartControllers.GeneralEV.EOrder), nameof(MechanicalPartControllers.AnalogicalSwitch.OutgoingEOrder));

			foreach (ComputingModule module in DigitalPart.ComputingModules)
			{		
				foreach (var sensor in module.HandlePosition.Sensors)
					Bind(nameof(sensor.CheckValue), nameof(Cockpit.PilotHandle.PilotHandlePosition));

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

			for (int i = 0; i < 440; i++)
			{
				simulator.SimulateStep();

				Debug.WriteLine($"Pilot: {model.Pilot.Position}");
				Debug.WriteLine($"Handle Moved: {model.Cockpit.PilotHandle.Moved}");
				Debug.WriteLine($"HandleMoved: {model.DigitalPart.ComputingModules[0].HandleHasMoved}");
				Debug.WriteLine($"OpenEV DigiPart: {model.DigitalPart.ComputingModules[0].OpenEV}");
				Debug.WriteLine($"GeneralEV DigiPart: {model.DigitalPart.ComputingModules[0].GeneralEV}");
				Debug.WriteLine($"RetractEV DigiPart: {model.DigitalPart.ComputingModules[0].RetractEV}");
				Debug.WriteLine($"GearShockAbsorberRelaxed DigiPart: {model.DigitalPart.ComputingModules[0].GearShockAbsorberRelaxed}");
				Debug.WriteLine($"ActSeq State: {model.DigitalPart.ComputingModules[0]._actionSequence.StateMachine.State}");
				Debug.WriteLine($"AnalogicalSwitch: {model.MechanicalPartControllers.AnalogicalSwitch.StateMachine.State}");
				Debug.WriteLine($"GeneralEV: {model.MechanicalPartControllers.GeneralEV.StateMachine.State}");
				Debug.WriteLine($"OpenEV: {model.MechanicalPartControllers.OpenEV.StateMachine.State}");
				Debug.WriteLine($"Extension Pressure: {model.MechanicalPartControllers.ExtensionCircuitDoors.Pressure}");
				Debug.WriteLine($"Extension Enabled: {model.MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled}");
				Debug.WriteLine($"General Hin: {model.MechanicalPartControllers.GeneralEV.Hin}");
				Debug.WriteLine($"General Hout: {model.MechanicalPartControllers.GeneralEV.Hout}");
				Debug.WriteLine($"Open Hin: {model.MechanicalPartControllers.OpenEV.Hin}");
				Debug.WriteLine($"Open Hout: {model.MechanicalPartControllers.OpenEV.Hout}");
				Debug.WriteLine($"First Pressure: {model.MechanicalPartControllers.FirstPressureCircuit.Pressure}");
				Debug.WriteLine($"First Enabled: {model.MechanicalPartControllers.FirstPressureCircuit.IsEnabled}");
				Debug.WriteLine($"Door: {model.MechanicalActuators.FrontDoorCylinder.StateMachine.State}");
				Debug.WriteLine($"Door Ext Pressure: {model.MechanicalActuators.FrontDoorCylinder.CheckPressureExtensionCircuit}");
				Debug.WriteLine($"Gear Retract Pressure: {model.MechanicalActuators.FrontGearCylinder.CheckPressureRetractionCircuit}");
				Debug.WriteLine($"Latching: {model.MechanicalActuators.FrontDoorCylinder._latchingBoxClosedOne.StateMachine.State}");
				Debug.WriteLine($"Latching timer: {model.MechanicalActuators.FrontDoorCylinder._latchingBoxClosedOne._timer.HasElapsed}");
				Debug.WriteLine($"Latching timer time: {model.MechanicalActuators.FrontDoorCylinder._latchingBoxClosedOne._timer.RemainingTime}");
				Debug.WriteLine($"GearFront: {model.MechanicalPartPlants.GearFront.State}");
                Debug.WriteLine($"GearLeft: {model.MechanicalPartPlants.GearLeft.State}");
                Debug.WriteLine($"GearRight: {model.MechanicalPartPlants.GearRight.State}");
                Debug.WriteLine($"Sensor GearFront: {model.DigitalPart.ComputingModules[0].FrontGearRetracted.Value}");
                Debug.WriteLine($"Sensor GearLeft: {model.DigitalPart.ComputingModules[0].LeftGearRetracted.Value}");
                Debug.WriteLine($"Sensor GearRight: {model.DigitalPart.ComputingModules[0].RightGearRetracted.Value}");
                Debug.WriteLine($"GearsRetracted: {model.DigitalPart.ComputingModules[0].GearsRetracted}");
                Debug.WriteLine($"F: {model.Pilot.f}");
                Debug.WriteLine($"================== (step: {i}) ==========================================");
			}

			// var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );
		}
	}
}
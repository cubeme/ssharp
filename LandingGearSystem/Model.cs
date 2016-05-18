namespace LandingGearSystem
{
	using System;
	using System.Diagnostics;
	using NUnit.Framework;
	using SafetySharp.Analysis;
	using SafetySharp.Modeling;

	class Model : ModelBase
	{
		public const int PressureLimit = 60;

		//Im Konstruktor des Modells instanziieren
		public const AirplaneStates AirplaneState = AirplaneStates.Flight;

		public const Mode Modus = Mode.Any;

		public const int Count = 1;
		//--> Pilot / PilotHandle
		//--> ???
		public static TimeSpan Step = TimeSpan.FromSeconds(0.1);

		[Root(RootKind.Plant)]
		public Airplane Airplane = new Airplane(AirplaneState);

		//--> Roles? Rootkind.Controller?
		[Root(RootKind.Controller)]
		public DigitalPart DigitalPart = new DigitalPart(Modus, Count);

		[Root(RootKind.Controller)]
		public MechanicalPart MechanicalPart = new MechanicalPart(PressureLimit);

		[Root(RootKind.Controller)]
		public PilotInterface PilotInterface = new PilotInterface();

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

			Bind(nameof(MechanicalPart.ExtendEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.Pressure));
			Bind(nameof(MechanicalPart.RetractEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.Pressure));
			Bind(nameof(MechanicalPart.OpenEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.Pressure));
			Bind(nameof(MechanicalPart.CloseEV.Hin), nameof(MechanicalPart.FirstPressureCircuit.Pressure));

			Bind(nameof(MechanicalPart.FirstPressureCircuit.GetInputPressure), nameof(MechanicalPart.GeneralEV.Hout));

			Bind(nameof(MechanicalPart.GeneralEV.Hin), nameof(MechanicalPart.AircraftHydraulicCircuit.Pressure));

			Bind(nameof(PilotInterface.Handle.GetPilotHandlePosition), nameof(PilotInterface.Pilot.HandlePosition));
			Bind(nameof(MechanicalPart.AnalogicalSwitch.GetHandleHasBeenMoved), nameof(PilotInterface.Handle.Moved));

			Bind(nameof(PilotInterface.GreenLight.LightValue), nameof(DigitalPart.GearsLockedDownComposition));
			Bind(nameof(PilotInterface.OrangeLight.LightValue), nameof(DigitalPart.GearsManeuveringComposition));
			Bind(nameof(PilotInterface.RedLight.LightValue), nameof(DigitalPart.AnomalyComposition));

			Bind(nameof(MechanicalPart.ExtendEV.EOrder), nameof(DigitalPart.ExtendEVComposition));
			Bind(nameof(MechanicalPart.RetractEV.EOrder), nameof(DigitalPart.RetractEVComposition));
			Bind(nameof(MechanicalPart.OpenEV.EOrder), nameof(DigitalPart.OpenEVComposition));
			Bind(nameof(MechanicalPart.CloseEV.EOrder), nameof(DigitalPart.CloseEVComposition));

			Bind(nameof(MechanicalPart.AnalogicalSwitch.IncomingEOrder), nameof(DigitalPart.GeneralEVComposition));
			Bind(nameof(MechanicalPart.GeneralEV.EOrder), nameof(MechanicalPart.AnalogicalSwitch.OutgoingEOrder));

			foreach (ComputingModule module in DigitalPart.ComputingModules)
			{
				//var --> compiler sucht type selbst
				foreach (var sensor in module.HandlePosition.Sensors)
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

				foreach (Sensor<bool> sensor in module.FrontDoorClosed.Sensors)
					Bind(nameof(sensor.CheckValue), nameof(MechanicalPart.DoorFront.DoorIsClosed));

				foreach (Sensor<bool> sensor in module.LeftDoorOpen.Sensors)
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

		[Test]
		public void EnumerateAllStates()
		{
			var model = new Model();

			var modelchecker = new SSharpChecker() { Configuration = { CpuCount = 1, StateCapacity = 1 << 26, StackCapacity = 1 << 22 } };

			// var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );

			var result = modelchecker.CheckInvariant(model, true);

			Assert.IsTrue(result.FormulaHolds);
		}

		[Test]
		public void Main()
		{
			var model = new Model();

			var modelchecker = new SSharpChecker() { Configuration = { StateCapacity = 1 << 16, CpuCount = 1 } };

			// var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );

			var result = modelchecker.CheckInvariant(model, !model.MechanicalPart.DoorFront.DoorIsOpen);

			Assert.IsTrue(result.FormulaHolds);
		}

		[Test]
		public void Simulation()
		{
			var model = new Model();

			var simulator = new Simulator(model);
			model = (Model)simulator.Model;

			for (int i = 0; i < 440; i++)
			{
				simulator.SimulateStep();

				Debug.WriteLine($"Pilot: {model.PilotInterface.Pilot.HandlePosition}");
				Debug.WriteLine($"Handle Moved: {model.PilotInterface.Handle.Moved}");
				Debug.WriteLine($"HandleMoved: {model.DigitalPart.ComputingModules[0].HandleHasMoved}");
				Debug.WriteLine($"OpenEV DigiPart: {model.DigitalPart.ComputingModules[0].OpenEV}");
				Debug.WriteLine($"GeneralEV DigiPart: {model.DigitalPart.ComputingModules[0].GeneralEV}");
				Debug.WriteLine($"RetractEV DigiPart: {model.DigitalPart.ComputingModules[0].RetractEV}");
				Debug.WriteLine($"GearShockAbsorberRelaxed DigiPart: {model.DigitalPart.ComputingModules[0].GearShockAbsorberRelaxed}");
				Debug.WriteLine($"ActSeq State: {model.DigitalPart.ComputingModules[0]._actionSequence.StateMachine.State}");
				Debug.WriteLine($"AnalogicalSwitch: {model.MechanicalPart.AnalogicalSwitch.StateMachine.State}");
				Debug.WriteLine($"GeneralEV: {model.MechanicalPart.GeneralEV.StateMachine.State}");
				Debug.WriteLine($"OpenEV: {model.MechanicalPart.OpenEV.StateMachine.State}");
				Debug.WriteLine($"Extension Pressure: {model.MechanicalPart.ExtensionCircuitDoors.Pressure}");
				Debug.WriteLine($"Extension Enabled: {model.MechanicalPart.ExtensionCircuitDoors.IsEnabled}");
				Debug.WriteLine($"General Hin: {model.MechanicalPart.GeneralEV.Hin}");
				Debug.WriteLine($"General Hout: {model.MechanicalPart.GeneralEV.Hout}");
				Debug.WriteLine($"Open Hin: {model.MechanicalPart.OpenEV.Hin}");
				Debug.WriteLine($"Open Hout: {model.MechanicalPart.OpenEV.Hout}");
				Debug.WriteLine($"First Pressure: {model.MechanicalPart.FirstPressureCircuit.Pressure}");
				Debug.WriteLine($"First Enabled: {model.MechanicalPart.FirstPressureCircuit.IsEnabled}");
				Debug.WriteLine($"Door: {model.MechanicalPart.FrontDoorCylinder.StateMachine.State}");
				Debug.WriteLine($"Door Ext Pressure: {model.MechanicalPart.FrontDoorCylinder.CheckPressureExtensionCircuit}");
				Debug.WriteLine($"Gear Retract Pressure: {model.MechanicalPart.FrontGearCylinder.CheckPressureRetractionCircuit}");
				Debug.WriteLine($"Latching: {model.MechanicalPart.FrontDoorCylinder._latchingBoxClosedOne.StateMachine.State}");
				Debug.WriteLine($"Latching timer: {model.MechanicalPart.FrontDoorCylinder._latchingBoxClosedOne._timer.HasElapsed}");
				Debug.WriteLine($"Latching timer time: {model.MechanicalPart.FrontDoorCylinder._latchingBoxClosedOne._timer.RemainingTime}");
				Debug.WriteLine($"Latching unlocked: {model.MechanicalPart.FrontDoorCylinder._latchingBoxClosedOne.Unlock}");
				Debug.WriteLine($"Gear: {model.MechanicalPart.GearFront.StateMachine.State}");
				Debug.WriteLine($"================== (step: {i}) ==========================================");
			}

			// var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );
		}
	}
}
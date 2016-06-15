

namespace SafetySharp.CaseStudies.LandingGear.Tests
{
    using System.Diagnostics;
    using Analysis;
    using NUnit.Framework;
    using SafetySharp.Modeling;
    using Modeling;

    class Tests
    {
        [Test]
        public void EnumerateAllStates()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new SSharpChecker() { Configuration = { CpuCount = 1, StateCapacity = 1 << 26, StackCapacity = 1 << 22 } };

            // var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );

            var result = modelchecker.CheckInvariant(model, true);

            Assert.IsTrue(result.FormulaHolds);
        }

        [Test]
        public void Main()
        {
            var model = new Model(new InitializeOne());

            var modelchecker = new SSharpChecker() { Configuration = { StateCapacity = 1 << 16, CpuCount = 1 } };

            // var result = modelchecker.CheckInvariant(model, !model.DigitalPart.ComputingModules.Any(element => element.Anomaly) );

            var result = modelchecker.CheckInvariant(model, !model.MechanicalPartPlants.DoorFront.DoorIsOpen);

            Assert.IsTrue(result.FormulaHolds);
        }

        [Test]
        public void Simulation()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();
            model.MechanicalPartControllers.FirstPressureCircuit.CircuitEnabledFault.ForceActivation();

            var simulator = new Simulator(model);
            model = (Model)simulator.Model;

            for (var i = 0; i < 250; i++)
            {
                simulator.SimulateStep();

                Debug.WriteLine($"Pilot: {model.Pilot.Position}");
                Debug.WriteLine($"DigiPart HandleHasMoved: {model.DigitalPart.ComputingModules[0].HandleHasMoved}");
                Debug.WriteLine($"OpenEV DigiPart: {model.DigitalPart.ComputingModules[0].OpenEV}");
                Debug.WriteLine($"GeneralEV DigiPart: {model.DigitalPart.ComputingModules[0].GeneralEV}");
                Debug.WriteLine($"RetractEV DigiPart: {model.DigitalPart.ComputingModules[0].RetractEV}");
                Debug.WriteLine($"GearShockAbsorberRelaxed DigiPart: {model.DigitalPart.ComputingModules[0].GearShockAbsorberRelaxed}");
                Debug.WriteLine($"FirstPressureCircuit Pressure: {model.MechanicalPartControllers.FirstPressureCircuit.Pressure}");
                Debug.WriteLine($"ExtensionCircuitDoors Pressure: {model.MechanicalPartControllers.ExtensionCircuitDoors.Pressure}");
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



namespace SafetySharp.CaseStudies.LandingGear.Tests
{
    using System.Diagnostics;
    using System.Linq;
    using Analysis;
    using NUnit.Framework;
    using SafetySharp.Modeling;
    using Modeling;
    using static Analysis.Operators;

    internal class Tests
    {
        //Enumaretes all reachable states.
        [Test]
        public void EnumerateAllStates()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new SSharpChecker() { Configuration = { CpuCount = 1, StateCapacity = 1 << 26, StackCapacity = 1 << 22 } };

            var result = modelchecker.CheckInvariant(model, true);

            Assert.IsTrue(result.FormulaHolds);
        }

        //Simulates an outgoing or retraction sequence. Used for debugging purposes.
        [Test]
        public void Simulation()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

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
                Debug.WriteLine($"Anomaly: {model.DigitalPart.AnomalyComposition()}");
                Debug.WriteLine($"================== (step: {i}) ==========================================");
            }
        }

        //Test Requirements

        //When system is in normal mode, if the lnading gear command handle has been pushed DOWN and stays DOWN,
        //then eventually the gears will be locked down and the doors will be seen closed.
        [Test]
        public void R11Bis()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            //Handle has been moved and in the next state, the handle is in down position and stays down and normal mode.
            var handleDown = normalMode && model.DigitalPart.ComputingModules.Any(module => module.HandleHasMoved) && X(G(model.Cockpit.PilotHandle.Position == HandlePosition.Down));
            //Check that premise does in fact occur
            var result = modelchecker.Check(model, G(!handleDown));
            Assert.IsFalse(result.FormulaHolds);
            //NormalMode and HandleDown imply that eventually the gears are locked down and the doors locked in closed position.
            result = modelchecker.Check(model, G(handleDown.Implies(F(model.DigitalPart.GearsLockedDownComposition() && model.DigitalPart.ComputingModules.All(module => module.DoorsClosed)))));

            Assert.IsTrue(result.FormulaHolds);
        }

        //When the system is in normal mode, if the lnading gear command handle has been pushed UP and stays UP,
        //then eventually the gears will be locked retracted and the doors will be seen closed.
        [Test]
        public void R12Bis()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            //Handle has been moved and in the next state, the handle is in up position and stays up and normal mode.
            var handleUp = normalMode && model.DigitalPart.ComputingModules.Any(module => module.HandleHasMoved) && X(G(model.Cockpit.PilotHandle.Position == HandlePosition.Up));
            //Check that premise does in fact occur
            var result = modelchecker.Check(model, G(!handleUp));
            Assert.IsFalse(result.FormulaHolds);
            //NormalMode and HandleUp imply that eventually the gears are locked up and the doors locked in closed position.
            result = modelchecker.Check(model, G(handleUp.Implies(F(model.DigitalPart.ComputingModules.All(module => module.GearsRetracted) && model.DigitalPart.ComputingModules.All(module => module.DoorsClosed)))));

            Assert.IsTrue(result.FormulaHolds);
        }

        //When the system is in normal mode, if the landing gear command handle remains in the DOWN position,
        //then the retraction sequence is not observed.
        [Test]
        public void R21()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            //Handle stays in the down position and normal mode.
            var handleIsDown = normalMode && F(G(model.Cockpit.PilotHandle.Position == HandlePosition.Down));
            //Check that premise does in fact occur
            var result = modelchecker.Check(model, G(!handleIsDown));
            Assert.IsFalse(result.FormulaHolds);
            //NormalMode and HandleIsDown imply that the retraction sequence is not observed.
            result = modelchecker.Check(model, G(handleIsDown.Implies(model.DigitalPart.ComputingModules.All(module => module.NotRetracting))));

            Assert.IsTrue(result.FormulaHolds);
        }

        //When the system is in normal mode, if the landing gear command handle remains in the UP position,
        //then the outgoing sequence is not observed.
        [Test]
        public void R22()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            //Handle stays in the up position and normal mode.
            var handleIsUp = normalMode && F(G(model.Cockpit.PilotHandle.Position == HandlePosition.Up));
            //Check that premise does in fact occur
            var result = modelchecker.Check(model, G(!handleIsUp));
            Assert.IsFalse(result.FormulaHolds);
            //NormalMode and HandleIsUp imply that the outgoing sequence is not observed.
            result = modelchecker.Check(model, G(handleIsUp.Implies(model.DigitalPart.ComputingModules.All(module => module.NotOutgoing))));

            Assert.IsTrue(result.FormulaHolds);
        }

        //When the system is in normal mode, the stimulation of the gears outgoing or the retraction
        //electro-valves can only happen when the three doors are locked open.
        [Test]
        public void R31()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            //Stimulation of the gear extension or retraction electro-valves.
            var stimulation = model.DigitalPart.ComputingModules.All(module => module.RetractEV || module.ExtendEV);
            //If stimulations is to be true in the next step, then doors have to be open.
            var result = modelchecker.Check(model, G(normalMode.Implies(X(stimulation).Implies(model.DigitalPart.ComputingModules.All(module => module.DoorsOpen)))));

            Assert.IsTrue(result.FormulaHolds);
        }

        //When the system is in normal mode, the stimulation of the doors opening or closure
        //electro-valves can only happen when the three gears are locked down or up.
        [Test]
        public void R32()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            //Stimulation of the door opening or closure electro-valves.
            var stimulation = model.DigitalPart.ComputingModules.All(module => module.OpenEV || module.CloseEV);
            //If stimulations is to be true in the next step, then gears have to be locked down or up.
            var result = modelchecker.Check(model, G(normalMode.Implies(X(stimulation).Implies(model.DigitalPart.ComputingModules.All(module => module.GearsExtended || module.GearsRetracted)))));

            Assert.IsTrue(result.FormulaHolds);
        }

        //When the system is in normal mode, 
        //doors opening and closure electro-valves are not stimulated simultanously.
        [Test]
        public void R41()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            // Normal mode implies that doors opening and closure electro-valves are not stimulated simultanously.
            var result = modelchecker.Check(model, G(normalMode.Implies(model.DigitalPart.ComputingModules.All(module => !(module.OpenEV && module.CloseEV)))));

            Assert.IsTrue(result.FormulaHolds);
        }

        //When the system is in normal mode, 
        //gears outgoing and retraction electro-valves are not stimulated simultanously.
        [Test]
        public void R42()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            // Normal mode implies that gears outgoing and retraction electro-valves are not stimulated simultanously.
            var result = modelchecker.Check(model, G(normalMode.Implies(model.DigitalPart.ComputingModules.All(module => !(module.ExtendEV && module.RetractEV)))));

            Assert.IsTrue(result.FormulaHolds);
        }


        //When the system is in normal mode, it is not possible to stimulate the maneuvering elevtro-valves
        //(opening, closure, outgoing, or retraction) without stimulating the general electro-valve.
        [Test]
        public void R51()
        {
            var model = new Model(new InitializeOne());
            model.Faults.SuppressActivations();

            var modelchecker = new LtsMin();

            //System is in normal mode.
            var normalMode = G(!model.DigitalPart.AnomalyComposition());
            //Stimulation of the maneuvering electro-valves.
            var stimulation = model.DigitalPart.ComputingModules.All(module => module.OpenEV || module.CloseEV || module.ExtendEV || module.RetractEV);
            //If stimulations is to be true in the next step, then the general electro-valve has to be stimulated.
            var result = modelchecker.Check(model, G(normalMode.Implies(X(stimulation).Implies(model.DigitalPart.ComputingModules.All(module => module.GeneralEV)))));

            Assert.IsTrue(result.FormulaHolds);
        }

    }
}

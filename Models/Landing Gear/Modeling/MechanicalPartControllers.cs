

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class MechanicalPartControllers: Component
    {     			    
        public readonly ElectroValve GeneralEV;
        public readonly ElectroValve OpenEV ;
        public readonly ElectroValve CloseEV ;
        public readonly ElectroValve ExtendEV ;
        public readonly ElectroValve RetractEV;
			    
        public readonly AircraftHydraulicCircuit AircraftHydraulicCircuit;
			    
        public readonly AnalogicalSwitch AnalogicalSwitch = new AnalogicalSwitch();
			    
        public readonly PressureCircuit FirstPressureCircuit;
        public readonly PressureCircuit RetractionCircuitDoors;
        public readonly PressureCircuit ExtensionCircuitDoors;
        public readonly PressureCircuit RetractionCircuitGears;
        public readonly PressureCircuit ExtensionCircuitGears;

        public MechanicalPartControllers(int limit)
        {
            AircraftHydraulicCircuit = new AircraftHydraulicCircuit(limit);
            FirstPressureCircuit = new PressureCircuit(limit, "FirstPressureCircuit");
            RetractionCircuitDoors = new PressureCircuit(limit, "RetractionCircuitDoors");
            ExtensionCircuitDoors = new PressureCircuit(limit, "ExtensionCircuitDoors");
            RetractionCircuitGears = new PressureCircuit(limit, "RetractionCircuitGears");
            ExtensionCircuitGears = new PressureCircuit(limit, "ExtensionCircuitGears");

            GeneralEV = new ElectroValve(limit, "GeneralEV");
            OpenEV = new ElectroValve(limit, "OpenEV");
            CloseEV = new ElectroValve(limit, "CloseEV");
            ExtendEV = new ElectroValve(limit, "ExtendEV");
            RetractEV = new ElectroValve(limit, "RetractEV");
    }

        public override void Update()
        {
            Update( GeneralEV, OpenEV, CloseEV, ExtendEV, RetractEV, AircraftHydraulicCircuit, AnalogicalSwitch, FirstPressureCircuit, RetractionCircuitDoors, ExtensionCircuitDoors, RetractionCircuitGears, ExtensionCircuitGears);
        }
    }
}

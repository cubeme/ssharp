

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class MechanicalPartControllers: Component
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
            FirstPressureCircuit = new PressureCircuit(limit);
            RetractionCircuitDoors = new PressureCircuit(limit);
            ExtensionCircuitDoors = new PressureCircuit(limit);
            RetractionCircuitGears = new PressureCircuit(limit);
            ExtensionCircuitGears = new PressureCircuit(limit);

            GeneralEV = new ElectroValve(limit);
            OpenEV = new ElectroValve(limit);
            CloseEV = new ElectroValve(limit);
            ExtendEV = new ElectroValve(limit);
            RetractEV = new ElectroValve(limit);
    }

        public override void Update()
        {
            Update( GeneralEV, OpenEV, CloseEV, ExtendEV, RetractEV, AircraftHydraulicCircuit, AnalogicalSwitch, FirstPressureCircuit, RetractionCircuitDoors, ExtensionCircuitDoors, RetractionCircuitGears, ExtensionCircuitGears);
        }
    }
}

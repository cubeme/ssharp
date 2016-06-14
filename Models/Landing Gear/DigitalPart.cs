namespace SafetySharp.CaseStudies.LandingGear
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SafetySharp.Modeling;

    /// <summary>
    /// Modus the digital part operates in.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Any one value of the computing module has to be true to return a true value (logical OR).
        /// </summary>
        Any,
        /// <summary>
        /// All values of the computing module has to be true to return a true value (logical AND).
        /// </summary>
        All
    }

    class DigitalPart : Component
    {

        /// <summary>
        /// Array with computing modules the digital part is composed of.
        /// </summary>
        public readonly ComputingModule[] ComputingModules;

        private readonly Func<IEnumerable<ComputingModule>,Func<ComputingModule,bool>, bool> _comparisonFunction;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="mode">Indicates the mode the digital part is operating in: Any, All, One.</param>
        /// <param name="count">Indicates how many computing modules are to be used.</param>
        public DigitalPart(Mode mode, int count)
        {
            ComputingModules = new ComputingModule[count];
            for(var i = 0; i< count; i++)
            {
                ComputingModules[i] = new ComputingModule();
            }

            if (mode == Mode.All)
                _comparisonFunction = Enumerable.All;
            else
                _comparisonFunction = Enumerable.Any;

            InitializeSensors();
        }

        public DigitalPart()
        {
            ComputingModules = new[] {new ComputingModule()};
            _comparisonFunction = Enumerable.Any;

            InitializeSensors();
        }

        public override void Update()
        {     
            Update(ComputingModules);     
            
            //todo: etwas unschön
            if(_comparisonFunction(ComputingModules, element => element.CloseEV == true))
                OpenCloseEV();
            else
            {
                CloseCloseEV();
            }

            if (_comparisonFunction(ComputingModules, element => element.OpenEV == true))
                OpenOpenEV();
            else
            {
                CloseOpenEV();
            }

            if (_comparisonFunction(ComputingModules, element => element.RetractEV == true))
                OpenRetractEV();
            else
            {
                CloseRetractEV();
            }

            if (_comparisonFunction(ComputingModules, element => element.ExtendEV == true))
                OpenExtendEV();
            else
            {
                CloseExtendEV();
            }


        }

        /// <summary>
        /// Gets a value indicating whether the general electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary>       
        public bool GeneralEVComposition() => _comparisonFunction(ComputingModules, element => element.GeneralEV == true);

        /// <summary>
        /// Gets a value indicating whether the door closure electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public extern void CloseCloseEV();
        public extern void OpenCloseEV();

        /// <summary>
        /// Gets a value indicating whether the door opening electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public extern void CloseOpenEV();
        public extern void OpenOpenEV();

        /// <summary>
        /// Gets a value indicating whether the gear retraction electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public extern void CloseRetractEV();
        public extern void OpenRetractEV();

        /// <summary>
        /// Gets a value indicating whether the gear extension electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public extern void CloseExtendEV();
        public extern void OpenExtendEV();


        /// <summary>
        /// Gets a value indicating whether all three gears are locked down through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsLockedDownComposition() => _comparisonFunction(ComputingModules, element => element.GearsLockedDown == true);


        /// <summary>
        /// Gets a value indicating whether all three gears are maneuvering through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsManeuveringComposition() => _comparisonFunction(ComputingModules, element => element.GearsManeuvering == true);


        /// <summary>
        /// Gets a value indicating whether an anomaly has been detected through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool AnomalyComposition() => _comparisonFunction(ComputingModules, element => element.Anomaly == true);

        private void InitializeSensors()
        {
            var sensorHandle = new TripleSensor<HandlePosition>();

            var sensorSwitch = new TripleSensor<AnalogicalSwitchStates>();

            var sensorFrontGearExtended = new TripleSensor<bool>();
            var sensorFrontGearRetracted = new TripleSensor<bool>();
            var sensorFrontGearShockAbsorber = new TripleSensor<AirplaneStates>();

            var sensorLeftGearExtended = new TripleSensor<bool>();
            var sensorLeftGearRetracted = new TripleSensor<bool>();
            var sensorLeftGearShockAbsorber = new TripleSensor<AirplaneStates>();

            var sensorRightGearExtended = new TripleSensor<bool>();
            var sensorRightGearRetracted = new TripleSensor<bool>();
            var sensorRightGearShockAbsorber = new TripleSensor<AirplaneStates>();

            var sensorFrontDoorOpen = new TripleSensor<bool>();
            var sensorFrontDoorClosed = new TripleSensor<bool>();

            var sensorLeftDoorOpen = new TripleSensor<bool>();
            var sensorLeftDoorClosed = new TripleSensor<bool>();

            var sensorRightDoorOpen = new TripleSensor<bool>();
            var sensorRightDoorClosed = new TripleSensor<bool>();

            var sensorCircuitPressurized = new TripleSensor<bool>();


            foreach (var module in ComputingModules)
            {
                module.HandlePosition = sensorHandle;

                module.AnalogicalSwitch = sensorSwitch;

                module.FrontGearExtented = sensorFrontGearExtended;
                module.FrontGearRetracted = sensorFrontGearRetracted;
                module.FrontGearShockAbsorber = sensorFrontGearShockAbsorber;

                module.LeftGearExtented = sensorLeftGearExtended;
                module.LeftGearRetracted = sensorLeftGearRetracted;
                module.LeftGearShockAbsorber = sensorLeftGearShockAbsorber;

                module.RightGearExtented = sensorRightGearExtended;
                module.RightGearRetracted = sensorRightGearRetracted;
                module.RightGearShockAbsorber = sensorRightGearShockAbsorber;

                module.FrontDoorOpen = sensorFrontDoorOpen;
                module.FrontDoorClosed = sensorFrontDoorClosed;

                module.LeftDoorOpen = sensorLeftDoorOpen;
                module.LeftDoorClosed = sensorLeftDoorClosed;

                module.RightDoorOpen = sensorRightDoorOpen;
                module.RightDoorClosed = sensorRightDoorClosed;

                module.CircuitPressurized = sensorCircuitPressurized;

            }
        }
    }
}

namespace SafetySharp.CaseStudies.LandingGear
{
    using System;
    using System.Linq;
    using SafetySharp.Modeling;

    class ComputingModule : Component
    {

        /// <summary>
        /// Indicates the pilot handle position and the validity of the pilot handle sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<HandlePosition> HandlePosition { get; set; }

        /// <summary>
        /// Indicates the analogical switch position and the validity of the analogical switch sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AnalogicalSwitchStates> AnalogicalSwitch { get; set; }

        /// <summary>
        /// Indicates whether the front gear is extended and whether the sensor monitoring the front gear extension is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontGearExtented { get; set;  }

        /// <summary>
        /// Indicates whether the front gear is retracted and whether the sensor monitoring the front gear retraction is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontGearRetracted { get; set;  }

        /// <summary>
        /// Indicates the value of the front gear shock absorber and the validity of the front gear shock absorber sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AirplaneStates> FrontGearShockAbsorber { get; set;  }

        /// <summary>
        /// Indicates whether the left gear is extended and whether the sensor monitoring the left gear extension is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftGearExtented { get; set;  }

        /// <summary>
        /// Indicates whether the left gear is retracted and whether the sensor monitoring the left gear retraction is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftGearRetracted { get; set;  }

        /// <summary>
        /// Indicates the value of the left gear shock absorber and the validity of the left shock absorber sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AirplaneStates> LeftGearShockAbsorber { get; set;  }

        /// <summary>
        /// Indicates whether the right gear is extended and whether the sensor monitoring the right gear extension is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightGearExtented { get; set; }

        /// <summary>
        /// Indicates whether the right gear is retracted and whether the sensor monitoring the right gear retraction is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightGearRetracted { get; set; }

        /// <summary>
        /// Indicates the value of the right gear shock absorber and the validity of the rightgear shock absorber sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AirplaneStates> RightGearShockAbsorber { get; set; }


        /// <summary>
        /// Indicates whether the front door is closed and whether the sensor monitoring the front door closure is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontDoorClosed { get; set; }

        /// <summary>
        /// Indicates whether the front door is open and whether the sensor monitoring the front door opening is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontDoorOpen { get; set;  }

        /// <summary>
        /// Indicates whether the left door is closed and whether the sensor monitoring the left door closure is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftDoorClosed { get; set; }

        /// <summary>
        /// Indicates whether the left door is open and whether the sensor monitoring the left door opening is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftDoorOpen { get; set; }

        /// <summary>
        /// Indicates whether the right door is closed and whether the sensor monitoring the right door closure is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightDoorClosed { get; set; }

        /// <summary>
        /// Indicates whether the right door is open and whether the sensor monitoring the right door opening is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightDoorOpen { get; set; }


        /// <summary>
        ///  Indicates whether the hydraulic circuit is pressurized and the validity of the sensor monitoring the pressurization of the hydraulic circuit.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> CircuitPressurized { get; set; } 

        //Output values
        /// <summary>
        /// Value indicating whether the general electro valve is to be stimulated.
        /// </summary>
        public bool GeneralEV { get; private set; }
        /// <summary>
        /// Value indicating whether the door closure electro valve is to be stimulated.
        /// </summary>
        public bool CloseEV { get; private set; }

        /// <summary>
        /// Value indicating whether the door opening electro valve is to be stimulated.
        /// </summary>
        public bool OpenEV { get; private set; }
        /// <summary>
        /// Value indicating whether the gear retraction electro valve is to be stimulated.
        /// </summary>
        public bool RetractEV { get; private set; }
        /// <summary>
        /// Value indicating whether the gear extension electro valve is to be stimulated.
        /// </summary>
        public bool ExtendEV { get; private set; }

        /// <summary>
        /// value indicating whether the gears are locked down.
        /// </summary>
        public bool GearsLockedDown { get; private set; }
        /// <summary>
        /// Value indicating whether the gears are maneuvering.
        /// </summary>
        public bool GearsManeuvering { get; private set; }
        /// <summary>
        /// Value indicating whether an anomaly has been detected.
        /// </summary>
        public bool Anomaly { get; private set; }

        /// <summary>
        /// Carries out the outgoing and retraction sequence of the landing gear.
        /// </summary>
        public readonly ActionSequence _actionSequence;
        //todo: private

        private readonly HealthMonitoring[] _systemHealth;

        public ComputingModule()
        {
            _actionSequence = new ActionSequence(this);

            _systemHealth = new HealthMonitoring[]
            {new HealthSwitch(this), new HealthPressureSensor(this), new HealthDoors(this), new HealthGears(this)};

        }

        /// <summary>
        /// Value indicating the old pilot handle position to determine whether the pilot handle has been moved.
        /// </summary>
        private HandlePosition _oldHandlePosition;

        /// <summary>
        /// Gets a value indicating whether the pilot handle has been moved.
        /// </summary>
        public bool HandleHasMoved => _oldHandlePosition != HandlePosition.Value;

        /// <summary>
        /// Gets a value indicating wheter all three doors are open.
        /// </summary>
        public bool DoorsOpen => FrontDoorOpen.Value && LeftDoorOpen.Value && RightDoorOpen.Value;

        /// <summary>
        /// Gets a value indicating whether all three doors are closed.
        /// </summary>
        public bool DoorsClosed => FrontDoorClosed.Value && LeftDoorClosed.Value && RightDoorClosed.Value;   

        /// <summary>
        /// Gets a value indicating whether all three gears are extended.
        /// </summary>
        public bool GearsExtended => FrontGearExtented.Value && LeftGearExtented.Value && RightGearExtented.Value;

        /// <summary>
        /// Gets a value indicating whether all three gears are retracted.
        /// </summary>
        public bool GearsRetracted => FrontGearRetracted.Value && LeftGearRetracted.Value && RightGearRetracted.Value;

        /// <summary>
        /// Gets a value indicating whether all three shock absorbers are relaxed.
        /// </summary>
        public bool GearShockAbsorberRelaxed => FrontGearShockAbsorber.Value == AirplaneStates.Flight && LeftGearShockAbsorber.Value == AirplaneStates.Flight && RightGearShockAbsorber.Value == AirplaneStates.Flight;  

        /// <summary>
        /// Updates the computing model according to the computed values and received inputs.
        /// </summary>
        public override void Update()
        { 
            _oldHandlePosition = HandlePosition.Value;

            //Compute new values
            Update(HandlePosition, AnalogicalSwitch, FrontGearExtented, FrontGearRetracted, FrontGearShockAbsorber, LeftGearExtented, LeftGearRetracted, LeftGearShockAbsorber, RightGearExtented, RightGearRetracted, RightGearShockAbsorber, FrontDoorClosed, FrontDoorOpen, LeftDoorClosed, LeftDoorOpen, RightDoorClosed, RightDoorOpen, CircuitPressurized, _actionSequence);

            Array.ForEach(_systemHealth, element => element.Update());

            //Look for anomaly
            if (_systemHealth.Any(element => element.AnomalyDetected))
                Anomaly = true;

            if (HandlePosition.Valid == false || AnalogicalSwitch.Valid == false || FrontGearExtented.Valid == false || FrontGearRetracted.Valid == false || FrontGearShockAbsorber.Valid == false || LeftGearExtented.Valid == false || LeftGearRetracted.Valid == false || LeftGearShockAbsorber.Valid == false || RightGearExtented.Valid == false || RightGearRetracted.Valid == false || RightGearShockAbsorber.Valid == false || FrontDoorClosed.Valid == false || FrontDoorOpen.Valid == false || LeftDoorClosed.Valid == false || LeftDoorOpen.Valid == false || RightDoorClosed.Valid == false || RightDoorOpen.Valid == false || CircuitPressurized.Valid == false)
                Anomaly = true;

            //Cockpit Lights
            GearsLockedDown = GearsExtended;
            GearsManeuvering = !GearsExtended && !GearsRetracted;

        }

        public void One()
        {

            //Reverse
            CloseEV = false;

            //Step 1
            GeneralEV = true;
            //Step 2
            OpenEV = true;

        }

        //---> ?? ActionSequence.start(new Statement(OutgoingOne())
        public void OutgoingTwo()
        {

            //Reverse
            RetractEV = false;

            //Step 3
            ExtendEV = true;

        }


        public void RetractionTwo()
        {
            //Reverse
            ExtendEV = false;

            //Step 3
            RetractEV = true;

        }

        public void OutgoingThree()
        {
            //Step 4
            ExtendEV = false;
        }


        public void RetractionThree()
        {
            //Step 4
            RetractEV = false;
        }


        public void Four()
        {
            //Step 5
            OpenEV = false;
            //Step 6
            CloseEV = true;
        }
           

        public void Zero()
        {
            //Step 7
            CloseEV = false;
            //Step 8
            GeneralEV = false;
        }


    }
}

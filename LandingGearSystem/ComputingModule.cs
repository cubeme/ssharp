using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class ComputingModule : Component
    {

        /// <summary>
        /// Indicates the pilot handle position and the validity of the pilot handle sensor.
        /// </summary>
        public TripleSensor<HandlePosition> HandlePosition { get; } = new TripleSensor<HandlePosition>();

        /// <summary>
        /// Indicates the analogical switch position and the validity of the analogical switch sensor.
        /// </summary>
        public TripleSensor<AnalogicalSwitchStates> AnalogicalSwitch { get; } = new TripleSensor<AnalogicalSwitchStates>();

        /// <summary>
        /// Indicates whether the front gear is extended and whether the sensor monitoring the front gear extension is valid.
        /// </summary>
        public TripleSensor<bool> FrontGearExtented { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the front gear is retracted and whether the sensor monitoring the front gear retraction is valid.
        /// </summary>
        public TripleSensor<bool> FrontGearRetracted { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates the value of the front gear shock absorber and the validity of the front gear shock absorber sensor.
        /// </summary>
        public TripleSensor<AirplaneStates> FrontGearShockAbsorber { get; } = new TripleSensor<AirplaneStates>();

        /// <summary>
        /// Indicates whether the left gear is extended and whether the sensor monitoring the left gear extension is valid.
        /// </summary>
        public TripleSensor<bool> LeftGearExtented { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the left gear is retracted and whether the sensor monitoring the left gear retraction is valid.
        /// </summary>
        public TripleSensor<bool> LeftGearRetracted { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates the value of the left gear shock absorber and the validity of the left shock absorber sensor.
        /// </summary>
        public TripleSensor<AirplaneStates> LeftGearShockAbsorber { get; } = new TripleSensor<AirplaneStates>();

        /// <summary>
        /// Indicates whether the right gear is extended and whether the sensor monitoring the right gear extension is valid.
        /// </summary>
        public TripleSensor<bool> RightGearExtented { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the right gear is retracted and whether the sensor monitoring the right gear retraction is valid.
        /// </summary>
        public TripleSensor<bool> RightGearRetracted { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates the value of the right gear shock absorber and the validity of the rightgear shock absorber sensor.
        /// </summary>
        public TripleSensor<AirplaneStates> RightGearShockAbsorber { get; } = new TripleSensor<AirplaneStates>();


        /// <summary>
        /// Indicates whether the front door is closed and whether the sensor monitoring the front door closure is valid.
        /// </summary>
        public TripleSensor<bool> FrontDoorClosed { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the front door is open and whether the sensor monitoring the front door opening is valid.
        /// </summary>
        public TripleSensor<bool> FrontDoorOpen { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the left door is closed and whether the sensor monitoring the left door closure is valid.
        /// </summary>
        public TripleSensor<bool> LeftDoorClosed { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the left door is open and whether the sensor monitoring the left door opening is valid.
        /// </summary>
        public TripleSensor<bool> LeftDoorOpen { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the right door is closed and whether the sensor monitoring the right door closure is valid.
        /// </summary>
        public TripleSensor<bool> RightDoorClosed { get; } = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the right door is open and whether the sensor monitoring the right door opening is valid.
        /// </summary>
        public TripleSensor<bool> RightDoorOpen { get; } = new TripleSensor<bool>();


        /// <summary>
        ///  Indicates whether the hydraulic circuit is pressurized and the validity of the sensor monitoring the pressurization of the hydraulic circuit.
        /// </summary>
        public TripleSensor<bool> CircuitPressurized { get; } = new TripleSensor<bool>();

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

        /// <summary>
        /// Monitors the system health by detecting anomalies.
        /// </summary>
        private readonly HealthMonitoring _systemHealth;

        public ComputingModule()
        {
            _actionSequence = new ActionSequence(this);

            _systemHealth = new HealthMonitoring(this);
        }

        /// <summary>
        /// Value indicating the old pilot handle position to determine whether the pilot handle has been moved.
        /// </summary>
        [Hidden]
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
        /// </summary
        public override void Update()
        { 
            _oldHandlePosition = HandlePosition.Value;

            //Compute new values
            Update(HandlePosition, AnalogicalSwitch, FrontGearExtented, FrontGearRetracted, FrontGearShockAbsorber, LeftGearExtented, LeftGearRetracted, LeftGearShockAbsorber, FrontDoorClosed, FrontDoorOpen, LeftDoorClosed, LeftDoorOpen, RightDoorClosed, RightDoorOpen, RightGearShockAbsorber, CircuitPressurized, _actionSequence, _systemHealth);

            //Look for anomaly
            if (_systemHealth.AnomalyDetected)
                Anomaly = true;

            if (HandlePosition.Valid == false || AnalogicalSwitch.Valid == false || FrontGearExtented.Valid == false || FrontGearRetracted.Valid == false || FrontGearShockAbsorber.Valid == false || LeftGearExtented.Valid == false || LeftGearRetracted.Valid == false || LeftGearShockAbsorber.Valid == false || RightGearExtented.Valid == false || RightGearRetracted.Valid == false || RightGearShockAbsorber.Valid == false || FrontDoorClosed.Valid == false || FrontDoorOpen.Valid == false || LeftDoorClosed.Valid == false || LeftDoorOpen.Valid == false || RightDoorClosed.Valid == false || RightDoorOpen.Valid == false || CircuitPressurized.Valid == false)
                Anomaly = true;

            //Cockpit Lights
            GearsLockedDown = GearsExtended;
            GearsManeuvering = !GearsExtended && !GearsRetracted;

        }

        public void One()
        {
            //Step 1
            GeneralEV = true;
            //Step 2
            OpenEV = true;

            //Reverse
            CloseEV = false;
        }

        //---> ?? ActionSequence.start(new Statement(OutgoingOne())
        public void OutgoingTwo()
        {
            //Step 3
            ExtendEV = true;

            //Reverse
            RetractEV = false;
        }


        public void RetractionTwo()
        {
            //Step 3
            RetractEV = true;

            //Reverse
            ExtendEV = false;
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

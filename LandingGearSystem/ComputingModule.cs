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
        private TripleSensor<HandlePosition> _handlePosition = new TripleSensor<HandlePosition>();

        /// <summary>
        /// Indicates the analogical switch position and the validity of the analogical switch sensor.
        /// </summary>
        private TripleSensor<AnalogicalSwitchStates> _analogicalSwitch = new TripleSensor<AnalogicalSwitchStates>();

        /// <summary>
        /// Indicates whether the front gear is extended and whether the sensor monitoring the front gear extension is valid.
        /// </summary>
        private TripleSensor<bool> _frontGearExtented = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the front gear is retracted and whether the sensor monitoring the front gear retraction is valid.
        /// </summary>
        private TripleSensor<bool> _frontGearRetracted = new TripleSensor<bool>();

        /// <summary>
        /// Indicates the value of the front gear shock absorber and the validity of the front gear shock absorber sensor.
        /// </summary>
        private TripleSensor<AirplaneStates> _frontGearShockAbsorber = new TripleSensor<AirplaneStates>();

        /// <summary>
        /// Indicates whether the left gear is extended and whether the sensor monitoring the left gear extension is valid.
        /// </summary>
        private TripleSensor<bool> _leftGearExtented = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the left gear is retracted and whether the sensor monitoring the left gear retraction is valid.
        /// </summary>
        private TripleSensor<bool> _leftGearRetracted = new TripleSensor<bool>();

        /// <summary>
        /// Indicates the value of the left gear shock absorber and the validity of the left shock absorber sensor.
        /// </summary>
        private TripleSensor<AirplaneStates> _leftGearShockAbsorber = new TripleSensor<AirplaneStates>();

        /// <summary>
        /// Indicates whether the right gear is extended and whether the sensor monitoring the right gear extension is valid.
        /// </summary>
        private TripleSensor<bool> _rightGearExtented = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the right gear is retracted and whether the sensor monitoring the right gear retraction is valid.
        /// </summary>
        private TripleSensor<bool> _rightGearRetracted = new TripleSensor<bool>();

        /// <summary>
        /// Indicates the value of the right gear shock absorber and the validity of the rightgear shock absorber sensor.
        /// </summary>
        private TripleSensor<AirplaneStates> _rightGearShockAbsorber = new TripleSensor<AirplaneStates>();


        /// <summary>
        /// Indicates whether the front door is closed and whether the sensor monitoring the front door closure is valid.
        /// </summary>
        private TripleSensor<bool> _frontDoorClosed = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the front door is open and whether the sensor monitoring the front door opening is valid.
        /// </summary>
        private TripleSensor<bool> _frontDoorOpen = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the left door is closed and whether the sensor monitoring the left door closure is valid.
        /// </summary>
        private TripleSensor<bool> _leftDoorClosed = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the left door is open and whether the sensor monitoring the left door opening is valid.
        /// </summary>
        private TripleSensor<bool> _leftDoorOpen = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the right door is closed and whether the sensor monitoring the right door closure is valid.
        /// </summary>
        private TripleSensor<bool> _rightDoorClosed = new TripleSensor<bool>();

        /// <summary>
        /// Indicates whether the right door is open and whether the sensor monitoring the right door opening is valid.
        /// </summary>
        private TripleSensor<bool> _rightDoorOpen = new TripleSensor<bool>();


        /// <summary>
        ///  Indicates whether the hydraulic circuit is pressurized and the validity of the sensor monitoring the pressurization of the hydraulic circuit.
        /// </summary>
        private TripleSensor<bool> _circuitPressurized = new TripleSensor<bool>();

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
        /// Carries out the outgoing sequence of the landing gear.
        /// </summary>
        private ActionSequence OutgoingSequence = new ActionSequence();

        /// <summary>
        /// Carries out the retraction sequence of the landing gear.
        /// </summary>
        private ActionSequence RetractionSequence = new ActionSequence();

        /// <summary>
        /// Monitors the system health by detecting anomalies.
        /// </summary>
        private HealthMonitoring SystemHealth;

        public ComputingModule()
        {
            OutgoingSequence.Add(new SequenceStep(new Func<bool>(() => GearsRetracted && HandleHasMoved && _handlePosition.Value == HandlePosition.Down), new Action (OutgoingOne)));
            OutgoingSequence.Add(new SequenceStep(new Func<bool>(() => DoorsOpen), new Action(OutgoingTwo)));
            OutgoingSequence.Add(new SequenceStep(new Func<bool>(() => GearsExtended), new Action(OutgoingThree)));
            OutgoingSequence.Add(new SequenceStep(new Func<bool>(() => DoorsClosed), new Action(OutgoingFour)));

            RetractionSequence.Add(new SequenceStep(new Func<bool>(() => GearsExtended && DoorsClosed && _handlePosition.Value == HandlePosition.Up), new Action(RetractionOne)));
            RetractionSequence.Add(new SequenceStep(new Func<bool>(() => DoorsOpen && GearShockAbsorberRelaxed()), new Action(RetractionTwo)));
            RetractionSequence.Add(new SequenceStep(new Func<bool>(() => GearsRetracted), new Action(RetractionThree)));
            RetractionSequence.Add(new SequenceStep(new Func<bool>(() => true), new Action(RetractionFour)));
            RetractionSequence.Add(new SequenceStep(new Func<bool>(() => DoorsClosed), new Action(RetractionFive)));
            RetractionSequence.Add(new SequenceStep(new Func<bool>(() => DoorsOpen == true && GearShockAbsorberRelaxed() == false), new Action(RetractionFour)));

            SystemHealth = new HealthMonitoring(new Func<AnalogicalSwitchStates>(() => _analogicalSwitch.Value), new Func<bool>(() => _circuitPressurized.Value), new Func<bool>(() => DoorsOpen), new Func<bool>(() => DoorsClosed), new Func<bool>(() => GearsRetracted), new Func<bool>(() => GearsExtended), new Func<bool>(() => HandleHasMoved), new Func<bool>(() => GeneralEV), new Func<bool>(() => OpenEV), new Func<bool>(() => CloseEV), new Func<bool>(() => ExtendEV), new Func<bool>(() => RetractEV));
        }

        /// <summary>
        /// Value indicating the old pilot handle position to determine whether the pilot handle has been moved.
        /// </summary>
        private HandlePosition _oldHandlePosition;

        /// <summary>
        /// Gets a value indicating whether the pilot handle has been moved.
        /// </summary>
        private bool HandleHasMoved => _oldHandlePosition != _handlePosition.Value;

        /// <summary>
        /// Gets a value indicating wheter all three doors are open.
        /// </summary>
        private bool DoorsOpen => _frontDoorOpen.Value && _leftDoorOpen.Value && _rightDoorOpen.Value;

        /// <summary>
        /// Gets a value indicating whether all three doors are closed.
        /// </summary>
        private bool DoorsClosed => _frontDoorClosed.Value && _leftDoorClosed.Value && _rightDoorClosed.Value;   

        /// <summary>
        /// Gets a value indicating whether all three gears are extended.
        /// </summary>
        private bool GearsExtended => _frontGearExtented.Value && _leftGearExtented.Value && _rightGearExtented.Value;

        /// <summary>
        /// Gets a value indicating whether all three gears are retracted.
        /// </summary>
        private bool GearsRetracted => _frontGearRetracted.Value && _leftGearRetracted.Value && _rightGearRetracted.Value;

        /// <summary>
        /// Gets a value indicating whether all three shock absorbers are relaxed.
        /// </summary>
        private bool GearShockAbsorberRelaxed() => _frontGearShockAbsorber.Value == AirplaneStates.Flight && _leftGearShockAbsorber.Value == AirplaneStates.Flight && _rightGearShockAbsorber.Value == AirplaneStates.Flight;  

        /// <summary>
        /// Updates the computing model according to the computed values and received inputs.
        /// </summary
        public void update()
        { 
            _oldHandlePosition = _handlePosition.Value;

            //Compute new values
            Update(OutgoingSequence, RetractionSequence, SystemHealth, _handlePosition, _analogicalSwitch, _frontGearExtented, _frontGearRetracted, _frontGearShockAbsorber, _leftGearExtented, _leftGearRetracted, _leftGearShockAbsorber, _frontDoorClosed, _frontDoorOpen, _leftDoorClosed, _leftDoorOpen, _rightDoorClosed, _rightDoorOpen, _circuitPressurized);

            //Look for anomaly
            if (SystemHealth.AnomalyDetected)
                Anomaly = true;

            if (_handlePosition.Valid == false || _analogicalSwitch.Valid == false || _frontGearExtented.Valid == false || _frontGearRetracted.Valid == false || _frontGearShockAbsorber.Valid == false || _leftGearExtented.Valid == false || _leftGearRetracted.Valid == false || _leftGearShockAbsorber.Valid == false || _rightGearExtented.Valid == false || _rightGearRetracted.Valid == false || _rightGearShockAbsorber.Valid == false || _frontDoorClosed.Valid == false || _frontDoorOpen.Valid == false || _leftDoorClosed.Valid == false || _leftDoorOpen.Valid == false || _rightDoorClosed.Valid == false || _rightDoorOpen.Valid == false || _circuitPressurized.Valid == false)
                Anomaly = true;

            //Cockpit Lights
            GearsLockedDown = GearsExtended;
            GearsManeuvering = !GearsExtended && !GearsRetracted;

            if (Anomaly)
            {
                Console.WriteLine("An anomaly has been detected!");
                System.Environment.Exit(1);
            }  

        }

        private void OutgoingOne()
        {
            //Step 1
            GeneralEV = true;
            //Step 2
            OpenEV = true;
        }

        //---> ?? ActionSequence.start(new Statement(OutgoingOne())
        private void OutgoingTwo()
        {
            //Step 3
            ExtendEV = true;
        }

        private void OutgoingThree()
        {
            //Step 4
            ExtendEV = false;
            //Step 5
            OpenEV = false;
            //Step 6
            CloseEV = true;
        }

        private void OutgoingFour()
        {
            //Step 7
            CloseEV = false;
            //Step 8
            GeneralEV = false;
        }

        private void RetractionOne()
        {
            //Step 1
            GeneralEV = true;
            //Step 2
            OpenEV = true;
        }

        private void RetractionTwo()
        {
            //Step 3
            RetractEV = true;
        }

        private void RetractionThree()
        {
            //Step 4
            RetractEV = false;
        }

        private void RetractionFour()
        { 
            //Step 5
            OpenEV = false;
            //Step 6
            CloseEV = true;
        }

        private void RetractionFive()
        {
            //Step 7
            CloseEV = false;
            //Step 8
            GeneralEV = false;
        }

    }
}

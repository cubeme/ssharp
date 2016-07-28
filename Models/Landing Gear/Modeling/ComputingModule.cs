// The MIT License (MIT)
// 
// Copyright (c) 2014-2016, Institute for Software & Systems Engineering
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using System.Linq;
    using SafetySharp.Modeling;

    public class ComputingModule : Component
    {
        /// <summary>
        ///   Carries out the outgoing and retraction sequence of the landing gear.
        /// </summary>
        private readonly ActionSequence _actionSequence;

        /// <summary>
        ///   An array consisting of components that monitor system health.
        /// </summary>
        private readonly HealthMonitoring[] _systemHealth;

        /// <summary>
        ///   Value indicating the old pilot handle position to determine whether the pilot handle has been moved.
        /// </summary>
        private HandlePosition _oldHandlePosition;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="startState"> Indicates the initial state of the action sequence. </param>
        public ComputingModule(ActionSequenceStates startState)
        {
            _actionSequence = new ActionSequence(this, startState);

            //_systemHealth is initialized with child components of HealthMonitoring
            _systemHealth = new HealthMonitoring[]
            { new HealthSwitch(this), new HealthPressureSensor(this), new HealthDoors(this), new HealthGears(this) };
        }

        /// <summary>
        ///   Indicates the pilot handle position and the validity of the pilot handle sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<HandlePosition> HandlePosition { get; set; }

        /// <summary>
        ///   Indicates the analogical switch position and the validity of the analogical switch sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AnalogicalSwitchStates> AnalogicalSwitch { get; set; }

        /// <summary>
        ///   Indicates whether the front gear is extended and whether the sensor monitoring the front gear extension is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontGearExtented { get; set; }

        /// <summary>
        ///   Indicates whether the front gear is retracted and whether the sensor monitoring the front gear retraction is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontGearRetracted { get; set; }

        /// <summary>
        ///   Indicates the value of the front gear shock absorber and the validity of the front gear shock absorber sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AirplaneStates> FrontGearShockAbsorber { get; set; }

        /// <summary>
        ///   Indicates whether the left gear is extended and whether the sensor monitoring the left gear extension is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftGearExtented { get; set; }

        /// <summary>
        ///   Indicates whether the left gear is retracted and whether the sensor monitoring the left gear retraction is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftGearRetracted { get; set; }

        /// <summary>
        ///   Indicates the value of the left gear shock absorber and the validity of the left shock absorber sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AirplaneStates> LeftGearShockAbsorber { get; set; }

        /// <summary>
        ///   Indicates whether the right gear is extended and whether the sensor monitoring the right gear extension is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightGearExtented { get; set; }

        /// <summary>
        ///   Indicates whether the right gear is retracted and whether the sensor monitoring the right gear retraction is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightGearRetracted { get; set; }

        /// <summary>
        ///   Indicates the value of the right gear shock absorber and the validity of the rightgear shock absorber sensor.
        /// </summary>
        [Hidden]
        public TripleSensor<AirplaneStates> RightGearShockAbsorber { get; set; }

        /// <summary>
        ///   Indicates whether the front door is closed and whether the sensor monitoring the front door closure is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontDoorClosed { get; set; }

        /// <summary>
        ///   Indicates whether the front door is open and whether the sensor monitoring the front door opening is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> FrontDoorOpen { get; set; }

        /// <summary>
        ///   Indicates whether the left door is closed and whether the sensor monitoring the left door closure is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftDoorClosed { get; set; }

        /// <summary>
        ///   Indicates whether the left door is open and whether the sensor monitoring the left door opening is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> LeftDoorOpen { get; set; }

        /// <summary>
        ///   Indicates whether the right door is closed and whether the sensor monitoring the right door closure is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightDoorClosed { get; set; }

        /// <summary>
        ///   Indicates whether the right door is open and whether the sensor monitoring the right door opening is valid.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> RightDoorOpen { get; set; }

        /// <summary>
        ///   Indicates whether the hydraulic circuit is pressurized and the validity of the sensor monitoring the pressurization of the
        ///   hydraulic circuit.
        /// </summary>
        [Hidden]
        public TripleSensor<bool> CircuitPressurized { get; set; }

        //Output values
        /// <summary>
        ///   Value indicating whether the general electro valve is to be stimulated.
        /// </summary>
        public bool GeneralEV { get; private set; }

        /// <summary>
        ///   Value indicating whether the door closure electro valve is to be stimulated.
        /// </summary>
        public bool CloseEV { get; private set; }

        /// <summary>
        ///   Value indicating whether the door opening electro valve is to be stimulated.
        /// </summary>
        public bool OpenEV { get; private set; }

        /// <summary>
        ///   Value indicating whether the gear retraction electro valve is to be stimulated.
        /// </summary>
        public bool RetractEV { get; private set; }

        /// <summary>
        ///   Value indicating whether the gear extension electro valve is to be stimulated.
        /// </summary>
        public bool ExtendEV { get; private set; }

        /// <summary>
        ///   value indicating whether the gears are locked down.
        /// </summary>
        public bool GearsLockedDown { get; private set; }

        /// <summary>
        ///   Value indicating whether the gears are maneuvering.
        /// </summary>
        public bool GearsManeuvering { get; private set; }

        /// <summary>
        ///   Value indicating whether an anomaly has been detected.
        /// </summary>
        public bool Anomaly { get; private set; }

        /// <summary>
        ///   An instance of the action sequence that executes the landing and retraction sequence.
        /// </summary>
        public ActionSequenceStates ActionSequenceState => _actionSequence.State;

        //needed for unit tests
        /// <summary>
        /// Gets a value indicating whether the gears are not being retracted.
        /// </summary>
        public bool NotRetracting
            =>
                _actionSequence.State != ActionSequenceStates.RetractOne && _actionSequence.State != ActionSequenceStates.RetractTwo &&
                _actionSequence.State != ActionSequenceStates.RetractThree && _actionSequence.State != ActionSequenceStates.RetractFour;

        /// <summary>
        /// Gets a value indicating whether the gears are not being extended.
        /// </summary>
        public bool NotOutgoing
            =>
                _actionSequence.State != ActionSequenceStates.OutgoingOne && _actionSequence.State != ActionSequenceStates.OutgoingTwo &&
                _actionSequence.State != ActionSequenceStates.OutgoingThree && _actionSequence.State != ActionSequenceStates.OutgoingFour;

        /// <summary>
        ///   Gets a value indicating whether the pilot handle has been moved.
        /// </summary>
        public bool HandleHasMoved => _oldHandlePosition != HandlePosition.Value;

        /// <summary>
        ///   Gets a value indicating wheter all three doors are open.
        /// </summary>
        public bool DoorsOpen => FrontDoorOpen.Value && LeftDoorOpen.Value && RightDoorOpen.Value;

        /// <summary>
        ///   Gets a value indicating whether all three doors are closed.
        /// </summary>
        public bool DoorsClosed => FrontDoorClosed.Value && LeftDoorClosed.Value && RightDoorClosed.Value;

        /// <summary>
        ///   Gets a value indicating whether all three gears are extended.
        /// </summary>
        public bool GearsExtended => FrontGearExtented.Value && LeftGearExtented.Value && RightGearExtented.Value;

        /// <summary>
        ///   Gets a value indicating whether all three gears are retracted.
        /// </summary>
        public bool GearsRetracted => FrontGearRetracted.Value && LeftGearRetracted.Value && RightGearRetracted.Value;

        /// <summary>
        ///   Gets a value indicating whether all three shock absorbers are relaxed.
        /// </summary>
        public bool GearShockAbsorberRelaxed
            =>
                FrontGearShockAbsorber.Value == AirplaneStates.Flight && LeftGearShockAbsorber.Value == AirplaneStates.Flight &&
                RightGearShockAbsorber.Value == AirplaneStates.Flight;

        /// <summary>
        ///   Updates the computing model according to the computed values and received inputs.
        /// </summary>
        public override void Update()
        {
            _oldHandlePosition = HandlePosition.Value;

            //Compute new values
            Update(HandlePosition, AnalogicalSwitch, FrontGearExtented, FrontGearRetracted, FrontGearShockAbsorber, LeftGearExtented,
                LeftGearRetracted, LeftGearShockAbsorber, RightGearExtented, RightGearRetracted, RightGearShockAbsorber, FrontDoorClosed,
                FrontDoorOpen, LeftDoorClosed, LeftDoorOpen, RightDoorClosed, RightDoorOpen, CircuitPressurized, _actionSequence);

            //Reset system health monitoring if action sequence has been reversed.
            if (_actionSequence.Reset)
                foreach (var m in _systemHealth)
                    m.Reset();

            Update(_systemHealth);

            //Look for anomaly
            if (_systemHealth.Any(element => element.AnomalyDetected))
                Anomaly = true;

            if (HandlePosition.Valid == false || AnalogicalSwitch.Valid == false || FrontGearExtented.Valid == false ||
                FrontGearRetracted.Valid == false || FrontGearShockAbsorber.Valid == false || LeftGearExtented.Valid == false ||
                LeftGearRetracted.Valid == false || LeftGearShockAbsorber.Valid == false || RightGearExtented.Valid == false ||
                RightGearRetracted.Valid == false || RightGearShockAbsorber.Valid == false || FrontDoorClosed.Valid == false ||
                FrontDoorOpen.Valid == false || LeftDoorClosed.Valid == false || LeftDoorOpen.Valid == false ||
                RightDoorClosed.Valid == false || RightDoorOpen.Valid == false || CircuitPressurized.Valid == false)
                Anomaly = true;

            //Cockpit Lights
            //True if and only if all three gears are seen as locked in extended position
            GearsLockedDown = GearsExtended;
            //True if and only if at least one door or one gear is maneuvering, i.e. at least on edoor is not locked in closed position or one gear is not locked in retracted or extended position
            GearsManeuvering = (!FrontDoorClosed.Value || !LeftDoorClosed.Value || !RightDoorClosed.Value) ||
                               (!FrontGearExtented.Value && !FrontGearRetracted.Value) ||
                               (!LeftGearExtented.Value && !LeftGearRetracted.Value) ||
                               (!RightGearExtented.Value && !RightGearRetracted.Value);
        }

        /// <summary>
        ///   Method that is exuted on action sequence transitions going into OutgoingOne and RetractOne.
        /// </summary>
        public void One()
        {
            //Reverse
            CloseEV = false;

            //Step 1
            GeneralEV = true;
            //Step 2
            OpenEV = true;
        }

        /// <summary>
        ///   Method that is executed on actionsequence transitions going into OutgoingTwo.
        /// </summary>
        public void OutgoingTwo()
        {
            //Reverse
            RetractEV = false;

            //Step 3
            ExtendEV = true;
        }

        /// <summary>
        ///   Method that is executed on actionsequence transitions going into RetractTwo.
        /// </summary>
        public void RetractionTwo()
        {
            //Reverse
            ExtendEV = false;

            //Step 3
            RetractEV = true;
        }

        /// <summary>
        ///   Method that is executed on actionsequence transitions going into OutgoingThree.
        /// </summary>
        public void OutgoingThree()
        {
            //Step 4
            ExtendEV = false;
        }

        /// <summary>
        ///   Method that is executed on actionsequence transitions going into RetractThree.
        /// </summary>
        public void RetractionThree()
        {
            //Step 4
            RetractEV = false;
        }

        /// <summary>
        ///   Method that is executed on actionsequence transitions going into OutoingFour and RetractFour.
        /// </summary>
        public void Four()
        {
            //Step 5
            OpenEV = false;
            //Step 6
            CloseEV = true;
        }

        /// <summary>
        ///   Method that is executed on actionsequence transitions going into WaitOutgoing and WaitRetract.
        /// </summary>
        public void Zero()
        {
            //Step 7
            CloseEV = false;
            //Step 8
            GeneralEV = false;
        }
    }
}
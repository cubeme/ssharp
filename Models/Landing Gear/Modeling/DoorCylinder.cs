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
    using SafetySharp.Modeling;

    public class DoorCylinder : Cylinder
    {
        /// <summary>
        ///   Latchin box locking the door cylinder in closed position.
        /// </summary>
        private readonly LatchingBox _latchingBoxClosedOne;

        /// <summary>
        ///   Latchin box locking the door cylinder in closed position.
        /// </summary>
        private readonly LatchingBox _latchingBoxClosedTwo;

        /// <summary>
        ///   Gets the state machine that manages the state of the door cylinder.
        /// </summary>
        private readonly StateMachine<DoorStates> _stateMachine = DoorStates.LockedClosed;

        /// <summary>
        ///   The fault keeps the door cylinder stuck in a certain state.
        /// </summary>
        public readonly Fault DoorCylinderIsStuckFault = new PermanentFault();

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="position">Indicates the position of the clyinder (front, left, right).</param>
        public DoorCylinder(Position position)
            : base(position)
        {
            DoorCylinderIsStuckFault.Name = $"{Position}DoorCylinderIsStuck";
            _latchingBoxClosedOne = new LatchingBox(4, 3, $"{Position}DoorCylinderLatchingBoxClosedOne");
            _latchingBoxClosedTwo = new LatchingBox(4, 3, $"{Position}DoorCylinderLatchingBoxClosedTwo");
        }

        /// <summary>
        ///   Gets the current state of the door cylinder.
        /// </summary>
        public DoorStates DoorCylinderState => _stateMachine.State;

        /// <summary>
        ///   Updates the DoorCylinder instance.
        /// </summary>
        public override void Update()
        {
            Update(Timer, _latchingBoxClosedOne, _latchingBoxClosedTwo);

            _stateMachine
                .Transition(
                    @from: new[] { DoorStates.LockedClosed, DoorStates.LockingClosed },
                    to: DoorStates.UnlockingClosed,
                    guard: ExtensionCircuitIsPressurized,
                    action: () =>
                    {
                        _latchingBoxClosedOne.Unlock();
                        _latchingBoxClosedTwo.Unlock();
                    })
                .Transition(
                    @from: DoorStates.UnlockingClosed,
                    to: DoorStates.MoveOpening,
                    guard:
                        ExtensionCircuitIsPressurized && _latchingBoxClosedOne.IsUnlocked &&
                        _latchingBoxClosedTwo.IsUnlocked,
                    action: () => { Timer.Start(Position == Position.Front ? 12 : 15); })
                .Transition(
                    @from: DoorStates.MoveOpening,
                    to: DoorStates.Open,
                    guard: ExtensionCircuitIsPressurized && Timer.HasElapsed)
                .Transition(
                    @from: DoorStates.Open,
                    to: DoorStates.Open,
                    guard: ExtensionCircuitIsPressurized)
                .Transition(
                    @from: new[] { DoorStates.Open, DoorStates.OpenLoose },
                    to: DoorStates.MoveClosing,
                    guard: RetractionCircuitIsPressurized,
                    action: () => { Timer.Start(Position == Position.Front ? 12 : 16); })
                .Transition(
                    @from: DoorStates.Open,
                    to: DoorStates.OpenLoose,
                    guard: RetractionCircuitIsPressurized == false && ExtensionCircuitIsPressurized == false)
                .Transition(
                    @from: DoorStates.OpenLoose,
                    to: DoorStates.Open,
                    guard: ExtensionCircuitIsPressurized)
                .Transition(
                    @from: DoorStates.MoveClosing,
                    to: DoorStates.LockingClosed,
                    guard: RetractionCircuitIsPressurized && Timer.HasElapsed,
                    action: () =>
                    {
                        _latchingBoxClosedOne.Lock();
                        _latchingBoxClosedTwo.Lock();
                    })
                .Transition(
                    @from: DoorStates.LockingClosed,
                    to: DoorStates.LockedClosed,
                    guard:
                        RetractionCircuitIsPressurized && _latchingBoxClosedOne.IsLocked &&
                        _latchingBoxClosedTwo.IsLocked)
                .Transition(
                    @from: DoorStates.MoveClosing,
                    to: DoorStates.MoveOpening,
                    guard: ExtensionCircuitIsPressurized,
                    action:
                        () =>
                        {
                            Timer.Start(Position == Position.Front ? 12 - Timer.RemainingTime : 15 - (15 * Timer.RemainingTime) / 16);
                        })
                .Transition(
                    @from: DoorStates.MoveOpening,
                    to: DoorStates.MoveClosing,
                    guard: RetractionCircuitIsPressurized,
                    action:
                        () =>
                        {
                            Timer.Start(Position == Position.Front ? 12 - Timer.RemainingTime : 16 - (16 * Timer.RemainingTime) / 15);
                        })
                .Transition(
                    @from: DoorStates.UnlockingClosed,
                    to: DoorStates.LockingClosed,
                    guard: RetractionCircuitIsPressurized,
                    action: () =>
                    {
                        _latchingBoxClosedOne.Lock();
                        _latchingBoxClosedTwo.Lock();
                    });
        }

        /// <summary>
        ///   Keeps the door cylinder stuck in one state.
        /// </summary>
        [FaultEffect(Fault = nameof(DoorCylinderIsStuckFault))]
        public class DoorCylinderIsStuckFaultEffect : DoorCylinder
        {
            public DoorCylinderIsStuckFaultEffect(Position position)
                : base(position)
            {
            }

            public override void Update()
            {
                Update(Timer, _latchingBoxClosedOne, _latchingBoxClosedTwo);

                //no statemachine transtiions
            }
        }
    }
}
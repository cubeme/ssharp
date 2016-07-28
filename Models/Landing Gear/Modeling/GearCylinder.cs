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

    public class GearCylinder : Cylinder
    {
        /// <summary>
        ///   Latching box locking the gear cylinder in extended position.
        /// </summary>
        private readonly LatchingBox _latchingBoxExtended;

        /// <summary>
        ///   Latching box locking the gear cylinder in retracted position.
        /// </summary>
        private readonly LatchingBox _latchingBoxRetracted;

        /// <summary>
        ///   Gets the state machine that manages the state of the gear cylinder.
        /// </summary>
        private readonly StateMachine<GearStates> _stateMachine;

        /// <summary>
        ///   The fault keeps the gear cylinder stuck in a certain state.
        /// </summary>
        public readonly Fault GearCylinderIsStuckFault = new PermanentFault();

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="position">Indicates the position of the gear cylinder (front, left, right).</param>
        /// <param name="startState">Indicates the initital state of the gear cylinder.</param>
        public GearCylinder(Position position, GearStates startState)
            : base(position)
        {
            _stateMachine = startState;
            GearCylinderIsStuckFault.Name = $"{Position}GearCylinderIsStuck";

            _latchingBoxExtended = new LatchingBox(8, 4, $"{Position}GearCylinderLatchingBoxExtended");
            _latchingBoxRetracted = new LatchingBox(8, 4, $"{Position}GearCylinderLatchingBoxRetracted");
        }

        /// <summary>
        ///   Gets the current state of the gear cylinder.
        /// </summary>
        public GearStates GearCylinderState => _stateMachine.State;

        /// <summary>
        ///   Updates the GearCylnder instance.
        /// </summary>
        public override void Update()
        {
            Update(Timer, _latchingBoxExtended, _latchingBoxRetracted);

            _stateMachine
                .Transition(
                    @from: new[] { GearStates.LockedExtended, GearStates.LockingExtended },
                    to: GearStates.UnlockingExtended,
                    guard: RetractionCurcuitIsPressurized,
                    action: () => { _latchingBoxExtended.Unlock(); })
                .Transition(
                    @from: GearStates.UnlockingExtended,
                    to: GearStates.MoveRetracting,
                    guard: RetractionCurcuitIsPressurized && _latchingBoxExtended.IsUnlocked,
                    action: () => { Timer.Start(Position == Position.Front ? 16 : 20); })
                .Transition(
                    @from: GearStates.MoveRetracting,
                    to: GearStates.LockingRetracted,
                    guard: RetractionCurcuitIsPressurized && Timer.HasElapsed,
                    action: () => { _latchingBoxRetracted.Lock(); })
                .Transition(
                    @from: GearStates.LockingRetracted,
                    to: GearStates.LockedRetracted,
                    guard: RetractionCurcuitIsPressurized && _latchingBoxRetracted.IsLocked)
                .Transition(
                    @from: new[] { GearStates.LockedRetracted, GearStates.LockingRetracted },
                    to: GearStates.UnlockingRetracted,
                    guard: ExtensionCircuitIsPressurized,
                    action: () => { _latchingBoxRetracted.Unlock(); })
                .Transition(
                    @from: GearStates.UnlockingRetracted,
                    to: GearStates.MoveExtending,
                    guard: ExtensionCircuitIsPressurized && _latchingBoxRetracted.IsUnlocked,
                    action: () => { Timer.Start(Position == Position.Front ? 12 : 16); })
                .Transition(
                    @from: GearStates.MoveExtending,
                    to: GearStates.LockingExtended,
                    guard: ExtensionCircuitIsPressurized && Timer.HasElapsed,
                    action: () => { _latchingBoxExtended.Lock(); })
                .Transition(
                    @from: GearStates.LockingExtended,
                    to: GearStates.LockedExtended,
                    guard: ExtensionCircuitIsPressurized && _latchingBoxExtended.IsLocked)
                .Transition(
                    @from: GearStates.UnlockingExtended,
                    to: GearStates.LockingExtended,
                    guard: ExtensionCircuitIsPressurized,
                    action: () => { _latchingBoxExtended.Lock(); })
                .Transition(
                    @from: GearStates.MoveRetracting,
                    to: GearStates.MoveExtending,
                    guard: ExtensionCircuitIsPressurized,
                    action:
                        () =>
                        {
                            Timer.Start(Position == Position.Front ? 12 - (3 * Timer.RemainingTime) / 4 : 16 - (4 * Timer.RemainingTime) / 5);
                        })
                .Transition(
                    @from: GearStates.MoveExtending,
                    to: GearStates.MoveRetracting,
                    guard: RetractionCurcuitIsPressurized,
                    action:
                        () =>
                        {
                            Timer.Start(Position == Position.Front ? 16 - (4 * Timer.RemainingTime) / 3 : 20 - (5 * Timer.RemainingTime) / 4);
                        })
                .Transition(
                    @from: GearStates.UnlockingRetracted,
                    to: GearStates.LockingRetracted,
                    guard: RetractionCurcuitIsPressurized,
                    action: () => { _latchingBoxRetracted.Lock(); });
        }

        /// <summary>
        ///   Keeps the gear cylinder stuck in one state.
        /// </summary>
        [FaultEffect(Fault = nameof(GearCylinderIsStuckFault))]
        public class GearCylinderIsStuckFaultEffect : GearCylinder
        {
            public GearCylinderIsStuckFaultEffect(Position position, GearStates start)
                : base(position, start)
            {
            }

            public override void Update()
            {
                Update(Timer, _latchingBoxExtended, _latchingBoxRetracted);

                //no statemachine transtiions
            }
        }
    }
}
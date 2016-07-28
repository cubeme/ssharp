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

    /// <summary>
    ///   Describes the state of the gear.
    /// </summary>
    public enum GearStates
    {
        /// <summary>
        ///   Position indicating the gear is locked in extended position.
        /// </summary>
        LockedExtended,

        /// <summary>
        ///   State indicating the gear is being unlocked in the extended position.
        /// </summary>
        UnlockingExtended,

        /// <summary>
        ///   State indicating the gear is moving to retract.
        /// </summary>
        MoveRetracting,

        /// <summary>
        ///   State indicating the gear is being locked in retracted position.
        /// </summary>
        LockingRetracted,

        /// <summary>
        ///   State indicating the gear is locked in retracted position.
        /// </summary>
        LockedRetracted,

        /// <summary>
        ///   State indicating the gear is being unlocked in the retracted position
        /// </summary>
        UnlockingRetracted,

        /// <summary>
        ///   State indicating the gear is moving to extend.
        /// </summary>
        MoveExtending,

        /// <summary>
        ///   State indicating the gear is being locked in extended position.
        /// </summary>
        LockingExtended
    }

    public class Gear : Component
    {
        /// <summary>
        ///   The fault keeps the gear stuck in a certain state.
        /// </summary>
        public readonly Fault GearIsStuckFault = new PermanentFault();

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="position">The position the gear is located at on the airplane.</param>
        /// <param name="startState">Indicates the state the gear is in when the simulation is started.</param>
        public Gear(Position position, GearStates startState)
        {
            Position = position;
            State = startState;
            GearIsStuckFault.Name = $"{Position}GearIsStuck";
        }

        /// <summary>
        ///   Indicates the position of the gear, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public Position Position { get; }

        /// <summary>
        ///   Indicates the current state of the gears.
        /// </summary>
        public GearStates State { get; private set; }

        /// <summary>
        ///   Gets a value indicating which state the gear cylinder is currently in.
        /// </summary>
        public extern GearStates GearCylinderState { get; }

        /// <summary>
        ///   Gets a value indicating whether the gear is locked in extended position.
        /// </summary>
        public bool GearIsExtended => State == GearStates.LockedExtended;

        /// <summary>
        ///   Gets a value indicating whether the gear is locked in retracted position.
        /// </summary>
        public bool GearIsRetracted => State == GearStates.LockedRetracted;

        /// <summary>
        ///   Updates the Gear instance.
        /// </summary>
        public override void Update()
        {
            State = GearCylinderState;
        }

        /// <summary>
        ///   Keeps the gear stuck in one state.
        /// </summary>
        [FaultEffect(Fault = nameof(GearIsStuckFault))]
        public class GearIsStuckFaultEffect : Gear
        {
            public GearIsStuckFaultEffect(Position position, GearStates start)
                : base(position, start)
            {
            }

            public override void Update()
            {
            }
        }
    }
}
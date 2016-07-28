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
    ///   Describes the state of the door.
    /// </summary>
    public enum DoorStates
    {
        /// <summary>
        ///   State indicating the door is the in open position while pressure is being applied.
        /// </summary>
        Open,

        /// <summary>
        ///   State indicating the door is the in open position while no pressure is being applied.
        /// </summary>
        OpenLoose,

        /// <summary>
        ///   State indicating the door is moving to close.
        /// </summary>
        MoveClosing,

        /// <summary>
        ///   State indicating the door is being locked in closed position.
        /// </summary>
        LockingClosed,

        /// <summary>
        ///   State indicating the door is locked in closed position.
        /// </summary>
        LockedClosed,

        /// <summary>
        ///   State indicating the door is being unlocked in closed position.
        /// </summary>
        UnlockingClosed,

        /// <summary>
        ///   State indicating the door is moving to open.
        /// </summary>
        MoveOpening
    }

    /// <summary>
    ///   Describes the position of the door.
    /// </summary>
    public enum Position
    {
        /// <summary>
        ///   Position indicating the door is located in the front of the airplane.
        /// </summary>
        Front,

        /// <summary>
        ///   Position indicating the door is located on the left side of the airplane.
        /// </summary>
        Left,

        /// <summary>
        ///   Position indicating the door is located on the right side of the airplane.
        /// </summary>
        Right
    }

    public class Door : Component
    {
        /// <summary>
        ///   The fault keeps the door stuck in a certain state.
        /// </summary>
        public readonly Fault DoorIsStuckFault = new PermanentFault();

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// ///
        /// <param name="position">The position the door is located at on the airplane.</param>
        public Door(Position position)
        {
            Position = position;
            DoorIsStuckFault.Name = $"{Position}DoorIsStuck";
        }

        /// <summary>
        ///   Indicates the position of the door, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public Position Position { get; }

        /// <summary>
        ///   Indicates the current state of the doors.
        /// </summary>
        public DoorStates State { get; private set; } = DoorStates.LockedClosed;

        /// <summary>
        ///   Gets a value indicating which state the door cylinder is currently in.
        /// </summary>
        public extern DoorStates DoorCylinderState { get; }

        /// <summary>
        ///   Gets a value indicating whether the door is open.
        /// </summary>
        public bool DoorIsOpen => State == DoorStates.Open;

        /// <summary>
        ///   Gets a value indicating whether the door is closed.
        /// </summary>
        public bool DoorIsClosed => State == DoorStates.LockedClosed;

        /// <summary>
        ///   Updated the Door instance.
        /// </summary>
        public override void Update()
        {
            State = DoorCylinderState;
        }

        /// <summary>
        ///   Keeps the door stuck in one state.
        /// </summary>
        [FaultEffect(Fault = nameof(DoorIsStuckFault))]
        public class DoorIsStuckFaultEffect : Door
        {
            public DoorIsStuckFaultEffect(Position position)
                : base(position)
            {
            }

            public override void Update()
            {
            }
        }
    }
}
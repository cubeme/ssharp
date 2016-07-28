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

    public class MechanicalPartActuators : Component
    {
        /// <summary>
        ///   Instance of DoorCylinder representing the front door cylinder.
        /// </summary>
        public readonly DoorCylinder FrontDoorCylinder = new DoorCylinder(Position.Front);

        /// <summary>
        ///   Instance of GearCylinder representing the front gear cylinder.
        /// </summary>
        public readonly GearCylinder FrontGearCylinder;

        /// <summary>
        ///   Instance of DoorCylinder representing the right door cylinder.
        /// </summary>
        public readonly DoorCylinder LeftDoorCylinder = new DoorCylinder(Position.Left);

        /// <summary>
        ///   Instance of GearCylinder representing the left gear cylinder.
        /// </summary>
        public readonly GearCylinder LeftGearCylinder;

        /// <summary>
        ///   Instance of DoorCylinder representing the left door cylinder.
        /// </summary>
        public readonly DoorCylinder RightDoorCylinder = new DoorCylinder(Position.Right);

        /// <summary>
        ///   Instance of GearCylinder representing the right gear cylinder.
        /// </summary>
        public readonly GearCylinder RightGearCylinder;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="startState">Indicates the initital states of the gear cylinder state machines. </param>
        public MechanicalPartActuators(GearStates startState)
        {
            FrontGearCylinder = new GearCylinder(Position.Front, startState);
            LeftGearCylinder = new GearCylinder(Position.Left, startState);
            RightGearCylinder = new GearCylinder(Position.Right, startState);
        }

        /// <summary>
        ///   Updates the MechanicalPartActuartors instance.
        /// </summary>
        public override void Update()
        {
            Update(FrontDoorCylinder, LeftDoorCylinder, RightDoorCylinder, FrontGearCylinder, LeftGearCylinder, RightGearCylinder);
        }
    }
}
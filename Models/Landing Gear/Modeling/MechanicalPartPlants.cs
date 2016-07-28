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

    public class MechanicalPartPlants : Component
    {
        /// <summary>
        ///   Instance of Door representing the front door.
        /// </summary>
        public readonly Door DoorFront = new Door(Position.Front);

        /// <summary>
        ///   Instance of Door representing the left door.
        /// </summary>
        public readonly Door DoorLeft = new Door(Position.Left);

        /// <summary>
        ///   Instance of Door representing the right door.
        /// </summary>
        public readonly Door DoorRight = new Door(Position.Right);

        /// <summary>
        ///   Instance of Gear representing the front gear.
        /// </summary>
        public readonly Gear GearFront;

        /// <summary>
        ///   Instance of Gear representing the left gear.
        /// </summary>
        public readonly Gear GearLeft;

        /// <summary>
        ///   Instance of Gear representing the right gear.
        /// </summary>
        public readonly Gear GearRight;

        /// <summary>
        ///   Instance of MechanicalPartActuators
        /// </summary>
        public MechanicalPartActuators Actuators;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="startState">Indicates the initial state of the gears.</param>
        public MechanicalPartPlants(GearStates startState)
        {
            GearFront = new Gear(Position.Front, startState);
            GearLeft = new Gear(Position.Left, startState);
            GearRight = new Gear(Position.Right, startState);
        }

        /// <summary>
        ///   Updates the MechanicalPartPlants instance.
        /// </summary>
        public override void Update()
        {
            Update(Actuators, DoorFront, DoorLeft, DoorRight, GearFront, GearLeft, GearRight);
        }
    }
}
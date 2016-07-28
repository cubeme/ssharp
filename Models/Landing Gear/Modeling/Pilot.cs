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
    ///   Describes the position of the pilot handle.
    /// </summary>
    public enum HandlePosition
    {
        /// <summary>
        ///   Position indicating the pilot handle is switched to up.
        /// </summary>
        Down,

        /// <summary>
        ///   Position indicating the pilot handle is switched to down.
        /// </summary>
        Up
    }

    public class Pilot : Component
    {
        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="startPosition">Indicates the initial position of the pilot handle.</param>
        public Pilot(HandlePosition startPosition)
        {
            Position = startPosition;
        }

        /// <summary>
        ///   The instance of the pilot handle being controlled by the pilot.
        /// </summary>
        public Cockpit Cockpit { get; set; }

        /// <summary>
        ///   Getas a value indicating which position the pilot handle is to move to.
        /// </summary>
        [Hidden]
        public HandlePosition Move { private get; set; }

        /// <summary>
        ///   Gets the current handle position.
        /// </summary>
        public HandlePosition Position { get; private set; }

        /// <summary>
        ///   Updates the Pilot instance.
        /// </summary>
        public override void Update()
        {
            Update(Cockpit);

            var oldPosition = Position;
            Position = Move;

            if (oldPosition == Position)
                return;

            //Set PilotHandle to new position.
            Cockpit.PilotHandle.Position = Position;
            Cockpit.PilotHandle.Moved();
        }
    }
}
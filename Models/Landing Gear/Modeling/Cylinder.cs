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

    public class Cylinder : Component
    {
        /// <summary>
        ///   Timer to time the movement of the cylinders.
        /// </summary>
        protected readonly Timer Timer = new Timer();

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// ///
        /// <param name="position">The position the cylinder is located at on the airplane.</param>
        public Cylinder(Position position)
        {
            Position = position;
        }

        /// <summary>
        ///   Indicates the type of the cylinder, i.e. whether it is located in the front, on the left or right side of the plane.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether the retraction circuit is pressurized.
        /// </summary>
        public extern bool RetractionCircuitIsPressurized { get; }

        /// <summary>
        ///   Gets a value indictaing whether the extension circuit is pressurized.
        /// </summary>
        public extern bool ExtensionCircuitIsPressurized { get; }
    }
}
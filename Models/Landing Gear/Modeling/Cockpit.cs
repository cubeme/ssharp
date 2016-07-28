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

    public class Cockpit : Component
    {
        /// <summary>
        ///   Cockpit light indicating the gears are locked down.
        /// </summary>
        public readonly Light GreenLight = new Light();

        /// <summary>
        ///   Cockpit light indicating the gears are maneuvering.
        /// </summary>
        public readonly Light OrangeLight = new Light();

        /// <summary>
        ///   The instance of the pilot handle which is controlled by the pilot.
        /// </summary>
        public readonly PilotHandle PilotHandle = new PilotHandle();

        /// <summary>
        ///   Cockpit light indicating an anomaly has been detected.
        /// </summary>
        public readonly Light RedLight = new Light();

        /// <summary>
        ///   Updates the Cockpit instance.
        /// </summary>
        public override void Update()
        {
            Update(PilotHandle, GreenLight, OrangeLight, RedLight);
        }
    }
}
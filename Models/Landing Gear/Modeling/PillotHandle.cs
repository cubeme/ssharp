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

    public class PilotHandle : Component
    {
        /// <summary>
        ///   The fault keeps the handle stuck in the down position.
        /// </summary>
        public readonly Fault HandleDownFault = new PermanentFault();

        /// <summary>
        ///   The fault keeps the handle stuck in the up position.
        /// </summary>
        public readonly Fault HandleUpFault = new PermanentFault();

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        public PilotHandle()
        {
            HandleUpFault.Name = "PilotHandleIsStuckUp";
            HandleDownFault.Name = "PilotHandleIsStuckDown";
        }

        /// <summary>
        ///   Gets the current position of the pilot handle.
        /// </summary>
        public virtual HandlePosition Position { get; set; }

        /// <summary>
        ///   Indicates whether the pilot handle has been moved.
        /// </summary>
        public extern void Moved();

        /// <summary>
        ///   Keeps the pilot handle stuck in the up position.
        /// </summary>
        [FaultEffect(Fault = nameof(HandleUpFault))]
        public class HandleUpFaultEffect : PilotHandle
        {
            public override HandlePosition Position => HandlePosition.Up;
        }

        /// <summary>
        ///   Keeps the pilot handle stuck in the down position.
        /// </summary>
        [FaultEffect(Fault = nameof(HandleDownFault))]
        public class HandleDownFaultEffect : PilotHandle
        {
            public override HandlePosition Position => HandlePosition.Down;
        }
    }
}
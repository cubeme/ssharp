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

    public class TripleSensor<TSensorType> : Component
    {
        /// <summary>
        ///   Indicates whether the micro sensor on channel one is valid.
        /// </summary>
        private bool _validOne = true;

        /// <summary>
        ///   Indicates whether the micro sensor on channel three is valid.
        /// </summary>
        private bool _validThree = true;

        /// <summary>
        ///   Indicates whether the micro sensor on channel two is valid.
        /// </summary>
        private bool _validTwo = true;

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        public TripleSensor(string type)
        {
            Valid = true;
            Value = default(TSensorType);
            for (var i = 0; i < 3; i++)
            {
                Sensors[i] = new Sensor<TSensorType>(type);
            }
        }

        /// <summary>
        ///   Array of three micro sensors the triple sensor is made up out of.
        /// </summary>
        [Hidden(HideElements = true)]
        public Sensor<TSensorType>[] Sensors { get; } = new Sensor<TSensorType>[3];

        /// <summary>
        ///   Indicates whether the sensor is valid.
        /// </summary>
        public bool Valid { get; private set; }

        /// <summary>
        ///   Indicates the value of the sensor.
        /// </summary>
        public TSensorType Value { get; private set; }

        /// <summary>
        ///   Updates the TripleSensor instance.
        /// </summary>
        public override void Update()
        {
            var channel0 = Sensors[0].Value;
            var channel1 = Sensors[1].Value;
            var channel2 = Sensors[2].Value;

            //  All channels are valid.
            if (_validOne && _validTwo && _validThree)
            {
                if (channel0.Equals(channel1) && channel1.Equals(channel2))
                {
                    Value = channel0;
                }
                else if (channel0.Equals(channel1) && !channel1.Equals(channel2))
                {
                    _validThree = false;
                    Value = channel0;
                }

                else if (channel0.Equals(channel2) && !channel0.Equals(channel1))
                {
                    _validTwo = false;
                    Value = channel0;
                }

                else if (channel1.Equals(channel2) && !channel1.Equals(channel0))
                {
                    _validOne = false;
                    Value = channel1;
                }
                else //all are different
                {
                    Valid = false;
                    Value = default(TSensorType);
                }
            }

            // Only channels one and two are valid.
            else if (_validOne && _validTwo)
            {
                channel0 = Sensors[0].Value;
                channel1 = Sensors[1].Value;

                if (channel0.Equals(channel1))
                    Value = channel0;

                else //if (!channel0.Equals(channel1))
                {
                    Valid = false;
                    Value = default(TSensorType);
                }
            }
            // Only channels one and three are valid.
            else if (_validOne && _validThree)
            {
                channel0 = Sensors[0].Value;
                channel2 = Sensors[2].Value;

                if (channel0.Equals(channel2))
                    Value = channel0;

                else //if (!channel0.Equals(channel2))
                {
                    Valid = false;
                    Value = default(TSensorType);
                }
            }

            // Only channels two and three are valid.
            else if (_validTwo && _validThree)
            {
                channel1 = Sensors[1].Value;
                channel2 = Sensors[2].Value;

                if (channel1.Equals(channel2))
                    Value = Value = channel1;

                else //if (!channel1.Equals(channel2))
                {
                    Valid = false;
                    Value = default(TSensorType);
                }
            }

            // Alls channels are imvalid.
            else
                Value = default(TSensorType);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Class with channel and validity of the channel.
    /// </summary>
    class ChannelIsValidPair<T>
    {
        public bool Valid = true;
    }

    class TripleSensor<SensorType> : Component
    {
        /// <summary>
        /// Array of three micro sensors the triple sensor is made up out of.
        /// </summary>
        [Hidden(HideElements = true)]
        public Sensor<SensorType>[] Sensors { get; } = new Sensor<SensorType>[3];

		/// <summary>
		/// Array with value for computing and validity of all three micro sensors.
		/// </summary>
		[Hidden(HideElements = true)]
		private readonly ChannelIsValidPair<SensorType>[] _channels = new ChannelIsValidPair<SensorType>[3];       

        /// <summary>
        /// Indicates whether the sensor is valid.
        /// </summary>
        public bool Valid { get; private set; }

        /// <summary>
        /// Indicates the value of the sensor.
        /// </summary>
        public SensorType Value { get; private set; }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TripleSensor()
        {
            Valid = true;
            Value = default(SensorType);
            for (int i = 0; i < 3; i++)
            {
                Sensors[i] = new Sensor<SensorType>();
                _channels[i] = new ChannelIsValidPair<SensorType>();
            }
        }

        public override void Update()
        {
			var channel0 = Sensors[0].Value;
			var channel1 = Sensors[1].Value;
			var channel2 = Sensors[2].Value;

			/// <summary>
			///  All channels are valid.
			/// </summary
			if (_channels[0].Valid && _channels[1].Valid && _channels[2].Valid)
            {
              

                if (channel0.Equals(channel1) && channel1.Equals(channel2))

                    Value = channel0;

                else if (channel0.Equals(channel1) && !channel1.Equals(channel2))
                {
                    _channels[2].Valid = false;
                    Value = channel0;
                }

                else if (channel0.Equals(channel2) && !channel0.Equals(channel1))
                {
                    _channels[1].Valid = false;
                    Value = channel0;
                }

                else if (channel1.Equals(channel2) && !channel1.Equals(channel0))
                {
                    _channels[0].Valid = false;
                    Value = channel1;
                }
                else //all are different
                {
                    Valid = false;
                    Value = default(SensorType);
                }
            }
            /// <summary>
            /// Only channels one and two are valid.
            /// </summary
            else if (_channels[0].Valid && _channels[1].Valid)
            {
                channel0 = Sensors[0].Value;
                channel1 = Sensors[1].Value;

                if (channel0.Equals(channel1))
                    Value = channel0;

                else //if (!channel0.Equals(channel1))
                {
                    Valid = false;
                    Value = default(SensorType);
                }
            }
            /// <summary>
            /// Only channels one and three are valid.
            /// </summary
            else if (_channels[0].Valid && _channels[2].Valid)
            {
                channel0 = Sensors[0].Value;
                channel2 = Sensors[2].Value;

                if (channel0.Equals(channel2))
                    Value = channel0;

                else //if (!channel0.Equals(channel2))
                {
                    Valid = false;
                    Value = default(SensorType);
                }
            }
            /// <summary>
            /// Only channels two and three are valid.
            /// </summary
            else if (_channels[1].Valid && _channels[2].Valid)
            {
                channel1 = Sensors[1].Value;
                channel2 = Sensors[2].Value;

                if (channel1.Equals(channel2))
                    Value = Value = channel1;

                else //if (!channel1.Equals(channel2))
                {
                    Valid = false;
                    Value = default(SensorType);
                }
            }
            /// <summary>
            /// Alls channelsare valid.
            /// </summary
            else
                Value = default(SensorType);
        }

    }
}

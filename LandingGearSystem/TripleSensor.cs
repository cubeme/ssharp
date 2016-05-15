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
        public T Channel = default(T);
        public bool Valid = true;
    }

    class TripleSensor<SensorType> : Component
    {
        /// <summary>
        /// Array of three micro sensors the triple sensor is made up out of.
        /// </summary>
        public Sensor<SensorType>[] Sensors { get; private set; } = new Sensor<SensorType>[3];

        /// <summary>
        /// Array with value for computing and validity of all three micro sensors.
        /// </summary>
        private ChannelIsValidPair<SensorType>[] _channels = new ChannelIsValidPair<SensorType>[3];       

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
            for (int i = 3; i < 3; i++)
            {
                Sensors[i] = new Sensor<SensorType>();
                _channels[i] = new ChannelIsValidPair<SensorType>();
            }
        }

        public override void Update()
        {
            /// <summary>
            ///  All channels are valid.
            /// </summary
            if (_channels[0].Valid && _channels[1].Valid && _channels[2].Valid)
            {
                _channels[0].Channel = Sensors[0].Value;
                _channels[1].Channel = Sensors[1].Value;
                _channels[2].Channel = Sensors[2].Value;

                if (_channels[0].Channel.Equals(_channels[1].Channel) && _channels[1].Channel.Equals(_channels[2].Channel))

                    Value = _channels[0].Channel;

                else if (_channels[0].Channel.Equals(_channels[1].Channel) && !_channels[1].Channel.Equals(_channels[2].Channel))
                {
                    _channels[2].Valid = false;
                    Value = _channels[0].Channel;
                }

                else if (_channels[0].Channel.Equals(_channels[2].Channel) && !_channels[0].Channel.Equals(_channels[1].Channel))
                {
                    _channels[1].Valid = false;
                    Value = _channels[0].Channel;
                }

                else if (_channels[1].Channel.Equals(_channels[2].Channel) && !_channels[1].Channel.Equals(_channels[0].Channel))
                {
                    _channels[0].Valid = false;
                    Value = _channels[1].Channel;
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
                _channels[0].Channel = Sensors[0].Value;
                _channels[1].Channel = Sensors[1].Value;

                if (_channels[0].Channel.Equals(_channels[1].Channel))
                    Value = _channels[0].Channel;

                else //if (!_channels[0].Channel.Equals(_channels[1].Channel))
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
                _channels[0].Channel = Sensors[0].Value;
                _channels[2].Channel = Sensors[2].Value;

                if (_channels[0].Channel.Equals(_channels[2].Channel))
                    Value = _channels[0].Channel;

                else //if (!_channels[0].Channel.Equals(_channels[2].Channel))
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
                _channels[1].Channel = Sensors[1].Value;
                _channels[2].Channel = Sensors[2].Value;

                if (_channels[1].Channel.Equals(_channels[2].Channel))
                    Value = Value = _channels[1].Channel;

                else //if (!_channels[1].Channel.Equals(_channels[2].Channel))
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

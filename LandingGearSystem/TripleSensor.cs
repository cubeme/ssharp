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
        /// Array with value and validity of all thre micro sensors.
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
        }

        /// <summary>
        /// Gets the sensor value of the micro sensor in channel one.
        /// </summary>
        public extern SensorType ChannelOne();
        /// <summary>
        /// Gets the sensor value of the micro sensor in channel two.
        /// </summary>
        public extern SensorType ChannelTwo();
        /// <summary>
        /// Gets the sensor value of the micro sensor in channel three.
        /// </summary>
        public extern SensorType ChannelThree();

        public override void Update()
        {
            /// <summary>
            ///  All channels are valid.
            /// </summary
            if (_channels[1].Valid && _channels[2].Valid && _channels[3].Valid)
            {
                _channels[1].Channel = ChannelOne();
                _channels[2].Channel = ChannelTwo();
                _channels[3].Channel = ChannelThree();

                if (_channels[1].Channel.Equals(_channels[2].Channel) && _channels[2].Channel.Equals(_channels[3].Channel))

                    Value = _channels[1].Channel;

                else if (_channels[1].Channel.Equals(_channels[2].Channel) && !_channels[2].Channel.Equals(_channels[3].Channel))
                {
                    _channels[3].Valid = false;
                    Value = _channels[1].Channel;
                }

                else if (_channels[1].Channel.Equals(_channels[3].Channel) && !_channels[1].Channel.Equals(_channels[2].Channel))
                {
                    _channels[2].Valid = false;
                    Value = _channels[1].Channel;
                }

                else if (_channels[2].Channel.Equals(_channels[3].Channel) && !_channels[2].Channel.Equals(_channels[1].Channel))
                {
                    _channels[1].Valid = false;
                    Value = _channels[2].Channel;
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
            else if (_channels[1].Valid && _channels[2].Valid)
            {
                _channels[1].Channel = ChannelOne();
                _channels[2].Channel = ChannelTwo();

                if (_channels[1].Channel.Equals(_channels[2].Channel))
                    Value = _channels[1].Channel;

                else //if (!_channels[1].Channel.Equals(_channels[2].Channel))
                {
                    Valid = false;
                    Value = default(SensorType);
                }
            }
            /// <summary>
            /// Only channels one and three are valid.
            /// </summary
            else if (_channels[1].Valid && _channels[3].Valid)
            {
                _channels[1].Channel = ChannelOne();
                _channels[3].Channel = ChannelThree();

                if (_channels[1].Channel.Equals(_channels[3].Channel))
                    Value = _channels[1].Channel;

                else //if (!_channels[1].Channel.Equals(_channels[3].Channel))
                {
                    Valid = false;
                    Value = default(SensorType);
                }
            }
            /// <summary>
            /// Only channels two and three are valid.
            /// </summary
            else if (_channels[2].Valid && _channels[3].Valid)
            {
                _channels[2].Channel = ChannelTwo();
                _channels[3].Channel = ChannelThree();

                if (_channels[2].Channel.Equals(_channels[3].Channel))
                    Value = Value = _channels[2].Channel;

                else //if (!_channels[2].Channel.Equals(_channels[3].Channel))
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

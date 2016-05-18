﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class AircraftHydraulicCircuit : Component
    {
        /// <summary>
        ///  Indicates the pressure of the aircraft hydraulic circuit.
        /// </summary>
        public int Pressure { get; }

        /// <summary>
        ///   Initializes a new instance.
        /// </summary>
        /// <param name="pressure">The pressure of the aircraft hydraulic circuitk.</param>
        public AircraftHydraulicCircuit(int pressure)
        {
            Pressure = pressure;
        }
    }
}

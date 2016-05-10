using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the position of the pilot handle.
    /// </summary
    public enum HandlePosition
    {
        /// <summary>
        /// Position indicating the pilot handle is switched to up.
        /// </summary>
        Down,
        /// <summary>
        /// Position indicating the pilot handle is switched to down.
        /// </summary>
        Up
    }

    class Pilot : Component
    {
        /// <summary>
        /// Nondeterministically chooses a handle position.
        /// </summary>
        private HandlePosition _position;

        /// <summary>
        /// Gets the current handle position.
        /// </summary>
        public HandlePosition GetHandlePosition() => _position;

        public override void Update()
        {
            _position = Choose(HandlePosition.Up, HandlePosition.Down);
        }
    }
}

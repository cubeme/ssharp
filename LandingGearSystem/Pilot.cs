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
        /// The instance of the pilot handle being controlled by the pilot.
        /// </summary>
        public Cockpit Cockpit { get; set;  }


        /// <summary>
        /// Remembers the old handle position to determine whether or not the handle has been moved.
        /// </summary>
        [Hidden]
        private HandlePosition _oldPosition;

        /// <summary>
        /// Gets the current handle position.
        /// </summary>
        public HandlePosition Position { get; private set; } = HandlePosition.Down;


        [Range(0, 500, OverflowBehavior.Clamp)]
	    public int f;

        public override void Update()
        {
            Update(Cockpit);

            _oldPosition = Position;
            //Position = Choose(HandlePosition.Up, HandlePosition.Down);
            Position = (HandlePosition.Up);

            if (_oldPosition != Position)
                Cockpit.PilotHandle.HasMoved();

            //if (f == 0)
            //    Position = (HandlePosition.Up);

            //if (f < 500)
                ++f;



        }
    }
}

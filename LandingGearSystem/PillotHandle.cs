using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class PilotHandle : Component
    {

        // <summary>
        ///  Indicates whether the pilot handle has been moved.
        /// </summary>
        public bool Moved { get; private set; }

        /// <summary>
        /// Gets the handle position chosen by the Pilot.
        /// </summary>
        public extern HandlePosition PilotHandlePosition();

        /// <summary>
		///   Gets the state machine that manages the state of the pilot handle.
		/// </summary>
		public readonly StateMachine<HandlePosition> StateMachine = HandlePosition.down;
    
        public override void Update()
        {
            StateMachine
                .Transition(
                    from: HandlePosition.down,
                    to: HandlePosition.up,
                    guard: PilotHandlePosition() == HandlePosition.up,
                    action: () => Moved = true)

                .Transition(
                    from: HandlePosition.down,
                    to: HandlePosition.down,
                    guard: PilotHandlePosition() == HandlePosition.down,
                    action: () => Moved = false)

                .Transition(
                    from: HandlePosition.up,
                    to: HandlePosition.down,
                    guard: PilotHandlePosition() == HandlePosition.down,
                    action: () => Moved = true)

                .Transition(
                    from: HandlePosition.up,
                    to: HandlePosition.up,
                    guard: PilotHandlePosition() == HandlePosition.up,
                    action: () => Moved = false);
                
        }
    }
}


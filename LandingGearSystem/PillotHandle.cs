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
        public extern HandlePosition GetPilotHandlePosition { get;  }

        public HandlePosition PilotHandlePosition => StateMachine.State;

        /// <summary>
		///   Gets the state machine that manages the state of the pilot handle.
		/// </summary>
		public readonly StateMachine<HandlePosition> StateMachine = HandlePosition.Down;
    
        public override void Update()
        {
            StateMachine
                .Transition(
                    from: HandlePosition.Down,
                    to: HandlePosition.Up,
                    guard: GetPilotHandlePosition == HandlePosition.Up,
                    action: () => Moved = true)

                .Transition(
                    from: HandlePosition.Down,
                    to: HandlePosition.Down,
                    guard: GetPilotHandlePosition == HandlePosition.Down,
                    action: () => Moved = false)

                .Transition(
                    from: HandlePosition.Up,
                    to: HandlePosition.Down,
                    guard: GetPilotHandlePosition == HandlePosition.Down,
                    action: () => Moved = true)

                .Transition(
                    from: HandlePosition.Up,
                    to: HandlePosition.Up,
                    guard: GetPilotHandlePosition == HandlePosition.Up,
                    action: () => Moved = false);
                
        }
    }
}


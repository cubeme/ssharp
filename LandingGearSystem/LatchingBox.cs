using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the state of the latching box.
    /// </summary>
    public enum LatchingBoxState
    {
        /// <summary>
        /// State indicating the latching box is locked.
        /// </summary>
        Locked,
        /// <summary>
        /// State indicating the latching box is unlocked.
        /// </summary>
        Unlocked,
        /// <summary>
        /// State indicating the latching box is being unlocked.
        /// </summary>
        Unlocking,
        /// <summary>
        /// State indicating the latching box is being locked.
        /// </summary>
        Locking
    }

    class LatchingBox : Component
    {
        /// <summary>
        /// Indicates the duraction of time it takes to unlock the latching box.
        /// </summary>
        public int DurationUnlock { get; }

		/// <summary>
		/// Indicates the duraction of time it takes to lock the latching box.
		/// </summary>
		public int DurationLock { get; }

        public LatchingBox(int durationUnlock, int durationLock)
        {
            DurationUnlock = durationUnlock;
            DurationLock = durationLock;
        }

        /// <summary>
		///   Gets the state machine that manages the state of the latching box.
		/// </summary>
		public readonly StateMachine<LatchingBoxState> StateMachine = LatchingBoxState.Locked;

        /// <summary>
        /// Times the unlocking/locking of the latching box.
        /// </summary>
        public readonly Timer _timer = new Timer();

       public void Unlock()
        {
            StateMachine
                .Transition(
                    from: new[] { LatchingBoxState.Locked, LatchingBoxState.Locking },
                    to: LatchingBoxState.Unlocking,
                    action: () =>
                    {
                        _timer.Start(DurationUnlock - (DurationUnlock / DurationLock) * _timer.RemainingTime);
                    });
        }

        public void Lock()
        {
            StateMachine
                .Transition(
                    from: new[] {LatchingBoxState.Unlocked, LatchingBoxState.Unlocking},
                    to: LatchingBoxState.Locking,
                    action: () =>
                    {
                        _timer.Start(DurationLock - (DurationLock/DurationUnlock)*_timer.RemainingTime);
                    });
        }

        public override void Update()
        {
	        Update(_timer);

            StateMachine

                .Transition(
                    from: LatchingBoxState.Unlocking,
                    to: LatchingBoxState.Unlocked,
                    guard: _timer.HasElapsed)

                .Transition(
                    from: LatchingBoxState.Locking,
                    to: LatchingBoxState.Locked,
                    guard: _timer.HasElapsed);

        }
    }
}

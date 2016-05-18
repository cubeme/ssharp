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

        /// <summary>
        /// Indicates whether the latching box is to be unlocked.
        /// </summary>
        public bool Unlock {  get; set; }

        /// <summary>
        /// Indicates whether the latching box is to be locked.
        /// </summary>
        public bool Lock { private get; set; }

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

        //IsLocked, IsUnlocked, Lock(){--Transition)

       public void Unlock2()
        {
            StateMachine
                .Transition(
                    from: new[] { LatchingBoxState.Locked, LatchingBoxState.Locking },
                    to: LatchingBoxState.Unlocking,
                    guard: Unlock == true,
                    action: () =>
                    {
                        _timer.Start(DurationUnlock - (DurationUnlock / DurationLock) * _timer.RemainingTime);
                    });
        }

        public override void Update()
        {
            //todo: Event, Transitionen zusammenfassen
	        Update(_timer);

            StateMachine.
                Transition(
                    from: LatchingBoxState.Locked,
                    to: LatchingBoxState.Unlocking,
                    guard: Unlock == true,
                    action: () =>
                    {
                        _timer.Start(DurationUnlock);
                    })
                .Transition(
                    from: LatchingBoxState.Unlocking,
                    to: LatchingBoxState.Unlocked,
                    guard: Unlock == true && _timer.HasElapsed,
                    action: () => Unlock = false)
                .Transition(
                    from: LatchingBoxState.Unlocked,
                    to: LatchingBoxState.Locking,
                    guard: Lock == true,
                    action: () =>
                    {
                        _timer.Start(DurationLock);
                    })
                .Transition(
                    from: LatchingBoxState.Locking,
                    to: LatchingBoxState.Locked,
                    guard: Lock == true && _timer.HasElapsed,
                    action: () => Lock = false)
                .Transition(
                    from: LatchingBoxState.Locking,
                    to: LatchingBoxState.Unlocking,
                    guard: Unlock = true && Lock == false,
                    action: () =>
                    {
                        _timer.Start(DurationUnlock - (DurationUnlock / DurationLock) * _timer.RemainingTime);
                    })
                .Transition(
                    from: LatchingBoxState.Unlocking,
                    to: LatchingBoxState.Locking,
                    guard: Lock == true && Unlock == false,
                    action: () =>
                    {
                        _timer.Start(DurationLock - (DurationLock / DurationUnlock) * _timer.RemainingTime);
                    });           
                
        }
    }
}

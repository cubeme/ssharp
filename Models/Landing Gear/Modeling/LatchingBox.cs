
namespace SafetySharp.CaseStudies.LandingGear.Modeling
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
        ///   The fault will not lock the latching box.
        /// </summary>
        public readonly Fault LatchingBoxLockFault = new PermanentFault();

        /// <summary>
        ///   The fault will not unlock the latching box.
        /// </summary>
        public readonly Fault LatchingBoxUnlockFault = new PermanentFault();

        /// <summary>
        /// Indicates the duraction of time it takes to unlock the latching box.
        /// </summary>
        public int DurationUnlock { get; }

		/// <summary>
		/// Indicates the duraction of time it takes to lock the latching box.
		/// </summary>
		public int DurationLock { get; }

        public LatchingBox(int durationUnlock, int durationLock, string type)
        {
            DurationUnlock = durationUnlock;
            DurationLock = durationLock;
            LatchingBoxLockFault.Name = $"{type}CannotLock";
            LatchingBoxUnlockFault.Name = $"{type}CannotUnLock";
        }

        /// <summary>
		///   Gets the state machine that manages the state of the latching box.
		/// </summary>
		private readonly StateMachine<LatchingBoxState> _stateMachine = LatchingBoxState.Locked;

        /// <summary>
        ///   Gets a value indicating whether the latching box is locked.
        /// </summary>
        public bool IsLocked => _stateMachine.State == LatchingBoxState.Locked;

        /// <summary>
		///   Gets a value indicating whether the latching box is unlocked.
		/// </summary>
        public bool IsUnlocked => _stateMachine.State == LatchingBoxState.Unlocked;

        /// <summary>
        /// Times the unlocking/locking of the latching box.
        /// </summary>
        private readonly Timer _timer = new Timer();

        public virtual void Unlock()
        {
            _stateMachine
                .Transition(
                    @from: new[] { LatchingBoxState.Locked, LatchingBoxState.Locking },
                    to: LatchingBoxState.Unlocking,
                    action: () =>
                    {
                        _timer.Start(DurationUnlock - (DurationUnlock / DurationLock) * _timer.RemainingTime);
                    });
        }

        public virtual void Lock()
        {
            _stateMachine
                .Transition(
                    @from: new[] {LatchingBoxState.Unlocked, LatchingBoxState.Unlocking},
                    to: LatchingBoxState.Locking,
                    action: () =>
                    {
                        _timer.Start(DurationLock - (DurationLock/DurationUnlock)*_timer.RemainingTime);
                    });
        }

        public override void Update()
        {
	        Update(_timer);

            _stateMachine

                .Transition(
                    @from: LatchingBoxState.Unlocking,
                    to: LatchingBoxState.Unlocked,
                    guard: _timer.HasElapsed)

                .Transition(
                    @from: LatchingBoxState.Locking,
                    to: LatchingBoxState.Locked,
                    guard: _timer.HasElapsed);

        }

        /// <summary>
        ///   Will not lock the latching box.
        /// </summary>
        [FaultEffect(Fault = nameof(LatchingBoxLockFault))]
        public class LatchingBoxLockFaultEffect : LatchingBox
        {
            public LatchingBoxLockFaultEffect(int dUnlock, int dLock, string type) : base(dUnlock, dLock, type) { }

            public override void Lock()
            {
            }
        }

        /// <summary>
        ///   Will not unlock the latching box.
        /// </summary>
        [FaultEffect(Fault = nameof(LatchingBoxUnlockFault))]
        public class LatchingBoxUnlockFaultEffect : LatchingBox
        {
            public LatchingBoxUnlockFaultEffect(int dUnlock, int dLock, string type) : base(dUnlock, dLock, type) { }

            public override void Unlock()
            {
            }
        }
    }
}

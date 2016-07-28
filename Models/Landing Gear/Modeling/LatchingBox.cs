// The MIT License (MIT)
// 
// Copyright (c) 2014-2016, Institute for Software & Systems Engineering
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    /// <summary>
    ///   Describes the state of the latching box.
    /// </summary>
    public enum LatchingBoxState
    {
        /// <summary>
        ///   State indicating the latching box is locked.
        /// </summary>
        Locked,

        /// <summary>
        ///   State indicating the latching box is unlocked.
        /// </summary>
        Unlocked,

        /// <summary>
        ///   State indicating the latching box is being unlocked.
        /// </summary>
        Unlocking,

        /// <summary>
        ///   State indicating the latching box is being locked.
        /// </summary>
        Locking
    }

    internal class LatchingBox : Component
    {
        /// <summary>
        ///   Gets the state machine that manages the state of the latching box.
        /// </summary>
        private readonly StateMachine<LatchingBoxState> _stateMachine = LatchingBoxState.Locked;

        /// <summary>
        ///   Times the unlocking/locking of the latching box.
        /// </summary>
        private readonly Timer _timer = new Timer();

        /// <summary>
        ///   The fault will not lock the latching box.
        /// </summary>
        public readonly Fault LatchingBoxLockFault = new PermanentFault();

        /// <summary>
        ///   The fault will not unlock the latching box.
        /// </summary>
        public readonly Fault LatchingBoxUnlockFault = new PermanentFault();

        /// <summary>
        ///   Initilaizes a new instance.
        /// </summary>
        /// <param name="durationUnlock">Indicates the duration of the unlocking process.</param>
        /// <param name="durationLock">Indicates the duration of the locking process.</param>
        /// <param name="type">Indicates the name of the latching box faults.</param>
        public LatchingBox(int durationUnlock, int durationLock, string type)
        {
            DurationUnlock = durationUnlock;
            DurationLock = durationLock;
            LatchingBoxLockFault.Name = $"{type}CannotLock";
            LatchingBoxUnlockFault.Name = $"{type}CannotUnLock";
        }

        /// <summary>
        ///   Indicates the duraction of time it takes to unlock the latching box.
        /// </summary>
        public int DurationUnlock { get; }

        /// <summary>
        ///   Indicates the duraction of time it takes to lock the latching box.
        /// </summary>
        public int DurationLock { get; }

        /// <summary>
        ///   Gets a value indicating whether the latching box is locked.
        /// </summary>
        public bool IsLocked => _stateMachine.State == LatchingBoxState.Locked;

        /// <summary>
        ///   Gets a value indicating whether the latching box is unlocked.
        /// </summary>
        public bool IsUnlocked => _stateMachine.State == LatchingBoxState.Unlocked;

        /// <summary>
        ///   Unlocks the latching box.
        /// </summary>
        public virtual void Unlock()
        {
            _stateMachine
                .Transition(
                    @from: new[] { LatchingBoxState.Locked, LatchingBoxState.Locking },
                    to: LatchingBoxState.Unlocking,
                    action: () => { _timer.Start(DurationUnlock - (DurationUnlock / DurationLock) * _timer.RemainingTime); });
        }

        /// <summary>
        ///   Locks the latching box.
        /// </summary>
        public virtual void Lock()
        {
            _stateMachine
                .Transition(
                    @from: new[] { LatchingBoxState.Unlocked, LatchingBoxState.Unlocking },
                    to: LatchingBoxState.Locking,
                    action: () => { _timer.Start(DurationLock - (DurationLock / DurationUnlock) * _timer.RemainingTime); });
        }

        /// <summary>
        ///   Updates the LatchingBox instance.
        /// </summary>
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
            public LatchingBoxLockFaultEffect(int dUnlock, int dLock, string type)
                : base(dUnlock, dLock, type)
            {
            }

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
            public LatchingBoxUnlockFaultEffect(int dUnlock, int dLock, string type)
                : base(dUnlock, dLock, type)
            {
            }

            public override void Unlock()
            {
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    ///  Describes the state of the electro valve.
    /// </summary
    public enum EVStates
    {
        /// <summary>
        /// State indicating the EV is open.
        /// </summary>
        Open,
        /// <summary>
        /// State indicating the EV is closed.
        /// </summary>
        Closed,
        /// <summary>
        /// State indicating the EV is opening.
        /// </summary>
        MoveOpening,
        /// <summary>
        /// State indicating the EV is closing.
        /// </summary>
        MoveClosing
    }

    class ElectroValve : Component
    {

        /// <summary>
        ///   Gets the state machine that manages the state of the gear cylinder.
        /// </summary>
        public readonly StateMachine<EVStates> StateMachine = EVStates.Open;

        /// <summary>
        ///  Timer to time the pressure increase.
        /// </summary>
        private readonly Timer _timer = new Timer();

        ///<summary>
        /// Gets the hydraulic output pressure of the EV.
        /// </summary>
        public int Hout => StateMachine == EVStates.Closed ? Hin : 0;

        ///<summary>
        /// Gets the hydraulic input pressure of the EV.
        /// </summary>
        public extern int Hin { get; }

        ///<summary>
        /// Gets the electric order.
        /// </summary>
        public extern bool EOrder();

        ///<summary>
        /// Updates the EV.
        /// </summary>
        public void update()
        {
            Update(_timer);

            StateMachine
                .Transition(
                from: EVStates.Open,
                to: EVStates.MoveClosing,
                guard: EOrder() == true,
                action: () =>
                {
                    _timer.SetTimeout(10);
                    _timer.Start();
                })

                .Transition(
                    from: EVStates.MoveClosing,
                    to: EVStates.Closed,
                    guard: _timer.HasElapsed && EOrder() == true)

                .Transition(
                    from: EVStates.Closed,
                    to: EVStates.MoveOpening,
                    guard: EOrder() == false,
                    action: () =>
                    {
                        _timer.SetTimeout(36);
                        _timer.Start();
                    })

                .Transition(
                    from: EVStates.MoveOpening,
                    to: EVStates.Open,
                    guard: _timer.HasElapsed && EOrder() == false)


                .Transition(
                    from: EVStates.MoveOpening,
                    to: EVStates.MoveClosing,
                    guard: EOrder() == true,
                    action: () =>
                    {
                        _timer.SetTimeout(10 - (_timer.RemainingTime * 5) / 18);
                        _timer.Start();
                    })

                .Transition(
                    from: EVStates.MoveClosing,
                    to: EVStates.MoveOpening,
                    guard: EOrder() == false,
                    action: () =>
                    {
                        _timer.SetTimeout(36 - (_timer.RemainingTime * 36) / 10);
                        _timer.Start();
                    });
        }
    }
}

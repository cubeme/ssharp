using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class PilotInterface : Component
    {
        public readonly Pilot Pilot = new Pilot();

        public readonly PilotHandle Handle = new PilotHandle();

        public readonly Light GreenLight = new Light();
        public readonly Light OrangeLight = new Light();
        public readonly Light RedLight = new Light();

        public override void Update()
        {
            Update(Pilot, Handle, GreenLight, OrangeLight, RedLight);
        }
    }
}

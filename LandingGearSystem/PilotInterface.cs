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
        public Pilot Pilot = new Pilot();

        public PilotHandle Handle = new PilotHandle();

        public Light GreenLight = new Light();
        public Light OrangeLight = new Light();
        public Light RedLight = new Light();

        public override void Update()
        {
            Update(Pilot, Handle, GreenLight, OrangeLight, RedLight);
        }
    }
}

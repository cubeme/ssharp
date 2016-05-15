using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;
    using SafetySharp.Analysis;

    class Model : ModelBase
    {
        public const int PressureLimit = 60;

        public const AirplaneStates AirplaneState = AirplaneStates.Ground;

        public const Mode Modus = Mode.Any;

        public const int Count = 2;

        [Root(Role.System)]
        public DigitalPart DigitalPart = new DigitalPart(Modus, Count);

        public MechanicalPart MechanicalPart = new MechanicalPart(PressureLimit);

        public PilotInterface PilotInterface = new PilotInterface();

        public Airplane Airplane = new Airplane(AirplaneState);
     
        public Model()
        {          
            //Bind...
        }
    }
}

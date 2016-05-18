using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    /// <summary>
    /// Modus the digital part operates in.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Any one value of the computing module has to be true to return a true value (logical OR).
        /// </summary>
        Any,
        /// <summary>
        /// All values of the computing module has to be true to return a true value (logical AND).
        /// </summary>
        All,
// --> Necessary?? --> Nein! Default Konstruktor mit einem 
        /// <summary>
        /// Only one computing module is to be used.
        /// </summary>
        One
        
    }

    class DigitalPart : Component
    {
        /// <summary>
        /// Indicates the modus the digital part is operating in.
        /// </summary>
        private readonly Mode _mode;

        /// <summary>
        /// Array with computing modules the digital part is composed of.
        /// </summary>
        public readonly ComputingModule[] ComputingModules;

        //private Func<IEnumerable<ComputingModule>,Func<ComputingModule,bool>, bool> _comparisonFunction;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="mode">Indicates the mode the digital part is operating in: Any, All, One.</param>
        /// <param name="count">Indicates how many computing modules are to be used.</param>
        public DigitalPart(Mode mode, int count)
        {
            _mode = mode;
            ComputingModules = new ComputingModule[count];
            for(int i = 0; i< count; i++)
            {
                ComputingModules[i] = new ComputingModule();
            }

            //if (_mode == Mode.All)
            //    _comparisonFunction = Enumerable.All;
            //else
            //    _comparisonFunction = Enumerable.Any;
        }

        public override void Update()
        {
            Update(ComputingModules);      
        }

        /// <summary>
        /// Gets a value indicating whether the general electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary>
        public bool GeneralEVComposition()  //=>  _comparisonFunction(ComputingModules, element => element.GeneralEV == true);
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.GeneralEV == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.GeneralEV == true);
                case (int)Mode.One:
                    return ComputingModules[0].GeneralEV;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the door closure electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool CloseEVComposition()
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.CloseEV == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.CloseEV == true);
                case (int)Mode.One:
                    return ComputingModules[0].CloseEV;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the door opening electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool OpenEVComposition()
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.OpenEV == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.OpenEV == true);
                case (int)Mode.One:
                    return ComputingModules[0].OpenEV;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the gear retraction electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool RetractEVComposition()
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.RetractEV == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.RetractEV == true);
                case (int)Mode.One:
                    return ComputingModules[0].RetractEV;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the gear extension electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool ExtendEVComposition()
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.ExtendEV == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.ExtendEV == true);
                case (int)Mode.One:
                    return ComputingModules[0].ExtendEV;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether all three gears are locked down through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsLockedDownComposition()
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.GearsLockedDown == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.GearsLockedDown == true);
                case (int)Mode.One:
                    return ComputingModules[0].GearsLockedDown;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether all three gears are maneuvering through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsManeuveringComposition()
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.GearsManeuvering == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.GearsManeuvering == true);
                case (int)Mode.One:
                    return ComputingModules[0].GearsManeuvering;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an anomaly has been detected through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool AnomalyComposition()
        {
            switch ((int)_mode)
            {
                case (int)Mode.All:
                    return ComputingModules.All(element => element.Anomaly == true);
                case (int)Mode.Any:
                    return ComputingModules.Any(element => element.Anomaly == true);
                case (int)Mode.One:
                    return ComputingModules[0].Anomaly;
                default:
                    return false;
            }
        }

    }
}

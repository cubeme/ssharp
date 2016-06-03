using System;
using System.Collections.Generic;
using System.Linq;


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
        All
    }

    class DigitalPart : Component
    {

        /// <summary>
        /// Array with computing modules the digital part is composed of.
        /// </summary>
        public readonly ComputingModule[] ComputingModules;

        private readonly Func<IEnumerable<ComputingModule>,Func<ComputingModule,bool>, bool> _comparisonFunction;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="mode">Indicates the mode the digital part is operating in: Any, All, One.</param>
        /// <param name="count">Indicates how many computing modules are to be used.</param>
        public DigitalPart(Mode mode, int count)
        {
            ComputingModules = new ComputingModule[count];
            for(var i = 0; i< count; i++)
            {
                ComputingModules[i] = new ComputingModule();
            }

            if (mode == Mode.All)
                _comparisonFunction = Enumerable.All;
            else
                _comparisonFunction = Enumerable.Any;
        }

        public DigitalPart()
        {
            ComputingModules = new[] {new ComputingModule()};
            _comparisonFunction = Enumerable.Any;
        }

        public override void Update()
        {
            //todo: Doch Elemente einzeln aufrufen?
            Array.ForEach(ComputingModules, element => element.Update());            
        }

        /// <summary>
        /// Gets a value indicating whether the general electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary>       
        public bool GeneralEVComposition() => _comparisonFunction(ComputingModules, element => element.GeneralEV == true);

        /// <summary>
        /// Gets a value indicating whether the door closure electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool CloseEVComposition() => _comparisonFunction(ComputingModules, element => element.CloseEV == true);

        /// <summary>
        /// Gets a value indicating whether the door opening electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool OpenEVComposition() => _comparisonFunction(ComputingModules, element => element.OpenEV == true);

        /// <summary>
        /// Gets a value indicating whether the gear retraction electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool RetractEVComposition() => _comparisonFunction(ComputingModules, element => element.RetractEV == true);

        /// <summary>
        /// Gets a value indicating whether the gear extension electro valve is to be stimulated through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool ExtendEVComposition() => _comparisonFunction(ComputingModules, element => element.ExtendEV == true);

        /// <summary>
        /// Gets a value indicating whether all three gears are locked down through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsLockedDownComposition() => _comparisonFunction(ComputingModules, element => element.GearsLockedDown == true);


        /// <summary>
        /// Gets a value indicating whether all three gears are maneuvering through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool GearsManeuveringComposition() => _comparisonFunction(ComputingModules, element => element.GearsManeuvering == true);


        /// <summary>
        /// Gets a value indicating whether an anomaly has been detected through composition of the two computing modules outputs with a logical or.
        /// </summary
        public bool AnomalyComposition() => _comparisonFunction(ComputingModules, element => element.Anomaly == true);

    }
}

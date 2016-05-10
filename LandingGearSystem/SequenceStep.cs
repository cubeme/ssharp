using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class SequenceStep : Component
    {
        public Func<bool> Condition { get; set; } 
        public Action Statement { get; private set; }  
        
        public SequenceStep(Func<bool> condition, Action statement)
        {
            Condition = condition;
            Statement = statement;
        }
           
        public void RunStatement()
        {
            Statement();
        }
    }
}

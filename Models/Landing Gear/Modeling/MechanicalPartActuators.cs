

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class MechanicalPartActuators : Component
    {

        public readonly DoorCylinder FrontDoorCylinder = new DoorCylinder(Position.Front);
        public readonly DoorCylinder LeftDoorCylinder = new DoorCylinder(Position.Left);
        public readonly DoorCylinder RightDoorCylinder = new DoorCylinder(Position.Right);

        public readonly GearCylinder FrontGearCylinder;
        public readonly GearCylinder LeftGearCylinder;
        public readonly GearCylinder RightGearCylinder;

        public MechanicalPartActuators(GearStates startState)
        {
            FrontGearCylinder = new GearCylinder(Position.Front, startState);
            LeftGearCylinder = new GearCylinder(Position.Left, startState);
            RightGearCylinder = new GearCylinder(Position.Right, startState);
        }

        public override void Update()
        {
            Update(FrontDoorCylinder, LeftDoorCylinder, RightDoorCylinder, FrontGearCylinder, LeftGearCylinder, RightGearCylinder);
        }
    }
}



namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class MechanicalPartActuators : Component
    {

        public readonly DoorCylinder FrontDoorCylinder = new DoorCylinder(CylinderPosition.Front);
        public readonly DoorCylinder LeftDoorCylinder = new DoorCylinder(CylinderPosition.Left);
        public readonly DoorCylinder RightDoorCylinder = new DoorCylinder(CylinderPosition.Right);

        public readonly GearCylinder FrontGearCylinder;
        public readonly GearCylinder LeftGearCylinder;
        public readonly GearCylinder RightGearCylinder;

        public MechanicalPartActuators(GearStates startState)
        {
            FrontGearCylinder = new GearCylinder(CylinderPosition.Front, startState);
            LeftGearCylinder = new GearCylinder(CylinderPosition.Left, startState);
            RightGearCylinder = new GearCylinder(CylinderPosition.Right, startState);
        }

        public override void Update()
        {
            Update(FrontDoorCylinder, LeftDoorCylinder, RightDoorCylinder, FrontGearCylinder, LeftGearCylinder, RightGearCylinder);
        }
    }
}

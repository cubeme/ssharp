

namespace SafetySharp.CaseStudies.LandingGear
{
    using SafetySharp.Modeling;

    class MechanicalPartActuators : Component
    {

        public readonly DoorCylinder FrontDoorCylinder = new DoorCylinder(CylinderPosition.Front);
        public readonly DoorCylinder LeftDoorCylinder = new DoorCylinder(CylinderPosition.Left);
        public readonly DoorCylinder RightDoorCylinder = new DoorCylinder(CylinderPosition.Right);

        public readonly GearCylinder FrontGearCylinder = new GearCylinder(CylinderPosition.Front);
        public readonly GearCylinder LeftGearCylinder = new GearCylinder(CylinderPosition.Left);
        public readonly GearCylinder RightGearCylinder = new GearCylinder(CylinderPosition.Right);

        public override void Update()
        {
            Update(FrontDoorCylinder, LeftDoorCylinder, RightDoorCylinder, FrontGearCylinder, LeftGearCylinder, RightGearCylinder);
        }
    }
}

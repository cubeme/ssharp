

namespace LandingGearSystem
{
    using SafetySharp.Modeling;

    class MechanicalPartPlants : Component
    {
        public MechanicalPartActuators Actuators;

        public readonly Door DoorFront = new Door(DoorPosition.Front);
        public readonly Door DoorLeft = new Door(DoorPosition.Left);
        public readonly Door DoorRight = new Door(DoorPosition.Right);

        public readonly Gear GearFront = new Gear(GearPosition.Front);
        public readonly Gear GearLeft = new Gear(GearPosition.Left);
        public readonly Gear GearRight = new Gear(GearPosition.Right);

        public override void Update()
        {
            Update(Actuators, DoorFront, DoorLeft, DoorRight, GearFront, GearLeft, GearRight);
        }
    }
}

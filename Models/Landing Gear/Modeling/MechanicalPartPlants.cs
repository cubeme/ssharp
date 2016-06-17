

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class MechanicalPartPlants : Component
    {
        public MechanicalPartActuators Actuators;

        public readonly Door DoorFront = new Door(DoorPosition.Front);
        public readonly Door DoorLeft = new Door(DoorPosition.Left);
        public readonly Door DoorRight = new Door(DoorPosition.Right);

        public readonly Gear GearFront;
        public readonly Gear GearLeft; 
        public readonly Gear GearRight;

        public MechanicalPartPlants(GearStates startState)
        {
            GearFront = new Gear(GearPosition.Front, startState);
            GearLeft = new Gear(GearPosition.Left, startState);
            GearRight = new Gear(GearPosition.Right, startState);
        }

        public override void Update()
        {
            Update(Actuators, DoorFront, DoorLeft, DoorRight, GearFront, GearLeft, GearRight);
        }
    }
}

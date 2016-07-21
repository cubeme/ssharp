

namespace SafetySharp.CaseStudies.LandingGear.Modeling
{
    using SafetySharp.Modeling;

    public class MechanicalPartPlants : Component
    {
        public MechanicalPartActuators Actuators;

        public readonly Door DoorFront = new Door(Position.Front);
        public readonly Door DoorLeft = new Door(Position.Left);
        public readonly Door DoorRight = new Door(Position.Right);

        public readonly Gear GearFront;
        public readonly Gear GearLeft; 
        public readonly Gear GearRight;

        public MechanicalPartPlants(GearStates startState)
        {
            GearFront = new Gear(Position.Front, startState);
            GearLeft = new Gear(Position.Left, startState);
            GearRight = new Gear(Position.Right, startState);
        }

        public override void Update()
        {
            Update(Actuators, DoorFront, DoorLeft, DoorRight, GearFront, GearLeft, GearRight);
        }
    }
}

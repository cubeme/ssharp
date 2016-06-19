using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SafetySharp.CaseStudies.Visualizations
{
    //todo: pilot handle up down instead of extend/retract
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Analysis;
    using Infrastructure;
    using Modeling;
    using SafetySharp.CaseStudies.LandingGear.Modeling;

    /// <summary>
    /// Interaktionslogik für LandingGear.xaml
    /// </summary>
    public partial class LandingGear : INotifyPropertyChanged
    {
        private readonly Model _model = new Model(new InitializeOne());

        public HandlePosition MoveToPosition { get; private set; }

        public LandingGear()
        {
            InitializeComponent();

            _model.Faults.SuppressActivations();

            MoveToPosition = _model.Cockpit.PilotHandle.Position == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;
            HandleButtonText.DataContext = this;

            SimulationControls.ModelStateChanged += (o, e) => UpdateModelState();
            SimulationControls.SetModel(new Model(new InitializeOne()));

            UpdateModelState();

        }

        private void UpdateModelState()
        {
           //Actionsequence State
            txtSequence.Text = _model.DigitalPart.ComputingModules[0].ActionSequenceState.ToString();

            //Pilot Handle
            txtHandle.Text = _model.Cockpit.PilotHandle.Position.ToString();

            //General EV
            txtGeneralEV.Text = _model.MechanicalPartControllers.GeneralEV.State.ToString();

            //Doors
            txtOpenEV.Text = _model.MechanicalPartControllers.OpenEV.State.ToString();
            txtCloseEV.Text = _model.MechanicalPartControllers.CloseEV.State.ToString();
            txtExtensionDoors.Text = $"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitDoors.Pressure}";
            txtRetractionDoors.Text = $"Enabled: {_model.MechanicalPartControllers.RetractionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitDoors.Pressure}";
            txtFrontDoorCyl.Text = _model.MechanicalActuators.FrontDoorCylinder.DoorCylinderState.ToString();
            txtLeftDoorCyl.Text = _model.MechanicalActuators.LeftDoorCylinder.DoorCylinderState.ToString();
            txtRightDoorCyl.Text = _model.MechanicalActuators.RightDoorCylinder.DoorCylinderState.ToString();
            txtFrontDoor.Text = _model.MechanicalPartPlants.DoorFront.State.ToString();
            txtLeftDoor.Text = _model.MechanicalPartPlants.DoorLeft.State.ToString();
            txtRightDoor.Text = _model.MechanicalPartPlants.DoorRight.State.ToString();

            //Gears
            txtExtendEV.Text = _model.MechanicalPartControllers.ExtendEV.State.ToString();
            txtRetractEV.Text = _model.MechanicalPartControllers.RetractEV.State.ToString();
            txtExtensionGears.Text = $"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitGears.Pressure}";
            txtRetractionGears.Text = $"Enabled: {_model.MechanicalPartControllers.RetractionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitGears.Pressure}";
            txtFrontGearCyl.Text = _model.MechanicalActuators.FrontGearCylinder.GearCylinderState.ToString();
            txtLeftGearCyl.Text = _model.MechanicalActuators.LeftGearCylinder.GearCylinderState.ToString();
            txtRightGearCyl.Text = _model.MechanicalActuators.RightGearCylinder.GearCylinderState.ToString();
            txtFrontGear.Text = _model.MechanicalPartPlants.GearFront.State.ToString();
            txtLeftGear.Text = _model.MechanicalPartPlants.GearLeft.State.ToString();
            txtRightGear.Text = _model.MechanicalPartPlants.GearRight.State.ToString();

        }
   
        private void HandleClicked(object sender, RoutedEventArgs e)
        {
            _model.Pilot.Move = MoveToPosition;
            MoveToPosition = MoveToPosition == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;
            NotifyPropertyChanged("MoveToPosition");
            UpdateModelState();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

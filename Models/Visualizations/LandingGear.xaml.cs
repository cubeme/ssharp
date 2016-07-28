// The MIT License (MIT)
// 
// Copyright (c) 2014-2016, Institute for Software & Systems Engineering
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SafetySharp.CaseStudies.Visualizations
{

	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Media;
	using CaseStudies.LandingGear.Modeling;
	using Modeling;

    /// <summary>
    ///   Interaktionslogik für LandingGear.xaml
    /// </summary>
    public partial class LandingGear
	{
		private readonly Model _model;

        //Collections for the ListBoxes
        private readonly ObservableCollection<Fault> _inactiveFaultList;
        private readonly ObservableCollection<Fault> _activeFaultList = new ObservableCollection<Fault>();

        //Bools that indicates whether an the pilot handle has been moved or the state of the airplane has been changed
	    private bool _handleButtonClicked;
        private bool _airplaneButtonClicked;

        //Used for displaying a detected anomaly
        private bool _anomalyToggled = true;

        //Used for prohibiting any onClick actions of HandleButton and AirplaneButton if the simulation has not yet started/is paused
        private bool _simulationStarted;

        public LandingGear()
		{
			InitializeComponent();

			//Initialize the simulation environment
			MySimulationControls.ModelStateChanged += (o, e) => UpdateModelState();
            var model = (new Model(new InitializeOne()));
            model.Faults.SuppressActivations();
            MySimulationControls.SetModel(model);

            MySimulationControls.StartButton.Clicked += StartButtonOnClicked;
            MySimulationControls.PauseButton.Clicked += PauseButtonOnClicked;

            _model = (Model)MySimulationControls.Model;
			_model.Faults.SuppressActivations();

            //Set fault lists
            _inactiveFaultList = new ObservableCollection<Fault>(_model.Faults);

            InactiveFaultList.ItemsSource = _inactiveFaultList;
            ActiveFaultList.ItemsSource = _activeFaultList;

            //Initialize the visualization state
            UpdateModelState();

			MySimulationControls.MaxSpeed = 64;
			MySimulationControls.ChangeSpeed(8);
		}

        private void UpdateModelState()
		{
			if (_model == null)
				return;

            //Actionsequence State
            TxtSequence.Content = _model.DigitalPart.ComputingModules[0].ActionSequenceState.ToString();

            //Cockpit Lights
            Green.Foreground = _model.DigitalPart.GearsLockedDownComposition() ? Brushes.Green : Brushes.White;
            Orange.Foreground = _model.DigitalPart.GearsManeuveringComposition() ? Brushes.Orange : Brushes.White;
            Red.Foreground = _model.DigitalPart.AnomalyComposition() ? Brushes.Red : Brushes.White;

            if (_anomalyToggled && _model.DigitalPart.AnomalyComposition())
            {
                _model.DigitalPart.AnomalyComposition();

                AnomalyPopup.IsOpen = true;
                TxtPopup.Content = "An anomaly has been detected! \nPlease reset the simulation.";

                _anomalyToggled = false;
            }

            //Pilot Handle
            TxtHandle.Content = _model.Cockpit.PilotHandle.Position.ToString();

            //Button to move the pilot handle
            var moveToPosition = _model.Cockpit.PilotHandle.Position == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;
            HandleButton.Content = $"Move Handle {moveToPosition}";

            if (_handleButtonClicked)
		    {
                _model.Pilot.Move = moveToPosition;
                moveToPosition = _model.Cockpit.PilotHandle.Position == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;
                HandleButton.Content = $"Move Handle {moveToPosition}";
		        _handleButtonClicked = false;
		    }

            //Airplane state
		    TxtAirplane.Content = _model.Airplane.AirPlaneStatus.ToString();

            //Button to change airplane status
		    var changeAirplaneState = _model.Airplane.AirPlaneStatus == AirplaneStates.Flight ? AirplaneStates.Ground : AirplaneStates.Flight;
            AirplaneButton.Content = $"Set Airplane Status to {changeAirplaneState}";

            if (_airplaneButtonClicked)
            {
                _model.Airplane.AirPlaneStatus = changeAirplaneState;
                changeAirplaneState = _model.Airplane.AirPlaneStatus == AirplaneStates.Flight ? AirplaneStates.Ground : AirplaneStates.Flight;
                AirplaneButton.Content = $"Set Airplane Status to {changeAirplaneState}";
                _airplaneButtonClicked = false;
            }

            //General EV
            TxtGeneralEv.Content = _model.MechanicalPartControllers.GeneralEV.State.ToString();

            //First Pressure Circuit
		    TxtFirstPressure.Content = $"Enabled: {_model.MechanicalPartControllers.FirstPressureCircuit.IsEnabled}, Pressure: {_model.MechanicalPartControllers.FirstPressureCircuit.Pressure}";

            //Doors
            TxtOpenEv.Content = _model.MechanicalPartControllers.OpenEV.State.ToString();
			TxtCloseEv.Content = _model.MechanicalPartControllers.CloseEV.State.ToString();
			TxtExtensionDoors.Content =
				$"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitDoors.Pressure}";
			TxtRetractionDoors.Content =
				$"Enabled: {_model.MechanicalPartControllers.RetractionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitDoors.Pressure}";
			TxtFrontDoorCyl.Content = _model.MechanicalActuators.FrontDoorCylinder.DoorCylinderState.ToString();
			TxtLeftDoorCyl.Content = _model.MechanicalActuators.LeftDoorCylinder.DoorCylinderState.ToString();
			TxtRightDoorCyl.Content = _model.MechanicalActuators.RightDoorCylinder.DoorCylinderState.ToString();
			TxtFrontDoor.Content = _model.MechanicalPartPlants.DoorFront.State.ToString();
			TxtLeftDoor.Content = _model.MechanicalPartPlants.DoorLeft.State.ToString();
			TxtRightDoor.Content = _model.MechanicalPartPlants.DoorRight.State.ToString();

			//Gears
			TxtExtendEv.Content = _model.MechanicalPartControllers.ExtendEV.State.ToString();
			TxtRetractEv.Content = _model.MechanicalPartControllers.RetractEV.State.ToString();
			TxtExtensionGears.Content =
				$"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitGears.Pressure}";
			TxtRetractionGears.Content =
				$"Enabled: {_model.MechanicalPartControllers.RetractionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitGears.Pressure}";
			TxtFrontGearCyl.Content = _model.MechanicalActuators.FrontGearCylinder.GearCylinderState.ToString();
			TxtLeftGearCyl.Content = _model.MechanicalActuators.LeftGearCylinder.GearCylinderState.ToString();
			TxtRightGearCyl.Content = _model.MechanicalActuators.RightGearCylinder.GearCylinderState.ToString();
			TxtFrontGear.Content = _model.MechanicalPartPlants.GearFront.State.ToString();
			TxtLeftGear.Content = _model.MechanicalPartPlants.GearLeft.State.ToString();
			TxtRightGear.Content = _model.MechanicalPartPlants.GearRight.State.ToString();
		}

        private void HandleClicked(object sender, RoutedEventArgs e)
        {
            if (!_simulationStarted)
                return;
		    _handleButtonClicked = true;
			UpdateModelState();
		}

        private void AirplaneClicked(object sender, RoutedEventArgs e)
        {
            if (!_simulationStarted)
                return;
            _airplaneButtonClicked = true;
            UpdateModelState();
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            var fault = (Fault)ActiveFaultList.SelectedItem;
            if (fault == null)
                return;
            _inactiveFaultList.Add(fault);
            _activeFaultList.Remove(fault);

            fault.ToggleActivationMode();
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            var fault = (Fault)InactiveFaultList.SelectedItem;
            if (fault == null)
                return;
            _activeFaultList.Add(fault);
            _inactiveFaultList.Remove(fault);

            fault.ToggleActivationMode();
        }

        private void StartButtonOnClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            _simulationStarted = true;
        }

        private void PauseButtonOnClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            _simulationStarted = false;
        }

    }
}
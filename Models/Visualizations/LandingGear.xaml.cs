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
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Windows;
	using System.Windows.Controls.Primitives;
	using System.Windows.Media;
	using Analysis;
	using CaseStudies.LandingGear.Modeling;
	using Modeling;

    /// <summary>
    ///   Interaktionslogik für LandingGear.xaml
    /// </summary>
    public partial class LandingGear
	{
		private Model _model;

        private readonly List<Fault> _faultList;
        private readonly List<Fault> _activeFaultList = new List<Fault>();

	    private bool _handleButtonClicked;
        private bool _airplaneButtonClicked;

        private bool _anomalyToggled = true;


        public LandingGear()
		{
			InitializeComponent();

			//Initialize the simulation environment
			SimulationControls.ModelStateChanged += (o, e) => UpdateModelState();
            SimulationControls.SetModel(new Model(new InitializeOne()));

			_model = (Model)SimulationControls.Model;
			_model.Faults.SuppressActivations();

            _faultList = new List<Fault>(_model.Faults);
            _faultList.Sort((x, y) => String.CompareOrdinal(x.Name, y.Name));

            InactiveFaultList.ItemsSource = _faultList;
            ActiveFaultList.ItemsSource = _activeFaultList;

            //Initialize the visualization state
            UpdateModelState();

			SimulationControls.MaxSpeed = 64;
			SimulationControls.ChangeSpeed(8);
		}


		private void UpdateModelState()
		{
			if (_model == null)
				return;

            //Actionsequence State
            txtSequence.Content = _model.DigitalPart.ComputingModules[0].ActionSequenceState.ToString();

            //Cockpit Lights
            Green.Foreground = _model.DigitalPart.GearsLockedDownComposition() ? Brushes.Green : Brushes.White;
            Orange.Foreground = _model.DigitalPart.GearsManeuveringComposition() ? Brushes.Orange : Brushes.White;
            Red.Foreground = _model.DigitalPart.AnomalyComposition() ? Brushes.Red : Brushes.White;

		    if (_anomalyToggled && _model.DigitalPart.AnomalyComposition())
		    {
		        _model.DigitalPart.AnomalyComposition();

                AnomalyPopup.IsOpen = true;
		        txtPopup.Content = $"An anomaly has been detected!";
		        _anomalyToggled = false;
		    }

            //Pilot Handle
            txtHandle.Content = _model.Cockpit.PilotHandle.Position.ToString();

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
		    txtAirplane.Content = _model.Airplane.AirPlaneStatus.ToString();

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
            txtGeneralEV.Content = _model.MechanicalPartControllers.GeneralEV.State.ToString();

            //First Pressure Circuit
		    txtFirstPressure.Content = $"Enabled: {_model.MechanicalPartControllers.FirstPressureCircuit.IsEnabled}, Pressure: {_model.MechanicalPartControllers.FirstPressureCircuit.Pressure}";

            //Doors
            txtOpenEV.Content = _model.MechanicalPartControllers.OpenEV.State.ToString();
			txtCloseEV.Content = _model.MechanicalPartControllers.CloseEV.State.ToString();
			txtExtensionDoors.Content =
				$"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitDoors.Pressure}";
			txtRetractionDoors.Content =
				$"Enabled: {_model.MechanicalPartControllers.RetractionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitDoors.Pressure}";
			txtFrontDoorCyl.Content = _model.MechanicalActuators.FrontDoorCylinder.DoorCylinderState.ToString();
			txtLeftDoorCyl.Content = _model.MechanicalActuators.LeftDoorCylinder.DoorCylinderState.ToString();
			txtRightDoorCyl.Content = _model.MechanicalActuators.RightDoorCylinder.DoorCylinderState.ToString();
			txtFrontDoor.Content = _model.MechanicalPartPlants.DoorFront.State.ToString();
			txtLeftDoor.Content = _model.MechanicalPartPlants.DoorLeft.State.ToString();
			txtRightDoor.Content = _model.MechanicalPartPlants.DoorRight.State.ToString();

			//Gears
			txtExtendEV.Content = _model.MechanicalPartControllers.ExtendEV.State.ToString();
			txtRetractEV.Content = _model.MechanicalPartControllers.RetractEV.State.ToString();
			txtExtensionGears.Content =
				$"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitGears.Pressure}";
			txtRetractionGears.Content =
				$"Enabled: {_model.MechanicalPartControllers.RetractionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitGears.Pressure}";
			txtFrontGearCyl.Content = _model.MechanicalActuators.FrontGearCylinder.GearCylinderState.ToString();
			txtLeftGearCyl.Content = _model.MechanicalActuators.LeftGearCylinder.GearCylinderState.ToString();
			txtRightGearCyl.Content = _model.MechanicalActuators.RightGearCylinder.GearCylinderState.ToString();
			txtFrontGear.Content = _model.MechanicalPartPlants.GearFront.State.ToString();
			txtLeftGear.Content = _model.MechanicalPartPlants.GearLeft.State.ToString();
			txtRightGear.Content = _model.MechanicalPartPlants.GearRight.State.ToString();


            InactiveFaultList.Items.Refresh();
		    ActiveFaultList.Items.Refresh();


		}

        private void HandleClicked(object sender, RoutedEventArgs e)
		{
		    _handleButtonClicked = true;
			UpdateModelState();
		}

        private void AirplaneClicked(object sender, RoutedEventArgs e)
        {
            _airplaneButtonClicked = true;
            UpdateModelState();
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            var fault = (Fault)ActiveFaultList.SelectedItem;
            if (fault != null)
            {

                _faultList.Add(fault);
                _faultList.Sort((x, y) => String.CompareOrdinal(x.Name, y.Name));
                _activeFaultList.Remove(fault);

                fault.ToggleActivationMode();
            }
            UpdateModelState();
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            var fault = (Fault)InactiveFaultList.SelectedItem;
            if (fault != null)
            {
                _activeFaultList.Add(fault);
                _activeFaultList.Sort((x, y) => String.CompareOrdinal(x.Name, y.Name));
                _faultList.Remove(fault);

                fault.ToggleActivationMode();
            }

            UpdateModelState();
        }

    }
}
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
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using System.Windows;
	using CaseStudies.LandingGear.Modeling;
	using Modeling;

	/// <summary>
	///   Interaktionslogik für LandingGear.xaml
	/// </summary>
	public partial class LandingGear : INotifyPropertyChanged
	{
		private readonly Model _model;

		public LandingGear()
		{
			InitializeComponent();
			
			HandleButtonText.DataContext = this;

			//Initialize the simulation environment
			SimulationControls.ModelStateChanged += (o, e) => UpdateModelState();
			SimulationControls.SetModel(new Model(new InitializeOne()));

			_model = (Model)SimulationControls.Model;
			_model.Faults.SuppressActivations();
			MoveToPosition = _model.Cockpit.PilotHandle.Position == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;

			//Initialize the visualization state
			UpdateModelState();

			SimulationControls.MaxSpeed = 64;
			SimulationControls.ChangeSpeed(8);
		}

		public HandlePosition MoveToPosition { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void UpdateModelState()
		{
			if (_model == null)
				return;

			//Actionsequence State
			txtSequence.Text = _model.DigitalPart.ComputingModules[0].ActionSequenceState.ToString();

			//Pilot Handle
			txtHandle.Text = _model.Cockpit.PilotHandle.Position.ToString();

			//General EV
			txtGeneralEV.Text = _model.MechanicalPartControllers.GeneralEV.State.ToString();

			//Doors
			txtOpenEV.Text = _model.MechanicalPartControllers.OpenEV.State.ToString();
			txtCloseEV.Text = _model.MechanicalPartControllers.CloseEV.State.ToString();
			txtExtensionDoors.Text =
				$"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitDoors.Pressure}";
			txtRetractionDoors.Text =
				$"Enabled: {_model.MechanicalPartControllers.RetractionCircuitDoors.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitDoors.Pressure}";
			txtFrontDoorCyl.Text = _model.MechanicalActuators.FrontDoorCylinder.DoorCylinderState.ToString();
			txtLeftDoorCyl.Text = _model.MechanicalActuators.LeftDoorCylinder.DoorCylinderState.ToString();
			txtRightDoorCyl.Text = _model.MechanicalActuators.RightDoorCylinder.DoorCylinderState.ToString();
			txtFrontDoor.Text = _model.MechanicalPartPlants.DoorFront.State.ToString();
			txtLeftDoor.Text = _model.MechanicalPartPlants.DoorLeft.State.ToString();
			txtRightDoor.Text = _model.MechanicalPartPlants.DoorRight.State.ToString();

			//Gears
			txtExtendEV.Text = _model.MechanicalPartControllers.ExtendEV.State.ToString();
			txtRetractEV.Text = _model.MechanicalPartControllers.RetractEV.State.ToString();
			txtExtensionGears.Text =
				$"Enabled: {_model.MechanicalPartControllers.ExtensionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.ExtensionCircuitGears.Pressure}";
			txtRetractionGears.Text =
				$"Enabled: {_model.MechanicalPartControllers.RetractionCircuitGears.IsEnabled}, Pressure: {_model.MechanicalPartControllers.RetractionCircuitGears.Pressure}";
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

		// The CallerMemberName attribute that is applied to the optional propertyName
		// parameter causes the property name of the caller to be substituted as an argument.
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
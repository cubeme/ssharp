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
    using Modeling;
    using SafetySharp.CaseStudies.LandingGear.Modeling;

    /// <summary>
    /// Interaktionslogik für LandingGear.xaml
    /// </summary>
    public partial class LandingGear : INotifyPropertyChanged
    {
        private readonly Model _model;

        public HandlePosition MoveToPosition { get; private set; }

        public LandingGear()
        {
            InitializeComponent();

            _model = new Model(new InitializeOne());
            _model.Faults.SuppressActivations();

            txtSequence.DataContext = _model.DigitalPart.ComputingModules[0];

            MoveToPosition = _model.Cockpit.PilotHandle.Position == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;
            HandleButtonText.DataContext = this;

            txtHandle.DataContext = _model.Cockpit.PilotHandle;

            txtGeneralEV.DataContext = _model.MechanicalPartControllers.GeneralEV;

        }


        private void HandleClicked(object sender, RoutedEventArgs e)
        {
            _model.Pilot.Move = MoveToPosition;
            MoveToPosition = MoveToPosition == HandlePosition.Up ? HandlePosition.Down : HandlePosition.Up;
            NotifyPropertyChanged("MoveToPosition");
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

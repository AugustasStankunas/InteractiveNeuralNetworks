using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;
using System;
using System.IO;


namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBActivationFunctionViewModel : ToolbarItemViewModel
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "activationFunction.png");
        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }
        public TBActivationFunctionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
             Name = "Activation Function";
           // Color = "Blue";
            IconPath = filePath;
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSActivationFunctionViewModel("ReLU", mousePos.X, mousePos.Y, 60, 60, "Blue");
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

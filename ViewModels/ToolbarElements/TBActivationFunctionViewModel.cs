using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;


namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBActivationFunctionViewModel : ToolbarItemViewModel
    {
        public TBActivationFunctionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Activation Function";
            Color = "Blue";
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

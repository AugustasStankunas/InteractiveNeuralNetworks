using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;

namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBFullyConnectedViewModel : ToolbarItemViewModel
    {
        public TBFullyConnectedViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Fully Connected layer";
            Color = "Orange";
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSFullyConnectedViewModel(128, 128, mousePos.X, mousePos.Y, 60, 60, "Orange");
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

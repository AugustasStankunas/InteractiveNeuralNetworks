using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;

namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBPoolingViewModel : ToolbarItemViewModel
    {
        public TBPoolingViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Pooling layer";
            Color = "Red";
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSPoolingViewModel(128, 128, mousePos.X, mousePos.Y, 60, 60, "Red");
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

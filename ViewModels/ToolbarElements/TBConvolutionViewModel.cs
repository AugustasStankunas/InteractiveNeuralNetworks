using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;


namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBConvolutionViewModel: ToolbarItemViewModel
    {
        public TBConvolutionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Convolutional layer";
            Color = "Green";
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSConvolutionViewModel(3, 32, 3, 1, mousePos.X, mousePos.Y, 60, 60, "Green");
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

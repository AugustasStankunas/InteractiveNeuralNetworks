using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;
using System.IO;



namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBConvolutionViewModel: ToolbarItemViewModel
    {
        public TBConvolutionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Convolutional layer";
            WorkspaceItem = new WSConvolutionViewModel(3, 32, 3, 1, 0,0, 60, 60);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
			IsSelected = true;
			Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSConvolutionViewModel(3, 32, 3, 1, mousePos.X, mousePos.Y, 60, 60);
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

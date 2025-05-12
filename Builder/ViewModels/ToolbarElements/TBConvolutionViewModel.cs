using System.Windows;
using System.Windows.Input;
using Builder.Enums;
using Builder.Helpers;
using Builder.ViewModels.WorkspaceElements;



namespace Builder.ViewModels.ToolbarElements
{
    class TBConvolutionViewModel : ToolbarItemViewModel
    {
        public TBConvolutionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Convolutional";
            TooltipText = LayerType.Convolutional.GetDescription();
            WorkspaceItem = new WSConvolutionViewModel(0, 0, 3, 1, 0, 0, 0);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSConvolutionViewModel(0, 0, 3, 1, 0, mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

using System.Windows;
using System.Windows.Input;
using Builder.Enums;
using Builder.Helpers;
using Builder.ViewModels.WorkspaceElements;


namespace Builder.ViewModels.ToolbarElements
{
    class TBPoolingViewModel : ToolbarItemViewModel
    {
        public TBPoolingViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Pooling";
            TooltipText = LayerType.Pooling.GetDescription();
            WorkspaceItem = new WSPoolingViewModel(0, 0, 0, 0);
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSPoolingViewModel(0, 0, mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

using System.Windows;
using System.Windows.Input;
using Builder.ViewModels.WorkspaceElements;


namespace Builder.ViewModels.ToolbarElements
{
    class TBPoolingViewModel : ToolbarItemViewModel
    {
        public TBPoolingViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Pooling layer";
            WorkspaceItem = new WSPoolingViewModel(128, 128, 0, 0, 60, 60);
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSPoolingViewModel(128, 128, mousePos.X, mousePos.Y, 60, 60);
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

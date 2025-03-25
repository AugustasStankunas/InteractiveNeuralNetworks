using System.Windows;
using System.Windows.Input;
using Builder.ViewModels.WorkspaceElements;


namespace Builder.ViewModels.ToolbarElements
{
    class TBFullyConnectedViewModel : ToolbarItemViewModel
    {
        public TBFullyConnectedViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Fully Connected layer";
            WorkspaceItem = new WSFullyConnectedViewModel(128, 128, 0, 0);
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSFullyConnectedViewModel(128, 128, mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

using System.Windows;
using System.Windows.Input;
using Builder.ViewModels.WorkspaceElements;

namespace Builder.ViewModels.ToolbarElements
{
    class TBInputViewModel : ToolbarItemViewModel
    {
        public TBInputViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Input layer";
            WorkspaceItem = new WSInputViewModel(0, 0, 0, 0, 0, 0);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSInputViewModel(0, 0, 0, mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

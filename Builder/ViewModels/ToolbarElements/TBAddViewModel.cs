using System.Windows;
using System.Windows.Input;
using Builder.ViewModels.WorkspaceElements;

namespace Builder.ViewModels.ToolbarElements
{
    class TBAddViewModel : ToolbarItemViewModel
    {
        public TBAddViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Add layer";
            WorkspaceItem = new WSAddViewModel(0, 0, 0);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSAddViewModel(0, mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

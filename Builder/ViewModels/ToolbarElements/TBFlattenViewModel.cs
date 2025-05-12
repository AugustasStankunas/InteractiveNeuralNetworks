using System.Windows;
using System.Windows.Input;
using Builder.Enums;
using Builder.Helpers;
using Builder.ViewModels.WorkspaceElements;

namespace Builder.ViewModels.ToolbarElements
{
    class TBFlattenViewModel : ToolbarItemViewModel
    {
        public TBFlattenViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Flattten";
            TooltipText = LayerType.Flatten.GetDescription();
            WorkspaceItem = new WSFlattenViewModel(0, 0);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSFlattenViewModel(mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

using System.Windows;
using System.Windows.Input;
using Builder.Enums;
using Builder.Helpers;
using Builder.ViewModels.WorkspaceElements;

namespace Builder.ViewModels.ToolbarElements
{
    class TBDropoutViewModel : ToolbarItemViewModel
    {
        public TBDropoutViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Dropout";
            TooltipText = LayerType.Dropout.GetDescription();
            WorkspaceItem = new WSDropoutViewModel(0.5, 0, 0);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSDropoutViewModel(0.5, mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

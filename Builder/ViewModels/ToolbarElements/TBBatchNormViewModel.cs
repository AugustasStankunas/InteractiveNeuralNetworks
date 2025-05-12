using System.Windows;
using System.Windows.Input;
using Builder.Enums;
using Builder.Helpers;
using Builder.ViewModels.WorkspaceElements;

namespace Builder.ViewModels.ToolbarElements
{
    class TBBatchNormViewModel : ToolbarItemViewModel
    {
        public TBBatchNormViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Batch Normalization";
            TooltipText = LayerType.BatchNorm.GetDescription();
            WorkspaceItem = new WSBatchNormViewModel(0.1, 1e-3, 0, 0);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSBatchNormViewModel(0.1, 1e-3, mousePos.X, mousePos.Y, opacity: 0.5);
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

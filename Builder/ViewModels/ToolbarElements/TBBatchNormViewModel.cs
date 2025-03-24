using Builder.ViewModels.WorkspaceElements;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Builder.ViewModels.ToolbarElements
{
    class TBBatchNormViewModel : ToolbarItemViewModel
    {
        public TBBatchNormViewModel(ToolbarViewModel toolbar): base(toolbar)
        {
            Name = "Batch Normalization layer";
            WorkspaceItem = new WSBatchNormViewModel(0.1, 1e-3, 0, 0, 60, 60);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSBatchNormViewModel(0.1, 1e-3, mousePos.X, mousePos.Y, 60, 60);
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

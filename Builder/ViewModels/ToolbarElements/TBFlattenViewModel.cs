using Builder.ViewModels.WorkspaceElements;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Builder.ViewModels.ToolbarElements
{
    class TBFlattenViewModel: ToolbarItemViewModel
    {
        public TBFlattenViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Flattten layer";
            WorkspaceItem = new WSFlattenViewModel(0, 0, 60, 60);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSFlattenViewModel(mousePos.X, mousePos.Y, 60, 60);
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

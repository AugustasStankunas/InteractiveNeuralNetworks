using Builder.ViewModels.WorkspaceElements;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Builder.ViewModels.ToolbarElements
{
    class TBDropoutViewModel: ToolbarItemViewModel
    {
        public TBDropoutViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Dropout layer";
            WorkspaceItem = new WSDropoutViewModel(0.5, 0, 0, 60, 60);
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSDropoutViewModel(0.5, mousePos.X, mousePos.Y, 60, 60);
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

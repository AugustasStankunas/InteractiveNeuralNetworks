using System.Windows.Input;
using Builder.ViewModels.WorkspaceElements;

namespace Builder.ViewModels.ToolbarElements
{
    class TBConnectionViewModel : ToolbarItemViewModel
    {
        public TBConnectionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Connection";
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsSelected = true;
            Toolbar.Builder.isMakingConnection = true;
            Toolbar.Builder.connectionInProgress = new WSConnectionViewModel();
        }
    }
}

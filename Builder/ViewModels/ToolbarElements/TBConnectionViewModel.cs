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
            if (Toolbar.Builder.isMakingConnection)
                Name = "Connection";
            else
                Name = "Connection ON";

            IsSelected = true;
            Toolbar.Builder.isMakingConnection = !Toolbar.Builder.isMakingConnection;
        }
    }
}

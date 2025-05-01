using System.Windows.Input;
using Builder.Enums;
using Builder.Helpers;

namespace Builder.ViewModels.ToolbarElements
{
    class TBConnectionViewModel : ToolbarItemViewModel
    {
        public TBConnectionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Connection";
            TooltipText = LayerType.Connection.GetDescription();
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (Toolbar.Builder.isMakingConnection)
                Name = "Connection";
            else
                Name = "Connection ON";

            IsSelected = true; Toolbar.Builder.isMakingConnection = !Toolbar.Builder.isMakingConnection;
        }
    }
}

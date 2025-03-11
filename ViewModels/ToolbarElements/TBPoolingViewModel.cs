using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;
using System.IO;


namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBPoolingViewModel : ToolbarItemViewModel
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "pooling.png");
        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }
        public TBPoolingViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Pooling layer";
          //  Color = "Red";
            IconPath = filePath;
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSPoolingViewModel(128, 128, mousePos.X, mousePos.Y, 60, 60, "Red");
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

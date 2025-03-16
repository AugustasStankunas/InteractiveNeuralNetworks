using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;
using System.IO;



namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBConvolutionViewModel: ToolbarItemViewModel
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "conv.png");
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
        public TBConvolutionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Convolutional layer";
            Color = "Green";
            IconPath = filePath;
        }
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
			IsSelected = true;
			Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSConvolutionViewModel(3, 32, 3, 1, mousePos.X, mousePos.Y, 60, 60);
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}

using System.IO;


namespace Builder.ViewModels.WorkspaceElements
{
    class WSPoolingViewModel : WorkspaceItemViewModel
    {
        private int _kernelSize;
        [Attributes.EditableProperty]
        public int KernelSize
        {
            get => _kernelSize;
            set
            {
                _kernelSize = value;
                OnPropertyChanged(nameof(KernelSize));
                DisplayName = $"{nameof(KernelSize)}";

            }
        }

        private int _stride;
        [Attributes.EditableProperty]
        public int Stride
        {
            get => _stride;
            set
            {
                _stride = value;
                OnPropertyChanged(nameof(Stride));
                DisplayName = $"{nameof(Stride)}";

            }
        }
        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = $"I:{KernelSize} O:{Stride}";
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public WSPoolingViewModel(int kernelSize, int stride, double x, double y, int width, int height, double opacity = 1, string displayName = "")
            : base(x, y, width, height, opacity, displayName)
        {
            KernelSize = kernelSize;
            Stride = stride;
            DisplayName = displayName;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "poolingl.jpg");
        }
    }
}

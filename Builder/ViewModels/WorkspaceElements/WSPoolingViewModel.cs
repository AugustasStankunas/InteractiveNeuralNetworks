using System.IO;
using System.Text.Json.Serialization;


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
                OnPropertyChanged(nameof(DisplayName));

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
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nK:{KernelSize} S:{Stride}";

        public WSPoolingViewModel(int kernelSize, int stride, double x, double y, int width, int height, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            KernelSize = kernelSize;
            Stride = stride;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "poolingl.jpg");
        }
    }
}

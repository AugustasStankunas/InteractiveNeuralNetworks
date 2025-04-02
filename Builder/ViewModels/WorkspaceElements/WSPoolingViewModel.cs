using Builder.Enums;
using System.IO;
using System.Text.Json.Serialization;
using Shared.Attributes;



namespace Builder.ViewModels.WorkspaceElements
{
    class WSPoolingViewModel : WorkspaceItemViewModel
    {
        private int _kernelSize;
        [EditableProperty]
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
        [EditableProperty]
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
        private PoolingType _poolingType;
        [EditableProperty("ComboBox")]
        public PoolingType PoolingType
        {
            get => _poolingType;
            set
            {
                _poolingType = value;
                OnPropertyChanged(nameof(PoolingType));
            }
        }
        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nK:{KernelSize} S:{Stride}";

        public WSPoolingViewModel(int kernelSize, int stride, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            KernelSize = kernelSize;
            Stride = stride;
            PoolingType = PoolingType.Average;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Poolingg.png");
        }
    }
}

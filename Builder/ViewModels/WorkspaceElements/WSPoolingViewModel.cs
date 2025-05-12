using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;



namespace Builder.ViewModels.WorkspaceElements
{
    class WSPoolingViewModel : WorkspaceItemViewModel
    {
        private int _kernelSize;
        [EditableProperty]
        [Description(
            "Size of the (square) pooling window in pixels.\n" +
            "Common values are 2 or 3; larger kernels reduce spatial dimensions more aggressively.\n" +
            "E.g. a 2×2 window halves width/height when used with stride=2."
        )]
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
        [Description(
            "Stride of the pooling window across the feature map.\n" +
            "A stride of 1 applies overlapping windows (minimal downsampling).\n" +
            "A stride equal to KernelSize yields non-overlapping pooling, downsampling by that factor."
        )]
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
        [Description(
            "Pooling operation to apply over each window:\n" +
            "• Max – takes the maximum value, preserving strongest activations.\n" +
            "• Average – computes the mean, yielding smoother feature maps."
        )]
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
        [JsonConstructor]
        public WSPoolingViewModel(int kernelSize, int stride, Point position, PoolingType poolingType,
                                  string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            KernelSize = kernelSize;
            Stride = stride;
            PoolingType = PoolingType.Average;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Poolingg.png");
        }
    }
}

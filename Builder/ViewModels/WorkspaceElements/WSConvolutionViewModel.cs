using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;



namespace Builder.ViewModels.WorkspaceElements
{
    class WSConvolutionViewModel : WorkspaceItemViewModel
    {
        private int _inputChannels;
        [EditableProperty]
        [Description(
            "Number of channels in the incoming feature map. " +
            "This must match the previous layer’s output depth (e.g. 3 for RGB images, 64 for deeper conv layers)."
        )]
        public int InputChannels
        {
            get => _inputChannels;
            set
            {
                _inputChannels = value;
                OnPropertyChanged(nameof(InputChannels));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        private int _outputChannels;
        [EditableProperty]
        [Description(
            "Number of convolution filters (i.e. output channels) this layer learns. \n" +
            "More filters let the network capture a richer set of features, " +
            "but also increase computation and memory usage."
        )]
        public int OutputChannels
        {
            get => _outputChannels;
            set
            {
                _outputChannels = value;
                OnPropertyChanged(nameof(OutputChannels));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        private int _kernelSize;
        [EditableProperty]
        [Description(
            "Size of the (square) convolution kernel, in pixels. " +
            "Common choices are 1, 3, or 5. Larger kernels see a wider context but add more parameters."
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
            "Stride (step) of the convolution window across the input. " +
            "A stride of 1 moves one pixel at a time (full resolution); \n" +
            "a stride >1 downsamples the spatial dimensions (e.g. stride=2 halves width/height)."
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
        private int _padding;

        [EditableProperty]
        [Description(
            "Padding applied to the input before the convolution operation. \n" +
            "Padding helps control the spatial dimensions of the output. \n" +
            "For example, padding=1 adds a one-pixel border around the input."
        )]
        public int Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                OnPropertyChanged(nameof(Padding));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nI:{InputChannels} O:{OutputChannels} \n K:{KernelSize} S:{Stride} P:{Padding}";

        public WSConvolutionViewModel(int inputChannels, int outputChannels, int kernelSize, int stride, int padding, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            InputChannels = inputChannels;
            OutputChannels = outputChannels;
            KernelSize = kernelSize;
            Stride = stride;
            Padding = padding;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Convolution.png");
        }

        [JsonConstructor]
        public WSConvolutionViewModel(int inputChannels, int outputChannels, int kernelSize, int stride, int padding, Point position,
                                      string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            InputChannels = inputChannels;
            OutputChannels = outputChannels;
            KernelSize = kernelSize;
            Stride = stride;
            Padding = padding;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Convolution.png");
        }
    }
}

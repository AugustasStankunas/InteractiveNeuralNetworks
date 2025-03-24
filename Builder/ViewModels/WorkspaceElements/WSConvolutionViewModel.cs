using System.IO;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;


namespace Builder.ViewModels.WorkspaceElements
{
    class WSConvolutionViewModel : WorkspaceItemViewModel
    {
        private int _inputChannels;
        [Attributes.EditableProperty]
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
        [Attributes.EditableProperty]
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
            $"{Name}\nI:{InputChannels} O:{OutputChannels} \n K:{KernelSize} S:{Stride}";

        public WSConvolutionViewModel(int inputChannels, int outputChannels, int kernelSize, int stride, double x, double y, int width, int height, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            InputChannels = inputChannels;
            OutputChannels = outputChannels;
            KernelSize = kernelSize;
            Stride = stride;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Convolution.png");
        }
    }
}

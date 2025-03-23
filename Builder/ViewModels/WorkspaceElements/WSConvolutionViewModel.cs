using System.IO;
using System.Text.RegularExpressions;


namespace Builder.ViewModels.WorkspaceElements
{
    class WSConvolutionViewModel : WorkspaceItemViewModel
    {
        /*private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(DisplayName));
                DisplayName = $"{nameof(Name)}";
            }
        }*/
        private int _inputChannels;
        [Attributes.EditableProperty]
        public int InputChannels
        {
            get => _inputChannels;
            set
            {
                _inputChannels = value;
                OnPropertyChanged(nameof(InputChannels));
                DisplayName = $"{nameof(InputChannels)}";
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
                DisplayName = $"{nameof(OutputChannels)}";
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
                _displayName = $"{Name}\nI:{InputChannels} O:{OutputChannels} \n K:{KernelSize} S:{Stride}";
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        public WSConvolutionViewModel(int inputChannels, int outputChannels, int kernelSize, int stride, double x, double y, int width, int height, double opacity = 1, string displayName = "")
            : base(x, y, width, height, opacity, displayName)
        {
       //     Name = name;
            InputChannels = inputChannels;
            OutputChannels = outputChannels;
            KernelSize = kernelSize;
            Stride = stride;
            DisplayName = displayName;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "convolutionall.jpg");
        }
    }
}

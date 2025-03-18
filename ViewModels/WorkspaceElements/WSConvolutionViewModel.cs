using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace InteractiveNeuralNetworks.ViewModels.WorkspaceElements
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
            }
        }
        public WSConvolutionViewModel(int inputChannels, int outputChannels, int kernelSize, int stride, double x, double y, int width, int height, double opacity = 1)
            : base(x, y, width, height,  opacity)
        {
            InputChannels = inputChannels;
            OutputChannels = outputChannels;
            KernelSize = kernelSize;
            Stride = stride;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "conv.png"); ;
        }
    }
}

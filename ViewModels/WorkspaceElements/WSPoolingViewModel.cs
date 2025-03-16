using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace InteractiveNeuralNetworks.ViewModels.WorkspaceElements
{
    class WSPoolingViewModel : WorkspaceItemViewModel
    {
        private int _kernelSize;
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
        public int Stride
        {
            get => _stride;
            set
            {
                _stride = value;
                OnPropertyChanged(nameof(Stride));
            }
        }

        public WSPoolingViewModel(int kernelSize, int stride, double x, double y, int width, int height, double opacity = 1)
            : base(x, y, width, height, opacity)
        {
            KernelSize = kernelSize;
            Stride = stride;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "pooling.png"); ;
        }
    }
}

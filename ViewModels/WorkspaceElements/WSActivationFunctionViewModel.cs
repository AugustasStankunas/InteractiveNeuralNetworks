using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace InteractiveNeuralNetworks.ViewModels.WorkspaceElements
{
    class WSActivationFunctionViewModel : WorkspaceItemViewModel
    {
        private string _activationFunction;
        public string ActivationFunction
        {
            get => _activationFunction;
            set
            {
                _activationFunction = value;
                OnPropertyChanged(nameof(ActivationFunction));
            }
        }

        public WSActivationFunctionViewModel(string activationFunction, double x, double y, int width, int height, string color, double opacity = 1)
            : base(x, y, width, height, color, opacity)
        {
            ActivationFunction = activationFunction;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "activationFunction.png"); ;
        }
       
    }
}

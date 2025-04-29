using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;


namespace Builder.ViewModels.WorkspaceElements
{
    class WSBatchNormViewModel : WorkspaceItemViewModel
    {
        private int _inputChannels;
        [EditableProperty]
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

        private double _momentum;
        [EditableProperty]
        public double Momentum
        {
            get => _momentum;
            set
            {
                _momentum = value;
                OnPropertyChanged(nameof(Momentum));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        private double _epsilon;
        [EditableProperty]
        public double Epsilon
        {
            get => _epsilon;
            set
            {
                _epsilon = value;
                OnPropertyChanged(nameof(Epsilon));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nM:{Momentum} E:{Epsilon}";

        public WSBatchNormViewModel(double momentum, double epsilon, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            Momentum = momentum;
            Epsilon = epsilon;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "BatchNorm.png");
        }

        [JsonConstructor]
        public WSBatchNormViewModel(double momentum, double epsilon, Point position, string name, ActivationFunctionType activationFunction, LayerType layer)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction, layerType: layer)
        {
            Momentum = momentum;
            Epsilon = epsilon;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "BatchNorm.png");
        }
    }
}

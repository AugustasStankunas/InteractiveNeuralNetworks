using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;
using System.ComponentModel;


namespace Builder.ViewModels.WorkspaceElements
{
    class WSBatchNormViewModel : WorkspaceItemViewModel
    {
        private int _inputChannels;
        [EditableProperty]
        [Description(
            "Number of channels (depth) in the incoming feature map. " +
            "This must match the number of feature maps produced by the previous layer. " +
            "For example, if your prior convolution has 64 filters, set InputChannels = 64."
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

        private double _momentum;
        [EditableProperty]
        [Description(
            "Momentum factor for the exponential moving average of batch statistics. " +
            "Typical values are between 0.9 and 0.99. \n" +
            "Higher values (closer to 1) yield more stable but slower-updating statistics; " +
            "lower values adapt faster but can introduce noise."
        )]
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
        [Description(
            "Small constant added to the variance term for numerical stability, " +
            "preventing division by zero. " +
            "Common values range from 1e-5 to 1e-3; larger epsilon increases safety " +
            "but may under-normalize."
        )]
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
        public WSBatchNormViewModel(double momentum, double epsilon, Point position, string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            Momentum = momentum;
            Epsilon = epsilon;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "BatchNorm.png");
        }
    }
}

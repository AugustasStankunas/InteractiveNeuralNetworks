using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;


namespace Builder.ViewModels.WorkspaceElements
{
    class WSDropoutViewModel : WorkspaceItemViewModel
    {
        private double _rate;
        [EditableProperty]
        public double Rate
        {
            get => _rate;
            set
            {
                _rate = value;
                OnPropertyChanged(nameof(Rate));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nR:{Rate}";

        public WSDropoutViewModel(double rate, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            Rate = rate;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Dropout.png");
        }

        [JsonConstructor]
        public WSDropoutViewModel(double rate, Point position, string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            Rate = rate;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Dropout.png");
        }
    }
}

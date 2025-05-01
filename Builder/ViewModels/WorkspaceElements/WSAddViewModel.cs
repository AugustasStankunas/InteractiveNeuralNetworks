using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSAddViewModel : WorkspaceItemViewModel
    {
        private int _numInputs;
        [EditableProperty]
        [Description("Number of inputs for the add operation.")]
        public int NumInputs
        {
            get => _numInputs;
            set
            {
                _numInputs = value;
                OnPropertyChanged(nameof(NumInputs));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nN:{NumInputs}";

        public WSAddViewModel(int numInputs, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            NumInputs = numInputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }

        [JsonConstructor]
        public WSAddViewModel(int numInputs, Point position, string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            NumInputs = numInputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}

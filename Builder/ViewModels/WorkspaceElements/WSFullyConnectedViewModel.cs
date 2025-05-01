using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;


namespace Builder.ViewModels.WorkspaceElements
{
    class WSFullyConnectedViewModel : WorkspaceItemViewModel
    {
        private int _inputNeurons;
        [EditableProperty]
        [Description(
            "Number of neurons in the incoming vector. " +
            "Must match the size of the previous layer’s output.\n" +
            "For example, if the prior layer outputs a 512‐dimensional feature, " +
            "set InputNeurons = 512."
        )]
        public int InputNeurons
        {
            get => _inputNeurons;
            set
            {
                _inputNeurons = value;
                OnPropertyChanged(nameof(InputNeurons));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private int _outputNeurons;
        [EditableProperty]
        [Description(
            "Number of neurons in this dense (fully-connected) layer. " +
            "This determines the dimensionality of the output vector.\n" +
            "Larger values increase model capacity (and parameter count) " +
            "but can also risk overfitting. Typical ranges are 32–1024."
        )]
        public int OutputNeurons
        {
            get => _outputNeurons;
            set
            {
                _outputNeurons = value;
                OnPropertyChanged(nameof(OutputNeurons));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nI:{InputNeurons} O:{OutputNeurons}";

        public WSFullyConnectedViewModel(int inputNeurons, int outputNeurons, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "fullyConnectedl.png");
        }

        [JsonConstructor]
        public WSFullyConnectedViewModel(int inputNeurons, int outputNeurons, Point position, string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "fullyConnectedl.png");
        }
    }
}

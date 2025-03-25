using System.IO;
using System.Text.Json.Serialization;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSFullyConnectedViewModel : WorkspaceItemViewModel
    {
        private int _inputNeurons;
        [Attributes.EditableProperty]
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
        [Attributes.EditableProperty]
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

        public WSFullyConnectedViewModel(int inputNeurons, int outputNeurons, double x, double y, int width = 60, int height = 60, double opacity = 1, string name="")
            : base(x, y, width, height, opacity, name)
        {
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "fullyConnectedl.png");
        }
    }
}

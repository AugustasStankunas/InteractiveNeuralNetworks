using System.IO;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSFullyConnectedViewModel : WorkspaceItemViewModel
    {
        private string _name1;
        public string Name1
        {
            get => _name1;
            set
            {
                _name1 = value;
                OnPropertyChanged(nameof(DisplayName));
                DisplayName = $"{nameof(Name1)}";
            }
        }
        private int _inputNeurons;
        [Attributes.EditableProperty]
        public int InputNeurons
        {
            get => _inputNeurons;
            set
            {
                _inputNeurons = value;
                OnPropertyChanged(nameof(InputNeurons));
                DisplayName = $"{nameof(InputNeurons)}";

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
                DisplayName = $"{nameof(OutputNeurons)}";

            }
        }
        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = $"{Name1}\nI:{InputNeurons} O:{OutputNeurons}";
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public WSFullyConnectedViewModel(int inputNeurons, int outputNeurons, string name1, double x, double y, int width, int height, double opacity = 1, string displayName="")
            : base(x, y, width, height, opacity, displayName)
        {
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;
            Name1 = name1;
            DisplayName = displayName;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "fullyConnectedl.png");
        }
    }
}

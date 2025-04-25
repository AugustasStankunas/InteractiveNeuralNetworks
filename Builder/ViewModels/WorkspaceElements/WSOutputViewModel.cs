using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;
using System.Collections.ObjectModel;
using System.Linq;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSOutputViewModel : WorkspaceItemViewModel
    {
        private int _numInputs;
        [EditableProperty]
        public int NumInputs
        {
            get => _numInputs;
            set
            {
                _numInputs = value;
                OnPropertyChanged(nameof(NumInputs));
                OnPropertyChanged(nameof(DisplayName));
                GenerateInputFields();
            }
        }

        private OutputType _outputParam;
        [EditableProperty("ComboBox")]
        public OutputType OutputParam
        {
            get => _outputParam;
            set
            {
                _outputParam = value;
                OnPropertyChanged(nameof(ActivationFunction));
            }
        }
        public ObservableCollection<string> InputValues { get; set; } = new();

        private void GenerateInputFields()
        {
            var oldValues = InputValues.ToList();

            InputValues.Clear();
            for (int i = 0; i < NumInputs; i++)
            {
                InputValues.Add(i < oldValues.Count ? oldValues[i] : "");
            }
            OnPropertyChanged(nameof(InputValues));
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nN:{NumInputs}";
        

        public WSOutputViewModel(int numInputs, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            NumInputs = numInputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }

        [JsonConstructor]
        public WSOutputViewModel(int numInputs, Point position, string name, ActivationFunctionType activationFunction, LayerType layer)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction, layerType: layer)
        {
            NumInputs = numInputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}

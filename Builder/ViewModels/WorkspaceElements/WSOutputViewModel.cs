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
        private int _numOutputs;
        [EditableProperty("GenTextBox")]
        public int NumOutputs
        {
            get => _numOutputs;
            set
            {
                _numOutputs = value;
                OnPropertyChanged(nameof(NumOutputs));
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
            for (int i = 0; i < NumOutputs; i++)
            {
                InputValues.Add(i < oldValues.Count ? oldValues[i] : "");
            }
            OnPropertyChanged(nameof(InputValues));
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nN:{NumOutputs}";
        
        public WSOutputViewModel(int numOutputs, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            NumOutputs = numOutputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }

        [JsonConstructor]
        public WSOutputViewModel(int numOutputs, Point position, string name, ActivationFunctionType activationFunction, LayerType layer)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction, layerType: layer)
        {
            NumOutputs = numOutputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}

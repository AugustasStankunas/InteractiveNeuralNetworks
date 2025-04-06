using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shared.Attributes;
using Builder.Enums;
using System.Windows;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSAddViewModel: WorkspaceItemViewModel
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
        public WSAddViewModel(int numInputs, Point position, string name, ActivationFunctionType activationFunction, LayerType layer)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction, layerType: layer)
        {
            NumInputs = numInputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}

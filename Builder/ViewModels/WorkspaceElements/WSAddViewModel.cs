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
        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}";

        public WSAddViewModel(double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }

        [JsonConstructor]
        public WSAddViewModel(int numInputs, Point position, string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}

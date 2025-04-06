using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSFlattenViewModel : WorkspaceItemViewModel
    {
        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}";

        public WSFlattenViewModel(double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Flatten.png");
        }

        [JsonConstructor]
        public WSFlattenViewModel(Point position, string name, ActivationFunctionType activationFunction, LayerType layer)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction, layerType: layer)
        {
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Flatten.png");
        }
    }
}

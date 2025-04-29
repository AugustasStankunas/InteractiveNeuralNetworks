using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSInputViewModel : WorkspaceItemViewModel
    {
        private int _imageHeight;
        [EditableProperty]
        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                OnPropertyChanged(nameof(_imageHeight));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        private int _imageWidth;
        [EditableProperty]
        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                OnPropertyChanged(nameof(_imageWidth));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        private int _numChannels;
        [EditableProperty]
        public int NumChannels
        {
            get => _numChannels;
            set
            {
                _numChannels = value;
                OnPropertyChanged(nameof(_numChannels));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nH:{ImageHeight} W:{ImageWidth} C:{NumChannels}";

        public WSInputViewModel(int imageHeight, int imageWidth, int numChannels, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
            NumChannels = numChannels;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }

        [JsonConstructor]
        public WSInputViewModel(int imageHeight, int imageWidth, int numChannels, Point position, string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
            NumChannels = numChannels;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}

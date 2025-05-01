using System.ComponentModel;
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
        [Description(
            "Height of the input image in pixels.\n" +
            "Typical values include 28 (MNIST), 32 (CIFAR), 224 (ImageNet).\n" +
            "Must match the height of your training images."
        )]
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
        [Description(
            "Width of the input image in pixels.\n" +
            "Typical values include 28 (MNIST), 32 (CIFAR), 224 (ImageNet).\n" +
            "Must match the width of your training images."
        )]
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
        [Description(
            "Number of color channels in the input image.\n" +
            "Use 1 for grayscale, 3 for RGB, or more for multi-spectral data.\n" +
            "Ensure this matches the channels in your dataset."
        )]
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

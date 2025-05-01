using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using Builder.Enums;
using Builder.ViewModels.WorkspaceElements;
using Shared.Attributes;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(WSConvolutionViewModel), "Conv2D")]
    [JsonDerivedType(typeof(WSFullyConnectedViewModel), "Linear")]
    [JsonDerivedType(typeof(WSPoolingViewModel), "Pool2D")]
    [JsonDerivedType(typeof(WSAddViewModel), "Add")]
    [JsonDerivedType(typeof(WSBatchNormViewModel), "BatchNorm")]
    [JsonDerivedType(typeof(WSFlattenViewModel), "Flatten")]
    [JsonDerivedType(typeof(WSDropoutViewModel), "Dropout")]
    [JsonDerivedType(typeof(WSInputViewModel), "Input")]
    [JsonDerivedType(typeof(WSOutputViewModel), "Output")]


    public class WorkspaceItemViewModel : ViewModelBase
    {
        public string _Name;
        [EditableProperty(Priority = true)]
        [Description("Variable name of the workspace item later used in code. Must be unique.")]
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private bool _IsSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                if (IsSelected) Border = 2;
                else Border = 0;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        [JsonIgnore]
        public Point SelectionPosition { get; set; }

        private int _Border;
        [JsonIgnore]
        public int Border
        {
            get => _Border;
            set
            {
                _Border = value;
                OnPropertyChanged(nameof(Border));
            }
        }


        private Point _Position;
        public Point Position
        {
            get => _Position;
            set
            {
                _Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }



        private string _iconPath = default!;
        [JsonIgnore]
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }

        private int _Width;
        [JsonIgnore]
        public int Width
        {
            get => _Width;
            set
            {
                _Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private int _Height;
        [JsonIgnore]
        public int Height
        {
            get => _Height;
            set
            {
                _Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private double _Opacity;
        [JsonIgnore]
        public double Opacity
        {
            get => _Opacity;
            set
            {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }

        private FaceDirection? _markerDirection;
        [JsonIgnore]
        public FaceDirection? MarkerDirection
        {
            get => _markerDirection;
            set
            {
                _markerDirection = value;
                OnPropertyChanged(nameof(MarkerDirection));
            }
        }

        [JsonIgnore]
        public virtual string DisplayName =>
            $"{Name}";

        private ActivationFunctionType _activationFunction;
        [EditableProperty("ComboBox")]
        [Description(
            "Activation function to apply after this layer:\n" +
            "• None       – identity; no activation is applied.\n" +
            "• Sigmoid    – squeezes outputs into (0,1); good for binary probabilities.\n" +
            "• ReLU       – rectified linear unit (max(0,x)); speeds up training and sparsifies activations.\n" +
            "• Tanh       – hyperbolic tangent, maps to (–1,1); zero-centered.\n" +
            "• Linear     – identity; useful for regression tasks.\n" +
            "• SoftMax    – converts a vector of logits into a probability distribution across classes."
        )]
        public ActivationFunctionType ActivationFunction
        {
            get => _activationFunction;
            set
            {
                _activationFunction = value;
                OnPropertyChanged(nameof(ActivationFunction));
            }
        }
        
        public WorkspaceItemViewModel(double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "",
                                      ActivationFunctionType activationFunction = ActivationFunctionType.None)
        {
            Position = new Point(x, y);
            Width = width;
            Height = height;
            Opacity = opacity;
            Name = name;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "defaultIcon.png");
            ActivationFunction = activationFunction;
            MarkerDirection = null;
        } 

        public WorkspaceItemViewModel() { }

        public override bool Equals(object? obj)
        {
            if (obj is not WorkspaceItemViewModel other || obj == null)
                return false;

            WorkspaceItemViewModel otherItem = (WorkspaceItemViewModel)obj;
            return Position == otherItem.Position && Name == otherItem.Name;
        }
    }
}

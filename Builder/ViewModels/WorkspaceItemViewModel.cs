using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Builder.Enums;
using Shared.ViewModels;
using System.Text.Json.Serialization;
using Builder.ViewModels.WorkspaceElements;


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

    public class WorkspaceItemViewModel : ViewModelBase
    {
        public string _Name;
        [Attributes.EditableProperty(Priority = true)]
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

                if (value)
                {
                    Mouse.AddMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
                }
            }
        }
        public void GlobalMouseUpHandler(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                IsSelected = false;
                Mouse.RemoveMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
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
        [JsonIgnore]
        public virtual string DisplayName =>
            $"{Name}";

        private ActivationFunctionType _activationFunction;
        [Attributes.EditableProperty("ComboBox")]
        public ActivationFunctionType ActivationFunction
        {
            get => _activationFunction;
            set
            {
                _activationFunction = value;
                OnPropertyChanged(nameof(ActivationFunction));
            }
        }

        public WorkspaceItemViewModel(double x, double y, int width = 60, int height = 60, double opacity = 1, string name="")
        {
            Position = new Point(x, y);
            Width = width;
            Height = height;
            Opacity = opacity;
            Name = name;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "defaultIcon.png");
            ActivationFunction = ActivationFunctionType.None;
        }

        public WorkspaceItemViewModel() { }
    }
}

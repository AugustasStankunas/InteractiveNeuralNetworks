using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Builder.Enums;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    public class WorkspaceItemViewModel : ViewModelBase
    {
        private string _Name = "text";
        [Attributes.EditableProperty(Priority = true)]
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged(Name);
                //DisplayName = $"{nameof(Name)}";
            }
        }

        private bool _IsSelected;
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

        public Point SelectionPosition { get; set; }

        private int _Border;
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

        private Point _StablePosition;
        public Point StablePosition
        {
            get => _StablePosition;
            set
            {
                _StablePosition = value;
                OnPropertyChanged(nameof(StablePosition));
            }
        }

        private string _iconPath = default!;
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
        public double Opacity
        {
            get => _Opacity;
            set
            {
                _Opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
        private string _DisplayName;
        public string DisplayName
        {
            get => _DisplayName;
            set
            {
                _DisplayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

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

        public WorkspaceItemViewModel(double x, double y, int width, int height, double opacity = 1, string displayName="")
        {
            Position = new Point(x, y);
            StablePosition = new Point(x, y);
            Width = width;
            Height = height;
            Opacity = opacity;
            DisplayName = displayName;
           // Name = name;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "defaultIcon.png");
            ActivationFunction = ActivationFunctionType.None;
        }

        public WorkspaceItemViewModel() { }
    }
}

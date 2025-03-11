using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using InteractiveNeuralNetworks.Commands;
using System.IO;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class WorkspaceItemViewModel : ViewModelBase
    {
        private bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                if (IsSelected) Border = 1;
                else Border = 0;
                OnPropertyChanged(nameof(IsSelected));
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
        private string _Color;
        public string Color
        {
            get => _Color;
            set
            {
                _Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private string _iconPath;
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

        public WorkspaceItemViewModel(double x, double y, int width, int height, string color,  double opacity = 1)
        {
            Position = new Point(x, y);
            StablePosition = new Point(x, y);
            Color = color;
            Width = width;
            Height = height;
            Opacity = opacity;
          //  IconPath = iconName;
        }
       
        public WorkspaceItemViewModel() { }
    }
}

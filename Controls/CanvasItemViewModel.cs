using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace InteractiveNeuralNetworks.Controls
{
    internal class CanvasItemViewModel : ViewModelBase
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

        public System.Windows.Point SelectionPosition { get; set; }

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


        private System.Windows.Point _Position;
        public System.Windows.Point Position
        {
            get => _Position;
            set
            {
                _Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private System.Windows.Point _StablePosition;
        public System.Windows.Point StablePosition
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

        public CanvasItemViewModel(double x, double y, int width, int height, string color)
        {
            Position = new System.Windows.Point(x, y);
            StablePosition = new System.Windows.Point(x, y);
            Color = color;
            Width = width;
            Height = height;
        }

        public void CheckIfSelected(MouseEventArgs e)
        {
            System.Windows.Point mousePos = e.GetPosition((IInputElement)e.Source);
            System.Windows.Point relativeMousePos = e.GetPosition((IInputElement)e.OriginalSource);
            if (mousePos.X >= StablePosition.X && mousePos.X <= StablePosition.X + Width &&
                mousePos.Y >= StablePosition.Y && mousePos.Y <= StablePosition.Y + Height)
            {
                IsSelected = true;
                SelectionPosition = relativeMousePos;
            }
        }
    }
}

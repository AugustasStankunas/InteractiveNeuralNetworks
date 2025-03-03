using InteractiveNeuralNetworks.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InteractiveNeuralNetworks.ViewModels
{
    internal class CanvasViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

        private Point _MousePos;
        public Point MousePos 
        {
            get => _MousePos;
            set
            {
                _MousePos = value;
                OnPropertyChanged(nameof(MousePos));
            }
        }

        private Point _CurrentPos;
        public Point CurrentPos
        {
            get => _CurrentPos;
            set
            {
                _CurrentPos = value;
                OnPropertyChanged(nameof(CurrentPos));
            }
        }

        private Point _MouseMoveVector;
        public Point MouseMoveVector
        {
            get => _MouseMoveVector;
            set
            {
                _MouseMoveVector = value;
                OnPropertyChanged(nameof(MouseMoveVector));
            }
        }

        private Point _CanvasViewPortStablePos;
        private Point _CanvasViewPortPos;
        public Point CanvasViewPortPos
        {
            get => _CanvasViewPortPos;
            set
            {
                _CanvasViewPortPos = value;
                OnPropertyChanged(nameof(CanvasViewPortPos));
            }
        }

        //private System.Windows.Point _MouseSelectedMoveVector;
        //public System.Windows.Point MouseSelectedMoveVector
        //{
        //    get => _MouseSelectedMoveVector;
        //    set
        //    {
        //        _MouseSelectedMoveVector = value;
        //        OnPropertyChanged(nameof(MouseSelectedMoveVector));
        //    }
        //}

        private Point _mouseStart;
        private bool mouseLeftButtonDown = false;

        public ObservableCollection<CanvasItemViewModel> CanvasItems { get; set; } = new ObservableCollection<CanvasItemViewModel>();

        public CanvasViewModel()
        {
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            MouseLeftButtonDownCommand = new RelayCommand<MouseEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseEventArgs>(OnMouseLeftButtonUp);

            CanvasViewPortPos = new Point(0, 0);
            _CanvasViewPortStablePos = new Point(0, 0);

            CanvasItems.Add(new CanvasItemViewModel(0, 0, 60, 60, "Orange"));
            CanvasItems.Add(new CanvasItemViewModel(105, 123, 50, 50, "Pink"));
            CanvasItems.Add(new CanvasItemViewModel(220, 330, 75, 75, "LightBlue"));
        }

        // Mouse 
        private void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            mouseLeftButtonDown = true;
            _mouseStart = e.GetPosition((IInputElement)e.Source);

            foreach (var item in CanvasItems)
            {
                item.CheckIfSelected(e);
            }
        }
        private void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            mouseLeftButtonDown = false;

            _CanvasViewPortStablePos = new Point(
                CanvasViewPortPos.X, CanvasViewPortPos.Y);

            foreach (var item in CanvasItems)
            {
                if (!item.IsSelected)
                {
                    item.StablePosition = new Point(
                        item.StablePosition.X + MouseMoveVector.X,
                        item.StablePosition.Y + MouseMoveVector.Y);
                }
                else
                {
                    item.StablePosition = new Point(
                        item.Position.X,
                        item.Position.Y);
                    item.IsSelected = false;
                }
            }
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            Point mousePos = e.GetPosition((IInputElement)e.Source);
            MousePos = mousePos;

            if (mouseLeftButtonDown)
            {
                bool isAnySelected = false;
                foreach (var item in CanvasItems)
                {
                    if (item.IsSelected)
                    {
                        MouseMoveVector = new Point(0, 0);
                        Point relativeToItemMousePos = e.GetPosition((IInputElement)e.OriginalSource);

                        item.Position = new Point(
                            MousePos.X - item.SelectionPosition.X,
                            MousePos.Y - item.SelectionPosition.Y);

                        isAnySelected = true;
                    }
                }

                if (!isAnySelected)
                {
                    MouseMoveVector = (Point)(mousePos - _mouseStart);
                    CanvasViewPortPos = new Point(
                        _CanvasViewPortStablePos.X + MouseMoveVector.X,
                        _CanvasViewPortStablePos.Y + MouseMoveVector.Y );

                    foreach (var item in CanvasItems)
                    {
                        item.Position = new Point(
                            item.StablePosition.X + MouseMoveVector.X,
                            item.StablePosition.Y + MouseMoveVector.Y);
                    }
                } 
            }          
        }
    }
}

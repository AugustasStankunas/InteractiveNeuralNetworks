using InteractiveNeuralNetworks.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InteractiveNeuralNetworks.Controls
{
    internal class CanvasViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

        private System.Windows.Point _MousePos;
        public System.Windows.Point MousePos 
        {
            get => _MousePos;
            set
            {
                _MousePos = value;
                OnPropertyChanged(nameof(MousePos));
            }
        }

        private System.Windows.Point _CurrentPos;
        public System.Windows.Point CurrentPos
        {
            get => _CurrentPos;
            set
            {
                _CurrentPos = value;
                OnPropertyChanged(nameof(CurrentPos));
            }
        }

        private System.Windows.Point _MouseMoveVector;
        public System.Windows.Point MouseMoveVector
        {
            get => _MouseMoveVector;
            set
            {
                _MouseMoveVector = value;
                OnPropertyChanged(nameof(MouseMoveVector));
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

        private System.Windows.Point _mouseStart;
        private bool mouseLeftButtonDown = false;

        public ObservableCollection<CanvasItemViewModel> CanvasItems { get; set; } = new ObservableCollection<CanvasItemViewModel>();

        public CanvasViewModel()
        {
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            MouseLeftButtonDownCommand = new RelayCommand<MouseEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseEventArgs>(OnMouseLeftButtonUp);

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

            foreach (var item in CanvasItems)
            {
                if (!item.IsSelected)
                {
                    item.StablePosition = new System.Windows.Point(
                        item.StablePosition.X + MouseMoveVector.X,
                        item.StablePosition.Y + MouseMoveVector.Y);
                }
                else
                {
                    item.StablePosition = new System.Windows.Point(
                        item.Position.X,
                        item.Position.Y);
                    item.IsSelected = false;
                }
            }
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            System.Windows.Point mousePos = e.GetPosition((IInputElement)e.Source);
            MousePos = mousePos;

            if (mouseLeftButtonDown)
            {
                bool isAnySelected = false;
                foreach (var item in CanvasItems)
                {
                    if (item.IsSelected)
                    {
                        MouseMoveVector = new System.Windows.Point(0, 0);
                        System.Windows.Point relativeToItemMousePos = e.GetPosition((IInputElement)e.OriginalSource);

                        item.Position = new System.Windows.Point(
                            MousePos.X - item.SelectionPosition.X,
                            MousePos.Y - item.SelectionPosition.Y);

                        isAnySelected = true;
                    }
                }

                if (!isAnySelected)
                {
                    MouseMoveVector = (System.Windows.Point)(mousePos - _mouseStart);

                    foreach (var item in CanvasItems)
                    {
                        item.Position = new System.Windows.Point(
                            item.StablePosition.X + MouseMoveVector.X,
                            item.StablePosition.Y + MouseMoveVector.Y);
                    }
                } 
            }          
        }
    }
}

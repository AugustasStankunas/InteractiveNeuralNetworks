using InteractiveNeuralNetworks.Commands;
using InteractiveNeuralNetworks.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand MouseWheelCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

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

        private double _zoomFactor = 1.0;
        public double ZoomFactor
        {
            get => _zoomFactor;
            set { _zoomFactor = value; OnPropertyChanged(nameof(ZoomFactor)); }
        }

        private Point _canvasPanOffset;
        public Point CanvasPanOffset
        {
            get => _canvasPanOffset;
            set { _canvasPanOffset = value; OnPropertyChanged(nameof(CanvasPanOffset)); }
        }

        private Point mouseOffset; // The offset of the mouse to the top left corner of the rectangle
        
        //For panning
        private bool _isPanning;
        private Point _panStart;

        public ObservableCollection<WorkspaceItemViewModel> WorkspaceItems { get; set; } = new ObservableCollection<WorkspaceItemViewModel>();

        public WorkspaceViewModel()
        {

            CanvasViewPortPos = new Point(0, 0);
            _CanvasViewPortStablePos = new Point(0, 0);

            WorkspaceItems.Add(new WorkspaceItemViewModel(0, 0, 60, 60, "Orange"));
            WorkspaceItems.Add(new WorkspaceItemViewModel(105, 123, 50, 50, "Pink"));
            WorkspaceItems.Add(new WorkspaceItemViewModel(220, 330, 75, 75, "LightBlue"));

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            DragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);

            MouseWheelCommand = new RelayCommand<MouseWheelEventArgs>(OnMouseWheel);
            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);

        }
        //fires when mouse moved, but action happens when mouse is pressed on a rectangle and then moved - drag action starts
        

        private void OnDragOver(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(WorkspaceItemViewModel)))
            {
                var draggedItem = e.Data.GetData(typeof(WorkspaceItemViewModel)) as WorkspaceItemViewModel;
                Point dropPosition = e.GetPosition(e.Source as IInputElement);
                if (draggedItem != null)
                {
                    dropPosition.X -= mouseOffset.X;
                    dropPosition.Y -= mouseOffset.Y;
                    draggedItem.Position = dropPosition;
                }
                    
            }
            
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            Point mousePos;
            if (e.OriginalSource is Rectangle)
            {
                mousePos = e.GetPosition(e.Source as IInputElement);
            }
            else
            {
                mousePos = e.GetPosition(e.OriginalSource as IInputElement);
            }
            double oldZoom = ZoomFactor;
            double zoomDelta = e.Delta > 0 ? 0.1 : -0.1;
            ZoomFactor = Math.Max(0.1, ZoomFactor + zoomDelta);

            CanvasPanOffset = new Point(
                CanvasPanOffset.X - mousePos.X * (ZoomFactor - oldZoom),
                CanvasPanOffset.Y - mousePos.Y * (ZoomFactor - oldZoom));
        }

        private void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                var data = e.OriginalSource as FrameworkElement;
                if (data != null && e.LeftButton == MouseButtonState.Pressed && data.DataContext is WorkspaceItemViewModel)
                {
                    mouseOffset = e.GetPosition(data); // mouse position relative to top-left of the rectangle
                    DragDrop.DoDragDrop(e.OriginalSource as UIElement, new DataObject(typeof(WorkspaceItemViewModel), data.DataContext), DragDropEffects.Move);
                }
            }
            else
            {
                _isPanning = true;
                _panStart = e.GetPosition(null);
            }
            
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            if (_isPanning)
            {
                Point currentPosition = e.GetPosition(null);
                Vector delta = currentPosition - _panStart;
                CanvasPanOffset = new Point(CanvasPanOffset.X + delta.X, CanvasPanOffset.Y + delta.Y);
                _panStart = currentPosition;
            }
        }

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _isPanning = false;
        }

    }
}

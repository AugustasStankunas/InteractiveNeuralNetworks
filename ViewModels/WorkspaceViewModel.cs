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
using System.Windows.Input;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand ClickMeButtonComand { get; }

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

        private Point mouseOffset; // The offset of the mouse to the top left corner of the rectangle

        public ObservableCollection<WorkspaceItemViewModel> WorkspaceItems { get; set; } = new ObservableCollection<WorkspaceItemViewModel>();

        public WorkspaceViewModel()
        {

            CanvasViewPortPos = new Point(0, 0);
            _CanvasViewPortStablePos = new Point(0, 0);

            WorkspaceItems.Add(new WorkspaceItemViewModel(0, 0, 60, 60, "Orange"));
            WorkspaceItems.Add(new WorkspaceItemViewModel(105, 123, 50, 50, "Pink"));
            WorkspaceItems.Add(new WorkspaceItemViewModel(220, 330, 75, 75, "LightBlue"));

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(Rectangle_MouseMove);
            DragOverCommand = new RelayCommand<DragEventArgs>(Rectangle_DragOver);

        }
        //fires when mouse moved, but action happens when mouse is pressed on a rectangle and then moved - drag action starts
        private void Rectangle_MouseMove(MouseEventArgs e)
        {
            var data = e.OriginalSource as FrameworkElement;
            if (data != null && e.LeftButton == MouseButtonState.Pressed && data.DataContext is WorkspaceItemViewModel)
            {
                mouseOffset = e.GetPosition(data); // mouse position relative to top-left of the rectangle
                DragDrop.DoDragDrop(e.OriginalSource as UIElement, new DataObject(typeof(WorkspaceItemViewModel), data.DataContext), DragDropEffects.Move);
            }
        }

        private void Rectangle_DragOver(DragEventArgs e)
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
    }
}

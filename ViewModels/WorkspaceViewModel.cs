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
    public class WorkspaceViewModel : ViewModelBase
    {
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

        public ObservableCollection<WorkspaceItemViewModel> WorkspaceItems { get; set; } = new ObservableCollection<WorkspaceItemViewModel>();

        public WorkspaceViewModel()
        {

            CanvasViewPortPos = new Point(0, 0);
            _CanvasViewPortStablePos = new Point(0, 0);

            WorkspaceItems.Add(new WorkspaceItemViewModel(0, 0, 60, 60, "Orange"));
            WorkspaceItems.Add(new WorkspaceItemViewModel(105, 123, 50, 50, "Pink"));
            WorkspaceItems.Add(new WorkspaceItemViewModel(220, 330, 75, 75, "LightBlue"));
        }
    }
}

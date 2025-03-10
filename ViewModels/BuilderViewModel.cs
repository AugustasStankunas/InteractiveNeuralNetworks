using InteractiveNeuralNetworks.Commands;
using InteractiveNeuralNetworks.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class BuilderViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public BuilderModel Model { get; set; }
        public WorkspaceViewModel WorkspaceViewModel { get; set; }
        public ToolbarViewModel ToolbarViewModel { get; set; }
        public ObservableCollection<WorkspaceItemViewModel> WorkspaceItemSelected { get; set; } = new();

        // Hello world
        public RelayCommand ClickMeButtonCommand { get; set; }
        public int Counter
        {
            get => Model.ClickCounter;
            set
            {
                Model.ClickCounter = value;
                OnPropertyChanged(nameof(Counter));
            }
        }
 
        public BuilderViewModel()
        {
            Model = new BuilderModel();
            WorkspaceViewModel = new WorkspaceViewModel(this);
            ToolbarViewModel = new ToolbarViewModel(this);

            ClickMeButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
        }

        // Button methods
        private void ExecuteClickMe(object obj)
        {
            Counter++;
        }
        private bool CanExecuteClickMe(object obj)
        {
            return true;
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            if (WorkspaceItemSelected.Count > 0)
                WorkspaceItemSelected[0].Position = mousePos;
        }

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            WorkspaceItemSelected.Clear();
        }
    }
}

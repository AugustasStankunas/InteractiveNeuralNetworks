using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Builder.Commands;
using Builder.Models;
using Builder.ViewModels.WorkspaceElements;
using Shared.ViewModels;
using Shared.Commands;

namespace Builder.ViewModels
{
    public class BuilderViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

        internal bool isMakingConnection = false;
        internal WSConnectionViewModel connectionInProgress;

        public BuilderModel Model { get; set; }
        public WorkspaceViewModel WorkspaceViewModel { get; set; }
        public ToolbarViewModel ToolbarViewModel { get; set; }
        public PropertiesWindowViewModel PropertiesWindowViewModel { get; set; }
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
            PropertiesWindowViewModel = new PropertiesWindowViewModel(this);

            connectionInProgress = new WSConnectionViewModel();

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

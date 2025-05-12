using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Builder.Models;
using Shared.Commands;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    public class BuilderViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

        internal bool isMakingConnection = false;

        public BuilderModel Model { get; set; }
        public WorkspaceViewModel WorkspaceViewModel { get; set; }
        public ToolbarViewModel ToolbarViewModel { get; set; }
        public PropertiesWindowViewModel PropertiesWindowViewModel { get; set; }
        public ObservableCollection<WorkspaceItemViewModel> WorkspaceItemSelected { get; set; } = new();

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
            PropertiesWindowViewModel = new PropertiesWindowViewModel();

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
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

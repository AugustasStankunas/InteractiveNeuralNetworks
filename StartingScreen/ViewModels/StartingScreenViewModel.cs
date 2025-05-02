using System.Windows.Input;
using Shared.Commands;
using Shared.ViewModels;

namespace StartingScreen.ViewModels
{
    public class StartingScreenViewModel : ViewModelBase
    {
        public ICommand CreateNewProjectCommand { get; private set; }
        public ICommand OpenExistingProjectCommand { get; private set; }

        private object _mainWindow;
        public void SetMainWindow(object mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public StartingScreenViewModel()
        {
            CreateNewProjectCommand = new RelayCommand(CreateNewProject);
            OpenExistingProjectCommand = new RelayCommand(OpenExistingProject);
        }

        private void CreateNewProject(object parameter)
        {
            var method = _mainWindow.GetType().GetMethod("ShowBuilder");
            method.Invoke(_mainWindow, null);
        }

        private void OpenExistingProject(object parameter)
        {
            var method = _mainWindow.GetType().GetMethod("Load");
            method.Invoke(_mainWindow, null);
        }

    }
}

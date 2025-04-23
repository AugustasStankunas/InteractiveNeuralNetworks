using Microsoft.Win32;
using Shared.Commands;
using Shared.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace StartingScreen.ViewModels
{
	public class StartingWindowViewModel : ViewModelBase
	{
		public ICommand CreateNewProjectCommand { get; private set; }
		public ICommand OpenExistingProjectCommand { get; private set; }
		public ICommand CloseWindowCommand { get; private set; }

		public StartingWindowViewModel()
		{
			CreateNewProjectCommand = new RelayCommand(ExecuteCreateNewProject);
			OpenExistingProjectCommand = new RelayCommand(ExecuteOpenExistingProject);
		}

		private void ExecuteCreateNewProject(object parameter)
		{
			LaunchMainApplication();
			Application.Current.Shutdown();
		}

		private void ExecuteOpenExistingProject(object parameter)
		{
		}

		private void LaunchMainApplication(string filePath = null)
		{

			var mainAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MainApp.exe");
			var startInfo = new System.Diagnostics.ProcessStartInfo
			{
				FileName = mainAppPath,
				UseShellExecute = true
			};
			System.Diagnostics.Process.Start(startInfo);
			
		}

		

	}
}

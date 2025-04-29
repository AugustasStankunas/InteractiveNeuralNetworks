using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Builder.ViewModels;
using Microsoft.Win32;
using Shared.Commands;
using Shared.ViewModels;
using Test.ViewModels;
using Train.Helpers;
using Train.ViewModels;
using StartingScreen.ViewModels;


namespace MainApp.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private object _currentViewModel = default!;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        private BuilderViewModel Builder { get; set; }
        private TrainViewModel Train { get; set; }
        private TestViewModel Test { get; set; }
        private StartingScreenViewModel Start { get; set; }


        public ICommand ShowBuilderCommand { get; }
        public ICommand ShowTrainCommand { get; }
        public ICommand ShowTestCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand KeyboardSaveCommand { get; }
        public ICommand LoadCommand { get; }

        private string _workingFilePath = "";
        public string WorkingFilePath
        {
            get => _workingFilePath;
            set
            {
                _workingFilePath = value;
                OnPropertyChanged(nameof(WorkingFilePath));
            }
        }

        private string _pythonServerConfigPath;
        public string PythonServerConfigPath
        {
            get => _pythonServerConfigPath;
            set
            {
                _pythonServerConfigPath = value;
                OnPropertyChanged(nameof(PythonServerConfigPath));
            }
        }

        public MainWindowViewModel()
        {
            Builder = new BuilderViewModel();
            Train = new TrainViewModel();
            Test = new TestViewModel();
            Start = new StartingScreenViewModel();

            PythonServerConfigPath = "../../../../Python-server/config.json";

            Start.SetMainWindow(this);

            ShowBuilderCommand = new RelayCommand(_ => ShowBuilder());
            ShowTrainCommand = new RelayCommand(_ => ShowTrain());
            ShowTestCommand = new RelayCommand(_ => ShowTest());

            SaveCommand = new RelayCommand(_ => Save());
            SaveAsCommand = new RelayCommand(_ => SaveAs());
			KeyboardSaveCommand = new RelayCommand<KeyEventArgs>(keyEvent => {
				if (keyEvent != null && keyEvent.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
					Save();
			});
            LoadCommand = new RelayCommand(_ => Load());

            ShowStart();

        }

        public void ShowBuilder()
        {
            CurrentViewModel = Builder;
        }

        private void ShowTrain()
        {
            CurrentViewModel = Train;
        }

        private void ShowTest()
        {
            CurrentViewModel = Test;
        }
        private void ShowStart()
        {
            CurrentViewModel = Start;
        }

        private void SaveAs()
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "WorkspaceConfiguration";
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON documents (.json)|*.json";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string jsonItems = JsonSerializer.Serialize(Builder.WorkspaceViewModel.WorkspaceItems, new JsonSerializerOptions { WriteIndented = true });
                string jsonConnections = JsonSerializer.Serialize(Builder.WorkspaceViewModel.WorkspaceConnections, new JsonSerializerOptions { WriteIndented = true });
                string jsonHyperparameters = JsonSerializer.Serialize(Train, new JsonSerializerOptions { WriteIndented = true });
                var data = new CompositeType 
                {
                    Items = Builder.WorkspaceViewModel.WorkspaceItems,
                    Connections = Builder.WorkspaceViewModel.WorkspaceConnections,
                    Train = Train
                };
                string filename = dialog.FileName;
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(PythonServerConfigPath, json);
                File.WriteAllText(filename, json);
                WorkingFilePath = filename;
            }
        }

        private void Save()
        {

            if (string.IsNullOrEmpty(WorkingFilePath))
            {
					SaveAs();
                    return;
            }

			var data = new CompositeType 
			{
				Items = Builder.WorkspaceViewModel.WorkspaceItems,
				Connections = Builder.WorkspaceViewModel.WorkspaceConnections,
				Train = Train
			};
			string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(PythonServerConfigPath, json);
            File.WriteAllText(WorkingFilePath, json);
        }

        public void Load()
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = "WorkspaceConfiguration";
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON documents (.json)|*.json";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                string json = File.ReadAllText(filename);
                CompositeType? compositeObject = JsonSerializer.Deserialize<CompositeType>(File.ReadAllText(filename));

                if (compositeObject != null)
                {
                    Builder.WorkspaceViewModel.UpdateItemsAndConnections(compositeObject.Items, compositeObject.Connections);
                    File.WriteAllText(PythonServerConfigPath, json);
                    WorkingFilePath = filename;
                    ShowBuilder();
                }
                else
                {
                    MessageBox.Show("Failed to load workspace configuration.");
                }
            }
        }

    }
}

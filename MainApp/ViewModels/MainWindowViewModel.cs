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


        public ICommand ShowBuilderCommand { get; }
        public ICommand ShowTrainCommand { get; }
        public ICommand ShowTestCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand KeyboardSaveCommand { get; }
        public ICommand LoadCommand { get; }

        public MainWindowViewModel()
        {
            Builder = new BuilderViewModel();
            Train = new TrainViewModel();
            Test = new TestViewModel();

            ShowBuilderCommand = new RelayCommand(_ => ShowBuilder());
            ShowTrainCommand = new RelayCommand(_ => ShowTrain());
            ShowTestCommand = new RelayCommand(_ => ShowTest());

            SaveCommand = new RelayCommand(_ => Save());
			KeyboardSaveCommand = new RelayCommand<KeyEventArgs>(keyEvent => {
				if (keyEvent != null && keyEvent.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
					Save();
			});
            LoadCommand = new RelayCommand(_ => Load());

            ShowBuilder();
        }

        private void ShowBuilder()
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

        private void Save()
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
                File.WriteAllText(filename, json);
            }
        }

        private void Load()
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = "WorkspaceConfiguration";
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON documents (.json)|*.json";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                CompositeType? compositeObject = JsonSerializer.Deserialize<CompositeType>(File.ReadAllText(filename));

                if (compositeObject != null)
                {
                    Builder.WorkspaceViewModel.UpdateItemsAndConnections(compositeObject.Items, compositeObject.Connections);
                    Train = compositeObject.Train;
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

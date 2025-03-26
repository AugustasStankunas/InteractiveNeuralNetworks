using System.Windows.Input;
using Shared.ViewModels;
using Shared.Commands;
using System.Windows;
using Builder.Models;
using Builder.ViewModels;
using Train.ViewModels;
using Test.ViewModels;
using Microsoft.Win32;
using System.Text.Json;
using System.IO;


namespace MainApp.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private object _currentViewModel;
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

        public MainWindowViewModel()
        {
            Builder = new BuilderViewModel();
            Train = new TrainViewModel();
            Test = new TestViewModel();

            ShowBuilderCommand = new RelayCommand(_ => ShowBuilder());
            ShowTrainCommand = new RelayCommand(_ => ShowTrain());
            ShowTestCommand = new RelayCommand(_ => ShowTest());

            SaveCommand = new RelayCommand(_ => Save());

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
                string filename = dialog.FileName;
                string jsonItems = JsonSerializer.Serialize(Builder.WorkspaceViewModel.WorkspaceItems, new JsonSerializerOptions { WriteIndented = true });
                string jsonConnections = JsonSerializer.Serialize(Builder.WorkspaceViewModel.WorkspaceConnections, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filename, jsonItems);
                File.AppendAllText(filename, jsonConnections);
            }
        }

    }
}

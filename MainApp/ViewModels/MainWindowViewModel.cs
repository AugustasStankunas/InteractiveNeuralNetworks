using System.Windows.Input;
using Shared.ViewModels;
using Shared.Commands;
using System.Windows;
using Builder.Models;
using Builder.ViewModels;
using Train.ViewModels;
using Test.ViewModels;


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

        public MainWindowViewModel()
        {
            Builder = new BuilderViewModel();
            Train = new TrainViewModel();
            Test = new TestViewModel();

            ShowBuilderCommand = new RelayCommand(_ => ShowBuilder());
            ShowTrainCommand = new RelayCommand(_ => ShowTrain());
            ShowTestCommand = new RelayCommand(_ => ShowTest());

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
    }
}

using System.Text.Json.Serialization;
using Builder.Enums;
using Shared.Attributes;
using Shared.Commands;
using Shared.ViewModels;
using System.IO;
using Microsoft.Win32;
using System.Text.Json;
using System.Net.Http.Json;
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Windows.Controls;
using Train.Helpers;



namespace Train.ViewModels
{
    public class TrainViewModel : ViewModelBase
    {
        private double _learningRate;
        [EditableProperty]
        public double LearningRate
        {
            get => _learningRate;
            set
            {
                _learningRate = value;
                OnPropertyChanged(nameof(LearningRate));
            }
        }

        private LossFunctionType _lossFunction;
        [EditableProperty("ComboBox")]
        public LossFunctionType LossFunction
        {
            get => _lossFunction;
            set
            {
                _lossFunction = value;
                OnPropertyChanged(nameof(LossFunction));
            }
        }

        private int _batchSize;
        [EditableProperty]
        public int BatchSize
        {
            get => _batchSize;
            set
            {
                _batchSize = value;
                OnPropertyChanged(nameof(BatchSize));
            }
        }
        [JsonIgnore]
        public HyperparametersWindowViewModel HyperparametersWindowViewModel { get; set; }

        public RelayCommand GetDirectoryButtonCommand { get; set; }

        public TrainViewModel()
        {
            InitializeHyperparameters();
            HyperparametersWindowViewModel = new HyperparametersWindowViewModel(this);
            GetDirectoryButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
        }

        private void InitializeHyperparameters()
        {
            LearningRate = 0.01;
            LossFunction = LossFunctionType.MSE;
            BatchSize = 32;
        }
        public string TrainDataPath { get; set; }


        private void ExecuteClickMe(object obj)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK) //'TrainData': '../../' \n'ValData': '../../
            {
                TrainDataPath = dialog.SelectedPath;
            }
        }
        private bool CanExecuteClickMe(object obj)
        {
            return true;
        }
    }
}

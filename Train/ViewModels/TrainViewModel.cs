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
using Builder.ViewModels;

using Shared.Commands;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Builder;

namespace Train.ViewModels
{
    public class TrainViewModel : ViewModelBase
    {
        [JsonIgnore]
        public WorkspaceViewModel WorkspaceViewModel { get; set; }
        [JsonIgnore]
        public RelayCommand ClickMeButtonCommand { get; set; }

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

        [JsonIgnore]
        public RelayCommand GetDirectoryButtonCommand { get; set; }
        private string _outputText = "";
        [JsonIgnore]
        public string OutputText
        {
            get => _outputText;
            set 
            { 
                _outputText = value; 
                OnPropertyChanged(nameof(OutputText)); 
            }
        }

        [JsonIgnore]
        public ICommand RefreshLogCommand { get; }

        [JsonIgnore]
        private FileSystemWatcher _logFileWatcher;
        [JsonIgnore]
        private string _logFilePath;
        [JsonIgnore]
        private DispatcherTimer _refreshTimer;
        [JsonIgnore]
        private DateTime _lastReadTime = DateTime.MinValue;

        public TrainViewModel()
        {
            WorkspaceViewModel = new WorkspaceViewModel();
            InitializeHyperparameters();
            HyperparametersWindowViewModel = new HyperparametersWindowViewModel(this);
            GetDirectoryButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
            ClickMeButtonCommand = new RelayCommand(ExecuteTrainClickMe, CanExecuteTrainClickMe);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(baseDirectory, "../../../../Python-server/log.txt");
            InitializeWatcher();
            RefreshLogCommand = new RelayCommand(_ => RefreshLogContent());
            
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

        private void InitializeWatcher()
        {
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _refreshTimer.Tick += (s, e) => RefreshLogContent();
            _refreshTimer.Start();

            if (File.Exists(_logFilePath))
            {
                string directory = Path.GetDirectoryName(_logFilePath);
                string filename = Path.GetFileName(_logFilePath);

                _logFileWatcher = new FileSystemWatcher(directory, filename)
                {
                    EnableRaisingEvents = true,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                };
                _logFileWatcher.Changed += (s, e) => System.Windows.Application.Current.Dispatcher.Invoke(RefreshLogContent);

            }
            else
                OutputText = "The log file does not exist: " + _logFilePath;

            RefreshLogContent();
        }


        public async void RefreshLogContent()
        {
            try
            {
                var fileInfo = new FileInfo(_logFilePath);
                if (!fileInfo.Exists || fileInfo.LastWriteTime <= _lastReadTime)
                    return;

                _lastReadTime = fileInfo.LastWriteTime;

                using (var fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    OutputText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                OutputText = $"Error reading log file: {ex.Message}";
            }
        }

        private void ExecuteTrainClickMe(object obj)
        {
            string jsonItems = JsonSerializer.Serialize(WorkspaceViewModel.WorkspaceItems, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
            string jsonConnections = JsonSerializer.Serialize(WorkspaceViewModel.WorkspaceConnections, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("output.json", "{\n\"Items \":");
            File.AppendAllText("output.json", jsonItems);
            File.AppendAllText("output.json", ",\n");
            File.AppendAllText("output.json", "\"Connections \": ");
            File.AppendAllText("output.json", jsonConnections);
            File.AppendAllText("output.json", "\n}");
            PythonRunner.RunScript();
        }
        private bool CanExecuteTrainClickMe(object obj)
        {
            return true;
        }




    }
}

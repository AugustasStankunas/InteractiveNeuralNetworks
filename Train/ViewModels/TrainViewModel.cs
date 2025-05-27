using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Windows.Threading;
using Builder;
using Builder.Enums;
using Builder.ViewModels;
using Shared.Attributes;
using Shared.Commands;
using Shared.ViewModels;
using Train.Helpers;
using Train.Datasets;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.MessageBox;

namespace Train.ViewModels
{
    public class TrainViewModel : ViewModelBase, IDisposable
    {
        [JsonIgnore]
        public WorkspaceViewModel WorkspaceViewModel { get; set; }
        [JsonIgnore]
        public RelayCommand ClickMeButtonCommand { get; set; }


        private double _learningRate;
        [EditableProperty]
        [Description(
           "Learning rate for the optimizer (gradient descent step size). " +
           "Typical values range from 1e-6 (very fine updates) up to 1.0 (very coarse). " +
           "Common defaults are 0.001 or 0.01.")]
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
        [Description(
             "Which loss function to minimize during training.\n" +
             "Options include:\n" +
             "• MSE (Mean Squared Error) – average of squared differences, ideal for regression tasks.\n" +
             "• MAE (Mean Absolute Error) – average of absolute differences, more robust to outliers.\n" +
             "• BinaryCrossEntropy – log loss for binary classification (outputs between 0 and 1).\n" +
             "• CategoricalCrossEntropy – for multi-class classification; compares predicted probability distributions to true labels.")]
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
        [Description(
           "Number of training samples processed in one forward/backward pass. " +
           "Larger batch sizes (e.g. 128, 256, 1024) give more stable gradients " +
           "but use more memory. Typical range: 1 to 65536.")]
        public int BatchSize
        {
            get => _batchSize;
            set
            {
                _batchSize = value;
                OnPropertyChanged(nameof(BatchSize));
            }
        }

        private bool _horizontalFlip;
        [EditableProperty("CheckBox")]
        [Description(
           "Augmentation that mirrors the image along the vertical axis (left-right). " +
           "Useful for tasks where left-right symmetry is acceptable, like object detection or classification.")]
        public bool HorizontalFlip
        {
            get => _horizontalFlip;
            set
            {
                _horizontalFlip = value;
                OnPropertyChanged(nameof(HorizontalFlip));
            }
        }

        private bool _verticalFlip;
        [EditableProperty("CheckBox")]
        [Description(
           "Augmentation that mirrors the image along the horizontal axis (top-bottom). " +
           "Use with caution as some tasks (e.g. digit recognition) are not invariant to vertical flips.")]
        public bool VerticalFlip
        {
            get => _verticalFlip;
            set
            {
                _verticalFlip = value;
                OnPropertyChanged(nameof(VerticalFlip));
            }
        }

        private bool _pad;
        [EditableProperty("CheckBox")]
        [Description(
           "Augmentation type which adds extra space (usually filled with zeros or a constant value) around the image. " +
           "Can help preserve content when applying transformations like cropping or rotation.")]
        public bool Pad
        {
            get => _pad;
            set
            {
                _pad = value;
                OnPropertyChanged(nameof(Pad));
            }
        }
        private bool _zoomOut;
        [EditableProperty("CheckBox")]
        [Description(
           "Augmentation that scales down the image and places it within a larger canvas. " +
           "This simulates smaller objects and improves robustness to scale variance.")]
        public bool ZoomOut
        {
            get => _zoomOut;
            set
            {
                _zoomOut = value;
                OnPropertyChanged(nameof(ZoomOut));
            }
        }

        private bool _rotation;
        [EditableProperty("CheckBox")]
        [Description(
           "Augmentation type which randomly rotates the image around its center. " +
           "Helps the model learn rotational invariance, particularly in tasks where orientation varies.")]
        public bool Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                OnPropertyChanged(nameof(Rotation));
            }
        }

        private bool _affine;
        [EditableProperty("CheckBox")]
        [Description(
           "Augmentation type which applies linear transformations like translation, scaling, rotation, and shearing. " +
           "Useful for simulating geometric distortions and improving generalization.")]
        public bool Affine
        {
            get => _affine;
            set
            {
                _affine = value;
                OnPropertyChanged(nameof(Affine));
            }
        }
        private bool _perspective;
        [EditableProperty("CheckBox")]
        [Description(
           "Augmentation type which warps the image to simulate 3D perspective effects. " +
           "Helps the model become invariant to viewpoint and projective distortions.")]
        public bool Perspective
        {
            get => _perspective;
            set
            {
                _perspective = value;
                OnPropertyChanged(nameof(Perspective));
            }
        }

        [JsonIgnore]
        public HyperparametersWindowViewModel HyperparametersWindowViewModel { get; set; }
        [JsonIgnore]
        public ChartViewModel ChartViewModel { get; set; }

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

        [JsonIgnore]
        public RelayCommand DownloadDatasetCommand { get; set; }

        private string _selectedDataset;
        public string SelectedDataset
        {
            get => _selectedDataset;
            set
            {
                _selectedDataset = value;
                OnPropertyChanged(nameof(SelectedDataset));
            }
        }

        public TrainViewModel()
        {
            WorkspaceViewModel = new WorkspaceViewModel();
            InitializeHyperparameters();
            HyperparametersWindowViewModel = new HyperparametersWindowViewModel(this);
            ChartViewModel = new ChartViewModel();
            GetDirectoryButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
            ClickMeButtonCommand = new RelayCommand(ExecuteTrainClickMe, CanExecuteTrainClickMe);
            DownloadDatasetCommand = new RelayCommand(ExecuteDownloadDataset, CanExecuteDownloadDataset);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(baseDirectory, "../../../../Python-server/log.txt");
            InitializeWatcher();
            RefreshLogCommand = new RelayCommand(_ => RefreshLogContent());
        }

        private void InitializeHyperparameters()
        {
            LearningRate = 0.01;
            LossFunction = LossFunctionType.CategoricalCrossEntropy;
            BatchSize = 8192;
            HorizontalFlip = false;
            VerticalFlip = false;
            Pad = false;
            ZoomOut = false;
            Rotation = false;
            Affine = false;
            Perspective = false;
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

        private async void ExecuteDownloadDataset(object obj)
        {
            if (string.IsNullOrEmpty(SelectedDataset))
            {
                System.Windows.MessageBox.Show("Please select a dataset to download.", "No Dataset Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                System.Windows.MessageBox.Show($"Starting download of {SelectedDataset}...", "Download Started", MessageBoxButton.OK, MessageBoxImage.Information);
                var (success, downloadPath) = await DatasetDownloader.DownloadDatasetAsync(SelectedDataset);
                if (success && !string.IsNullOrEmpty(downloadPath))
                {
                    TrainDataPath = downloadPath;
                    System.Windows.MessageBox.Show($"Dataset downloaded successfully to: {downloadPath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show("Download failed or was cancelled.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error downloading dataset: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteDownloadDataset(object obj)
        {
            return true;
        }

        public void Dispose()
        {
            try
            {
                //DatasetDownloader.Shutdown();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error during cleanup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

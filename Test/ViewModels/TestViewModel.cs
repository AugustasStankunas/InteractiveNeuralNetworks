using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using Shared.Commands;
using Shared.ViewModels;
using System.Windows.Media.Imaging;
using Builder;

namespace Test.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        public string TestDataPath { get; set; }
        public string ModelPath { get; set; }
        public RelayCommand GetModelButtonCommand { get; set; }
        public RelayCommand GetImageButtonCommand { get; set; }
        public RelayCommand TestButtonCommand { get; set; }
        public ICommand RefreshLogCommand { get; }

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

        private string _pythonServerModelConfigPath;
        public string PythonServerModelConfigPath
        {
            get => _pythonServerModelConfigPath;
            set
            {
                _pythonServerModelConfigPath = value;
                OnPropertyChanged(nameof(PythonServerModelConfigPath));
            }
        }

        private BitmapImage _selectedImage;
        public BitmapImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        private void ExecuteClickMe(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                SelectedImage = new BitmapImage(new Uri(dialog.FileName));
                TestDataPath = dialog.FileName;
                var data = new { PredictDataPath = TestDataPath };
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(PythonServerConfigPath, json);
            }

        }
        private bool CanExecuteClickMe(object obj)
        {
            return true;
        }

        private void ExecuteGetModel(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Model Files|*.pt;*.pth;";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                ModelPath = dialog.FileName;
                var data = new { InferenceModelPath = ModelPath };
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(PythonServerModelConfigPath, json);
            }

        }
        private bool CanExecuteGetModel(object obj)
        {
            return true;
        }


        public TestViewModel()
        {
            PythonServerConfigPath = "../../../../Python-server/predictImage.json";
            PythonServerModelConfigPath = "../../../../Python-server/predictModel.json";

            GetModelButtonCommand = new RelayCommand(ExecuteGetModel, CanExecuteGetModel);
            GetImageButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
            TestButtonCommand = new RelayCommand(ExecuteTestClickMe, CanExecuteTestClickMe);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(baseDirectory, "../../../../Python-server/log_inference.txt");
            InitializeWatcher();
            RefreshLogCommand = new RelayCommand(_ => RefreshLogContent());
        }


        private void ExecuteTestClickMe(object obj)
        {
            //juozapai dirbk nafyk!
            PythonRunner.RunScript();
        }
        private bool CanExecuteTestClickMe(object obj)
        {
            return true;
        }

        private FileSystemWatcher _logFileWatcher;
        private string _logFilePath;
        private DispatcherTimer _refreshTimer;
        private DateTime _lastReadTime = DateTime.MinValue;
        private string _outputText = "";
        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged(nameof(OutputText));
            }
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
    }
}

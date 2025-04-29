using System.Text.Json.Serialization;
using Shared.Attributes;
using Shared.Commands;
using Shared.ViewModels;
using System.IO;
using Microsoft.Win32;
using System.Text.Json;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;


using Shared.Commands;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Test.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        public string TestDataPath { get; set; }
        public RelayCommand GetImageButtonCommand { get; set; }
        public RelayCommand TestButtonCommand { get; set; }
        public ICommand RefreshLogCommand { get; }


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
            }
        }
        private bool CanExecuteClickMe(object obj)
        {
            return true;
        }
        public TestViewModel()
        {
            GetImageButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
            TestButtonCommand = new RelayCommand(ExecuteTestClickMe, CanExecuteTestClickMe);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(baseDirectory, "../../../../Python-server/log1.txt");
            InitializeWatcher();
            RefreshLogCommand = new RelayCommand(_ => RefreshLogContent());
        }
        private void ExecuteTestClickMe(object obj)
        {
            //juozapai dirbk nafyk!
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

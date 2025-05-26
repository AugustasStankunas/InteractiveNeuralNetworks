using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Threading;
using Shared.ViewModels;
using System.Globalization;
using System.Text;

namespace Train.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private readonly string _logFilePath;
        private FileSystemWatcher _watcher;

        private ChartValues<ObservablePoint> _trainingLossValues = new ChartValues<ObservablePoint>();
        private ChartValues<ObservablePoint> _validationLossValues = new ChartValues<ObservablePoint>();

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

        public ChartViewModel()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(baseDirectory, "../../../../Python-server/log1.txt");

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Training loss",
                    Values = _trainingLossValues,
                    StrokeThickness = 2,
                    PointGeometrySize = 8,
                },
                new LineSeries
                {
                    Title = "Validation loss",
                    Values = _validationLossValues,
                    StrokeThickness = 2,
                    PointGeometrySize = 8,
                }
            };

            YFormatter = value => value.ToString("N");

            SetupFileWatcher();
            LoadFile(); 
        }

        private void SetupFileWatcher()
        {
            var directory = Path.GetDirectoryName(_logFilePath);
            var fileName = Path.GetFileName(_logFilePath);

            _watcher = new FileSystemWatcher(directory, fileName)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            _watcher.Changed += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(LoadFile);
            };
        }

        private void LoadFile()
        {
            try
            {
                using (var fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    OutputText = reader.ReadToEnd();
                }

                ParseAndUpdateChart();
            }
            catch (Exception ex)
            {
                OutputText = $"Error reading log1 file: {ex.Message}";
            }
        }

        private void ParseAndUpdateChart()
        {
            _trainingLossValues.Clear();
            _validationLossValues.Clear();

            string[] lines = OutputText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int epoch = 0;

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"Train loss:\s*([\d.]+)\s+Validation loss:\s*([\d.]+)");

                if (match.Success)
                {
                    _trainingLossValues.Add(new ObservablePoint(epoch, double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture)));
                    _validationLossValues.Add(new ObservablePoint(epoch, double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture)));
                    epoch++;
                }
            }
        }
    }
}

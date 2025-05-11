using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Threading;
using Shared.ViewModels;

namespace Train.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private readonly string _logFilePath;
        private readonly DispatcherTimer _refreshTimer;

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

            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(2);
            _refreshTimer.Tick += (s, e) => RefreshAndUpdateChart();
            _refreshTimer.Start();
        }

        private async void RefreshAndUpdateChart()
        {
            try
            {
                using (var fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    OutputText = await reader.ReadToEndAsync();
                }

                CreateLinesFromOutput();
            }
            catch (Exception ex)
            {
                OutputText = $"Error reading log1 file: {ex.Message}";
            }
        }

        private void CreateLinesFromOutput()
        {
            _trainingLossValues.Clear();
            _validationLossValues.Clear();

            string[] lines = OutputText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int epoch = 0;

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"Train loss:\s*([\d.]+)\s+Validation loss:\s*([\d.]+)");

                if (match.Success &&
                    double.TryParse(match.Groups[1].Value, out double trainLoss) &&
                    double.TryParse(match.Groups[2].Value, out double valLoss))
                {
                    _trainingLossValues.Add(new ObservablePoint(epoch, trainLoss));
                    _validationLossValues.Add(new ObservablePoint(epoch, valLoss));
                    epoch++;
                }
            }
        }
    }
}

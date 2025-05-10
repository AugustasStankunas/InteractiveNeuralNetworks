using System.Collections.ObjectModel;
using System.Windows.Input;
using Shared.Attributes;
using Shared.ViewModels;
using Train.Helpers;
using Shared.Commands;
using Train.Datasets;
using System.Windows.Controls;

namespace Train.ViewModels
{
    public class HyperparametersWindowViewModel : ViewModelBase
    {
        private TrainViewModel _Trainer;
        public TrainViewModel Trainer
        {
            get => _Trainer;
            set
            {
                _Trainer = value;

                IEnumerable<System.Reflection.PropertyInfo> properties = Trainer.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(EditableProperty)));
                Properties = new();
                foreach (var property in properties)
                {
                    if (((EditableProperty)Attribute.GetCustomAttribute(property, typeof(EditableProperty))).Priority)
                        Properties.Add(new HyperparameterInfoViewModel(Trainer, property));
                }
                foreach (var property in properties)
                {
                    if (!((EditableProperty)Attribute.GetCustomAttribute(property, typeof(EditableProperty))).Priority)
                        Properties.Add(new HyperparameterInfoViewModel(Trainer, property));
                }

                OnPropertyChanged(nameof(Trainer));
            }
        }

        private ComboBoxItem _selectedDataset;
        public ComboBoxItem SelectedDataset
        {
            get => _selectedDataset;
            set
            {
                _selectedDataset = value;
                OnPropertyChanged(nameof(SelectedDataset));
            }
        }

        ObservableCollection<HyperparameterInfoViewModel>? _Properties;
        public ObservableCollection<HyperparameterInfoViewModel>? Properties
        {
            get => _Properties;
            set
            {
                _Properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }

        public ICommand DownloadDatasetCommand { get; }

        public HyperparametersWindowViewModel(TrainViewModel trainer)
        {
            Trainer = trainer;

            DownloadDatasetCommand = new RelayCommand(async (obj) =>
            {
                if (SelectedDataset != null)
                {
                    await DatasetDownloader.DownloadDatasetAsync(SelectedDataset.Content.ToString());
                }
            });
        }
    }
}

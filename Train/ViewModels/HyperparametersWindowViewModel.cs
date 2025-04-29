using System.Collections.ObjectModel;
using Shared.Attributes;
using Shared.ViewModels;
using Train.Helpers;


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

        public HyperparametersWindowViewModel(TrainViewModel trainer)
        {
            Trainer = trainer;
        }
        public List<AugmentationItem> SettingsTree { get; set; }

        public HyperparametersWindowViewModel()
        {
            SettingsTree = new List<AugmentationItem>
        {
            new AugmentationItem
            {
                Name = "Flip",
                IsChecked = true,
                Children = new List<AugmentationItem>
                {
                    new AugmentationItem { Name = "Horizontal", IsChecked = true },
                    new AugmentationItem { Name = "Vertical", IsChecked = false }
                }
            },
            new AugmentationItem
            {
                Name = "Color Adjust",
                Children = new List<AugmentationItem>
                {
                    new AugmentationItem { Name = "Brightness" },
                    new AugmentationItem { Name = "Contrast" }
                }
            }
        };
        }
    }
}

using System.Collections.ObjectModel;
using Builder.Attributes;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    public class PropertiesWindowViewModel : ViewModelBase
    {
        public BuilderViewModel Builder { get; set; }

        WorkspaceItemViewModel _SelectedWorkspaceItem;
        public WorkspaceItemViewModel SelectedWorkspaceItem
        {
            get => _SelectedWorkspaceItem;
            set
            {
                _SelectedWorkspaceItem = value;

                IEnumerable<System.Reflection.PropertyInfo> properties = SelectedWorkspaceItem.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(Attributes.EditableProperty)));
                ObservableCollection<PropertyInfoViewModel> propertiesVM = new();
                foreach (var property in properties)
                {
                    if (((EditableProperty)Attribute.GetCustomAttribute(property, typeof(EditableProperty))).Priority)
                        propertiesVM.Add(new PropertyInfoViewModel(SelectedWorkspaceItem, property));
                }
                foreach (var property in properties)
                {
                    if (!((EditableProperty)Attribute.GetCustomAttribute(property, typeof(EditableProperty))).Priority)
                        propertiesVM.Add(new PropertyInfoViewModel(SelectedWorkspaceItem, property));
                }

                Properties = propertiesVM;
                OnPropertyChanged(nameof(SelectedWorkspaceItem));
            }
        }

        ObservableCollection<PropertyInfoViewModel>? _Properties;
        public ObservableCollection<PropertyInfoViewModel>? Properties
        {
            get => _Properties;
            set
            {
                _Properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }

        public PropertiesWindowViewModel(BuilderViewModel builderViewModel)
        {
            Builder = builderViewModel;
        }
    }
}

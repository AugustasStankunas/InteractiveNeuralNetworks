using System.Collections.ObjectModel;
using System.Reflection;
using Shared.Attributes;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    public class PropertyInfoViewModel : ViewModelBase
    {
        WorkspaceItemViewModel WorkspaceItem { get; set; }
        PropertyInfo PropertyInfo { get; set; }
        public string ControlType { get; set; }
        public string Name { get; set; }

        string _Value;
        public string Value
        {
            get => _Value;
            set
            {
                _Value = value;
                if (PropertyInfo.PropertyType == typeof(int))
                {
                    PropertyInfo.SetValue(WorkspaceItem, int.Parse(_Value));
                }
                else if (PropertyInfo.PropertyType == typeof(double))
                {
                    PropertyInfo.SetValue(WorkspaceItem, double.Parse(_Value));
                }
                else if (PropertyInfo.PropertyType.IsEnum)
                {
                    PropertyInfo.SetValue(WorkspaceItem, Enum.Parse(PropertyInfo.PropertyType, _Value));
                }
                else
                {
                    PropertyInfo.SetValue(WorkspaceItem, _Value);
                }

                OnPropertyChanged(Value);
            }
        }
        public ObservableCollection<string> InputValues
        {
            get
            {
                var prop = WorkspaceItem.GetType().GetProperty("InputValues");
                if (prop != null)
                {
                    return prop.GetValue(WorkspaceItem) as ObservableCollection<string>;
                }
                return null;
            }
        }

        public string[] Options { get; set; }

        public PropertyInfoViewModel(WorkspaceItemViewModel WorkspaceItem, PropertyInfo propertyInfo)
        {
            this.WorkspaceItem = WorkspaceItem;
            PropertyInfo = propertyInfo;
            ControlType = ((EditableProperty)Attribute.GetCustomAttribute(propertyInfo, typeof(EditableProperty))).ControlType;

            switch (propertyInfo.Name)
            {
                case "InputNeurons":
                    Name = "Input Neurons";
                    break;
                case "OutputNeurons":
                    Name = "Output Neurons";
                    break;
                case "ActivationFunction":
                    Name = "Activation Function";
                    break;
                case "InputChannels":
                    Name = "Input Channels";
                    break;
                case "OutputChannels":
                    Name = "Output Channels";
                    break;
                case "KernelSize":
                    Name = "Kernel Size";
                    break;
                case "PoolingType":
                    Name = "Pooling Type";
                    break;
                default:
                    Name = propertyInfo.Name;
                    break;
            }
            Value = propertyInfo.GetValue(WorkspaceItem).ToString();

            if (ControlType == "ComboBox")
            {
                Options = propertyInfo.PropertyType.GetEnumNames().Cast<string>().ToArray();
            }
            
        }
    }
}

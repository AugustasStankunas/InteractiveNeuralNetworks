using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using InteractiveNeuralNetworks.Attributes;
using InteractiveNeuralNetworks.Enums;

namespace InteractiveNeuralNetworks.ViewModels
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

        public string[] Options { get; set; }

        public PropertyInfoViewModel(WorkspaceItemViewModel WorkspaceItem, PropertyInfo propertyInfo)
        {
            this.WorkspaceItem = WorkspaceItem;
            PropertyInfo = propertyInfo;
            ControlType = ((EditableProperty)Attribute.GetCustomAttribute(propertyInfo, typeof(EditableProperty))).ControlType;

            Name = propertyInfo.Name;
            Value = propertyInfo.GetValue(WorkspaceItem).ToString();

            if (ControlType == "ComboBox")
            {
                Options = propertyInfo.PropertyType.GetEnumNames().Cast<string>().ToArray();
            }
        }
    }
}

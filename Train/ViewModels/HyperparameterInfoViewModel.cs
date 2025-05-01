using System.ComponentModel;
using System.IO;
using System.Reflection;
using Shared.Attributes;
using Shared.ViewModels;

namespace Train.ViewModels
{
    public class HyperparameterInfoViewModel : ViewModelBase
    {
        TrainViewModel Trainer { get; set; }
        PropertyInfo PropertyInfo { get; set; }
        public string ControlType { get; set; }
        public string Name { get; set; }
        public string TooltipText { get; }

        string _Value;
        public string Value
        {
            get => _Value;
            set
            {
                _Value = value;
                if (PropertyInfo.PropertyType == typeof(int))
                {
                    PropertyInfo.SetValue(Trainer, int.Parse(_Value));
                }
                else if (PropertyInfo.PropertyType == typeof(double))
                {
                    PropertyInfo.SetValue(Trainer, double.Parse(_Value));
                }
                else if (PropertyInfo.PropertyType.IsEnum)
                {
                    PropertyInfo.SetValue(Trainer, Enum.Parse(PropertyInfo.PropertyType, _Value));
                }
                else
                {
                    PropertyInfo.SetValue(Trainer, _Value);
                }

                OnPropertyChanged(Value);
            }
        }

        public string[] Options { get; set; }

        public HyperparameterInfoViewModel(TrainViewModel trainer, PropertyInfo propertyInfo)
        {
            Trainer = trainer;
            PropertyInfo = propertyInfo;
            ControlType = ((EditableProperty)Attribute.GetCustomAttribute(propertyInfo, typeof(EditableProperty))).ControlType;

            Name = propertyInfo.Name;
            Value = propertyInfo.GetValue(Trainer).ToString();

            if (ControlType == "ComboBox")
            {
                Options = propertyInfo.PropertyType.GetEnumNames().Cast<string>().ToArray();
            }

            var descAttr = propertyInfo.GetCustomAttribute<DescriptionAttribute>(inherit: false);

            TooltipText = descAttr?.Description ?? "No description available.";
        }
    }
}

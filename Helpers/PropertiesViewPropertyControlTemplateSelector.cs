using System.Windows;
using System.Windows.Controls;
using Builder.ViewModels;

namespace Builder.Helpers
{
    public class PropertiesViewPropertyControlTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextBoxTemplate { get; set; }
        public DataTemplate ComboBoxTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var obj = item as PropertyInfoViewModel;
            if (obj == null) return base.SelectTemplate(item, container);

            switch (obj.ControlType)
            {
                case "TextBox":
                    return TextBoxTemplate;
                case "ComboBox":
                    return ComboBoxTemplate;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}

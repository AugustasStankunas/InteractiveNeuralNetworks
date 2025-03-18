using InteractiveNeuralNetworks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveNeuralNetworks.Helpers
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

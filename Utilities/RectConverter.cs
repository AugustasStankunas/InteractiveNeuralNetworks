using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace InteractiveNeuralNetworks.Utilities
{
    public class RectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Expect the value to be a Point holding X and Y
            if (value is Point pt)
            {
                // The parameter is expected to be a comma-separated string: "width,height"
                string[] parts = ((string)parameter).Split(',');
                if (parts.Length != 2)
                    return DependencyProperty.UnsetValue;
                double width = double.Parse(parts[0]);
                double height = double.Parse(parts[1]);
                // Create a Rect using the bound X and Y, with fixed width and height
                return new Rect(pt.X, pt.Y, width, height);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

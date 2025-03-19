using System.Globalization;
using System.Windows.Data;


namespace Builder.Converters
{
    /// <summary>
    /// Used for creating connections between layers
    /// </summary>
    class CenterXConverter : IMultiValueConverter

    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double x && values[1] is int width)
                return x + (double)width / 2;
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

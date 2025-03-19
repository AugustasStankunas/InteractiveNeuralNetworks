using System.Windows.Data;

namespace Builder.Converters
{
    class CenterYConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double y && values[1] is int height)
                return y + (double)height / 2;
            return Binding.DoNothing;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

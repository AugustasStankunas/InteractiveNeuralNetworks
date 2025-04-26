using System;
using System.Globalization;
using System.Windows.Data;
using Builder.Enums;

namespace Builder.Converters
{
    /// <summary>
    /// Given an endpoint coordinate (double) and a FaceDirection asa well as item width and height, returns
    /// an adjusted coordinate so an arrow sits just outside the item.
    /// ConverterParameter must be "X" or "Y".
    /// </summary>
    public class FaceDirectionToOffsetConverter : IMultiValueConverter
    {
        private const double Gap = 4.0;  // extra spacing outside the item

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] = coord (double)
            // values[1] = FaceDirection
            // values[2] = item Width (double)
            // values[3] = item Height (double)
            if (values.Length < 4
                || !(values[0] is double coord)
                || !(values[1] is FaceDirection dir)
                || !(values[2] is int width)
                || !(values[3] is int height)
                || !(parameter is string axis))
            {
                return Binding.DoNothing;
            }

            double halfW = width / 2;
            double halfH = height / 2;

            return axis switch
            {
                "X" when dir == FaceDirection.Left => coord - halfW,
                "X" when dir == FaceDirection.Right => coord + width,
                "Y" when dir == FaceDirection.Left => coord,
                "Y" when dir == FaceDirection.Right => coord + halfW,
                "Y" when dir == FaceDirection.Top => coord - halfH,
                "Y" when dir == FaceDirection.Bottom => coord + halfH,

                // if axis doesn’t match the direction, leave it untouched
                "X" => coord,
                "Y" => coord,
                _ => coord,
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}

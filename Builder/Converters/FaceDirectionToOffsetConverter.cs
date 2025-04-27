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
        private const double gap = 4.0;  // extra spacing outside the item

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] = coord (double)
            // values[1] = FaceDirection
            // values[2] = item Width (double)
            // values[3] = item Height (double)
            if (values.Length < 5
                || !(values[0] is double coord)
                || !(values[1] is FaceDirection dir)
                || !(values[2] is int width)
                || !(values[3] is int height)
                || !(values[4] is int angle)
                || !(parameter is string axis))
            {
                return Binding.DoNothing;
            }

            double halfW = width / 2;
            double halfH = height / 2;

            switch (axis)
            {
                case "X":
                    if (dir == FaceDirection.Left)
                        return coord;
                    else if (dir == FaceDirection.Right)
                        return coord + width;
                    else if (dir == FaceDirection.Top)
                        return coord + halfW;
                    else if (dir == FaceDirection.Bottom)
                        return coord + halfW;
                    else
                        return coord;

                case "Y":
                    if (dir == FaceDirection.Left)
                        return coord + halfH - gap;
                    else if (dir == FaceDirection.Right)
                        return coord + halfW - gap;
                    else if (dir == FaceDirection.Top)
                        return coord - gap;
                    else if (dir == FaceDirection.Bottom)
                        return coord + height - gap;
                    else
                        return coord;

                default:
                    return coord;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using Builder.Enums;

namespace Builder.Converters
{
    public class FaceDirectionToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 1) Map FaceDirection → base angle (outgoing)
            double baseAngle = 0.0;
            if (value is FaceDirection dir)
            {
                baseAngle = dir switch
                {
                    FaceDirection.Top => -90.0,
                    FaceDirection.Right => 0.0,
                    FaceDirection.Bottom => 90.0,
                    FaceDirection.Left => 180.0,
                    _ => 0.0,
                };
            }

            // 2) If this is for the "Target" end, flip by 180° so it points inward
            if (parameter is string side &&
                side.Equals("Target", StringComparison.OrdinalIgnoreCase))
            {
                baseAngle = (baseAngle + 180.0) % 360.0;
            }

            return baseAngle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace InteractiveNeuralNetworks.Converters
{
    class DragDropParametersConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return null;
            return new Helpers.DragDropParameters
            {
                EventArgs = values[0] as System.Windows.DragEventArgs,
                ReferenceCanvas = values[1] as System.Windows.Controls.Canvas
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace ZapanControls.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public sealed class BooleanToWidthConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
                return isVisible ? double.NaN : 0;

            return double.NaN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }
    }
}

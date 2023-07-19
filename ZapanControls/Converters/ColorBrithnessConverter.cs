using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ZapanControls.Libraries;

namespace ZapanControls.Converters
{
    [ValueConversion(typeof(SolidColorBrush), typeof(SolidColorBrush))]
    public sealed class ColorBrithnessConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Brush brush && int.TryParse(parameter?.ToString() ?? "0", out int correction) 
                ? brush.ChangeBrightness(correction) 
                : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

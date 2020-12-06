using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ZapanControls.Converters
{
    /// <summary>
    /// Converti un type <see cref="SolidColorBrush"/> en <see cref="Color"/>
    /// </summary>
    [ValueConversion(typeof(SolidColorBrush), typeof(Color))]
    public sealed class SolidColorBrushToColor : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush solidColorBrush = (SolidColorBrush)value;

            if (solidColorBrush != null && parameter != null)
            {
                Color color = solidColorBrush.Color;

                if (int.TryParse(parameter.ToString(), out int opacity))
                    color.A = System.Convert.ToByte(opacity * 255 / 100);

                return color;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

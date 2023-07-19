using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ZapanControls.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class IsValidConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                PropertyInfo[] properties = value.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (value is IDataErrorInfo data
                        && !string.IsNullOrEmpty(data[property.Name]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }
    }
}

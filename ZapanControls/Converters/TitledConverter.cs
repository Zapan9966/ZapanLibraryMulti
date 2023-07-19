using System;
using System.Globalization;
using System.Windows.Data;

namespace ZapanControls.Converters
{
    /// <summary>
    /// Convertisseur qui met toutes les premières lettres d'une chaine de caractère en majuscule.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class TitledConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result;
            if (value is DateTime dt)
            {
                string format = "dddd dd MMMM yyyy";
                if (!string.IsNullOrEmpty(parameter?.ToString()))
                {
                    format = parameter.ToString();
                }
                result = dt.ToString(format);
            }
            else
            {
                result = value?.ToString();
            }
            return culture?.TextInfo.ToTitleCase(result) ?? result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

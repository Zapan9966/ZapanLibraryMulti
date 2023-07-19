using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace ZapanControls.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class ComparisonConverter : BaseConverter, IValueConverter
    {
        private readonly Regex _regOperator = new Regex(">=|<=|==|>|<", RegexOptions.Compiled);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string txt = value?.ToString();
            string sParam = parameter?.ToString();

            if (!string.IsNullOrEmpty(txt) && !string.IsNullOrEmpty(sParam))
            {
                string param = _regOperator.Replace(sParam, "");

                if (int.TryParse(param, out int number))
                {
                    if (int.TryParse(txt, out int val))
                    {
                        return Operation(sParam, val, number);
                    }
                }
                else if (DateTime.TryParse(param, out DateTime date))
                {
                    if (DateTime.TryParse(txt, out DateTime val))
                    {
                        return Operation(sParam, val, date);
                    }
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }

        private static bool Operation(string comparator, dynamic val, dynamic val2)
        {
            if (comparator.Contains(">="))
            {
                return val >= val2;
            }
            else if (comparator.Contains("<="))
            {
                return val <= val2;
            }
            else if (comparator.Contains("=="))
            {
                return val == val2;
            }
            else if (comparator.Contains(">"))
            {
                return val > val2;
            }
            else if (comparator.Contains("<"))
            {
                return val < val2;
            }
            return false;
        }
    }
}

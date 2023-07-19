using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ZapanControls.Converters
{
    //
    // Source: https://rachel53461.wordpress.com/2011/08/20/the-math-converter/
    //

    /// <summary>
    /// Math converter.
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public sealed class MathConverter : BaseConverter, IValueConverter
    {
        private static readonly char[] _allOperators = new[] { '+', '-', '*', '/', '%', '(', ')', '>', '<' };

        private static readonly List<string> _grouping = new List<string> { "(", ")" };
        private static readonly List<string> _operators = new List<string> { "+", "-", "*", "/", "%" };
        private static readonly List<string> _limits = new List<string> { ">", "<" };

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Parse value into equation and remove spaces
            var mathEquation = parameter as string;
            mathEquation = mathEquation?.Replace(" ", "").Replace("@VALUE", value?.ToString());

            // Validate values and get list of numbers in equation
            var numbers = new List<double>();

            foreach (string s in mathEquation.Split(_allOperators))
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out double tmp))
                    {
                        numbers.Add(tmp);
                    }
                    else
                    {
                        throw new InvalidCastException(); // Handle Error - Some non-numeric, operator, or grouping character found in string
                    }
                }
            }

            // Begin parsing method
            EvaluateMathString(ref mathEquation, ref numbers, 0);

            // After parsing the numbers list should only have one value - the total
            return numbers[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Evaluates a mathematical string and keeps track of the results in a <see cref="List{double}"/> of numbers.
        /// </summary>
        /// <param name="mathEquation"></param>
        /// <param name="numbers"></param>
        /// <param name="index"></param>
        private void EvaluateMathString(ref string mathEquation, ref List<double> numbers, int index)
        {
            // Loop through each mathemtaical token in the equation
            string token = GetNextToken(mathEquation);

            while (!string.IsNullOrEmpty(token))
            {
                // Remove token from mathEquation
                mathEquation = mathEquation.Remove(0, token.Length);

                // If token is a grouping character, it affects program flow
                if (_grouping.Contains(token))
                {
                    switch (token)
                    {
                        case "(":
                            EvaluateMathString(ref mathEquation, ref numbers, index);
                            break;
                        case ")":
                            return;
                    }
                }

                // If token is an operator, do requested operation
                if (_operators.Contains(token))
                {
                    // If next token after operator is a parenthesis, call method recursively
                    string nextToken = GetNextToken(mathEquation);
                    if (nextToken == "(")
                    {
                        EvaluateMathString(ref mathEquation, ref numbers, index + 1);
                    }

                    // Verify that enough numbers exist in the List<double> to complete the operation
                    // and that the next token is either the number expected, or it was a ( meaning
                    // that this was called recursively and that the number changed
                    if (numbers.Count > (index + 1) &&
                        (double.Parse(nextToken, NumberStyles.Number, CultureInfo.InvariantCulture) == numbers[index + 1] || nextToken == "("))
                    {
                        switch (token)
                        {
                            case "+":
                                numbers[index] = numbers[index] + numbers[index + 1];
                                break;
                            case "-":
                                numbers[index] = numbers[index] - numbers[index + 1];
                                break;
                            case "*":
                                numbers[index] = numbers[index] * numbers[index + 1];
                                break;
                            case "/":
                                numbers[index] = numbers[index] / numbers[index + 1];
                                break;
                            case "%":
                                numbers[index] = numbers[index] % numbers[index + 1];
                                break;
                        }
                        numbers.RemoveAt(index + 1);
                    }
                    else
                    {
                        throw new FormatException("Next token is not the expected number"); // Handle Error - Next token is not the expected number
                    }
                }

                if (_limits.Contains(token))
                {
                    // If next token after operator is a parenthesis, call method recursively
                    string nextToken = GetNextToken(mathEquation);
                    if (nextToken == "(")
                    {
                        EvaluateMathString(ref mathEquation, ref numbers, index + 1);
                    }

                    if (numbers.Count > (index + 1) &&
                        (double.Parse(nextToken, NumberStyles.Number, CultureInfo.InvariantCulture) == numbers[index + 1] || nextToken == "("))
                    {
                        switch (token)
                        {
                            case ">":
                                if (numbers[index] < numbers[index + 1])
                                    numbers[index] = numbers[index + 1];
                                break;
                            case "<":
                                if (numbers[index] > numbers[index + 1])
                                    numbers[index] = numbers[index + 1];
                                break;
                        }
                        numbers.RemoveAt(index + 1);
                    }
                    else
                        throw new FormatException("Next token is not the expected number"); // Handle Error - Next token is not the expected number
                }

                token = GetNextToken(mathEquation);
            }
        }

        /// <summary>
        /// Gets the next mathematical token in the equation
        /// </summary>
        /// <param name="mathEquation"></param>
        /// <returns></returns>
        private static string GetNextToken(string mathEquation)
        {
            // If we're at the end of the equation, return string.empty
            if (string.IsNullOrEmpty(mathEquation))
                return string.Empty;

            // Get next operator or numeric value in equation and return it
            string tmp = "";
            foreach (char c in mathEquation)
            {
                if (_allOperators.Contains(c))
                {
                    return string.IsNullOrEmpty(tmp) ? c.ToString() : tmp;
                }
                else
                {
                    tmp += c;
                }
            }

            return tmp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace ZapanControls.Libraries
{
    public static class Extensions
    {
        private static readonly Regex _emailRegex = new Regex(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$"
            , RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Combine le <see cref="Style"/> avec le <see cref="Style"/> défini en paramètre
        /// </summary>
        /// <param name="styleToMerge">Le <see cref="Style"/> à combiner</param>
        /// <returns>Renvoi un <see cref="Style"/></returns>
        public static Style MergeStyle(this Style st, Style styleToMerge)
        {
            if (styleToMerge == null)
            {
                return st;
            }

            Style style = new Style(st?.TargetType, st);

            if (styleToMerge.Setters != null)
            {
                foreach (var setter in styleToMerge.Setters.OfType<Setter>())
                {
                    var setters = style.Setters.OfType<Setter>();
                    if (setters.Any(s => s.Property == setter.Property))
                    {
                        style.Setters.Remove(setters.FirstOrDefault(s => s.Property == setter.Property));
                    }
                    style.Setters.Add(setter);
                }
            }

            if (styleToMerge.Triggers != null)
            {
                for (int i = 0; i < styleToMerge.Triggers.Count; i++)
                {
                    if (styleToMerge.Triggers[i] is Trigger trigger)
                    {
                        var trig = style.Triggers
                            .OfType<Trigger>()
                            .FirstOrDefault(t => t.Property == trigger.Property && t.Value == trigger.Value);

                        if (trig != null)
                        {
                            foreach (TriggerAction ta in trigger.EnterActions)
                            {
                                trig.EnterActions.Add(ta);
                            }

                            foreach (TriggerAction ta in trigger.ExitActions)
                            {
                                trig.ExitActions.Add(ta);
                            }

                            foreach (Setter setter in trigger.Setters.OfType<Setter>())
                            {
                                var trigSetters = trig.Setters.OfType<Setter>();
                                if (trigSetters.Any(s => s.Property == setter.Property))
                                {
                                    trig.Setters.Remove(trigSetters.FirstOrDefault(s => s.Property == setter.Property));
                                }
                                trig.Setters.Add(setter);
                            }
                        }
                        else
                        {
                            style.Triggers.Add(trigger);
                        }
                    }
                    else if (styleToMerge.Triggers[i] is DataTrigger dataTrigger)
                    {
                        var dataTrig = style.Triggers
                            .OfType<DataTrigger>() 
                            .FirstOrDefault(t => t.Binding == dataTrigger.Binding && t.Value == dataTrigger.Value);

                        if (dataTrig != null)
                        {
                            foreach (var ta in dataTrigger.EnterActions)
                            {
                                dataTrig.EnterActions.Add(ta);
                            }

                            foreach (var ta in dataTrigger.ExitActions)
                            {
                                dataTrig.ExitActions.Add(ta);
                            }

                            foreach (var setter in dataTrigger.Setters.OfType<Setter>())
                            {
                                var dataTriggerSetters = dataTrig.Setters.OfType<Setter>();

                                if (dataTriggerSetters.Any(s => s.Property == setter.Property))
                                {
                                    dataTrig.Setters.Remove(dataTriggerSetters.FirstOrDefault(s => s.Property == setter.Property));
                                }
                                dataTrig.Setters.Add(setter);
                            }
                        }
                        else
                        {
                            style.Triggers.Add(dataTrigger);
                        }
                    }
                }
            }
            return style;
        }

        /// <summary>
        /// Remplace la valeur NULL par DBNull 
        /// </summary>
        /// <param name="obj"><see cref="Object"/> à vérifier</param>
        /// <returns></returns>
        public static object CheckDbNull(this object obj)
            => obj ?? DBNull.Value;

        /// <summary>
        /// Détermine si une chaîne de caractère contient une chaine de carctère spécifique en ignorant les majuscules
        /// </summary>
        /// <param name="value">Chaîne de caractère recherchée</param>
        public static bool ContainsInvariant(this string s, string value)
            => (s?.IndexOf(value, 0, StringComparison.CurrentCultureIgnoreCase) ?? -1) != -1;

        /// <summary>
        /// Replace une chaîne de caractère en ignorant les majuscules à l'aide d'une expression régulière
        /// </summary>
        /// <param name="pattern">Elément à rechercher pour remplacement</param>
        /// <param name="replacement">Remplacement de l'élément recherché</param>
        public static string ReplaceInvariant(this string s, string pattern, string replacement)
            => Regex.Replace(s, pattern, replacement, RegexOptions.IgnoreCase);

        /// <summary>
        /// Mélange le contenu d'une liste de façon aléatoire
        /// </summary>
        /// <typeparam name="T">Le type d'objet cotenu dans la liste.</typeparam>
        /// <param name="list">La liste à mélanger.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            // Source: https://stackoverflow.com/questions/273313/randomize-a-listt

            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                int n = list?.Count ?? 0;
                while (n > 1)
                {
                    byte[] box = new byte[1];
                    do provider.GetBytes(box);
                    while (!(box[0] < n * (byte.MaxValue / n)));
                    int k = (box[0] % n);
                    n--;
                    (list[n], list[k]) = (list[k], list[n]);
                }
            }
        }

        /// <summary>
        /// Retourne le numéro de la semaine dans l'année qui contient la date spécifié.
        /// </summary>
        /// <param name="date">Valeur de date et d'heure.</param>
        /// <returns>Numéro de la semaine.</returns>
        public static int WeekOfYear(this DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Recherche le prochain <see cref="DayOfWeek"/> recherché.
        /// </summary>
        /// <param name="date">
        /// Valeur de date et d'heure.
        /// Utiliser <code>DateTime.Today.AddDays(1)</code> pour ne pas prendre en compte la date du jour.
        /// </param>
        /// <param name="day">Le jour à rechercher.</param>
        /// <returns>La date du prochain jour de la semaine recherché.</returns>
        public static DateTime NextWeekday(this DateTime date, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)date.DayOfWeek + 7) % 7;
            return date.AddDays(daysToAdd);
        }

        public static DateTime PreviousWeekDay(this DateTime date, DayOfWeek day)
        {
            // The (... - 7) % 7 ensures we end up with a value in the range [-0, -6]
            int daysToAdd = ((int)day - (int)date.DayOfWeek - 7) % 7;
            return date.AddDays(daysToAdd);
        }

        /// <summary>
        /// Créé une couleur avec une luminosité modifiée.
        /// </summary>
        /// <param name="brush">Couleur à modifiée.</param>
        /// <param name="correction">Correction de luminosité. 
        /// Doit être compris entre -255 et 255.
        /// Les valeurs négatives assombrissent la couleur.
        /// </param>
        /// <returns><see cref="Brush"/> corrigée.</returns>
        public static Brush ChangeBrightness(this Brush brush, int correction)
        {
            return brush != null 
                ? new SolidColorBrush((brush as SolidColorBrush).Color.ChangeBrightness(correction)) 
                : (Brush)null;
        }

        /// <summary>
        /// Créé une couleur avec une luminosité modifiée.
        /// </summary>
        /// <param name="brush">Couleur à modifiée.</param>
        /// <param name="correction">Correction de luminosité. 
        /// Doit être compris entre -255 et 255.
        /// Les valeurs négatives assombrissent la couleur.
        /// </param>
        /// <returns><see cref="SolidColorBrush"/> corrigée.</returns>
        public static SolidColorBrush ChangeBrightness(this SolidColorBrush brush, int correction)
        {
            return brush != null 
                ? new SolidColorBrush(brush.Color.ChangeBrightness(correction)) 
                : null;
        }

        /// <summary>
        /// Créé une couleur avec une luminosité modifiée.
        /// </summary>
        /// <param name="color">Couleur à modifiée.</param>
        /// <param name="correction">Correction de luminosité. 
        /// Doit être compris entre -255 et 255.
        /// Les valeurs négatives assombrissent la couleur.
        /// </param>
        /// <returns><see cref="Color"/> corrigée</returns>
        public static Color ChangeBrightness(this Color color, int correction)
        {
            if (correction > 255) correction = 255;
            if (correction < -255) correction = -255;

            int red = color.R;
            int green = color.G;
            int blue = color.B;

            if (correction < 0)
            {
                red = red + correction > 0 ? red + correction : 0;
                green = green + correction > 0 ? green + correction : 0;
                blue = blue + correction > 0 ? blue + correction : 0;
            }
            else if (correction > 0)
            {
                red = red + correction < 255 ? red + correction : 255;
                green = green + correction < 255 ? green + correction : 255;
                blue = blue + correction < 255 ? blue + correction : 255;
            }

            return Color.FromArgb(color.A, Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
        }

        /// <summary>
        /// Rend toutes les propriétés de l'object actuel non modifiable.
        /// </summary>
        public static T FreezeProperties<T>(this T obj)
        {
            if (obj is Freezable freezableObj && freezableObj.CanFreeze)
            {
                freezableObj.Freeze();
            }

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                try
                {
                    if (prop.Name == "Item")
                        continue;

                    if (prop.GetValue(obj) is Freezable freezable && freezable.CanFreeze)
                    {
                        freezable.Freeze();
                    }
                }
                catch { }
            }
            return obj;
        }

        public static string TitleCase(this string text)
            => Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text);

        public static BindingBase Clone(this BindingBase bindingBase)
        {
            var xaml = XamlWriter.Save(bindingBase);
            var stringReader = new StringReader(xaml);
            var xmlReader = XmlReader.Create(stringReader);
            return (BindingBase)XamlReader.Load(xmlReader);
        }

        public static T Clone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)inst?.Invoke(obj, null);
        }

        public static T Cast<T>(this object obj)
            => (T)obj;

        public static bool DeepCompare(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            // Compare two object's class, return false if they are difference
            if (obj.GetType() != another.GetType()) return false;

            //properties: int, double, DateTime, etc, not class
            if (!obj.GetType().IsClass) return obj.Equals(another);
            if (obj is string) return obj?.ToString() == another?.ToString();

            // Get all properties of obj and compare each other
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.Name == "Item")
                    continue;

                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                if (!objValue.Equals(anotherValue)) return false;
            }
            return true;
        }

        public static bool CompareEx(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            //properties: int, double, DateTime, etc, not class
            if (!obj.GetType().IsClass) return obj.Equals(another);
            if (obj is string) return obj?.ToString() == another?.ToString();

            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.Name == "Item")
                    continue;

                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                //Recursion
                if (!objValue.DeepCompare(anotherValue)) return false;
            }
            return true;
        }

        public static bool IsValidSqlDateTime(this DateTime? dateTime)
        {
            if (dateTime == null) return true;

            DateTime minValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
            DateTime maxValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MaxValue.ToString());

            return minValue <= dateTime.Value && maxValue >= dateTime.Value;
        }

        public static bool IsValidSqlDateTime(this DateTime dateTime)
        {
            DateTime minValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
            DateTime maxValue = DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MaxValue.ToString());

            return minValue <= dateTime && maxValue >= dateTime;
        }

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static bool IsValidEmail(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var adresses = input.Replace(",", ";").Replace(" ", "").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                return adresses.All(a => _emailRegex.IsMatch(a));
            }
            return false;
        }

        public static bool In<T>(this T item, params T[] items)
            => items != null && items.Contains(item);
    }
}

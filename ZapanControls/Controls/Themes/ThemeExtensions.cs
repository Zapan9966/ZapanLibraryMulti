using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls.Themes
{
    internal static class ThemeExtensions
    {
        private const string ThemePropName = "Theme";

        private static string ThemeRegistrationName(this Enum theme, Type ownerType)
            => theme.ToString().ThemeRegistrationName(ownerType);

        public static string ThemeRegistrationName(this ThemePath theme, Type ownerType)
            => theme.Name.ThemeRegistrationName(ownerType);

        public static string ThemeRegistrationName(this string themeName, Type ownerType)
            => $"{ownerType};{themeName}";

        public static string GetThemeName(this string key)
            => key?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1];

        public static void LoadDefaultTheme(this FrameworkElement f, DependencyProperty p)
        {
            if (f is ITheme t && t.ThemeDictionaries.Any())
            {
                f.SetCurrentValue(p, t.ThemeDictionaries.First().Key.GetThemeName());
            }
        }

        public static void RegisterInternalThemes<ThemesEnum>(this DependencyObject o, string themeFolder = null)
            where ThemesEnum : Enum
        {
            if (o is ITheme t)
            {
                themeFolder ??= o.GetType().Name;
                string uriBase = $"/ZapanControls;component/Themes/{(themeFolder != string.Empty ? $"{themeFolder}/" : null)}";

                foreach (var enumName in Enum.GetNames(typeof(ThemesEnum)))
                {
                    t.RegisterTheme(new ThemePath(enumName, $"{uriBase}{enumName.Replace("_", ".")}.xaml"), o.GetType());
                }
            }
        }

        public static void RegisterTheme(this ITheme t, ThemePath theme, Type ownerType)
        {
            // test args
            if (theme.Name == null || theme.DictionaryPath == null)
                throw new ArgumentNullException("Theme name/path is null");

            if (ownerType == null)
                throw new ArgumentNullException("Invalid ownerType");

            string registrationName = theme.Name.ThemeRegistrationName(ownerType);

            try
            {
                // create the Uri
                Uri themeUri = new Uri(theme.DictionaryPath, UriKind.Relative);
                // register the new theme
                t.ThemeDictionaries[registrationName] = Application.LoadComponent(themeUri) as ResourceDictionary;
            }
            catch (Exception)
            { }
        }

        public static void SetThemePropertyDefault(this DependencyObject o, DependencyProperty p, ComponentResourceKey resourceKey)
        {
            if (o is FrameworkElement f)
            {
                o.SetThemePropertyDefault(p, f.TryFindResource(resourceKey));
            }
        }

        public static void SetThemePropertyDefault(this DependencyObject o, DependencyProperty p, object value)
        {
            if (o is FrameworkElement f && o is ITheme t)
            {
                t.DefaultThemeProperties.Add(p, value);

                if (BindingOperations.GetBinding(o, p) is BindingBase binding)
                {
                    var newBinding = binding.Clone();

                    if (binding.FallbackValue == null || binding.FallbackValue == DependencyProperty.UnsetValue)
                    {
                        newBinding.FallbackValue = value;
                    }

                    if (binding.TargetNullValue == null || binding.TargetNullValue == DependencyProperty.UnsetValue)
                    {
                        newBinding.TargetNullValue = value;
                    }

                    f.SetBinding(p, newBinding);
                }
                else
                {
                    o.SetCurrentValue(p, value);
                }
            }
        }

        public static void SetValueCommon(this DependencyObject o, DependencyProperty p, object value)
        {
            if (!(BindingOperations.GetBinding(o, p) is Binding))
            {
                if (value is Thickness t && t == new Thickness(0))
                {
                    value = null;
                }
                else if (value is double d && d == 0d)
                {
                    value = null;
                }

                if (value == null)
                {
                    if (o is ITheme theme && theme.DefaultThemeProperties.ContainsKey(p))
                    {
                        value = theme.DefaultThemeProperties[p];
                    }
                    o.SetCurrentValue(p, value);
                }
            }
        }

        public static void ThemeChanged(this DependencyObject o, DependencyPropertyChangedEventArgs e, RoutedEvent successEvent)
        {
            // test args
            if (!(o is ITheme t) || !(o is FrameworkElement f) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            // current theme
            string curThemeName = e.OldValue as string;
            string curRegisteredThemeName = curThemeName.ThemeRegistrationName(o.GetType());

            if (t.ThemeDictionaries.ContainsKey(curRegisteredThemeName))
            {
                // remove current theme
                ResourceDictionary curThemeDictionary = t.ThemeDictionaries[curRegisteredThemeName];
                f.Resources.MergedDictionaries.Remove(curThemeDictionary);
            }

            // new theme name
            string newThemeName = e.NewValue as string;
            string newRegisteredThemeName = !string.IsNullOrEmpty(newThemeName) 
                ? newThemeName.ThemeRegistrationName(o.GetType())
                : t.ThemeDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!t.ThemeDictionaries.ContainsKey(newRegisteredThemeName))
            {
                throw new ArgumentNullException("Invalid Theme property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newThemeDictionary = t.ThemeDictionaries[newRegisteredThemeName];
                f.Resources.MergedDictionaries.Add(newThemeDictionary);
                // Raise theme successfully changed event
                f.RaiseEvent(new ThemeChangedEventArgs(successEvent, o, curRegisteredThemeName, newRegisteredThemeName));
            }

            t.RaisePropertyChanged(new PropertyChangedEventArgs(ThemePropName));
        }
    }
}

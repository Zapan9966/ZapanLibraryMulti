using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Themes;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    public sealed class ZapContextMenu : ContextMenu, ITheme, INotifyPropertyChanged
    {
        #region Property Name Constants
        private const string ThemePropName = "Theme";
        #endregion

        #region Fields
        private readonly Dictionary<string, ResourceDictionary> _rdThemeDictionaries;
        private readonly Dictionary<DependencyProperty, object> _defaultThemeProperties;
        private bool _hasInitialized;
        #endregion

        #region Theme Declarations
        public static ThemePath Oceatech = new ThemePath(ZapContextMenuThemes.Oceatech, "/ZapanControls;component/Themes/ZapContextMenu/Oceatech.xaml");
        public static ThemePath Contactel = new ThemePath(ZapContextMenuThemes.Contactel, "/ZapanControls;component/Themes/ZapContextMenu/Contactel.xaml");
        #endregion

        #region Properties
        #region CornerRadius
        /// <summary>
        /// Identifies the <see cref="CornerRadiusProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(ZapContextMenu), new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Get/set the ContextMenu corner radius.
        /// </summary>
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        #endregion

        #region HasInitialized
        public bool HasInitialized { get => _hasInitialized; private set => Set(ref _hasInitialized, value); }
        #endregion

        #region Theme
        /// <summary>
        /// Get/Sets the theme
        /// </summary>
        public static DependencyProperty ThemeProperty = DependencyProperty.Register(
            "Theme", typeof(string), typeof(ZapContextMenu),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapContextMenu cm) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            // current theme
            string curThemeName = e.OldValue as string;
            string curRegisteredThemeName = cm.GetRegistrationName(curThemeName, cm.GetType());

            if (cm._rdThemeDictionaries.ContainsKey(curRegisteredThemeName))
            {
                // remove current theme
                ResourceDictionary curThemeDictionary = cm._rdThemeDictionaries[curRegisteredThemeName];
                cm.Resources.MergedDictionaries.Remove(curThemeDictionary);
            }

            // new theme name
            string newThemeName = e.NewValue as string;
            string newRegisteredThemeName = !string.IsNullOrEmpty(newThemeName) ?
                cm.GetRegistrationName(newThemeName, cm.GetType())
                : cm._rdThemeDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!cm._rdThemeDictionaries.ContainsKey(newRegisteredThemeName))
            {
                throw new ArgumentNullException("Invalid Theme property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newThemeDictionary = cm._rdThemeDictionaries[newRegisteredThemeName];
                cm.Resources.MergedDictionaries.Add(newThemeDictionary);
                // Raise theme successfully changed event
                cm.RaiseEvent(new RoutedEventArgs(ThemeChangedSuccessEvent, cm));
            }

            cm.RaisePropertyChanged(new PropertyChangedEventArgs(ThemePropName));
        }

        private static object CoerceThemeChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion
        #endregion

        #region Native Properties Changed
        #region Background
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, BorderBrushProperty, e.NewValue);
        #endregion

        #region BorderThickness
        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, BorderThicknessProperty, e.NewValue);
        #endregion

        #region Foreground
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Events
        #region ThemeChangedSuccessEvent
        public static readonly RoutedEvent ThemeChangedSuccessEvent = EventManager.RegisterRoutedEvent(
            "ThemeChangedSuccess", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapContextMenu));

        public event RoutedEventHandler ThemeChangedSuccess { add => AddHandler(ThemeChangedSuccessEvent, value); remove => RemoveHandler(ThemeChangedSuccessEvent, value); }

        private void OnThemeChangedSuccess(object sender, RoutedEventArgs e)
        {
            _defaultThemeProperties.Clear();
            // Control
            SetThemePropertyDefault(BackgroundProperty, TryFindResource(ResourceKeys.ZapContextMenuResourceKeys.BackgroundKey));
            SetThemePropertyDefault(BorderBrushProperty, TryFindResource(ResourceKeys.ZapContextMenuResourceKeys.BorderBrushKey));
            SetThemePropertyDefault(BorderThicknessProperty, TryFindResource(ResourceKeys.ZapContextMenuResourceKeys.BorderThicknessKey));
            SetThemePropertyDefault(ForegroundProperty, TryFindResource(ResourceKeys.ZapContextMenuResourceKeys.ForegroundKey));
        }
        #endregion
        #endregion

        #region Constructors
        static ZapContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapContextMenu), new FrameworkPropertyMetadata(typeof(ZapContextMenu)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;

            BackgroundProperty.OverrideMetadata(typeof(ZapContextMenu), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapContextMenu), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapContextMenu), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
            ForegroundProperty.OverrideMetadata(typeof(ZapContextMenu), new FrameworkPropertyMetadata(null, options, OnForegroundChanged));
        }

        public ZapContextMenu()
        {
            _defaultThemeProperties = new Dictionary<DependencyProperty, object>();

            _rdThemeDictionaries = new Dictionary<string, ResourceDictionary>();
            RegisterAttachedThemes();

            ThemeChangedSuccess += OnThemeChangedSuccess;

            // Load first theme
            if (_rdThemeDictionaries.Any())
                SetCurrentValue(ThemeProperty, GetThemeName(_rdThemeDictionaries.FirstOrDefault().Key));
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;

            foreach (var property in _defaultThemeProperties)
            {
                // Update bindings with theme default values
                if (BindingOperations.GetBinding(this, property.Key) is BindingBase binding)
                {
                    var newBinding = binding.Clone();

                    if (binding.FallbackValue == null || binding.FallbackValue == DependencyProperty.UnsetValue)
                        newBinding.FallbackValue = property.Value;

                    if (binding.TargetNullValue == null || binding.TargetNullValue == DependencyProperty.UnsetValue)
                        newBinding.TargetNullValue = property.Value;

                    SetBinding(property.Key, newBinding);
                }
            }
        }
        #endregion

        #region Theming
        /// <summary>
        /// Register a theme with internal dictionary
        /// </summary>
        public void RegisterTheme(ThemePath theme, Type ownerType)
        {
            // test args
            if (theme.Name == null || theme.DictionaryPath == null)
                throw new ArgumentNullException("Theme name/path is null");

            if (ownerType == null)
                throw new ArgumentNullException("Invalid ownerType");

            string registrationName = GetRegistrationName(theme, ownerType);

            try
            {
                if (!_rdThemeDictionaries.ContainsKey(registrationName))
                {
                    // create the Uri
                    Uri themeUri = new Uri(theme.DictionaryPath, UriKind.Relative);
                    // register the new theme
                    _rdThemeDictionaries[registrationName] = Application.LoadComponent(themeUri) as ResourceDictionary;
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Instance theme dictionary and add themes
        /// </summary>
        private void RegisterAttachedThemes()
        {
            // Attach control attached themes
            var themeFields = GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(ThemePath));

            foreach (var field in themeFields)
            {
                RegisterTheme((ThemePath)field.GetValue(this), GetType());
            }
        }

        /// <summary>
        /// Load the default theme
        /// </summary>
        internal void LoadDefaultTheme(ZapContextMenuThemes theme, Type ownerType)
        {
            string registrationName = GetRegistrationName(theme, ownerType);
            Resources.MergedDictionaries.Add(_rdThemeDictionaries[registrationName]);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        private string GetRegistrationName(ZapContextMenuThemes theme, Type ownerType)
        {
            return GetRegistrationName(theme.ToString(), ownerType);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        private string GetRegistrationName(ThemePath theme, Type ownerType)
        {
            return GetRegistrationName(theme.Name, ownerType);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        public string GetRegistrationName(string themeName, Type ownerType)
        {
            return $"{ownerType};{themeName}";
        }

        /// <summary>
        /// Get themes name from formal registration name
        /// </summary>
        public string GetThemeName(string key)
        {
            return key?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        /// <summary>
        /// Set themes default propertie value
        /// </summary>
        internal void SetThemePropertyDefault(DependencyProperty p, object value)
        {
            _defaultThemeProperties.Add(p, value);
            SetCurrentValue(p, value);
        }

        /// <summary>
        /// Set dependency property default theme value if value is null
        /// </summary>
        private static void SetValueCommon(DependencyObject o, DependencyProperty p, object value)
        {
            if (o is ZapContextMenu cm)
            {
                if (!(BindingOperations.GetBinding(cm, p) is Binding))
                {
                    if (value is Thickness t)
                    {
                        if (t == new Thickness(0))
                            value = null;
                    }
                    else if (value is double d)
                    {
                        if (d == 0d)
                            value = null;
                    }

                    if (value == null)
                    {
                        if (cm._defaultThemeProperties.ContainsKey(p))
                            value = cm._defaultThemeProperties[p];

                        cm.SetCurrentValue(p, value);
                    }
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        /// <summary>Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] String propertyName = null)
        {
            return Set(propertyName, ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <typeparam name="T">The type of the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(Expression<Func<T>> propertyNameExpression, ref T oldValue, T newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <typeparam name="TProp">The type of the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<TClass, TProp>(Expression<Func<TClass, TProp>> propertyNameExpression, ref TProp oldValue,
            TProp newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(String propertyName, ref T oldValue, T newValue)
        {
            if (Equals(oldValue, newValue))
                return false;

            oldValue = newValue;
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
            return true;
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyName">The property name. </param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        public void RaisePropertyChanged(Expression<Func<object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        public void RaisePropertyChanged<TClass>(Expression<Func<TClass, object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="args">The arguments. </param>
        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>Raises the property changed event for all properties (string.Empty). </summary>
        public void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(string.Empty));
        }
        #endregion
    }
}

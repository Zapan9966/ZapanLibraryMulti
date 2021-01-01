using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Themes;
using ZapanControls.Helpers;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    public sealed class ZapMenuItem : MenuItem, ITheme, INotifyPropertyChanged
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
        public static ThemePath Oceatech = new ThemePath(ZapMenuItemThemes.Oceatech, "/ZapanControls;component/Themes/ZapMenuItem/Oceatech.xaml");
        public static ThemePath Contactel = new ThemePath(ZapMenuItemThemes.Contactel, "/ZapanControls;component/Themes/ZapMenuItem/Contactel.xaml");
        #endregion

        #region Properties
        #region Popup
        /// <summary>
        /// Identifies the <see cref="IsRoundPopup"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsRoundPopupProperty = DependencyProperty.Register(
            "IsRoundPopup", typeof(bool), typeof(ZapMenuItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Get/set the popup border color.
        /// </summary>
        public bool IsRoundPopup { get => (bool)GetValue(IsRoundPopupProperty); set => SetValue(IsRoundPopupProperty, value); }
        #endregion

        #region HasInitialized
        public bool HasInitialized { get => _hasInitialized; private set => Set(ref _hasInitialized, value); }
        #endregion

        #region Normal
        #region SubMenuForeground
        public static readonly DependencyProperty SubMenuForegroundProperty = DependencyProperty.Register(
            "SubMenuForeground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnSubMenuForegroundChanged));

        public Brush SubMenuForeground { get => (Brush)GetValue(SubMenuForegroundProperty); set => SetValue(SubMenuForegroundProperty, value); }

        private static void OnSubMenuForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, SubMenuForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Focused
        #region FocusedBackground
        public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.Register(
            "FocusedBackground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedBackgroundChanged));

        public Brush FocusedBackground { get => (Brush)GetValue(FocusedBackgroundProperty); set => SetValue(FocusedBackgroundProperty, value); }

        private static void OnFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, FocusedBackgroundProperty, e.NewValue);
        #endregion

        #region FocusedBorderBrush
        public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(
            "FocusedBorderBrush", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedBorderBrushChanged));

        public Brush FocusedBorderBrush { get => (Brush)GetValue(FocusedBorderBrushProperty); set => SetValue(FocusedBorderBrushProperty, value); }

        private static void OnFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, FocusedBorderBrushProperty, e.NewValue);
        #endregion

        #region FocusedForeground
        public static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.Register(
            "FocusedForeground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedForegroundChanged));

        public Brush FocusedForeground {  get => (Brush)GetValue(FocusedForegroundProperty); set => SetValue(FocusedForegroundProperty, value); }

        private static void OnFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, FocusedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Checked
        #region CheckedBackground
        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(
            "CheckedBackground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedBackgroundChanged));

        public Brush CheckedBackground { get => (Brush)GetValue(CheckedBackgroundProperty); set => SetValue(CheckedBackgroundProperty, value); }

        private static void OnCheckedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, CheckedBackgroundProperty, e.NewValue);
        #endregion

        #region CheckedBorderBrush
        public static readonly DependencyProperty CheckedBorderBrushProperty = DependencyProperty.Register(
            "CheckedBorderBrush", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedBorderBrushChanged));

        public Brush CheckedBorderBrush { get => (Brush)GetValue(CheckedBorderBrushProperty); set => SetValue(CheckedBorderBrushProperty, value); }

        private static void OnCheckedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, CheckedBorderBrushProperty, e.NewValue);
        #endregion

        #region CheckedForeground
        public static readonly DependencyProperty CheckedForegroundProperty = DependencyProperty.Register(
            "CheckedForeground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedForegroundChanged));

        public Brush CheckedForeground { get => (Brush)GetValue(CheckedForegroundProperty); set => SetValue(CheckedForegroundProperty, value); }

        private static void OnCheckedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, CheckedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Pressed
        #region PressedBackground
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
            "PressedBackground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedBackgroundChanged));

        public Brush PressedBackground { get => (Brush)GetValue(PressedBackgroundProperty); set => SetValue(PressedBackgroundProperty, value); }

        private static void OnPressedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PressedBackgroundProperty, e.NewValue);
        #endregion

        #region PressedBorderBrush
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(
            "PressedBorderBrush", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedBorderBrushChanged));

        public Brush PressedBorderBrush { get => (Brush)GetValue(PressedBorderBrushProperty); set => SetValue(PressedBorderBrushProperty, value); }

        private static void OnPressedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PressedBorderBrushProperty, e.NewValue);
        #endregion

        #region PressedForeground
        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
            "PressedForeground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedForegroundChanged));

        public Brush PressedForeground { get => (Brush)GetValue(PressedForegroundProperty); set => SetValue(PressedForegroundProperty, value); }

        private static void OnPressedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PressedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Disabled
        #region DisabledBackground
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBackgroundChanged));

        public Brush DisabledBackground { get => (Brush)GetValue(DisabledBackgroundProperty); set => SetValue(DisabledBackgroundProperty, value); }

        private static void OnDisabledBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, DisabledBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledBorderBrush
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBorderBrushChanged));

        public Brush DisabledBorderBrush { get => (Brush)GetValue(DisabledBorderBrushProperty); set => SetValue(DisabledBorderBrushProperty, value); }

        private static void OnDisabledBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, DisabledBorderBrushProperty, e.NewValue);
        #endregion

        #region DisabledForeground
        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground", typeof(Brush), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledForegroundChanged));

        public Brush DisabledForeground { get => (Brush)GetValue(DisabledForegroundProperty); set => SetValue(DisabledForegroundProperty, value); }

        private static void OnDisabledForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, DisabledForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Theme
        /// <summary>
        /// Get/Sets the theme
        /// </summary>
        public static DependencyProperty ThemeProperty = DependencyProperty.Register(
            "Theme", typeof(string), typeof(ZapMenuItem),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapMenuItem mi) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            // current theme
            string curThemeName = e.OldValue as string;
            string curRegisteredThemeName = mi.GetRegistrationName(curThemeName, mi.GetType());

            if (mi._rdThemeDictionaries.ContainsKey(curRegisteredThemeName))
            {
                // remove current theme
                ResourceDictionary curThemeDictionary = mi._rdThemeDictionaries[curRegisteredThemeName];
                mi.Resources.MergedDictionaries.Remove(curThemeDictionary);
            }

            // new theme name
            string newThemeName = e.NewValue as string;
            string newRegisteredThemeName = !string.IsNullOrEmpty(newThemeName) ?
                mi.GetRegistrationName(newThemeName, mi.GetType())
                : mi._rdThemeDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!mi._rdThemeDictionaries.ContainsKey(newRegisteredThemeName))
            {
                throw new ArgumentNullException("Invalid Theme property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newThemeDictionary = mi._rdThemeDictionaries[newRegisteredThemeName];
                mi.Resources.MergedDictionaries.Add(newThemeDictionary);
                // Raise theme successfully changed event
                mi.RaiseEvent(new RoutedEventArgs(ThemeChangedSuccessEvent, mi));
            }

            mi.RaisePropertyChanged(new PropertyChangedEventArgs(ThemePropName));
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
            "ThemeChangedSuccess", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapMenuItem));

        public event RoutedEventHandler ThemeChangedSuccess { add => AddHandler(ThemeChangedSuccessEvent, value); remove => RemoveHandler(ThemeChangedSuccessEvent, value); }

        private void OnThemeChangedSuccess(object sender, RoutedEventArgs e)
        {
            _defaultThemeProperties.Clear();
            // Control
            SetThemePropertyDefault(BorderThicknessProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.BorderThicknessKey));
            // Normal
            SetThemePropertyDefault(BackgroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.BackgroundKey));
            SetThemePropertyDefault(BorderBrushProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.BorderBrushKey));
            SetThemePropertyDefault(ForegroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.ForegroundKey));
            SetThemePropertyDefault(SubMenuForegroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.SubMenuForegroundKey));
            // Focused
            SetThemePropertyDefault(FocusedBackgroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.FocusedBackgroundKey));
            SetThemePropertyDefault(FocusedBorderBrushProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.FocusedBorderBrushKey));
            SetThemePropertyDefault(FocusedForegroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.FocusedForegroundKey));
            // Checked
            SetThemePropertyDefault(CheckedBackgroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.CheckedBackgroundKey));
            SetThemePropertyDefault(CheckedBorderBrushProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.CheckedBorderBrushKey));
            SetThemePropertyDefault(CheckedForegroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.CheckedForegroundKey));
            // Pressed
            SetThemePropertyDefault(PressedBackgroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.PressedBackgroundKey));
            SetThemePropertyDefault(PressedBorderBrushProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.PressedBorderBrushKey));
            SetThemePropertyDefault(PressedForegroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.PressedForegroundKey));
            // Disabled
            SetThemePropertyDefault(DisabledBackgroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.DisabledBackgroundKey));
            SetThemePropertyDefault(DisabledBorderBrushProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.DisabledBorderBrushKey));
            SetThemePropertyDefault(DisabledForegroundProperty, TryFindResource(ResourceKeys.ZapMenuItemResourceKeys.DisabledForegroundKey));
        }
        #endregion
        #endregion

        #region Constructors
        static ZapMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapMenuItem), new FrameworkPropertyMetadata(typeof(ZapMenuItem)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;

            BackgroundProperty.OverrideMetadata(typeof(ZapMenuItem), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapMenuItem), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapMenuItem), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
            ForegroundProperty.OverrideMetadata(typeof(ZapMenuItem), new FrameworkPropertyMetadata(null, options, OnForegroundChanged) { Inherits = false });
        }

        public ZapMenuItem()
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
            if (!HasInitialized)
            {
                RelativeSource relativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorLevel = 1 };
                if (VisualTreeHelpers.FindTemplatedParent<ZapMenuItem>(this) != null)
                {
                    relativeSource.AncestorType = typeof(ZapMenuItem);
                }
                else if (VisualTreeHelpers.FindTemplatedParent<ZapContextMenu>(this) != null)
                {
                    relativeSource.AncestorType = typeof(ZapContextMenu);
                }

                if (relativeSource.AncestorType != null)
                {
                    SetBinding(ThemeProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath("Theme"),
                        Mode = BindingMode.OneWay
                    });
                }
            }

            base.OnApplyTemplate();
            HasInitialized = true;
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
        internal void LoadDefaultTheme(ZapMenuItemThemes theme, Type ownerType)
        {
            string registrationName = GetRegistrationName(theme, ownerType);
            Resources.MergedDictionaries.Add(_rdThemeDictionaries[registrationName]);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        private string GetRegistrationName(ZapMenuItemThemes theme, Type ownerType)
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

            if (BindingOperations.GetBinding(this, p) is BindingBase binding)
            {
                var newBinding = binding.Clone();

                if (binding.FallbackValue == null || binding.FallbackValue == DependencyProperty.UnsetValue)
                    newBinding.FallbackValue = value;

                if (binding.TargetNullValue == null || binding.TargetNullValue == DependencyProperty.UnsetValue)
                    newBinding.TargetNullValue = value;

                SetBinding(p, newBinding);
            }
            else
            {
                SetCurrentValue(p, value);
            }
        }

        /// <summary>
        /// Set dependency property default theme value if value is null
        /// </summary>
        private static void SetValueCommon(DependencyObject o, DependencyProperty p, object value)
        {
            if (o is ZapMenuItem mi)
            {
                if (!(BindingOperations.GetBinding(mi, p) is Binding))
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
                        if (mi._defaultThemeProperties.ContainsKey(p))
                            value = mi._defaultThemeProperties[p];

                        mi.SetCurrentValue(p, value);
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
        public bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
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
        public bool Set<T>(string propertyName, ref T oldValue, T newValue)
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

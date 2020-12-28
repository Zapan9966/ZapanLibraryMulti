using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Themes;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    public class ZapToggleButton : ToggleButton, ITheme, INotifyPropertyChanged
    {
        #region Property Name Constants
        private const string ThemePropName = "Theme";
        private const string ButtonTemplatePropName = "ButtonTemplate";
        #endregion

        #region Fields
        private readonly Dictionary<string, ResourceDictionary> _rdThemeDictionaries;
        private readonly Dictionary<string, ResourceDictionary> _rdTemplateDictionaries;
        private readonly Dictionary<DependencyProperty, object> _defaultThemeProperties;
        #endregion

        #region Theme Declarations
        public static ThemePath Oceatech = new ThemePath(ZapButtonThemes.Oceatech, "/ZapanControls;component/Themes/ZapButton/Oceatech.xaml");
        public static ThemePath Contactel = new ThemePath(ZapButtonThemes.Contactel, "/ZapanControls;component/Themes/ZapButton/Contactel.xaml");
        public static ThemePath Info = new ThemePath(ZapButtonThemes.Info, "/ZapanControls;component/Themes/ZapButton/Info.xaml");
        public static ThemePath Success = new ThemePath(ZapButtonThemes.Success, "/ZapanControls;component/Themes/ZapButton/Success.xaml");
        public static ThemePath Warning = new ThemePath(ZapButtonThemes.Warning, "/ZapanControls;component/Themes/ZapButton/Warning.xaml");
        public static ThemePath Danger = new ThemePath(ZapButtonThemes.Danger, "/ZapanControls;component/Themes/ZapButton/Danger.xaml");
        #endregion

        #region Template Declarations
        public static TemplatePath Flat = new TemplatePath(ZapButtonTemplates.Flat, "/ZapanControls;component/Themes/ZapButton/Template.Toggle.Flat.xaml");
        public static TemplatePath Glass = new TemplatePath(ZapButtonTemplates.Glass, "/ZapanControls;component/Themes/ZapButton/Template.Toggle.Glass.xaml");
        public static TemplatePath Round = new TemplatePath(ZapButtonTemplates.Round, "/ZapanControls;component/Themes/ZapButton/Template.Toggle.Round.xaml");
        #endregion

        #region Properties
        #region ButtonTemplate
        public static readonly DependencyProperty ButtonTemplateProperty = DependencyProperty.Register(
            "ButtonTemplate", typeof(string), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnButtonTemplateChanged),
                new CoerceValueCallback(CoerceButtonTemplateChange)));

        public string ButtonTemplate
        {
            get => (string)GetValue(ButtonTemplateProperty);
            set => SetValue(ButtonTemplateProperty, value);
        }

        private static void OnButtonTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapToggleButton ztb) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            string curTemplateName = e.OldValue as string;
            string curRegisteredTemplateName = ztb.GetRegistrationName(curTemplateName, ztb.GetType());

            if (ztb._rdTemplateDictionaries.ContainsKey(curRegisteredTemplateName))
            {
                // remove current template
                ResourceDictionary curTemplateDictionary = ztb._rdTemplateDictionaries[curRegisteredTemplateName];
                ztb.Resources.MergedDictionaries.Remove(curTemplateDictionary);
            }

            // new template name
            string newTemplateName = e.NewValue as string;

            if (string.IsNullOrEmpty(newTemplateName))
                newTemplateName = ztb._rdTemplateDictionaries.FirstOrDefault().Key;

            string newRegisteredTemplateName = ztb.GetRegistrationName(newTemplateName, ztb.GetType());

            // add the resource
            if (!ztb._rdTemplateDictionaries.ContainsKey(newRegisteredTemplateName))
            {
                throw new ArgumentNullException("Invalid Template property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newTemplateDictionary = ztb._rdTemplateDictionaries[newRegisteredTemplateName];
                ztb.Resources.MergedDictionaries.Add(newTemplateDictionary);

                if (ztb.ButtonTemplate == "Round")
                {
                    ztb.MinHeight = 20d;
                    ztb.MinWidth = 20d;
                }
                else
                {
                    ztb.MinHeight = 0d;
                    ztb.MinWidth = 0d;
                }
            }

            ztb.RaisePropertyChanged(new PropertyChangedEventArgs(ButtonTemplatePropName));
        }

        private static object CoerceButtonTemplateChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion

        #region Checked
        #region CheckedBackground
        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(
            "CheckedBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedBackgroundChanged));

        public Brush CheckedBackground
        {
            get => (Brush)GetValue(CheckedBackgroundProperty);
            set => SetValue(CheckedBackgroundProperty, value);
        }

        private static void OnCheckedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, CheckedBackgroundProperty, e.NewValue);
        #endregion

        #region CheckedBorderBrush
        public static readonly DependencyProperty CheckedBorderBrushProperty = DependencyProperty.Register(
            "CheckedBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedBorderBrushChanged));

        public Brush CheckedBorderBrush
        {
            get => (Brush)GetValue(CheckedBorderBrushProperty);
            set => SetValue(CheckedBorderBrushProperty, value);
        }

        private static void OnCheckedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, CheckedBorderBrushProperty, e.NewValue);
        #endregion

        #region CheckedForeground
        public static readonly DependencyProperty CheckedForegroundProperty = DependencyProperty.Register(
            "CheckedForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedForegroundChanged));

        public Brush CheckedForeground
        {
            get => (Brush)GetValue(CheckedForegroundProperty);
            set => SetValue(CheckedForegroundProperty, value);
        }

        private static void OnCheckedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, CheckedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Focused
        #region FocusedBackground
        public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.Register(
            "FocusedBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedBackgroundChanged));

        public Brush FocusedBackground
        {
            get => (Brush)GetValue(FocusedBackgroundProperty);
            set => SetValue(FocusedBackgroundProperty, value);
        }

        private static void OnFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, FocusedBackgroundProperty, e.NewValue);
        #endregion

        #region FocusedBorderBrush
        public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(
            "FocusedBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedBorderBrushChanged));

        public Brush FocusedBorderBrush
        {
            get => (Brush)GetValue(FocusedBorderBrushProperty);
            set => SetValue(FocusedBorderBrushProperty, value);
        }

        private static void OnFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, FocusedBorderBrushProperty, e.NewValue);
        #endregion

        #region FocusedForeground
        public static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.Register(
            "FocusedForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedForegroundChanged));

        public Brush FocusedForeground
        {
            get => (Brush)GetValue(FocusedForegroundProperty);
            set => SetValue(FocusedForegroundProperty, value);
        }

        private static void OnFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, FocusedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Pressed
        #region PressedBackground
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
            "PressedBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedBackgroundChanged));

        public Brush PressedBackground
        {
            get => (Brush)GetValue(PressedBackgroundProperty);
            set => SetValue(PressedBackgroundProperty, value);
        }

        private static void OnPressedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PressedBackgroundProperty, e.NewValue);
        #endregion

        #region PressedBorderBrush
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(
            "PressedBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedBorderBrushChanged));

        public Brush PressedBorderBrush
        {
            get => (Brush)GetValue(PressedBorderBrushProperty);
            set => SetValue(PressedBorderBrushProperty, value);
        }

        private static void OnPressedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PressedBorderBrushProperty, e.NewValue);
        #endregion

        #region PressedForeground
        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
            "PressedForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedForegroundChanged));

        public Brush PressedForeground
        {
            get => (Brush)GetValue(PressedForegroundProperty);
            set => SetValue(PressedForegroundProperty, value);
        }

        private static void OnPressedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PressedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Disabled
        #region DisabledBackground
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBackgroundChanged));

        public Brush DisabledBackground
        {
            get => (Brush)GetValue(DisabledBackgroundProperty);
            set => SetValue(DisabledBackgroundProperty, value);
        }

        private static void OnDisabledBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, DisabledBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledBorderBrush
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBorderBrushChanged));

        public Brush DisabledBorderBrush
        {
            get => (Brush)GetValue(DisabledBorderBrushProperty);
            set => SetValue(DisabledBorderBrushProperty, value);
        }

        private static void OnDisabledBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, DisabledBorderBrushProperty, e.NewValue);
        #endregion

        #region DisabledForeground
        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledForegroundChanged));

        public Brush DisabledForeground
        {
            get => (Brush)GetValue(DisabledForegroundProperty);
            set => SetValue(DisabledForegroundProperty, value);
        }

        private static void OnDisabledForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, DisabledForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Theme
        /// <summary>
        /// Get/Sets the theme
        /// </summary>
        public static DependencyProperty ThemeProperty = DependencyProperty.Register(
            "Theme", typeof(string), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme
        {
            get { return (string)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapToggleButton ztb) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            // current theme
            string curThemeName = e.OldValue as string;
            string curRegisteredThemeName = ztb.GetRegistrationName(curThemeName, ztb.GetType());

            if (ztb._rdThemeDictionaries.ContainsKey(curRegisteredThemeName))
            {
                // remove current theme
                ResourceDictionary curThemeDictionary = ztb._rdThemeDictionaries[curRegisteredThemeName];
                ztb.Resources.MergedDictionaries.Remove(curThemeDictionary);
            }

            // new theme name
            string newThemeName = e.NewValue as string;

            if (string.IsNullOrEmpty(newThemeName))
                newThemeName = ztb._rdTemplateDictionaries.FirstOrDefault().Key;

            string newRegisteredThemeName = ztb.GetRegistrationName(newThemeName, ztb.GetType());

            // add the resource
            if (!ztb._rdThemeDictionaries.ContainsKey(newRegisteredThemeName))
            {
                throw new ArgumentNullException("Invalid Theme property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newThemeDictionary = ztb._rdThemeDictionaries[newRegisteredThemeName];
                ztb.Resources.MergedDictionaries.Add(newThemeDictionary);
                // Raise theme successfully changed event
                ztb.RaiseEvent(new RoutedEventArgs(ThemeChangedSuccessEvent, ztb));
            }

            ztb.RaisePropertyChanged(new PropertyChangedEventArgs(ThemePropName));
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

        #region Padding
        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PaddingProperty, e.NewValue);
        #endregion

        #region Height
        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapToggleButton ztb)
            {
                if (ztb.ButtonTemplate == "Round")
                {
                    double height = (double)e.NewValue;
                    if (double.IsNaN(height) || double.IsInfinity(height))
                    {
                        ztb.Height = ztb.MinHeight;
                    }
                    else if (ztb.Width != height)
                    {
                        ztb.Width = height;
                    }
                }
            }
        }
        #endregion

        #region Width
        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapToggleButton ztb)
            {
                if (ztb.ButtonTemplate == "Round")
                {
                    double width = (double)e.NewValue;
                    if (double.IsNaN(width) || double.IsInfinity(width))
                    {
                        ztb.Width = ztb.MinWidth;
                    }
                    else if (ztb.Height != width)
                    {
                        ztb.Height = width;
                    }
                }
            }
        }
        #endregion
        #endregion

        #region ThemeChangedSuccessEvent
        public static readonly RoutedEvent ThemeChangedSuccessEvent = EventManager.RegisterRoutedEvent(
            "ThemeChangedSuccess", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapToggleButton));

        public event RoutedEventHandler ThemeChangedSuccess
        {
            add { AddHandler(ThemeChangedSuccessEvent, value); }
            remove { RemoveHandler(ThemeChangedSuccessEvent, value); }
        }

        protected virtual void OnThemeChangedSuccess(object sender, RoutedEventArgs e)
        {
            _defaultThemeProperties.Clear();
            // Control
            SetThemePropertyDefault(BorderThicknessProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.BorderThicknessKey));
            SetThemePropertyDefault(PaddingProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.PaddingKey));
            // Normal
            SetThemePropertyDefault(BackgroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.BackgroundKey));
            SetThemePropertyDefault(BorderBrushProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.BorderBrushKey));
            SetThemePropertyDefault(ForegroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.ForegroundKey));
            // Normal
            SetThemePropertyDefault(CheckedBackgroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.CheckedBackgroundKey));
            SetThemePropertyDefault(CheckedBorderBrushProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.CheckedBorderBrushKey));
            SetThemePropertyDefault(CheckedForegroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.CheckedForegroundKey));
            // Focused
            SetThemePropertyDefault(FocusedBackgroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.FocusedBackgroundKey));
            SetThemePropertyDefault(FocusedBorderBrushProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.FocusedBorderBrushKey));
            SetThemePropertyDefault(FocusedForegroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.FocusedForegroundKey));
            // Pressed
            SetThemePropertyDefault(PressedBackgroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.PressedBackgroundKey));
            SetThemePropertyDefault(PressedBorderBrushProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.PressedBorderBrushKey));
            SetThemePropertyDefault(PressedForegroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.PressedForegroundKey));
            // Disabled
            SetThemePropertyDefault(DisabledBackgroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.DisabledBackgroundKey));
            SetThemePropertyDefault(DisabledBorderBrushProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.DisabledBorderBrushKey));
            SetThemePropertyDefault(DisabledForegroundProperty, TryFindResource(ResourceKeys.ZapButtonResourceKeys.DisabledForegroundKey));
        }
        #endregion

        #region Constructors
        static ZapToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(typeof(ZapToggleButton)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;
            // Control
            BorderThicknessProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
            PaddingProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(new Thickness(0), options, OnPaddingChanged));
            WidthProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, OnWidthChanged));
            HeightProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, OnHeightChanged));
            // Normal
            BackgroundProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            ForegroundProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(null, options, OnForegroundChanged));
        }

        public ZapToggleButton()
        {
            _defaultThemeProperties = new Dictionary<DependencyProperty, object>();

            _rdTemplateDictionaries = new Dictionary<string, ResourceDictionary>();
            RegisterAttachedTemplates();

            // Load first template
            if (_rdTemplateDictionaries.Any())
                SetCurrentValue(ButtonTemplateProperty, GetThemeName(_rdTemplateDictionaries.FirstOrDefault().Key));

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

            ContentPresenter content = (ContentPresenter)Template.FindName("PART_Content", this);
            if (content != null)
            {
                content.SizeChanged += (s, e) =>
                {
                    if (ButtonTemplate == "Round")
                    {
                        Width = content.ActualWidth + 4;
                    }
                };
            }

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
        internal void LoadDefaultTheme(ZapButtonThemes theme, Type ownerType)
        {
            string registrationName = GetRegistrationName(theme, ownerType);
            Resources.MergedDictionaries.Add(_rdThemeDictionaries[registrationName]);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        private string GetRegistrationName(ZapButtonThemes theme, Type ownerType)
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

        public string GetThemeName(string key)
        {
            return key?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        internal void SetThemePropertyDefault(DependencyProperty p, object value)
        {
            _defaultThemeProperties.Add(p, value);
            SetCurrentValue(p, value);
        }

        private static void SetValueCommon(DependencyObject o, DependencyProperty p, object value)
        {
            if (o is ZapToggleButton ztb)
            {
                if (!(BindingOperations.GetBinding(ztb, p) is Binding))
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
                        if (ztb._defaultThemeProperties.ContainsKey(p))
                            value = ztb._defaultThemeProperties[p];
                    }

                    ztb.SetCurrentValue(p, value);
                }
            }
        }
        #endregion

        #region Templating
        public void RegisterTemplate(TemplatePath template, Type ownerType)
        {
            // test args
            if (template.Name == null || template.DictionaryPath == null)
                throw new ArgumentNullException("Template name/path is null");

            if (ownerType == null)
                throw new ArgumentNullException("Invalid ownerType");

            string registrationName = GetRegistrationName(template.Name, ownerType);

            try
            {
                if (!_rdTemplateDictionaries.ContainsKey(registrationName))
                {
                    // create the Uri
                    Uri templateUri = new Uri(template.DictionaryPath, UriKind.Relative);
                    // register the new template
                    _rdTemplateDictionaries[registrationName] = Application.LoadComponent(templateUri) as ResourceDictionary;
                }
            }
            catch (Exception)
            { }
        }

        private void RegisterAttachedTemplates()
        {
            var templateFields = GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(TemplatePath));

            foreach (var field in templateFields)
            {
                RegisterTemplate((TemplatePath)field.GetValue(this), GetType());
            }
        }

        /// <summary>
        /// Load the default template
        /// </summary>
        internal void LoadDefaultTemplate(string registrationName)
        {
            Resources.MergedDictionaries.Add(_rdTemplateDictionaries[registrationName]);
        }

        private string GetTemplateName(string key)
        {
            return key?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        /// <summary>
        /// Load the default template
        /// </summary>
        private void LoadDefaultTemplate(ZapButtonTemplates template, Type ownerType)
        {
            string registrationName = GetRegistrationName(template, ownerType);
            LoadDefaultTemplate(registrationName);
        }

        /// <summary>
        /// Get template formal registration name
        /// </summary>
        private string GetRegistrationName(ZapButtonTemplates template, Type ownerType)
        {
            return GetRegistrationName(template.ToString(), ownerType);
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /*
    public class ZapToggleButton : ToggleButton, ITemplate
    {
        #region Fields
        private bool _hasInitialized;
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
        #region Checked
        #region CheckedBackground
        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(
            "CheckedBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedBackgroundChanged));

        public Brush CheckedBackground { get => (Brush)GetValue(CheckedBackgroundProperty); set => SetValue(CheckedBackgroundProperty, value); }
        private static void OnCheckedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(CheckedBackgroundProperty, e.NewValue);
        #endregion

        #region CheckedBorderBrush
        public static readonly DependencyProperty CheckedBorderBrushProperty = DependencyProperty.Register(
            "CheckedBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedBorderBrushChanged));

        public Brush CheckedBorderBrush { get => (Brush)GetValue(CheckedBorderBrushProperty); set => SetValue(CheckedBorderBrushProperty, value); }
        private static void OnCheckedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(CheckedBorderBrushProperty, e.NewValue);
        #endregion

        #region CheckedForeground
        public static readonly DependencyProperty CheckedForegroundProperty = DependencyProperty.Register(
            "CheckedForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnCheckedForegroundChanged));

        public Brush CheckedForeground { get => (Brush)GetValue(CheckedForegroundProperty); set => SetValue(CheckedForegroundProperty, value); }
        private static void OnCheckedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(CheckedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Focused
        #region FocusedBackground
        public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.Register(
            "FocusedBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedBackgroundChanged));

        public Brush FocusedBackground { get => (Brush)GetValue(FocusedBackgroundProperty); set => SetValue(FocusedBackgroundProperty, value); }
        private static void OnFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(FocusedBackgroundProperty, e.NewValue);
        #endregion

        #region FocusedBorderBrush
        public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(
            "FocusedBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedBorderBrushChanged));

        public Brush FocusedBorderBrush { get => (Brush)GetValue(FocusedBorderBrushProperty); set => SetValue(FocusedBorderBrushProperty, value); }
        private static void OnFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(FocusedBorderBrushProperty, e.NewValue);
        #endregion

        #region FocusedForeground
        public static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.Register(
            "FocusedForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnFocusedForegroundChanged));

        public Brush FocusedForeground { get => (Brush)GetValue(FocusedForegroundProperty); set => SetValue(FocusedForegroundProperty, value); }
        private static void OnFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(FocusedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Pressed
        #region PressedBackground
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
            "PressedBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedBackgroundChanged));

        public Brush PressedBackground { get => (Brush)GetValue(PressedBackgroundProperty); set => SetValue(PressedBackgroundProperty, value); }
        private static void OnPressedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(PressedBackgroundProperty, e.NewValue);
        #endregion

        #region PressedBorderBrush
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(
            "PressedBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedBorderBrushChanged));

        public Brush PressedBorderBrush { get => (Brush)GetValue(PressedBorderBrushProperty); set => SetValue(PressedBorderBrushProperty, value); }
        private static void OnPressedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(PressedBorderBrushProperty, e.NewValue);
        #endregion

        #region PressedForeground
        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
            "PressedForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnPressedForegroundChanged));

        public Brush PressedForeground { get => (Brush)GetValue(PressedForegroundProperty); set => SetValue(PressedForegroundProperty, value); }
        private static void OnPressedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(PressedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Disabled
        #region DisabledBackground
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBackgroundChanged));

        public Brush DisabledBackground { get => (Brush)GetValue(DisabledBackgroundProperty); set => SetValue(DisabledBackgroundProperty, value); }
        private static void OnDisabledBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledBorderBrush
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBorderBrushChanged));

        public Brush DisabledBorderBrush { get => (Brush)GetValue(DisabledBorderBrushProperty); set => SetValue(DisabledBorderBrushProperty, value); }
        private static void OnDisabledBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledBorderBrushProperty, e.NewValue);
        #endregion

        #region DisabledForeground
        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground", typeof(Brush), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledForegroundChanged));

        public Brush DisabledForeground { get => (Brush)GetValue(DisabledForegroundProperty); set => SetValue(DisabledForegroundProperty, value); }
        private static void OnDisabledForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledForegroundProperty, e.NewValue);
        #endregion
        #endregion
        #endregion

        #region Template Properties
        #region HasInitialized
        public bool HasInitialized { get => _hasInitialized; private set => Set(ref _hasInitialized, value); }
        #endregion

        #region TemplateDictionaries
        public Dictionary<string, ResourceDictionary> TemplateDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion

        #region ZapTemplate
        public static readonly DependencyProperty ZapTemplateProperty = DependencyProperty.Register(
            "ZapTemplate", typeof(string), typeof(ZapToggleButton),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnZapTemplateChanged),
                new CoerceValueCallback(CoerceZapTemplateChange)));

        public string ZapTemplate { get => (string)GetValue(ZapTemplateProperty); set => SetValue(ZapTemplateProperty, value); }

        private static void OnZapTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.TemplateChanged(e, TemplateChangedEvent);
        //{
        //    // test args
        //    if (!(d is ZapToggleButton ztb) || e == null)
        //        throw new ArgumentNullException("Invalid ZapTemplate property");

        //    string curTemplateName = e.OldValue as string;
        //    string curRegisteredTemplateName = curTemplateName.TemplateRegistrationName(ztb.GetType());

        //    if (ztb.TemplateDictionaries.ContainsKey(curRegisteredTemplateName))
        //    {
        //        // remove current template
        //        ResourceDictionary curTemplateDictionary = ztb.TemplateDictionaries[curRegisteredTemplateName];
        //        ztb.Resources.MergedDictionaries.Remove(curTemplateDictionary);
        //    }

        //    // new template name
        //    string newTemplateName = e.NewValue as string;
        //    string newRegisteredTemplateName = !string.IsNullOrEmpty(newTemplateName) ?
        //        newTemplateName.TemplateRegistrationName(ztb.GetType())
        //        : ztb.TemplateDictionaries.FirstOrDefault().Key;

        //    // add the resource
        //    if (!ztb.TemplateDictionaries.ContainsKey(newRegisteredTemplateName))
        //    {
        //        throw new ArgumentNullException("Invalid Template property");
        //    }
        //    else
        //    {
        //        // add the dictionary
        //        ResourceDictionary newTemplateDictionary = ztb.TemplateDictionaries[newRegisteredTemplateName];
        //        ztb.Resources.MergedDictionaries.Add(newTemplateDictionary);

        //        if (ztb.HasInitialized)
        //            ztb.OnApplyTemplate();

        //        ztb.RaiseEvent(new TemplateChangedEventArgs(TemplateChangedEvent, ztb, curRegisteredTemplateName, newRegisteredTemplateName));
        //    }

        //    ztb.RaisePropertyChanged(new PropertyChangedEventArgs("ZapTemplate"));
        //}

        private static object CoerceZapTemplateChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion
        #endregion

        #region Theme Properties
        #region DefaultThemeProperties
        public Dictionary<DependencyProperty, object> DefaultThemeProperties { get; internal set; } = new Dictionary<DependencyProperty, object>();
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

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.ThemeChanged(e, ThemeChangedEvent);

        private static object CoerceThemeChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion

        #region ThemeDictionaries
        public Dictionary<string, ResourceDictionary> ThemeDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion
        #endregion

        #region Native Properties Changed
        #region Background
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BorderBrushProperty, e.NewValue);
        #endregion

        #region BorderThickness
        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BorderThicknessProperty, e.NewValue);
        #endregion

        #region Foreground
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ForegroundProperty, e.NewValue);
        #endregion

        #region Padding
        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(PaddingProperty, e.NewValue);
        #endregion

        #region Height
        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapToggleButton ztb)
            {
                if (ztb.ZapTemplate == "Round")
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
                if (ztb.ZapTemplate == "Round")
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

        #region Events
        #region TemplateChanged
        public static readonly RoutedEvent TemplateChangedEvent = EventManager.RegisterRoutedEvent(
            "TemplateChanged", RoutingStrategy.Bubble, typeof(ITemplate.TemplateChangedEventHandler), typeof(ZapToggleButton));

        public event ITemplate.TemplateChangedEventHandler TemplateChanged { add => AddHandler(TemplateChangedEvent, value); remove => RemoveHandler(TemplateChangedEvent, value); }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ZapToggleButton));

        public event ITheme.ThemeChangedEventHandler ThemeChanged { add => AddHandler(ThemeChangedEvent, value); remove => RemoveHandler(ThemeChangedEvent, value); }

        private void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();
            // Control
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ZapButtonResourceKeys.BorderThicknessKey);
            this.SetThemePropertyDefault(PaddingProperty, ResourceKeys.ZapButtonResourceKeys.PaddingKey);
            // Normal
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapButtonResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapButtonResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(ForegroundProperty, ResourceKeys.ZapButtonResourceKeys.ForegroundKey);
            // Checked
            this.SetThemePropertyDefault(CheckedBackgroundProperty, ResourceKeys.ZapButtonResourceKeys.CheckedBackgroundKey);
            this.SetThemePropertyDefault(CheckedBorderBrushProperty, ResourceKeys.ZapButtonResourceKeys.CheckedBorderBrushKey);
            this.SetThemePropertyDefault(CheckedForegroundProperty, ResourceKeys.ZapButtonResourceKeys.CheckedForegroundKey);
            // Focused
            this.SetThemePropertyDefault(FocusedBackgroundProperty, ResourceKeys.ZapButtonResourceKeys.FocusedBackgroundKey);
            this.SetThemePropertyDefault(FocusedBorderBrushProperty, ResourceKeys.ZapButtonResourceKeys.FocusedBorderBrushKey);
            this.SetThemePropertyDefault(FocusedForegroundProperty, ResourceKeys.ZapButtonResourceKeys.FocusedForegroundKey);
            // Pressed
            this.SetThemePropertyDefault(PressedBackgroundProperty, ResourceKeys.ZapButtonResourceKeys.PressedBackgroundKey);
            this.SetThemePropertyDefault(PressedBorderBrushProperty, ResourceKeys.ZapButtonResourceKeys.PressedBorderBrushKey);
            this.SetThemePropertyDefault(PressedForegroundProperty, ResourceKeys.ZapButtonResourceKeys.PressedForegroundKey);
            // Disabled
            this.SetThemePropertyDefault(DisabledBackgroundProperty, ResourceKeys.ZapButtonResourceKeys.DisabledBackgroundKey);
            this.SetThemePropertyDefault(DisabledBorderBrushProperty, ResourceKeys.ZapButtonResourceKeys.DisabledBorderBrushKey);
            this.SetThemePropertyDefault(DisabledForegroundProperty, ResourceKeys.ZapButtonResourceKeys.DisabledForegroundKey);
        }
        #endregion
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
            ForegroundProperty.OverrideMetadata(typeof(ZapToggleButton), new FrameworkPropertyMetadata(null, options, OnForegroundChanged) { Inherits = false });
        }

        public ZapToggleButton()
        {
            // Load Templates
            this.RegisterAttachedTemplates(GetType());
            this.LoadDefaultTemplate(ZapTemplateProperty);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterAttachedThemes(GetType());
            this.LoadDefaultTheme(ThemeProperty);
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;

            ContentPresenter content = (ContentPresenter)Template.FindName("PART_Content", this);
            if (content != null)
            {
                content.SizeChanged += (s, e) =>
                {
                    if (ZapTemplate == "Round")
                    {
                        Width = content.ActualWidth + 4;
                    }
                };
            }

            if (ZapTemplate == "Round")
            {
                MinHeight = 20d;
                MinWidth = 20d;
            }
            else
            {
                MinHeight = 0d;
                MinWidth = 0d;
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
    */
}

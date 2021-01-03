using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    public sealed class ZapScrollViewer : ScrollViewer, ITemplate
    {
        #region Fields
        private bool _hasInitialized;
        #endregion

        #region Theme Declarations
        public static ThemePath Oceatech = new ThemePath(ZapScrollViewerThemes.Oceatech, "/ZapanControls;component/Themes/ZapScrollViewer/Oceatech.xaml");
        public static ThemePath Contactel = new ThemePath(ZapScrollViewerThemes.Contactel, "/ZapanControls;component/Themes/ZapScrollViewer/Contactel.xaml");
        #endregion

        #region Properties
        #region Disabled
        #region DisabledBackground
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBackgroundChanged));

        public Brush DisabledBackground { get => (Brush)GetValue(DisabledBackgroundProperty); set => SetValue(DisabledBackgroundProperty, value); }

        private static void OnDisabledBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledBorderBrush
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBorderBrushChanged));

        public Brush DisabledBorderBrush { get => (Brush)GetValue(DisabledBorderBrushProperty); set => SetValue(DisabledBorderBrushProperty, value); }

        private static void OnDisabledBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledBorderBrushProperty, e.NewValue);
        #endregion
        #endregion

        #region ScrollBar Control
        #region ScrollBarBackground
        public static readonly DependencyProperty ScrollBarBackgroundProperty = DependencyProperty.Register(
            "ScrollBarBackground", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarBackgroundChanged));

        public Brush ScrollBarBackground { get => (Brush)GetValue(ScrollBarBackgroundProperty); set => SetValue(ScrollBarBackgroundProperty, value); }

        private static void OnScrollBarBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarBackgroundProperty, e.NewValue);
        #endregion

        #region ScrollBarBorderBrush
        public static readonly DependencyProperty ScrollBarBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarBorderBrush", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarBorderBrushChanged));

        public Brush ScrollBarBorderBrush { get => (Brush)GetValue(ScrollBarBorderBrushProperty); set => SetValue(ScrollBarBorderBrushProperty, value); }

        private static void OnScrollBarBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarBorderBrushProperty, e.NewValue);
        #endregion

        #region ScrollBarBorderThickness
        public static readonly DependencyProperty ScrollBarBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarBorderThickness", typeof(Thickness), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarBorderThicknessChanged));

        public Thickness ScrollBarBorderThickness { get => (Thickness)GetValue(ScrollBarBorderThicknessProperty); set => SetValue(ScrollBarBorderThicknessProperty, value); }

        private static void OnScrollBarBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarBorderThicknessProperty, e.NewValue);
        #endregion

        #region ScrollBarWidth
        public static readonly DependencyProperty ScrollBarWidthProperty = DependencyProperty.Register(
            "ScrollBarWidth", typeof(double), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(20.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnScrollBarWidthChanged));

        public double ScrollBarWidth { get => (double)GetValue(ScrollBarWidthProperty); set => SetValue(ScrollBarWidthProperty, value); }

        private static void OnScrollBarWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarWidthProperty, e.NewValue);
        #endregion

        #region ScrollBarHeight
        public static readonly DependencyProperty ScrollBarHeightProperty = DependencyProperty.Register(
            "ScrollBarHeight", typeof(double), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(20.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnScrollBarHeightChanged));

        public double ScrollBarHeight { get => (double)GetValue(ScrollBarHeightProperty); set => SetValue(ScrollBarHeightProperty, value); }

        private static void OnScrollBarHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarHeightProperty, e.NewValue);
        #endregion
        #endregion

        #region Buttons
        #region ButtonBackground
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register(
            "ButtonBackground", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnButtonBackgroundChanged));

        public Brush ButtonBackground { get => (Brush)GetValue(ButtonBackgroundProperty); set => SetValue(ButtonBackgroundProperty, value); }

        private static void OnButtonBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ButtonBackgroundProperty, e.NewValue);
        #endregion

        #region ButtonBorderBrush
        public static readonly DependencyProperty ButtonBorderBrushProperty = DependencyProperty.Register(
            "ButtonBorderBrush", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnButtonBorderBrushChanged));

        public Brush ButtonBorderBrush { get => (Brush)GetValue(ButtonBorderBrushProperty); set => SetValue(ButtonBorderBrushProperty, value); }

        private static void OnButtonBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ButtonBorderBrushProperty, e.NewValue);
        #endregion

        #region ButtonBorderThickness
        public static readonly DependencyProperty ButtonBorderThicknessProperty = DependencyProperty.Register(
            "ButtonBorderThickness", typeof(Thickness), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnButtonBorderThicknessChanged));

        public Thickness ButtonBorderThickness { get => (Thickness)GetValue(ButtonBorderThicknessProperty); set => SetValue(ButtonBorderThicknessProperty, value); }

        private static void OnButtonBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ButtonBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Thumb
        #region ThumbInnerBackground
        public static readonly DependencyProperty ThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ThumbInnerBackground", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbInnerBackgroundChanged));

        public Brush ThumbInnerBackground { get => (Brush)GetValue(ThumbInnerBackgroundProperty); set => SetValue(ThumbInnerBackgroundProperty, value); }

        private static void OnThumbInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ThumbInnerBackgroundProperty, e.NewValue);
        #endregion

        #region ThumbBackground
        public static readonly DependencyProperty ThumbBackgroundProperty = DependencyProperty.Register(
            "ThumbBackground", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbBackgroundChanged));

        public Brush ThumbBackground { get => (Brush)GetValue(ThumbBackgroundProperty); set => SetValue(ThumbBackgroundProperty, value); }

        private static void OnThumbBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ThumbBackgroundProperty, e.NewValue);
        #endregion

        #region ThumbBorderBrush
        public static readonly DependencyProperty ThumbBorderBrushProperty = DependencyProperty.Register(
            "ThumbBorderBrush", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbBorderBrushChanged));

        public Brush ThumbBorderBrush { get => (Brush)GetValue(ThumbBorderBrushProperty); set => SetValue(ThumbBorderBrushProperty, value); }

        private static void OnThumbBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ThumbBorderBrushProperty, e.NewValue);
        #endregion

        #region ThumbBorderThickness
        public static readonly DependencyProperty ThumbBorderThicknessProperty = DependencyProperty.Register(
            "ThumbBorderThickness", typeof(Thickness), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbBorderThicknessChanged));

        public Thickness ThumbBorderThickness { get => (Thickness)GetValue(ThumbBorderThicknessProperty); set => SetValue(ThumbBorderThicknessProperty, value); }

        private static void OnThumbBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ThumbBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Disabled
        #region DisabledThumbInnerBackground
        public static readonly DependencyProperty DisabledThumbInnerBackgroundProperty = DependencyProperty.Register(
            "DisabledThumbInnerBackground", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledThumbInnerBackgroundChanged));

        public Brush DisabledThumbInnerBackground { get => (Brush)GetValue(DisabledThumbInnerBackgroundProperty); set => SetValue(DisabledThumbInnerBackgroundProperty, value); }

        private static void OnDisabledThumbInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledThumbInnerBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledButtonBackground
        public static readonly DependencyProperty DisabledButtonBackgroundProperty = DependencyProperty.Register(
            "DisabledButtonBackground", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledButtonBackgroundChanged));

        public Brush DisabledButtonBackground { get => (Brush)GetValue(DisabledButtonBackgroundProperty); set => SetValue(DisabledButtonBackgroundProperty, value); }

        private static void OnDisabledButtonBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledButtonBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledButtonBorderBrush
        public static readonly DependencyProperty DisabledButtonBorderBrushProperty = DependencyProperty.Register(
            "DisabledButtonBorderBrush", typeof(Brush), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledButtonBorderBrushChanged));

        public Brush DisabledButtonBorderBrush { get => (Brush)GetValue(DisabledButtonBorderBrushProperty); set => SetValue(DisabledButtonBorderBrushProperty, value); }

        private static void OnDisabledButtonBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledButtonBorderBrushProperty, e.NewValue);
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
            "ZapTemplate", typeof(string), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnZapTemplateChanged),
                new CoerceValueCallback(CoerceZapTemplateChange)));

        public string ZapTemplate { get => (string)GetValue(ZapTemplateProperty); set => SetValue(ZapTemplateProperty, value); }

        private static void OnZapTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) //=> d.TemplateChanged(e, TemplateChangedEvent);
        {
            // test args
            if (!(d is ZapScrollViewer sv) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            string curTemplateName = e.OldValue as string;
            string newTemplateName = e.NewValue as string;

            sv.RaiseEvent(new TemplateChangedEventArgs(TemplateChangedEvent, sv, curTemplateName, newTemplateName));
        }

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
            "Theme", typeof(string), typeof(ZapScrollViewer),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }
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

        #region Padding
        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(PaddingProperty, e.NewValue);
        #endregion
        #endregion

        #region Events
        #region TemplateChanged
        public static readonly RoutedEvent TemplateChangedEvent = EventManager.RegisterRoutedEvent(
            "TemplateChanged", RoutingStrategy.Bubble, typeof(ITemplate.TemplateChangedEventHandler), typeof(ZapScrollViewer));

        public event ITemplate.TemplateChangedEventHandler TemplateChanged { add => AddHandler(TemplateChangedEvent, value); remove => RemoveHandler(TemplateChangedEvent, value); }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ZapScrollViewer));

        public event ITheme.ThemeChangedEventHandler ThemeChanged { add => AddHandler(ThemeChangedEvent, value); remove => RemoveHandler(ThemeChangedEvent, value); }

        private void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();
            // Control
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapScrollViewerResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ZapScrollViewerResourceKeys.BorderThicknessKey);
            this.SetThemePropertyDefault(PaddingProperty, ResourceKeys.ZapScrollViewerResourceKeys.PaddingKey);
            // Disabled
            this.SetThemePropertyDefault(DisabledBackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.DisabledBackgroundKey);
            this.SetThemePropertyDefault(DisabledBorderBrushProperty, ResourceKeys.ZapScrollViewerResourceKeys.DisabledBorderBrushKey);
            // ScrollBar Control
            this.SetThemePropertyDefault(ScrollBarBackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.ScrollBarBackgroundKey);
            this.SetThemePropertyDefault(ScrollBarBorderBrushProperty, ResourceKeys.ZapScrollViewerResourceKeys.ScrollBarBorderBrushKey);
            this.SetThemePropertyDefault(ScrollBarBorderThicknessProperty, ResourceKeys.ZapScrollViewerResourceKeys.ScrollBarBorderThicknessKey);
            this.SetThemePropertyDefault(ScrollBarHeightProperty, ResourceKeys.ZapScrollViewerResourceKeys.ScrollBarHeightKey);
            this.SetThemePropertyDefault(ScrollBarWidthProperty, ResourceKeys.ZapScrollViewerResourceKeys.ScrollBarWidthKey);
            // ScrollBar Buttons
            this.SetThemePropertyDefault(ButtonBackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.ButtonBackgroundKey);
            this.SetThemePropertyDefault(ButtonBorderBrushProperty, ResourceKeys.ZapScrollViewerResourceKeys.ButtonBorderBrushKey);
            this.SetThemePropertyDefault(ButtonBorderThicknessProperty, ResourceKeys.ZapScrollViewerResourceKeys.ButtonBorderThicknessKey);
            // ScrollBar Thumb
            this.SetThemePropertyDefault(ThumbInnerBackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.ThumbInnerBackgroundKey);
            this.SetThemePropertyDefault(ThumbBackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.ThumbBackgroundKey);
            this.SetThemePropertyDefault(ThumbBorderBrushProperty, ResourceKeys.ZapScrollViewerResourceKeys.ThumbBorderBrushKey);
            this.SetThemePropertyDefault(ThumbBorderThicknessProperty, ResourceKeys.ZapScrollViewerResourceKeys.ThumbBorderThicknessKey);
            // ScrollBar Disabled
            this.SetThemePropertyDefault(DisabledThumbInnerBackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.DisabledThumbInnerBackgroundKey);
            this.SetThemePropertyDefault(DisabledButtonBackgroundProperty, ResourceKeys.ZapScrollViewerResourceKeys.DisabledButtonBackgroundKey);
            this.SetThemePropertyDefault(DisabledButtonBorderBrushProperty, ResourceKeys.ZapScrollViewerResourceKeys.DisabledButtonBorderBrushKey);
        }
        #endregion
        #endregion

        #region Constructors

        static ZapScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapScrollViewer), new FrameworkPropertyMetadata(typeof(ZapScrollViewer)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;
            // Control
            BorderThicknessProperty.OverrideMetadata(typeof(ZapScrollViewer), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
            PaddingProperty.OverrideMetadata(typeof(ZapScrollViewer), new FrameworkPropertyMetadata(new Thickness(0), options, OnPaddingChanged));
            // Normal
            BackgroundProperty.OverrideMetadata(typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
        }

        public ZapScrollViewer()
        {
            // Load Templates
            this.RegisterAttachedTemplates(GetType());
            this.LoadDefaultTemplate(ZapTemplateProperty);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterAttachedThemes(typeof(ZapScrollViewer));
            this.RegisterAttachedThemes(GetType());
            this.LoadDefaultTheme(ThemeProperty);
        }

        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;
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
        public bool Set<TClass, TProp>(Expression<Func<TClass, TProp>> propertyNameExpression, ref TProp oldValue, TProp newValue)
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

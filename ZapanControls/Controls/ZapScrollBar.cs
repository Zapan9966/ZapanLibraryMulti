using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Helpers;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    public sealed class ZapScrollBar : ScrollBar, ITemplate<string>
    {
        #region Fields
        private bool _hasInitialized;
        #endregion

        #region Template Declaration
        public static TemplatePath Flat = new TemplatePath(ZapScrollBarTemplates.Flat, "/ZapanControls;component/Themes/ZapScrollBar/Template.Flat.xaml");
        public static TemplatePath Rounded = new TemplatePath(ZapScrollBarTemplates.Rounded, "/ZapanControls;component/Themes/ZapScrollBar/Template.Rounded.xaml");
        #endregion

        #region Properties
        #region Buttons
        #region ButtonBackground
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register(
            "ButtonBackground", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnButtonBackgroundChanged));

        public Brush ButtonBackground 
        { 
            get => (Brush)GetValue(ButtonBackgroundProperty); 
            set => SetValue(ButtonBackgroundProperty, value); 
        }

        private static void OnButtonBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ButtonBackgroundProperty, e.NewValue);
        #endregion

        #region ButtonBorderBrush
        public static readonly DependencyProperty ButtonBorderBrushProperty = DependencyProperty.Register(
            "ButtonBorderBrush", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnButtonBorderBrushChanged));

        public Brush ButtonBorderBrush 
        { 
            get => (Brush)GetValue(ButtonBorderBrushProperty); 
            set => SetValue(ButtonBorderBrushProperty, value); 
        }

        private static void OnButtonBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ButtonBorderBrushProperty, e.NewValue);
        #endregion

        #region ButtonBorderThickness
        public static readonly DependencyProperty ButtonBorderThicknessProperty = DependencyProperty.Register(
            "ButtonBorderThickness", typeof(Thickness), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnButtonBorderThicknessChanged));

        public Thickness ButtonBorderThickness 
        { 
            get => (Thickness)GetValue(ButtonBorderThicknessProperty); 
            set => SetValue(ButtonBorderThicknessProperty, value); 
        }

        private static void OnButtonBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ButtonBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Thumb
        #region ThumbInnerBackground
        public static readonly DependencyProperty ThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ThumbInnerBackground", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbInnerBackgroundChanged));

        public Brush ThumbInnerBackground 
        { 
            get => (Brush)GetValue(ThumbInnerBackgroundProperty); 
            set => SetValue(ThumbInnerBackgroundProperty, value); 
        }

        private static void OnThumbInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ThumbInnerBackgroundProperty, e.NewValue);
        #endregion

        #region ThumbBackground
        public static readonly DependencyProperty ThumbBackgroundProperty = DependencyProperty.Register(
            "ThumbBackground", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbBackgroundChanged));

        public Brush ThumbBackground 
        { 
            get => (Brush)GetValue(ThumbBackgroundProperty); 
            set => SetValue(ThumbBackgroundProperty, value); 
        }

        private static void OnThumbBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ThumbBackgroundProperty, e.NewValue);
        #endregion

        #region ThumbBorderBrush
        public static readonly DependencyProperty ThumbBorderBrushProperty = DependencyProperty.Register(
            "ThumbBorderBrush", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbBorderBrushChanged));

        public Brush ThumbBorderBrush 
        { 
            get => (Brush)GetValue(ThumbBorderBrushProperty); 
            set => SetValue(ThumbBorderBrushProperty, value); 
        }

        private static void OnThumbBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ThumbBorderBrushProperty, e.NewValue);
        #endregion

        #region ThumbBorderThickness
        public static readonly DependencyProperty ThumbBorderThicknessProperty = DependencyProperty.Register(
            "ThumbBorderThickness", typeof(Thickness), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnThumbBorderThicknessChanged));

        public Thickness ThumbBorderThickness 
        { 
            get => (Thickness)GetValue(ThumbBorderThicknessProperty); 
            set => SetValue(ThumbBorderThicknessProperty, value); 
        }

        private static void OnThumbBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ThumbBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Disabled
        #region DisabledThumbInnerBackground
        public static readonly DependencyProperty DisabledThumbInnerBackgroundProperty = DependencyProperty.Register(
            "DisabledThumbInnerBackground", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledThumbInnerBackgroundChanged));

        public Brush DisabledThumbInnerBackground 
        { 
            get => (Brush)GetValue(DisabledThumbInnerBackgroundProperty); 
            set => SetValue(DisabledThumbInnerBackgroundProperty, value); 
        }

        private static void OnDisabledThumbInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(DisabledThumbInnerBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledButtonBackground
        public static readonly DependencyProperty DisabledButtonBackgroundProperty = DependencyProperty.Register(
            "DisabledButtonBackground", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledButtonBackgroundChanged));

        public Brush DisabledButtonBackground 
        { 
            get => (Brush)GetValue(DisabledButtonBackgroundProperty); 
            set => SetValue(DisabledButtonBackgroundProperty, value); 
        }

        private static void OnDisabledButtonBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(DisabledButtonBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledButtonBorderBrush
        public static readonly DependencyProperty DisabledButtonBorderBrushProperty = DependencyProperty.Register(
            "DisabledButtonBorderBrush", typeof(Brush), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledButtonBorderBrushChanged));

        public Brush DisabledButtonBorderBrush 
        { 
            get => (Brush)GetValue(DisabledButtonBorderBrushProperty); 
            set => SetValue(DisabledButtonBorderBrushProperty, value); 
        }

        private static void OnDisabledButtonBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(DisabledButtonBorderBrushProperty, e.NewValue);
        #endregion
        #endregion
        #endregion

        #region Template Properties
        #region HasInitialized
        public bool HasInitialized 
        { 
            get => _hasInitialized; 
            private set => Set(ref _hasInitialized, value); 
        }
        #endregion

        #region TemplateDictionaries
        public Dictionary<string, ResourceDictionary> TemplateDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion

        #region ZapTemplate
        public static readonly DependencyProperty ZapTemplateProperty = DependencyProperty.Register(
            "ZapTemplate", typeof(string), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnZapTemplateChanged),
                new CoerceValueCallback(CoerceZapTemplateChange)));

        public string ZapTemplate 
        { 
            get => (string)GetValue(ZapTemplateProperty); 
            set => SetValue(ZapTemplateProperty, value); 
        }

        private static void OnZapTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.TemplateChanged<string>(e, TemplateChangedEvent);

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
            "Theme", typeof(string), typeof(ZapScrollBar),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme 
        { 
            get => (string)GetValue(ThemeProperty); 
            set => SetValue(ThemeProperty, value); 
        }
        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.ThemeChanged(e, ThemeChangedEvent);

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
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(BorderBrushProperty, e.NewValue);
        #endregion

        #region BorderThickness
        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(BorderThicknessProperty, e.NewValue);
        #endregion

        #region Orientation
        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZapScrollBar sb = (ZapScrollBar)d;
            if (e.NewValue is Orientation orientation)
            {
                if (orientation == Orientation.Horizontal)
                {
                    if (double.IsNaN(sb.Height))
                    {
                        sb.Height = sb.Width;
                    }
                }
                else
                {
                    if (double.IsNaN(sb.Width))
                    {
                        sb.Width = sb.Height;
                    }
                }
            }
        }
        #endregion

        #region Height
        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is ZapButton zb)
            //{
            //    if (zb.ZapTemplate == "Round")
            //    {
            //        double height = (double)e.NewValue;
            //        if (double.IsNaN(height) || double.IsInfinity(height))
            //        {
            //            zb.Height = zb.MinHeight;
            //        }
            //        else if (zb.Width != height)
            //        {
            //            zb.Width = height;
            //        }
            //    }
            //}
        }
        #endregion

        #region Width
        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is ZapButton zb)
            //{
            //    if (zb.ZapTemplate == "Round")
            //    {
            //        double width = (double)e.NewValue;
            //        if (double.IsNaN(width) || double.IsInfinity(width))
            //        {
            //            zb.Width = zb.MinWidth;
            //        }
            //        else if (zb.Height != width)
            //        {
            //            zb.Height = width;
            //        }
            //    }
            //}
        }
        #endregion
        #endregion

        #region Events
        #region TemplateChanged
        public static readonly RoutedEvent TemplateChangedEvent = EventManager.RegisterRoutedEvent(
            "TemplateChanged", RoutingStrategy.Bubble, typeof(ITemplate<string>.TemplateChangedEventHandler), typeof(ZapScrollBar));

        public event ITemplate<string>.TemplateChangedEventHandler TemplateChanged 
        { 
            add => AddHandler(TemplateChangedEvent, value); 
            remove => RemoveHandler(TemplateChangedEvent, value); 
        }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ZapScrollBar));

        public event ITheme.ThemeChangedEventHandler ThemeChanged 
        { 
            add => AddHandler(ThemeChangedEvent, value); 
            remove => RemoveHandler(ThemeChangedEvent, value); 
        }

        private void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();
            // Normal
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapScrollBarResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapScrollBarResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ZapScrollBarResourceKeys.BorderThicknessKey);
            // Buttons
            this.SetThemePropertyDefault(ButtonBackgroundProperty, ResourceKeys.ZapScrollBarResourceKeys.ButtonBackgroundKey);
            this.SetThemePropertyDefault(ButtonBorderBrushProperty, ResourceKeys.ZapScrollBarResourceKeys.ButtonBorderBrushKey);
            this.SetThemePropertyDefault(ButtonBorderThicknessProperty, ResourceKeys.ZapScrollBarResourceKeys.ButtonBorderThicknessKey);
            // Thumb
            this.SetThemePropertyDefault(ThumbInnerBackgroundProperty, ResourceKeys.ZapScrollBarResourceKeys.ThumbInnerBackgroundKey);
            this.SetThemePropertyDefault(ThumbBackgroundProperty, ResourceKeys.ZapScrollBarResourceKeys.ThumbBackgroundKey);
            this.SetThemePropertyDefault(ThumbBorderBrushProperty, ResourceKeys.ZapScrollBarResourceKeys.ThumbBorderBrushKey);
            this.SetThemePropertyDefault(ThumbBorderThicknessProperty, ResourceKeys.ZapScrollBarResourceKeys.ThumbBorderThicknessKey);
            // Disabled
            this.SetThemePropertyDefault(DisabledThumbInnerBackgroundProperty, ResourceKeys.ZapScrollBarResourceKeys.DisabledThumbInnerBackgroundKey);
            this.SetThemePropertyDefault(DisabledButtonBackgroundProperty, ResourceKeys.ZapScrollBarResourceKeys.DisabledButtonBackgroundKey);
            this.SetThemePropertyDefault(DisabledButtonBorderBrushProperty, ResourceKeys.ZapScrollBarResourceKeys.DisabledButtonBorderBrushKey);
        }
        #endregion
        #endregion

        #region Constructors
        static ZapScrollBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(typeof(ZapScrollBar)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;

            // Normal
            BackgroundProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));

            OrientationProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsArrange, OnOrientationChanged));
            HeightProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, OnHeightChanged));
            WidthProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(15.0, FrameworkPropertyMetadataOptions.AffectsMeasure, OnWidthChanged));
        }

        public ZapScrollBar()
        {
            // Load Templates
            this.RegisterAttachedTemplates<string>(GetType());
            this.LoadDefaultTemplate<string>(ZapTemplateProperty);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterInternalThemes<ZapScrollBarThemes>();
            this.LoadDefaultTheme(ThemeProperty);
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;

            if (ZapTemplate == ZapScrollBarTemplates.Rounded.ToString())
            {
                if (GetTemplateChild("InnerTrack") is Rectangle rect 
                    && VisualTreeHelpers.FindChild(this, "Rectangle") is Rectangle rectThumb)
                {
                    rect.RadiusX = rectThumb.RadiusX;
                    rect.RadiusY = rectThumb.RadiusY;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ZapanControls.Automation.Peers;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    [TemplatePart(Name = "PART_Root", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Border", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Track", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Progress", Type = typeof(Border))]
    [TemplatePart(Name = "PART_GlowRect", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_Text", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_OuterValue", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_InnerValue", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Indicator", Type = typeof(ZapLoadingIndicator))]
    public sealed class ZapProgress : RangeBase, ITemplate<string>
    {
        #region Properties Names
        private const string PercentPropName = "Percent";
        private const string BorderTemplateName = "PART_Border";
        private const string TrackTemplateName = "PART_Track";
        private const string ProgressTemplateName = "PART_Progress";
        private const string InnerValueTemplateName = "PART_InnerValue";
        private const string GlowingRectTemplateName = "PART_GlowRect";
        #endregion

        #region Fields
        private bool _hasInitialized;
        private Border _border;
        private Border _track;
        private Border _progress;
        private TextBlock _innerValue;
        private Rectangle _glow;
        #endregion

        #region Template Declaration
        public static TemplatePath Flat = new TemplatePath(ZapProgressTemplates.Flat, "/ZapanControls;component/Themes/ZapProgress/Template.Flat.xaml");
        public static TemplatePath Glass = new TemplatePath(ZapProgressTemplates.Glass, "/ZapanControls;component/Themes/ZapProgress/Template.Glass.xaml");
        #endregion

        #region Properties
        #region Control
        #region CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(ZapProgress),
            new FrameworkPropertyMetadata(new CornerRadius(0d),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnCornerRadiusChanged)));

        public CornerRadius CornerRadius 
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty); 
            set => SetValue(CornerRadiusProperty, value); 
        }

        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(CornerRadiusProperty, e.NewValue);
        #endregion

        #region IsIndeterminate
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            "IsIndeterminate", typeof(bool), typeof(ZapProgress),
            new FrameworkPropertyMetadata(false,
                new PropertyChangedCallback(OnIsIndeterminateChanged)));

        public bool IsIndeterminate 
        { 
            get => (bool)GetValue(IsIndeterminateProperty); 
            set => SetValue(IsIndeterminateProperty, value); 
        }

        private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapProgress p)
            {
                // Invalidate automation peer
                if (UIElementAutomationPeer.FromElement(p) is ZapProgressAutomationPeer peer)
                {
                    peer.InvalidatePeer();
                }

                p.SetProgressBarGlowElementBrush();
                p.SetProgressBarIndicatorLength();
            }
        }
        #endregion

        #region Percent
        public double Percent
        {
            get
            {
                double min = Minimum;
                double max = Maximum;
                double val = Value;

                return Math.Floor(val / (max - min) * 100);
            }
        }
        #endregion

        #region ShowPercent
        public static readonly DependencyProperty ShowPercentProperty = DependencyProperty.Register(
            "ShowPercent", typeof(bool), typeof(ZapProgress), new FrameworkPropertyMetadata(true));

        public bool ShowPercent 
        { 
            get => (bool)GetValue(ShowPercentProperty); 
            set => SetValue(ShowPercentProperty, value); 
        }
        #endregion

        #region Text
        public static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ZapProgress),
            new FrameworkPropertyMetadata("Chargement en cours, patienter...",
                FrameworkPropertyMetadataOptions.AffectsRender));

        public string Text 
        { 
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value); 
        }
        #endregion
        #endregion

        #region ProgressBar
        #region ProgressBarInnerBackground
        public static readonly DependencyProperty ProgressBarInnerBackgroundProperty = DependencyProperty.Register(
            "ProgressBarInnerBackground", typeof(Brush), typeof(ZapProgress),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnProgressBarInnerBackgroundChanged)));

        public Brush ProgressBarInnerBackground 
        { 
            get => (Brush)GetValue(ProgressBarInnerBackgroundProperty); 
            set => SetValue(ProgressBarInnerBackgroundProperty, value); 
        }

        private static void OnProgressBarInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ProgressBarInnerBackgroundProperty, e.NewValue);
        #endregion

        #region ProgressBarBackground
        public static readonly DependencyProperty ProgressBarBackgroundProperty = DependencyProperty.Register(
            "ProgressBarBackground", typeof(Brush), typeof(ZapProgress),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnProgressBarBackgroundChanged)));

        public Brush ProgressBarBackground 
        { 
            get => (Brush)GetValue(ProgressBarBackgroundProperty); 
            set => SetValue(ProgressBarBackgroundProperty, value); 
        }

        private static void OnProgressBarBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValueCommon(ProgressBarBackgroundProperty, e.NewValue);

            if (d is ZapProgress p)
            {
                p.SetProgressBarGlowElementBrush();
            }
        }
        #endregion

        #region ProgressBarBorderBrush
        public static readonly DependencyProperty ProgressBarBorderBrushProperty = DependencyProperty.Register(
            "ProgressBarBorderBrush", typeof(Brush), typeof(ZapProgress),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnProgressBarBorderBrushChanged)));

        public Brush ProgressBarBorderBrush 
        { 
            get => (Brush)GetValue(ProgressBarBorderBrushProperty); 
            set => SetValue(ProgressBarBorderBrushProperty, value); 
        }

        private static void OnProgressBarBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ProgressBarBorderBrushProperty, e.NewValue);
        #endregion

        #region ProgressBarBorderThickness
        public static readonly DependencyProperty ProgressBarBorderThicknessProperty = DependencyProperty.Register(
            "ProgressBarBorderThickness", typeof(Thickness), typeof(ZapProgress),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnProgressBarBorderThicknessChanged)));

        public Thickness ProgressBarBorderThickness 
        {
            get => (Thickness)GetValue(ProgressBarBorderThicknessProperty); 
            set => SetValue(ProgressBarBorderThicknessProperty, value); 
        }

        private static void OnProgressBarBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ProgressBarBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Indicator
        #region IndicatorTemplate
        public static readonly DependencyProperty IndicatorTemplateProperty = DependencyProperty.Register(
            "IndicatorTemplate", typeof(ZapLoadingIndicatorTemplates), typeof(ZapProgress),
            new FrameworkPropertyMetadata(ZapLoadingIndicatorTemplates.ThreeDots,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnIndicatorTemplateChanged)));

        public ZapLoadingIndicatorTemplates IndicatorTemplate 
        { 
            get => (ZapLoadingIndicatorTemplates)GetValue(IndicatorTemplateProperty); 
            set => SetValue(IndicatorTemplateProperty, value); 
        }

        private static void OnIndicatorTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(IndicatorTemplateProperty, e.NewValue);
        #endregion

        #region IndicatorAccentColor
        public static readonly DependencyProperty IndicatorAccentColorProperty = DependencyProperty.Register(
            "IndicatorAccentColor", typeof(Brush), typeof(ZapProgress),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnIndicatorAccentColorChanged)));

        public Brush IndicatorAccentColor 
        { 
            get => (Brush)GetValue(IndicatorAccentColorProperty); 
            set => SetValue(IndicatorAccentColorProperty, value); 
        }

        private static void OnIndicatorAccentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(IndicatorAccentColorProperty, e.NewValue);
        #endregion

        #region IndicatorHeight
        public static readonly DependencyProperty IndicatorHeightProperty = DependencyProperty.Register(
            "IndicatorHeight", typeof(double), typeof(ZapProgress),
            new FrameworkPropertyMetadata(40d,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                new PropertyChangedCallback(OnIndicatorHeightChanged)));

        public double IndicatorHeight 
        { 
            get => (double)GetValue(IndicatorHeightProperty); 
            set => SetValue(IndicatorHeightProperty, value); 
        }

        private static void OnIndicatorHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(IndicatorHeightProperty, e.NewValue);
        #endregion

        #region IndicatorSpeedRatio
        /// <summary>
        /// Identifies the <see cref="IndicatorSpeedRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndicatorSpeedRatioProperty = DependencyProperty.Register(
            "IndicatorSpeedRatio", typeof(double), typeof(ZapProgress), new PropertyMetadata(1d, new PropertyChangedCallback(OnSpeedRatioChanged)));

        /// <summary>
        /// Get/set the speed ratio of the animation.
        /// </summary>
        public double IndicatorSpeedRatio
        { 
            get => (double)GetValue(IndicatorSpeedRatioProperty); 
            set => SetValue(IndicatorSpeedRatioProperty, value); 
        }

        private static void OnSpeedRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(IndicatorSpeedRatioProperty, e.NewValue);
        #endregion

        #region IndicatorWidth
        public static readonly DependencyProperty IndicatorWidthProperty = DependencyProperty.Register(
            "IndicatorWidth", typeof(double), typeof(ZapProgress),
            new FrameworkPropertyMetadata(40d,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                new PropertyChangedCallback(OnIndicatorWidthChanged)));

        public double IndicatorWidth 
        { 
            get => (double)GetValue(IndicatorWidthProperty); 
            set => SetValue(IndicatorWidthProperty, value); 
        }

        private static void OnIndicatorWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(IndicatorWidthProperty, e.NewValue);
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
            "ZapTemplate", typeof(string), typeof(ZapProgress),
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
            "Theme", typeof(string), typeof(ZapProgress),
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

        #region Foreground
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(ForegroundProperty, e.NewValue);
        #endregion

        #region Padding
        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(PaddingProperty, e.NewValue);
        #endregion
        #endregion

        #region Events
        #region TemplateChanged
        public static readonly RoutedEvent TemplateChangedEvent = EventManager.RegisterRoutedEvent(
            "TemplateChanged", RoutingStrategy.Bubble, typeof(ITemplate<string>.TemplateChangedEventHandler), typeof(ZapProgress));

        public event ITemplate<string>.TemplateChangedEventHandler TemplateChanged
        {
            add => AddHandler(TemplateChangedEvent, value);
            remove => RemoveHandler(TemplateChangedEvent, value);
        }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ZapProgress));

        public event ITheme.ThemeChangedEventHandler ThemeChanged 
        { 
            add => AddHandler(ThemeChangedEvent, value); 
            remove => RemoveHandler(ThemeChangedEvent, value); 
        }

        private void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();
            // Control
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapProgressResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapProgressResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ZapProgressResourceKeys.BorderThicknessKey);
            this.SetThemePropertyDefault(CornerRadiusProperty, ResourceKeys.ZapProgressResourceKeys.CornerRadiusKey);
            this.SetThemePropertyDefault(ForegroundProperty, ResourceKeys.ZapProgressResourceKeys.ForegroundKey);
            this.SetThemePropertyDefault(PaddingProperty, ResourceKeys.ZapProgressResourceKeys.PaddingKey);
            // ProgressBar
            this.SetThemePropertyDefault(ProgressBarInnerBackgroundProperty, ResourceKeys.ZapProgressResourceKeys.ProgressBarInnerBackgroundKey);
            this.SetThemePropertyDefault(ProgressBarBackgroundProperty, ResourceKeys.ZapProgressResourceKeys.ProgressBarBackgroundKey);
            this.SetThemePropertyDefault(ProgressBarBorderBrushProperty, ResourceKeys.ZapProgressResourceKeys.ProgressBarBorderBrushKey);
            this.SetThemePropertyDefault(ProgressBarBorderThicknessProperty, ResourceKeys.ZapProgressResourceKeys.ProgressBarBorderThicknessKey);
            // Indicator
            this.SetThemePropertyDefault(IndicatorTemplateProperty, ResourceKeys.ZapProgressResourceKeys.IndicatorTemplateKey);
            this.SetThemePropertyDefault(IndicatorAccentColorProperty, ResourceKeys.ZapProgressResourceKeys.IndicatorAccentColorKey);
            this.SetThemePropertyDefault(IndicatorHeightProperty, ResourceKeys.ZapProgressResourceKeys.IndicatorHeightKey);
            this.SetThemePropertyDefault(IndicatorSpeedRatioProperty, ResourceKeys.ZapProgressResourceKeys.IndicatorSpeedRatioKey);
            this.SetThemePropertyDefault(IndicatorWidthProperty, ResourceKeys.ZapProgressResourceKeys.IndicatorWidthKey);
        }
        #endregion
        #endregion

        #region Internal Events Handlers
        private void OnBorderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetProgressBarIndicatorLength();
        }

        private void OnTrackIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateAnimation();
        }
        #endregion

        #region Constructor
        static ZapProgress()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(typeof(ZapProgress)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;
            // Control
            BackgroundProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
            FocusableProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(false));
            ForegroundProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(null, options, OnForegroundChanged) { Inherits = false });
            MaximumProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(100d));
            PaddingProperty.OverrideMetadata(typeof(ZapProgress), new FrameworkPropertyMetadata(new Thickness(0), options, OnPaddingChanged));
        }

        public ZapProgress()
        {
            // Load Templates
            this.RegisterAttachedTemplates<string>(GetType());
            this.LoadDefaultTemplate<string>(ZapTemplateProperty);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterInternalThemes<ZapProgressThemes>();
            this.LoadDefaultTheme(ThemeProperty);
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;

            if (_border != null)
            {
                _border.SizeChanged -= OnBorderSizeChanged;
            }

            if (_track != null)
            {
                _track.IsVisibleChanged -= OnTrackIsVisibleChanged;
            }

            _border = GetTemplateChild(BorderTemplateName) as Border;
            _track = GetTemplateChild(TrackTemplateName) as Border;
            _progress = GetTemplateChild(ProgressTemplateName) as Border;
            _innerValue = GetTemplateChild(InnerValueTemplateName) as TextBlock;
            _glow = GetTemplateChild(GlowingRectTemplateName) as Rectangle;

            if (_border != null)
            {
                _border.SizeChanged += OnBorderSizeChanged;
            }

            if (_track != null)
            {
                _track.IsVisibleChanged += OnTrackIsVisibleChanged;
            }

            SetProgressBarGlowElementBrush();
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ZapProgressAutomationPeer(this);
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            SetProgressBarIndicatorLength();
            RaisePropertyChanged(PercentPropName);
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            SetProgressBarIndicatorLength();
            RaisePropertyChanged(PercentPropName);
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            SetProgressBarIndicatorLength();
            RaisePropertyChanged(PercentPropName);
        }
        #endregion

        #region Control Methods
        public void SetProgress(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    IsIndeterminate = true;
                    Text = text;
                }));
            }
        }

        public void SetProgress(double value)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                IsIndeterminate = false;
                Value = value;
            }));
        }

        public void SetProgress(double value, string text)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                IsIndeterminate = false;
                Value = value;

                if (!string.IsNullOrEmpty(text))
                {
                    Text = text;
                }
            }));
        }

        private void SetProgressBarIndicatorLength()
        {
            if (_track != null && _progress != null)
            {
                double min = Minimum;
                double max = Maximum;
                double val = Value;

                double verticalBordersWidth = _track.BorderThickness.Left + _track.BorderThickness.Right;

                // When indeterminate or maximum == minimum, have the indicator stretch the whole length of track
                double percent = IsIndeterminate || max <= min ? 1.0 : (val - min) / (max - min);
                double width = percent * (_track.ActualWidth - verticalBordersWidth);
                _progress.Width = width < 0 ? 0 : width;

                if (_innerValue != null)
                {
                    _innerValue.Margin = new Thickness(-verticalBordersWidth, 0, 0, 0);
                }

                UpdateAnimation();
            }
        }

        private void UpdateAnimation()
        {
            if (_glow != null && _track != null)
            {
                if (_track.IsVisible && _glow.Width > 0 && _progress.Width > 0)
                {
                    //Set up the animation
                    double endPos = _progress.Width + _glow.Width;
                    double startPos = -1 * _glow.Width;

                    TimeSpan translateTime = TimeSpan.FromSeconds(((int)(endPos - startPos) / 200.0)); // travel at 200px /second
                    TimeSpan pauseTime = TimeSpan.FromSeconds(1.0);  // pause 1 second between animations
                    TimeSpan startTime;

                    //Is the animation currenly running (with one pixel fudge factor)
                    if (_glow.Margin.Left > startPos && _glow.Margin.Left < endPos - 1)
                    {
                        // make it appear that the timer already started.
                        // To do this find out how many pixels the glow has moved and divide by the speed to get time.
                        startTime = TimeSpan.FromSeconds(-1 * (_glow.Margin.Left - startPos) / 200.0);
                    }
                    else
                    {
                        startTime = TimeSpan.Zero;
                    }

                    ThicknessAnimationUsingKeyFrames animation = new ThicknessAnimationUsingKeyFrames
                    {
                        BeginTime = startTime,
                        Duration = new Duration(translateTime + pauseTime),
                        RepeatBehavior = RepeatBehavior.Forever
                    };

                    //Start with the glow hidden on the left.
                    animation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(startPos, 0, 0, 0), TimeSpan.FromSeconds(0)));
                    //Move to the glow hidden on the right.
                    animation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(endPos, 0, 0, 0), translateTime));
                    //There is a pause after the glow is off screen

                    _glow.BeginAnimation(MarginProperty, animation);
                }
                else
                {
                    _glow.BeginAnimation(MarginProperty, null);
                }
            }
        }

        // This is used to set the correct brush/opacity mask on the indicator.
        private void SetProgressBarGlowElementBrush()
        {
            if (_glow == null)
                return;

            _glow.InvalidateProperty(Shape.FillProperty);

            LinearGradientBrush b = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };

            if (ProgressBarBackground is SolidColorBrush brush)
            {
                brush = brush.ChangeBrightness(75);
                Color color1 = Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
                Color color2 = Color.FromArgb(0, brush.Color.R, brush.Color.G, brush.Color.B);

                //Create the gradient
                b.GradientStops.Add(new GradientStop(color2, 0.0));
                b.GradientStops.Add(new GradientStop(color1, 0.4));
                b.GradientStops.Add(new GradientStop(color1, 0.6));
                b.GradientStops.Add(new GradientStop(color2, 1.0));
            }
            else
            {
                b.GradientStops.Add(new GradientStop(Colors.Transparent, 0.0));
                b.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#600000FF"), 0.4));
                b.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#600000FF"), 0.6));
                b.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
            }
            _glow.SetCurrentValue(Shape.FillProperty, b);
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

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ZapanControls.Controls
{
    public sealed class ZapScrollBar : ScrollBar
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _thumbInnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#505050"));
        private static readonly SolidColorBrush _disabledButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE8EDF9"));

        private static readonly GradientStopCollection _thumbBackground_GradientStopCollection =
            new GradientStopCollection()
            {
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#363636"), Offset = 0 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#505050"), Offset = 0.5 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#363636"), Offset = 1 },
            };

        private static readonly LinearGradientBrush _thumbBackground =
            new LinearGradientBrush(_thumbBackground_GradientStopCollection)
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };

        #endregion

        #region Dependancy properties

        #region Thumb properties

        public static readonly DependencyProperty ThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ThumbInnerBackground", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(_thumbInnerBackground));

        public Brush ThumbInnerBackground
        {
            get { return (Brush)GetValue(ThumbInnerBackgroundProperty); }
            set { SetValue(ThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ThumbBackgroundProperty = DependencyProperty.Register(
            "ThumbBackground", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ThumbBorderBrushProperty = DependencyProperty.Register(
            "ThumbBorderBrush", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(null));

        public Brush ThumbBorderBrush
        {
            get { return (Brush)GetValue(ThumbBorderBrushProperty); }
            set { SetValue(ThumbBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ThumbBorderThicknessProperty = DependencyProperty.Register(
            "ThumbBorderThickness", typeof(Thickness), typeof(ZapScrollBar), new FrameworkPropertyMetadata(null));

        public Thickness ThumbBorderThickness
        {
            get { return (Thickness)GetValue(ThumbBorderThicknessProperty); }
            set { SetValue(ThumbBorderThicknessProperty, value); }
        }

        #endregion

        #region Buttons properties

        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register(
            "ButtonBackground", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderBrushProperty = DependencyProperty.Register(
            "ButtonBorderBrush", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(null));

        public Brush ButtonBorderBrush
        {
            get { return (Brush)GetValue(ButtonBorderBrushProperty); }
            set { SetValue(ButtonBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty ButtonBorderThicknessProperty = DependencyProperty.Register(
            "ButtonBorderThickness", typeof(double), typeof(ZapScrollBar), new FrameworkPropertyMetadata(0.0));

        public double ButtonBorderThickness
        {
            get { return (double)GetValue(ButtonBorderThicknessProperty); }
            set { SetValue(ButtonBorderThicknessProperty, value); }
        }

        #endregion

        #region Disabled properties

        private static readonly DependencyProperty DisabledThumbInnerBackgroundProperty = DependencyProperty.Register(
            "DisabledThumbInnerBackground", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(_disabledButtonBackground));

        public Brush DisabledThumbInnerBackground
        {
            get { return (Brush)GetValue(DisabledThumbInnerBackgroundProperty); }
            set { SetValue(DisabledThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty DisabledButtonBackgroundProperty = DependencyProperty.Register(
            "DisabledButtonBackground", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(_disabledButtonBackground));

        public Brush DisabledButtonBackground
        {
            get { return (Brush)GetValue(DisabledButtonBackgroundProperty); }
            set { SetValue(DisabledButtonBackgroundProperty, value); }
        }

        private static readonly DependencyProperty DisabledButtonBorderBrushProperty = DependencyProperty.Register(
            "DisabledButtonBorderBrush", typeof(Brush), typeof(ZapScrollBar), new FrameworkPropertyMetadata(null));

        public Brush DisabledButtonBorderBrush
        {
            get { return (Brush)GetValue(DisabledButtonBorderBrushProperty); }
            set { SetValue(DisabledButtonBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty DisabledButtonBorderThicknessProperty = DependencyProperty.Register(
            "DisabledButtonBorderThickness", typeof(double), typeof(ZapScrollBar), new FrameworkPropertyMetadata(0.0));

        public double DisabledButtonBorderThickness
        {
            get { return (double)GetValue(DisabledButtonBorderThicknessProperty); }
            set { SetValue(DisabledButtonBorderThicknessProperty, value); }
        }

        #endregion

        #endregion

        #region Constructors

        static ZapScrollBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(typeof(ZapScrollBar)));

            BackgroundProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(_thumbBackground));
            WidthProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(15.0));
            HeightProperty.OverrideMetadata(typeof(ZapScrollBar), new FrameworkPropertyMetadata(double.NaN));

            OrientationProperty.OverrideMetadata(typeof(ZapScrollBar), 
                new FrameworkPropertyMetadata(System.Windows.Controls.Orientation.Vertical, 
                    new PropertyChangedCallback(OnOrientationChanged)
                )
            );
        }

        #endregion

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZapScrollBar sb = (ZapScrollBar)d;
            if (e.NewValue is System.Windows.Controls.Orientation orientation)
            {
                if (orientation == System.Windows.Controls.Orientation.Horizontal)
                {
                    if (double.IsNaN(sb.Height))
                        sb.Height = 15.0;
                }
                else
                {
                    if (double.IsNaN(sb.Width))
                        sb.Width = 15.0;
                }
            }
        }
    }
}

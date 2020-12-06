using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZapanControls.Controls
{
    public sealed class ZapScrollViewer : ScrollViewer
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _scrollBarThumbInnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#505050"));
        private static readonly SolidColorBrush _scrollBarDisabledButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE8EDF9"));

        private static readonly GradientStopCollection _scrollBarBackground_GradientStopCollection =
            new GradientStopCollection()
            {
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#363636"), Offset = 0 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#505050"), Offset = 0.5 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#363636"), Offset = 1 },
            };

        private static readonly LinearGradientBrush _scrollBarBackground =
            new LinearGradientBrush(_scrollBarBackground_GradientStopCollection)
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };

        #endregion

        #region Dependency Properties

        #region ScrollBar

        private static readonly DependencyProperty ScrollBarBackgroundProperty = DependencyProperty.Register(
            "ScrollBarBackground", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(_scrollBarBackground));

        public Brush ScrollBarBackground
        {
            get { return (Brush)GetValue(ScrollBarBackgroundProperty); }
            set { SetValue(ScrollBarBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarWidthProperty = DependencyProperty.Register(
            "ScrollBarWidth", typeof(double), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(15.0));

        public double ScrollBarWidth
        {
            get { return (double)GetValue(ScrollBarWidthProperty); }
            set { SetValue(ScrollBarWidthProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarHeightProperty = DependencyProperty.Register(
            "ScrollBarHeight", typeof(double), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(15.0));

        public double ScrollBarHeight
        {
            get { return (double)GetValue(ScrollBarHeightProperty); }
            set { SetValue(ScrollBarHeightProperty, value); }
        }

        #region ScrollBar Thumb properties

        private static readonly DependencyProperty ScrollBarThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarThumbInnerBackground", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(_scrollBarThumbInnerBackground));

        public Brush ScrollBarThumbInnerBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbInnerBackgroundProperty); }
            set { SetValue(ScrollBarThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBackgroundProperty = DependencyProperty.Register(
            "ScrollBarThumbBackground", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ScrollBarThumbBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbBackgroundProperty); }
            set { SetValue(ScrollBarThumbBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarThumbBorderBrush", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null));

        public Brush ScrollBarThumbBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarThumbBorderBrushProperty); }
            set { SetValue(ScrollBarThumbBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarThumbBorderThickness", typeof(Thickness), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null));

        public Thickness ScrollBarThumbBorderThickness
        {
            get { return (Thickness)GetValue(ScrollBarThumbBorderThicknessProperty); }
            set { SetValue(ScrollBarThumbBorderThicknessProperty, value); }
        }

        #endregion

        #region ScrollBar Buttons properties

        private static readonly DependencyProperty ScrollBarButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarButtonBackground", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ScrollBarButtonBackgroundProperty); }
            set { SetValue(ScrollBarButtonBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarButtonBorderBrush", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null));

        public Brush ButtonBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarButtonBorderBrushProperty); }
            set { SetValue(ScrollBarButtonBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarButtonBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarButtonBorderThickness", typeof(double), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null));

        public double ButtonBorderThickness
        {
            get { return (double)GetValue(ScrollBarButtonBorderThicknessProperty); }
            set { SetValue(ScrollBarButtonBorderThicknessProperty, value); }
        }

        #endregion

        #region ScrollBar Disabled properties

        private static readonly DependencyProperty ScrollBarDisabledThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarDisabledThumbInnerBackground", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(_scrollBarDisabledButtonBackground));

        public Brush ScrollBarDisabledThumbInnerBackground
        {
            get { return (Brush)GetValue(ScrollBarDisabledThumbInnerBackgroundProperty); }
            set { SetValue(ScrollBarDisabledThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBackground", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(_scrollBarDisabledButtonBackground));

        public Brush ScrollBarDisabledButtonBackground
        {
            get { return (Brush)GetValue(ScrollBarDisabledButtonBackgroundProperty); }
            set { SetValue(ScrollBarDisabledButtonBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBorderBrush", typeof(Brush), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null));

        public Brush ScrollBarDisabledButtonBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarDisabledButtonBorderBrushProperty); }
            set { SetValue(ScrollBarDisabledButtonBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBorderThickness", typeof(double), typeof(ZapScrollViewer), new FrameworkPropertyMetadata(null));

        public double ScrollBarDisabledButtonBorderThickness
        {
            get { return (double)GetValue(ScrollBarDisabledButtonBorderThicknessProperty); }
            set { SetValue(ScrollBarDisabledButtonBorderThicknessProperty, value); }
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        static ZapScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapScrollViewer), new FrameworkPropertyMetadata(typeof(ZapScrollViewer)));
        }

        public ZapScrollViewer()
        {

        }

        #endregion

    }
}

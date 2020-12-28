using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ZapanControls.Controls.Primitives
{
    public class ZapButtonBaseOld : Button
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _disabledBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC808080"));

        #endregion

        #region Dependancy properties

        #region MouseOver properties

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.MouseOverBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
            "MouseOverBackground", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata());

        /// <summary>
        /// Get/set the background color when mouse over.
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.MouseOverBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register(
            "MouseOverBorderBrush", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when mouse over.
        /// </summary>
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.MouseOverForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register(
            "MouseOverForeground", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the foreground color when mouse over.
        /// </summary>
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        #endregion

        #region IsPressed properties

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.IsPressedBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedBackgroundProperty = DependencyProperty.Register(
            "IsPressedBackground", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the background color when button is pressed.
        /// </summary>
        public Brush IsPressedBackground
        {
            get { return (Brush)GetValue(IsPressedBackgroundProperty); }
            set { SetValue(IsPressedBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.IsPressedBackgroundOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedBackgroundOpacityProperty = DependencyProperty.Register(
            "IsPressedBackgroundOpacity", typeof(double), typeof(ZapButtonBaseOld), new PropertyMetadata(0.5));

        /// <summary>
        /// Get/set opacity of background when button is pressed.
        /// </summary>
        public double IsPressedBackgroundOpacity
        {
            get { return (double)GetValue(IsPressedBackgroundOpacityProperty); }
            set { SetValue(IsPressedBackgroundOpacityProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.IsPressedBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedBorderBrushProperty = DependencyProperty.Register(
            "IsPressedBorderBrush", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when button is pressed.
        /// </summary>
        public Brush IsPressedBorderBrush
        {
            get { return (Brush)GetValue(IsPressedBorderBrushProperty); }
            set { SetValue(IsPressedBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.IsPressedForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedForegroundProperty = DependencyProperty.Register(
            "IsPressedForeground", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the foreground color when button is pressed.
        /// </summary>
        public Brush IsPressedForeground
        {
            get { return (Brush)GetValue(IsPressedForegroundProperty); }
            set { SetValue(IsPressedForegroundProperty, value); }
        }

        #endregion

        #region Disabled properties

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.DisabledBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(_disabledBackground));

        /// <summary>
        /// Get/set the background color when button is disabled.
        /// </summary>
        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.DisabledBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(Brushes.Gray));

        /// <summary>
        /// Get/set the border color when button is disabled.
        /// </summary>
        public Brush DisabledBorderBrush
        {
            get { return (Brush)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBaseOld.DisabledForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground", typeof(Brush), typeof(ZapButtonBaseOld), new PropertyMetadata(Brushes.LightGray));

        /// <summary>
        /// Get/set the foreground color when button is disabled.
        /// </summary>
        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        #endregion

        #endregion

        #region Constructor

        public ZapButtonBaseOld()
        {
            this.Loaded += (s, e) => 
            {
                if (MouseOverBackground == null)
                    SetBinding(MouseOverBackgroundProperty, new Binding("Background") { Source = this });
                if (MouseOverBorderBrush == null)
                    SetBinding(MouseOverBorderBrushProperty, new Binding("BorderBrush") { Source = this });
                if (MouseOverForeground == null)
                    SetBinding(MouseOverForegroundProperty, new Binding("Foreground") { Source = this });

                if (IsPressedBackground == null)
                    SetBinding(IsPressedBackgroundProperty, new Binding("Background") { Source = this });
                if (IsPressedBorderBrush == null)
                    SetBinding(IsPressedBorderBrushProperty, new Binding("BorderBrush") { Source = this });
                if (IsPressedForeground == null)
                    SetBinding(IsPressedForegroundProperty, new Binding("Foreground") { Source = this });
            };
        }

        #endregion
    }
}

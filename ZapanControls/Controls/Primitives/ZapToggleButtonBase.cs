using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace ZapanControls.Controls.Primitives
{
    public class ZapToggleButtonBase : ToggleButton
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _disabledBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC808080"));

        #endregion

        #region Dependancy Properties

        #region MouseOver properties

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.MouseOverBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
            "MouseOverBackground", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the background color when mouse over.
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.MouseOverBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register(
            "MouseOverBorderBrush", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when mouse over.
        /// </summary>
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.MouseOverForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register(
            "MouseOverForeground", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

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
        /// Identifies the <see cref="ZapToggleButtonBase.IsPressedBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedBackgroundProperty = DependencyProperty.Register(
            "IsPressedBackground", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the background color when button is pressed.
        /// </summary>
        public Brush IsPressedBackground
        {
            get { return (Brush)GetValue(IsPressedBackgroundProperty); }
            set { SetValue(IsPressedBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.IsPressedBackgroundOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedBackgroundOpacityProperty = DependencyProperty.Register(
            "IsPressedBackgroundOpacity", typeof(double), typeof(ZapToggleButtonBase), new PropertyMetadata(0.5));

        /// <summary>
        /// Get/set opacity of background when button is pressed.
        /// </summary>
        public double IsPressedBackgroundOpacity
        {
            get { return (double)GetValue(IsPressedBackgroundOpacityProperty); }
            set { SetValue(IsPressedBackgroundOpacityProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.IsPressedBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedBorderBrushProperty = DependencyProperty.Register(
            "IsPressedBorderBrush", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when button is pressed.
        /// </summary>
        public Brush IsPressedBorderBrush
        {
            get { return (Brush)GetValue(IsPressedBorderBrushProperty); }
            set { SetValue(IsPressedBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.IsPressedForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedForegroundProperty = DependencyProperty.Register(
            "IsPressedForeground", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the foreground color when button is pressed.
        /// </summary>
        public Brush IsPressedForeground
        {
            get { return (Brush)GetValue(IsPressedForegroundProperty); }
            set { SetValue(IsPressedForegroundProperty, value); }
        }

        #endregion

        #region IsChecked properties

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.IsCheckedBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCheckedBackgroundProperty = DependencyProperty.Register(
            "IsCheckedBackground", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the background color when button is checked.
        /// </summary>
        public Brush IsCheckedBackground
        {
            get { return (Brush)GetValue(IsCheckedBackgroundProperty); }
            set { SetValue(IsCheckedBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.IsCheckedBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCheckedBorderBrushProperty = DependencyProperty.Register(
            "IsCheckedBorderBrush", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when button is checked.
        /// </summary>
        public Brush IsCheckedBorderBrush
        {
            get { return (Brush)GetValue(IsCheckedBorderBrushProperty); }
            set { SetValue(IsCheckedBorderBrushProperty, value); }
        }

        #endregion

        #region Disabled properties

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.DisabledBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(_disabledBackground));

        /// <summary>
        /// Get/set the background color when button is disabled.
        /// </summary>
        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.DisabledBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(Brushes.Gray));

        /// <summary>
        /// Get/set the border color when button is disabled.
        /// </summary>
        public Brush DisabledBorderBrush
        {
            get { return (Brush)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonBase.DisabledForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground", typeof(Brush), typeof(ZapToggleButtonBase), new PropertyMetadata(Brushes.LightGray));

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

        #region Constructors

        public ZapToggleButtonBase()
        {
            this.Loaded += (s, e) =>
            {
                if (MouseOverBackground == null)
                    SetBinding(MouseOverBackgroundProperty, new Binding("Background") { Source = this });
                if (MouseOverBorderBrush == null)
                    SetBinding(MouseOverBorderBrushProperty, new Binding("BorderBrush") { Source = this });
                if (MouseOverForeground == null)
                    SetBinding(MouseOverForegroundProperty, new Binding("Foreground") { Source = this });

                if (IsCheckedBackground == null)
                    SetBinding(IsCheckedBackgroundProperty, new Binding("Background") { Source = this });
                if (IsCheckedBorderBrush == null)
                    SetBinding(IsCheckedBorderBrushProperty, new Binding("BorderBrush") { Source = this });

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

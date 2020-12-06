using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZapanControls.Controls
{
    public sealed class ZapMenuItem : MenuItem
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _popupBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF959595"));

        #endregion

        #region Dependency Properties

        #region Popup Properties

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.PopupBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupBackgroundProperty = DependencyProperty.Register(
            "PopupBackground", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(_popupBackground));

        /// <summary>
        /// Get/set the popup background color.
        /// </summary>
        public Brush PopupBackground
        {
            get { return (Brush)GetValue(PopupBackgroundProperty); }
            set { SetValue(PopupBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.PopupBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupBorderBrushProperty = DependencyProperty.Register(
            "PopupBorderBrush", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(Brushes.DarkViolet));

        /// <summary>
        /// Get/set the popup border color.
        /// </summary>
        public Brush PopupBorderBrush
        {
            get { return (Brush)GetValue(PopupBorderBrushProperty); }
            set { SetValue(PopupBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.PopupBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PopupBorderThicknessProperty = DependencyProperty.Register(
            "PopupBorderThickness", typeof(Thickness), typeof(ZapMenuItem), new PropertyMetadata(new Thickness(1)));

        /// <summary>
        /// Get/set the popup border color.
        /// </summary>
        public Thickness PopupBorderThickness
        {
            get { return (Thickness)GetValue(PopupBorderBrushProperty); }
            set { SetValue(PopupBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.IsRoundPopup"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty IsRoundPopupProperty = DependencyProperty.Register(
            "IsRoundPopup", typeof(bool), typeof(ZapMenuItem), new PropertyMetadata(false));

        /// <summary>
        /// Get/set the popup border color.
        /// </summary>
        public bool IsRoundPopup
        {
            get { return (bool)GetValue(IsRoundPopupProperty); }
            set { SetValue(IsRoundPopupProperty, value); }
        }

        #endregion

        #region MouseOver properties

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.MouseOverBackground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
            "MouseOverBackground", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(Brushes.DarkViolet));

        /// <summary>
        /// Get/set the background color when mouse over.
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.MouseOverBorderBrush"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register(
            "MouseOverBorderBrush", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when mouse over.
        /// </summary>
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapButtonBase.MouseOverForeground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register(
            "MouseOverForeground", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(Brushes.White));

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
        /// Identifies the <see cref="ZapMenuItem.IsPressedBackground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty IsPressedBackgroundProperty = DependencyProperty.Register(
            "IsPressedBackground", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(Brushes.DarkViolet));

        /// <summary>
        /// Get/set the background color when button is pressed.
        /// </summary>
        public Brush IsPressedBackground
        {
            get { return (Brush)GetValue(IsPressedBackgroundProperty); }
            set { SetValue(IsPressedBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.IsPressedBackgroundOpacity"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty IsPressedBackgroundOpacityProperty = DependencyProperty.Register(
            "IsPressedBackgroundOpacity", typeof(double), typeof(ZapMenuItem), new PropertyMetadata(0.5));

        /// <summary>
        /// Get/set opacity of background when button is pressed.
        /// </summary>
        public double IsPressedBackgroundOpacity
        {
            get { return (double)GetValue(IsPressedBackgroundOpacityProperty); }
            set { SetValue(IsPressedBackgroundOpacityProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.IsPressedBorderBrush"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty IsPressedBorderBrushProperty = DependencyProperty.Register(
            "IsPressedBorderBrush", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when button is pressed.
        /// </summary>
        public Brush IsPressedBorderBrush
        {
            get { return (Brush)GetValue(IsPressedBorderBrushProperty); }
            set { SetValue(IsPressedBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.IsPressedForeground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty IsPressedForegroundProperty = DependencyProperty.Register(
            "IsPressedForeground", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(Brushes.White));

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
        /// Identifies the <see cref="ZapMenuItem.DisabledBackground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the background color when button is disabled.
        /// </summary>
        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.DisabledBorderBrush"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Get/set the border color when button is disabled.
        /// </summary>
        public Brush DisabledBorderBrush
        {
            get { return (Brush)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapMenuItem.DisabledForeground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground", typeof(Brush), typeof(ZapMenuItem), new PropertyMetadata(Brushes.DarkGray));

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

        static ZapMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapMenuItem), new FrameworkPropertyMetadata(typeof(ZapMenuItem)));
        }

        #endregion  
    }
}

using System.Windows;
using System.Windows.Media;
using ZapanControls.Controls.Primitives;

namespace ZapanControls.Controls
{
    public class ZapToggleButtonFlat : ZapToggleButtonBase
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _mouseOverBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BF808080"));
        private static readonly SolidColorBrush _isCheckedBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#239933FF"));

        #endregion

        #region Dependancy Properties

        /// <summary>
        /// Identifies the <see cref="ZapToggleButtonFlat.IsCheckedBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCheckedBorderThicknessProperty = DependencyProperty.Register(
            "IsCheckedBorderThickness", typeof(Thickness), typeof(ZapToggleButtonFlat), new PropertyMetadata(new Thickness(1)));

        /// <summary>
        /// Get/set the border thickness when button is checked.
        /// </summary>
        public Thickness IsCheckedBorderThickness
        {
            get { return (Thickness)GetValue(IsCheckedBorderThicknessProperty); }
            set { SetValue(IsCheckedBorderThicknessProperty, value); }
        }

        #endregion

        #region Constructors

        static ZapToggleButtonFlat()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapToggleButtonFlat), new FrameworkPropertyMetadata(typeof(ZapToggleButtonFlat)));

            MouseOverBackgroundProperty.OverrideMetadata(typeof(ZapToggleButtonFlat), new PropertyMetadata(_mouseOverBackground));
            IsCheckedBackgroundProperty.OverrideMetadata(typeof(ZapToggleButtonFlat), new PropertyMetadata(_isCheckedBackground));
            IsCheckedBorderBrushProperty.OverrideMetadata(typeof(ZapToggleButtonFlat), new PropertyMetadata(Brushes.MediumPurple));
        }

        #endregion
    }
}

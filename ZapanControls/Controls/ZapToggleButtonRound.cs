using System.Windows;
using System.Windows.Media;
using ZapanControls.Controls.Primitives;

namespace ZapanControls.Controls
{
    public class ZapToggleButtonRound : ZapToggleButtonBase
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _mouseOverBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BF808080"));
        private static readonly SolidColorBrush _isPressedBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BF808080"));

        #endregion

        #region Constructors

        static ZapToggleButtonRound()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapToggleButtonRound), new FrameworkPropertyMetadata(typeof(ZapToggleButtonRound)));

            BorderBrushProperty.OverrideMetadata(typeof(ZapToggleButtonRound), new FrameworkPropertyMetadata(Brushes.MediumPurple));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapToggleButtonRound), new FrameworkPropertyMetadata(new Thickness(1)));
            MouseOverBackgroundProperty.OverrideMetadata(typeof(ZapToggleButtonRound), new PropertyMetadata(_mouseOverBackground));
            IsPressedBackgroundProperty.OverrideMetadata(typeof(ZapToggleButtonRound), new PropertyMetadata(_isPressedBackground));
            HeightProperty.OverrideMetadata(typeof(ZapToggleButtonRound), new FrameworkPropertyMetadata(20.0));
            WidthProperty.OverrideMetadata(typeof(ZapToggleButtonRound), new FrameworkPropertyMetadata(20.0));
        }

        #endregion
    }
}

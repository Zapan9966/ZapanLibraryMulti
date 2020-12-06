using System.Windows;
using System.Windows.Media;
using ZapanControls.Controls.Primitives;

namespace ZapanControls.Controls
{
    public class ZapButtonRound : ZapButtonBase
    {
        #region Constructors

        static ZapButtonRound()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapButtonRound), new FrameworkPropertyMetadata(typeof(ZapButtonRound)));

            BorderBrushProperty.OverrideMetadata(typeof(ZapButtonRound), new FrameworkPropertyMetadata(Brushes.MediumPurple));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapButtonRound), new FrameworkPropertyMetadata(new Thickness(1)));
            HeightProperty.OverrideMetadata(typeof(ZapButtonRound), new FrameworkPropertyMetadata(20.0));
            WidthProperty.OverrideMetadata(typeof(ZapButtonRound), new FrameworkPropertyMetadata(20.0));
        }

        #endregion
    }
}

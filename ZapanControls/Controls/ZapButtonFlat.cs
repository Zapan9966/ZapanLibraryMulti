using System.Windows;
using System.Windows.Media;
using ZapanControls.Controls.Primitives;

namespace ZapanControls.Controls
{
    public class ZapButtonFlat : ZapButtonBaseOld
    {
        #region Default Static Properties

        private static readonly SolidColorBrush _mouseOverBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BF808080"));

        #endregion

        #region Constructors

        static ZapButtonFlat()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapButtonFlat), new FrameworkPropertyMetadata(typeof(ZapButtonFlat)));

            MouseOverBackgroundProperty.OverrideMetadata(typeof(ZapButtonFlat), new PropertyMetadata(_mouseOverBackground));
        }

        #endregion
    }
}

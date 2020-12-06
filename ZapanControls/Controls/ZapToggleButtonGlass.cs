using System.Windows;
using ZapanControls.Controls.Primitives;

namespace ZapanControls.Controls
{
    public class ZapToggleButtonGlass : ZapToggleButtonBase
    {

        #region Constructors

        static ZapToggleButtonGlass()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapToggleButtonGlass), new FrameworkPropertyMetadata(typeof(ZapToggleButtonGlass)));
        }

        #endregion

    }
}

using System.Windows;
using ZapanControls.Controls.Primitives;

namespace ZapanControls.Controls
{
    public class ZapButtonGlass : ZapButtonBase
    {

        #region Constructors

        static ZapButtonGlass()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapButtonGlass), new FrameworkPropertyMetadata(typeof(ZapButtonGlass)));
        }

        #endregion

    }
}

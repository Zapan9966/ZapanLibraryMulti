using System.Windows;
using ZapanControls.Controls.Primitives;

namespace ZapanControls.Controls
{
    public class ZapSplitButtonGlass : ZapSplitButtonBase
    {
        #region Constructors

        static ZapSplitButtonGlass()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapSplitButtonGlass), new FrameworkPropertyMetadata(typeof(ZapSplitButtonGlass)));
            IsContextMenuRoundProperty.OverrideMetadata(typeof(ZapSplitButtonGlass), new FrameworkPropertyMetadata(true));
        }

        #endregion
    }
}

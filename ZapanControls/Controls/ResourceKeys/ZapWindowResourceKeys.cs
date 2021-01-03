using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapWindowResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _ZapTemplateKey = null;

        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;

        private static readonly ComponentResourceKey _TitleBarBackgroundKey = null;
        private static readonly ComponentResourceKey _TitleBarForegroundKey = null;
        private static readonly ComponentResourceKey _TitleBarHeightKey = null;
        #endregion

        #region Resource Keys
        public static ComponentResourceKey ZapTemplateKey => _ZapTemplateKey.GetRegisteredKey(typeof(ZapWindowResourceKeys), "ZapTemplate");

        #region Control
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapWindowResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapWindowResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapWindowResourceKeys), "BorderThickness");
        #endregion

        #region TitleBar
        public static ComponentResourceKey TitleBarBackgroundKey => _TitleBarBackgroundKey.GetRegisteredKey(typeof(ZapWindowResourceKeys), "TitleBarBackground");
        public static ComponentResourceKey TitleBarForegroundKey => _TitleBarForegroundKey.GetRegisteredKey(typeof(ZapWindowResourceKeys), "TitleBarForeground");
        public static ComponentResourceKey TitleBarHeightKey => _TitleBarHeightKey.GetRegisteredKey(typeof(ZapWindowResourceKeys), "TitleBarHeight");
        #endregion
        #endregion
    }
}

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
        public static ComponentResourceKey ZapTemplateKey => _ZapTemplateKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "ZapTemplate");

        #region Control
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderThickness");
        #endregion

        #region TitleBar
        public static ComponentResourceKey TitleBarBackgroundKey => _TitleBarBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "TitleBarBackground");
        public static ComponentResourceKey TitleBarForegroundKey => _TitleBarForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "TitleBarForeground");
        public static ComponentResourceKey TitleBarHeightKey => _TitleBarHeightKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "TitleBarHeight");
        #endregion
        #endregion
    }
}

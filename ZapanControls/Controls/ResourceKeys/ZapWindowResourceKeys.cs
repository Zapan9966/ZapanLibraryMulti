using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapWindowResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _WindowTemplateKey = null;

        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;

        private static readonly ComponentResourceKey _TitleBarBackgroundKey = null;
        private static readonly ComponentResourceKey _TitleBarForegroundKey = null;
        private static readonly ComponentResourceKey _TitleBarHeightKey = null;
        #endregion

        #region Resource Keys
        #region Control
        public static ComponentResourceKey WindowTemplateKey
        {
            get { return _WindowTemplateKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "WindowTemplate"); }
        }

        public static ComponentResourceKey BackgroundKey
        {
            get { return _BackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Background"); }
        }

        public static ComponentResourceKey BorderBrushKey
        {
            get { return _BorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderBrush"); }
        }

        public static ComponentResourceKey BorderThicknessKey
        {
            get { return _BorderThicknessKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderThickness"); }
        }
        #endregion

        #region TitleBar
        public static ComponentResourceKey TitleBarBackgroundKey
        {
            get { return _TitleBarBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "TitleBarBackground"); }
        }

        public static ComponentResourceKey TitleBarForegroundKey
        {
            get { return _TitleBarForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "TitleBarForeground"); }
        }

        public static ComponentResourceKey TitleBarHeightKey
        {
            get { return _TitleBarHeightKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "TitleBarHeight"); }
        }
        #endregion
        #endregion
    }
}

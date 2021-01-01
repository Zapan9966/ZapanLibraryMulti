using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapContextMenuResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;
        private static readonly ComponentResourceKey _ForegroundKey = null;
        #endregion

        #region Resource Keys
        public static ComponentResourceKey BackgroundKey=> _BackgroundKey.GetRegisteredKey(typeof(ZapContextMenuResourceKeys), "Background");      
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapContextMenuResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapContextMenuResourceKeys), "BorderThickness");
        public static ComponentResourceKey ForegroundKey => _ForegroundKey.GetRegisteredKey(typeof(ZapContextMenuResourceKeys), "Foreground");
        #endregion
    }
}

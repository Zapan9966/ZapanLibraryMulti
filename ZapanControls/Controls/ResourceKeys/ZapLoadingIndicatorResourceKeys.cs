using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapLoadingIndicatorResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _ZapTemplateKey = null;

        private static readonly ComponentResourceKey _AccentColorKey = null;
        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;
        private static readonly ComponentResourceKey _SpeedRatioKey = null;
        #endregion

        #region Resource Keys
        public static ComponentResourceKey ZapTemplateKey => _ZapTemplateKey.GetRegisteredKey(typeof(ZapLoadingIndicatorResourceKeys), "ZapTemplate");

        public static ComponentResourceKey AccentColorKey => _AccentColorKey.GetRegisteredKey(typeof(ZapLoadingIndicatorResourceKeys), "AccentColor");
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapLoadingIndicatorResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapLoadingIndicatorResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapLoadingIndicatorResourceKeys), "BorderThickness");
        public static ComponentResourceKey SpeedRatioKey => _SpeedRatioKey.GetRegisteredKey(typeof(ZapLoadingIndicatorResourceKeys), "SpeedRatio");
        #endregion
    }
}

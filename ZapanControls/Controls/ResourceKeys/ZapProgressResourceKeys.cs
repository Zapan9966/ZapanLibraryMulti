using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapProgressResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _ZapTemplateKey = null;
        // Control
        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;
        private static readonly ComponentResourceKey _CornerRadiusKey = null;
        private static readonly ComponentResourceKey _ForegroundKey = null;
        private static readonly ComponentResourceKey _PaddingKey = null;
        // ProgressBar
        private static readonly ComponentResourceKey _ProgressBarInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _ProgressBarBackgroundKey = null;
        private static readonly ComponentResourceKey _ProgressBarBorderBrushKey = null;
        private static readonly ComponentResourceKey _ProgressBarBorderThicknessKey = null;
        // Indicator
        private static readonly ComponentResourceKey _IndicatorTemplateKey = null;
        private static readonly ComponentResourceKey _IndicatorAccentColorKey = null;
        private static readonly ComponentResourceKey _IndicatorHeightKey = null;
        private static readonly ComponentResourceKey _IndicatorSpeedRatioKey = null;
        private static readonly ComponentResourceKey _IndicatorWidthKey = null;
        #endregion

        #region Resource Keys
        #region Control
        public static ComponentResourceKey ZapTemplateKey => _ZapTemplateKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "ZapTemplate");
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "BorderThickness");
        public static ComponentResourceKey CornerRadiusKey => _CornerRadiusKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "CornerRadius");
        public static ComponentResourceKey ForegroundKey => _ForegroundKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "Foreground");
        public static ComponentResourceKey PaddingKey => _PaddingKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "Padding");
        #endregion

        #region ProgressBar
        public static ComponentResourceKey ProgressBarInnerBackgroundKey => _ProgressBarInnerBackgroundKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "ProgressBarInnerBackground");
        public static ComponentResourceKey ProgressBarBackgroundKey => _ProgressBarBackgroundKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "ProgressBarBackground");
        public static ComponentResourceKey ProgressBarBorderBrushKey => _ProgressBarBorderBrushKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "ProgressBarBorderBrush");
        public static ComponentResourceKey ProgressBarBorderThicknessKey => _ProgressBarBorderThicknessKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "ProgressBarBorderThickness");
        #endregion

        #region Indicator
        public static ComponentResourceKey IndicatorTemplateKey => _IndicatorTemplateKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "IndicatorTemplate");
        public static ComponentResourceKey IndicatorAccentColorKey => _IndicatorAccentColorKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "IndicatorAccentColor");
        public static ComponentResourceKey IndicatorHeightKey => _IndicatorHeightKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "IndicatorHeight");
        public static ComponentResourceKey IndicatorSpeedRatioKey => _IndicatorSpeedRatioKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "IndicatorSpeedRatio");
        public static ComponentResourceKey IndicatorWidthKey => _IndicatorWidthKey.GetRegisteredKey(typeof(ZapProgressResourceKeys), "IndicatorWidth");
        #endregion
        #endregion
    }
}

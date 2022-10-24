using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ListViewResourceKeys
    {
        #region Fields
        // Header Templates
        private static readonly ComponentResourceKey _HeaderTemplateDefaultKey = null;
        private static readonly ComponentResourceKey _HeaderTemplateDateKey = null;
        private static readonly ComponentResourceKey _HeaderTemplateComboBoxKey = null;
        private static readonly ComponentResourceKey _HeaderTemplateTextKey = null;

        // Styles
        private static readonly ComponentResourceKey _GridViewStyleKey = null;
        private static readonly ComponentResourceKey _ScrollViewerStyleKey = null;
        private static readonly ComponentResourceKey _ItemContainerStyleKey = null;
        private static readonly ComponentResourceKey _ColumnHeaderGripperStyleKey = null;
        private static readonly ComponentResourceKey _ColumnHeaderStyleKey = null;

        // Control
        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;

        private static readonly ComponentResourceKey _DisabledBackgroundKey = null;
        private static readonly ComponentResourceKey _DisabledBorderBrushKey = null;

        // Header
        private static readonly ComponentResourceKey _HeaderBackgroundKey = null;
        private static readonly ComponentResourceKey _HeaderBorderBrushKey = null;
        private static readonly ComponentResourceKey _HeaderForegroundKey = null;

        private static readonly ComponentResourceKey _HeaderFocusedBackgroundKey = null;
        private static readonly ComponentResourceKey _HeaderFocusedForegroundKey = null;

        private static readonly ComponentResourceKey _HeaderPressedBackgroundKey = null;
        private static readonly ComponentResourceKey _HeaderPressedForegroundKey = null;

        // Items
        private static readonly ComponentResourceKey _ItemsBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemsBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemsBorderThicknessKey = null;
        private static readonly ComponentResourceKey _ItemsForegroundKey = null;

        private static readonly ComponentResourceKey _ItemsFocusedBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemsFocusedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemsFocusedForegroundKey = null;

        private static readonly ComponentResourceKey _ItemsSelectedActiveBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemsSelectedInactiveBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemsSelectedActiveBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemsSelectedInactiveBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemsSelectedForegroundKey = null;

        // ScrollBars
        private static readonly ComponentResourceKey _ScrollBarsBackgroundKey = null;
        private static readonly ComponentResourceKey _ScrollBarsBorderBrushKey = null;
        private static readonly ComponentResourceKey _ScrollBarsBorderThicknessKey = null;

        private static readonly ComponentResourceKey _ScrollBarsButtonBackgroundKey = null;
        private static readonly ComponentResourceKey _ScrollBarsButtonBorderBrushKey = null;
        private static readonly ComponentResourceKey _ScrollBarsButtonBorderThicknessKey = null;

        private static readonly ComponentResourceKey _ScrollBarsThumbInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _ScrollBarsThumbBackgroundKey = null;
        private static readonly ComponentResourceKey _ScrollBarsThumbBorderBrushKey = null;
        private static readonly ComponentResourceKey _ScrollBarsThumbBorderThicknessKey = null;

        private static readonly ComponentResourceKey _ScrollBarsDisabledThumbInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _ScrollBarsDisabledButtonBackgroundKey = null;
        private static readonly ComponentResourceKey _ScrollBarsDisabledButtonBorderBrushKey = null;

        // Progress
        private static readonly ComponentResourceKey _ProgressBackgroundKey = null;
        private static readonly ComponentResourceKey _ProgressBorderBrushKey = null;
        private static readonly ComponentResourceKey _ProgressBorderThicknessKey = null;
        private static readonly ComponentResourceKey _ProgressCornerRadiusKey = null;
        private static readonly ComponentResourceKey _ProgressForegroundKey = null;
        private static readonly ComponentResourceKey _ProgressPaddingKey = null;

        private static readonly ComponentResourceKey _ProgressBarInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _ProgressBarBackgroundKey = null;
        private static readonly ComponentResourceKey _ProgressBarBorderBrushKey = null;
        private static readonly ComponentResourceKey _ProgressBarBorderThicknessKey = null;

        private static readonly ComponentResourceKey _IndicatorTemplateKey = null;
        private static readonly ComponentResourceKey _IndicatorAccentColorKey = null;
        private static readonly ComponentResourceKey _IndicatorHeightKey = null;
        private static readonly ComponentResourceKey _IndicatorSpeedRatioKey = null;
        private static readonly ComponentResourceKey _IndicatorWidthKey = null;
        #endregion

        #region Resource Keys
        #region Header Templates
        public static ComponentResourceKey HeaderTemplateDefaultKey => _HeaderTemplateDefaultKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderTemplateDefault");
        public static ComponentResourceKey HeaderTemplateDateKey => _HeaderTemplateDateKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderTemplateDate");
        public static ComponentResourceKey HeaderTemplateComboBoxKey => _HeaderTemplateComboBoxKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderTemplateComboBox");
        public static ComponentResourceKey HeaderTemplateTextKey => _HeaderTemplateTextKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderTemplateText");
        #endregion

        #region Styles
        public static ComponentResourceKey GridViewStyleKey => _GridViewStyleKey.GetRegisteredKey(typeof(ListViewResourceKeys), "GridViewStyle");
        public static ComponentResourceKey ScrollViewerStyleKey => _ScrollViewerStyleKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollViewerStyle");
        public static ComponentResourceKey ItemContainerStyleKey => _ItemContainerStyleKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemContainerStyle");
        public static ComponentResourceKey ColumnHeaderGripperStyleKey => _ColumnHeaderGripperStyleKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ColumnHeaderGripperStyle");
        public static ComponentResourceKey ColumnHeaderStyleKey => _ColumnHeaderStyleKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ColumnHeaderStyle");
        #endregion

        #region Control
        #region Normal
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ListViewResourceKeys), "BorderThickness");
        #endregion

        #region Disabled
        public static ComponentResourceKey DisabledBackgroundKey => _DisabledBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "DisabledBackground");
        public static ComponentResourceKey DisabledBorderBrushKey => _DisabledBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "DisabledBorderBrush");
        #endregion
        #endregion

        #region Header
        #region Normal
        public static ComponentResourceKey HeaderBackgroundKey => _HeaderBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderBackground");
        public static ComponentResourceKey HeaderBorderBrushKey => _HeaderBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderBorderBrush");
        public static ComponentResourceKey HeaderForegroundKey => _HeaderForegroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderForeground");
        #endregion

        #region Focused
        public static ComponentResourceKey HeaderFocusedBackgroundKey => _HeaderFocusedBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderFocusedBackground");
        public static ComponentResourceKey HeaderFocusedForegroundKey => _HeaderFocusedForegroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderFocusedForeground");
        #endregion

        #region Pressed
        public static ComponentResourceKey HeaderPressedBackgroundKey => _HeaderPressedBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderPressedBackground");
        public static ComponentResourceKey HeaderPressedForegroundKey => _HeaderPressedForegroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "HeaderPressedForeground");
        #endregion
        #endregion

        #region Items
        #region Normal
        public static ComponentResourceKey ItemsBackgroundKey => _ItemsBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsBackground");
        public static ComponentResourceKey ItemsBorderBrushKey => _ItemsBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsBorderBrush");
        public static ComponentResourceKey ItemsBorderThicknessKey => _ItemsBorderThicknessKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsBorderThickness");
        public static ComponentResourceKey ItemsForegroundKey => _ItemsForegroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsForeground");
        #endregion

        #region Focused
        public static ComponentResourceKey ItemsFocusedBackgroundKey => _ItemsFocusedBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsFocusedBackground");
        public static ComponentResourceKey ItemsFocusedBorderBrushKey => _ItemsFocusedBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsFocusedBorderBrush");
        public static ComponentResourceKey ItemsFocusedForegroundKey => _ItemsFocusedForegroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsFocusedForeground");
        #endregion

        #region Selected
        public static ComponentResourceKey ItemsSelectedActiveBackgroundKey => _ItemsSelectedActiveBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsSelectedActiveBackground");
        public static ComponentResourceKey ItemsSelectedInactiveBackgroundKey => _ItemsSelectedInactiveBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsSelectedInactiveBackground");
        public static ComponentResourceKey ItemsSelectedActiveBorderBrushKey => _ItemsSelectedActiveBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsSelectedActiveBorderBrush");
        public static ComponentResourceKey ItemsSelectedInactiveBorderBrushKey => _ItemsSelectedInactiveBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsSelectedInactiveBorderBrush");
        public static ComponentResourceKey ItemsSelectedForegroundKey => _ItemsSelectedForegroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ItemsSelectedForeground");
        #endregion
        #endregion

        #region ScrollBars
        public static ComponentResourceKey ScrollBarsBackgroundKey => _ScrollBarsBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsBackground");
        public static ComponentResourceKey ScrollBarsBorderBrushKey => _ScrollBarsBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsBorderBrush");
        public static ComponentResourceKey ScrollBarsBorderThicknessKey => _ScrollBarsBorderThicknessKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsBorderThickness");

        #region Button
        public static ComponentResourceKey ScrollBarsButtonBackgroundKey => _ScrollBarsButtonBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsButtonBackground");
        public static ComponentResourceKey ScrollBarsButtonBorderBrushKey => _ScrollBarsButtonBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsButtonBorderBrush");
        public static ComponentResourceKey ScrollBarsButtonBorderThicknessKey => _ScrollBarsButtonBorderThicknessKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsButtonBorderThickness");
        #endregion

        #region Thumb
        public static ComponentResourceKey ScrollBarsThumbInnerBackgroundKey => _ScrollBarsThumbInnerBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsThumbInnerBackground");
        public static ComponentResourceKey ScrollBarsThumbBackgroundKey => _ScrollBarsThumbBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsThumbBackground");
        public static ComponentResourceKey ScrollBarsThumbBorderBrushKey => _ScrollBarsThumbBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsThumbBorderBrush");
        public static ComponentResourceKey ScrollBarsThumbBorderThicknessKey => _ScrollBarsThumbBorderThicknessKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsThumbBorderThickness");
        #endregion

        #region Disabled
        public static ComponentResourceKey ScrollBarsDisabledThumbInnerBackgroundKey => _ScrollBarsDisabledThumbInnerBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsDisabledThumbInnerBackground");
        public static ComponentResourceKey ScrollBarsDisabledButtonBackgroundKey => _ScrollBarsDisabledButtonBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsDisabledButtonBackground");
        public static ComponentResourceKey ScrollBarsDisabledButtonBorderBrushKey => _ScrollBarsDisabledButtonBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ScrollBarsDisabledButtonBorderBrush");
        #endregion
        #endregion

        #region Progress
        #region Control
        public static ComponentResourceKey ProgressBackgroundKey => _ProgressBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressBackground");
        public static ComponentResourceKey ProgressBorderBrushKey => _ProgressBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressBorderBrush");
        public static ComponentResourceKey ProgressBorderThicknessKey => _ProgressBorderThicknessKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressBorderThickness");
        public static ComponentResourceKey ProgressCornerRadiusKey => _ProgressCornerRadiusKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressCornerRadius");
        public static ComponentResourceKey ProgressForegroundKey => _ProgressForegroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressForeground");
        public static ComponentResourceKey ProgressPaddingKey => _ProgressPaddingKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressPadding");
        #endregion

        #region ProgressBar
        public static ComponentResourceKey ProgressBarInnerBackgroundKey => _ProgressBarInnerBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressBarInnerBackground");
        public static ComponentResourceKey ProgressBarBackgroundKey => _ProgressBarBackgroundKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressBarBackground");
        public static ComponentResourceKey ProgressBarBorderBrushKey => _ProgressBarBorderBrushKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressBarBorderBrush");
        public static ComponentResourceKey ProgressBarBorderThicknessKey => _ProgressBarBorderThicknessKey.GetRegisteredKey(typeof(ListViewResourceKeys), "ProgressBarBorderThickness");
        #endregion

        #region Indicator
        public static ComponentResourceKey IndicatorTemplateKey => _IndicatorTemplateKey.GetRegisteredKey(typeof(ListViewResourceKeys), "IndicatorTemplate");
        public static ComponentResourceKey IndicatorAccentColorKey => _IndicatorAccentColorKey.GetRegisteredKey(typeof(ListViewResourceKeys), "IndicatorAccentColor");
        public static ComponentResourceKey IndicatorHeightKey => _IndicatorHeightKey.GetRegisteredKey(typeof(ListViewResourceKeys), "IndicatorHeight");
        public static ComponentResourceKey IndicatorSpeedRatioKey => _IndicatorSpeedRatioKey.GetRegisteredKey(typeof(ListViewResourceKeys), "IndicatorSpeedRatio");
        public static ComponentResourceKey IndicatorWidthKey => _IndicatorWidthKey.GetRegisteredKey(typeof(ListViewResourceKeys), "IndicatorWidth");
        #endregion
        #endregion
        #endregion
    }
}

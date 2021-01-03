using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapScrollViewerResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _ZapTemplateKey = null;
        // Control
        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;
        private static readonly ComponentResourceKey _PaddingKey = null;
        // Disabled
        private static readonly ComponentResourceKey _DisabledBackgroundKey = null;
        private static readonly ComponentResourceKey _DisabledBorderBrushKey = null;
        // ScrollBar Control
        private static readonly ComponentResourceKey _ScrollBarBackgroundKey = null;
        private static readonly ComponentResourceKey _ScrollBarBorderBrushKey = null;
        private static readonly ComponentResourceKey _ScrollBarBorderThicknessKey = null;
        private static readonly ComponentResourceKey _ScrollBarHeightKey = null;
        private static readonly ComponentResourceKey _ScrollBarWidthKey = null;
        // ScrollBar Buttons
        private static readonly ComponentResourceKey _ButtonBackgroundKey = null;
        private static readonly ComponentResourceKey _ButtonBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonBorderThicknessKey = null;
        // ScrollBar Thumb
        private static readonly ComponentResourceKey _ThumbInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _ThumbBackgroundKey = null;
        private static readonly ComponentResourceKey _ThumbBorderBrushKey = null;
        private static readonly ComponentResourceKey _ThumbBorderThicknessKey = null;
        // ScrollBar Disabled
        private static readonly ComponentResourceKey _DisabledThumbInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _DisabledButtonBackgroundKey = null;
        private static readonly ComponentResourceKey _DisabledButtonBorderBrushKey = null;
        #endregion

        #region Resource Keys
        #region Control
        public static ComponentResourceKey ZapTemplateKey => _ZapTemplateKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ZapTemplate");
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "BorderThickness");
        public static ComponentResourceKey PaddingKey => _PaddingKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "Padding");
        #endregion

        #region Disabled
        public static ComponentResourceKey DisabledBackgroundKey => _DisabledBackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "DisabledBackground");
        public static ComponentResourceKey DisabledBorderBrushKey => _DisabledBorderBrushKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "DisabledBorderBrush");
        #endregion

        #region ScrollBar Control
        public static ComponentResourceKey ScrollBarBackgroundKey => _ScrollBarBackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ScrollBarBackground");
        public static ComponentResourceKey ScrollBarBorderBrushKey => _ScrollBarBorderBrushKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ScrollBarBorderBrush");
        public static ComponentResourceKey ScrollBarBorderThicknessKey => _ScrollBarBorderThicknessKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ScrollBarBorderThickness");
        public static ComponentResourceKey ScrollBarHeightKey => _ScrollBarHeightKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ScrollBarHeight");
        public static ComponentResourceKey ScrollBarWidthKey => _ScrollBarWidthKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ScrollBarWidth");
        #endregion

        #region ScrollBar Buttons
        public static ComponentResourceKey ButtonBackgroundKey => _ButtonBackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ButtonBackground");
        public static ComponentResourceKey ButtonBorderBrushKey => _ButtonBorderBrushKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ButtonBorderBrush");
        public static ComponentResourceKey ButtonBorderThicknessKey => _ButtonBorderThicknessKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ButtonBorderThickness");
        #endregion

        #region ScrollBar Thumb
        public static ComponentResourceKey ThumbInnerBackgroundKey => _ThumbInnerBackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ThumbInnerBackground");
        public static ComponentResourceKey ThumbBackgroundKey => _ThumbBackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ThumbBackground");
        public static ComponentResourceKey ThumbBorderBrushKey => _ThumbBorderBrushKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ThumbBorderBrush");
        public static ComponentResourceKey ThumbBorderThicknessKey => _ThumbBorderThicknessKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "ThumbBorderThickness");
        #endregion

        #region ScrollBar Disabled
        public static ComponentResourceKey DisabledThumbInnerBackgroundKey => _DisabledThumbInnerBackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "DisabledThumbInnerBackground");
        public static ComponentResourceKey DisabledButtonBackgroundKey => _DisabledButtonBackgroundKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "DisabledButtonBackground");
        public static ComponentResourceKey DisabledButtonBorderBrushKey => _DisabledButtonBorderBrushKey.GetRegisteredKey(typeof(ZapScrollViewerResourceKeys), "DisabledButtonBorderBrush");
        #endregion
        #endregion
    }
}

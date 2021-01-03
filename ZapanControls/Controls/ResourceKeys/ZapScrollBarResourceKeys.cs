using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapScrollBarResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _ZapTemplateKey = null;
        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;
        // Buttons
        private static readonly ComponentResourceKey _ButtonBackgroundKey = null;
        private static readonly ComponentResourceKey _ButtonBorderBrushKey = null;
        private static readonly ComponentResourceKey _ButtonBorderThicknessKey = null;
        // Thumb
        private static readonly ComponentResourceKey _ThumbInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _ThumbBackgroundKey = null;
        private static readonly ComponentResourceKey _ThumbBorderBrushKey = null;
        private static readonly ComponentResourceKey _ThumbBorderThicknessKey = null;
        // Disabled
        private static readonly ComponentResourceKey _DisabledThumbInnerBackgroundKey = null;
        private static readonly ComponentResourceKey _DisabledButtonBackgroundKey = null;
        private static readonly ComponentResourceKey _DisabledButtonBorderBrushKey = null;
        #endregion

        #region Resource Keys
        #region Control
        public static ComponentResourceKey ZapTemplateKey => _ZapTemplateKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ZapTemplate");
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "BorderBrush");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "BorderThickness");
        #endregion

        #region Buttons
        public static ComponentResourceKey ButtonBackgroundKey => _ButtonBackgroundKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ButtonBackground");
        public static ComponentResourceKey ButtonBorderBrushKey => _ButtonBorderBrushKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ButtonBorderBrush");
        public static ComponentResourceKey ButtonBorderThicknessKey => _ButtonBorderThicknessKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ButtonBorderThickness");
        #endregion

        #region Thumb
        public static ComponentResourceKey ThumbInnerBackgroundKey => _ThumbInnerBackgroundKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ThumbInnerBackground");
        public static ComponentResourceKey ThumbBackgroundKey => _ThumbBackgroundKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ThumbBackground");
        public static ComponentResourceKey ThumbBorderBrushKey => _ThumbBorderBrushKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ThumbBorderBrush");
        public static ComponentResourceKey ThumbBorderThicknessKey => _ThumbBorderThicknessKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "ThumbBorderThickness");
        #endregion

        #region Disabled
        public static ComponentResourceKey DisabledThumbInnerBackgroundKey => _DisabledThumbInnerBackgroundKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "DisabledThumbInnerBackground");
        public static ComponentResourceKey DisabledButtonBackgroundKey => _DisabledButtonBackgroundKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "DisabledButtonBackground");
        public static ComponentResourceKey DisabledButtonBorderBrushKey => _DisabledButtonBorderBrushKey.GetRegisteredKey(typeof(ZapScrollBarResourceKeys), "DisabledButtonBorderBrush");
        #endregion
        #endregion
    }
}

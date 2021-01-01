using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapButtonResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _ZapTemplateKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;
        private static readonly ComponentResourceKey _PaddingKey = null;

        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _ForegroundKey = null;

        private static readonly ComponentResourceKey _CheckedBackgroundKey = null;
        private static readonly ComponentResourceKey _CheckedBorderBrushKey = null;
        private static readonly ComponentResourceKey _CheckedForegroundKey = null;

        private static readonly ComponentResourceKey _FocusedBackgroundKey = null;
        private static readonly ComponentResourceKey _FocusedBorderBrushKey = null;
        private static readonly ComponentResourceKey _FocusedForegroundKey = null;

        private static readonly ComponentResourceKey _PressedBackgroundKey = null;
        private static readonly ComponentResourceKey _PressedBorderBrushKey = null;
        private static readonly ComponentResourceKey _PressedForegroundKey = null;

        private static readonly ComponentResourceKey _DisabledBackgroundKey = null;
        private static readonly ComponentResourceKey _DisabledBorderBrushKey = null;
        private static readonly ComponentResourceKey _DisabledForegroundKey = null;
        #endregion

        #region Resource Keys
        #region Control
        public static ComponentResourceKey ZapTemplateKey => _ZapTemplateKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "ZapTemplate"); 
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderThickness");
        public static ComponentResourceKey PaddingKey => _PaddingKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Padding");
        #endregion

        #region Normal
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderBrush");
        public static ComponentResourceKey ForegroundKey => _ForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Foreground");
        #endregion

        #region Checked
        public static ComponentResourceKey CheckedBackgroundKey => _CheckedBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "CheckedBackground");
        public static ComponentResourceKey CheckedBorderBrushKey => _CheckedBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "CheckedBorderBrush");
        public static ComponentResourceKey CheckedForegroundKey => _CheckedForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "CheckedForeground");
        #endregion

        #region Focused
        public static ComponentResourceKey FocusedBackgroundKey => _FocusedBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "FocusedBackground");
        public static ComponentResourceKey FocusedBorderBrushKey => _FocusedBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "FocusedBorderBrush");
        public static ComponentResourceKey FocusedForegroundKey => _FocusedForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "FocusedForeground");
        #endregion

        #region Pressed
        public static ComponentResourceKey PressedBackgroundKey => _PressedBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "PressedBackground");
        public static ComponentResourceKey PressedBorderBrushKey => _PressedBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "PressedBorderBrush");
        public static ComponentResourceKey PressedForegroundKey => _PressedForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "PressedForeground");
        #endregion

        #region Disabled
        public static ComponentResourceKey DisabledBackgroundKey => _DisabledBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "DisabledBackground");
        public static ComponentResourceKey DisabledBorderBrushKey => _DisabledBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "DisabledBorderBrush");
        public static ComponentResourceKey DisabledForegroundKey => _DisabledForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "DisabledForeground");
        #endregion
        #endregion
    }
}

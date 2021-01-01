using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapMenuItemResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _BorderThicknessKey = null;

        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _ForegroundKey = null;
        private static readonly ComponentResourceKey _SubMenuForegroundKey = null;

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
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "BorderThickness");
        #endregion

        #region Normal
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "BorderBrush");
        public static ComponentResourceKey ForegroundKey => _ForegroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "Foreground");
        public static ComponentResourceKey SubMenuForegroundKey => _SubMenuForegroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "SubMenuForeground");
        #endregion

        #region Checked
        public static ComponentResourceKey CheckedBackgroundKey =>_CheckedBackgroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "CheckedBackground");
        public static ComponentResourceKey CheckedBorderBrushKey => _CheckedBorderBrushKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "CheckedBorderBrush");
        public static ComponentResourceKey CheckedForegroundKey => _CheckedForegroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "CheckedForeground");
        #endregion

        #region Focused
        public static ComponentResourceKey FocusedBackgroundKey => _FocusedBackgroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "FocusedBackground");
        public static ComponentResourceKey FocusedBorderBrushKey => _FocusedBorderBrushKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "FocusedBorderBrush");
        public static ComponentResourceKey FocusedForegroundKey => _FocusedForegroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "FocusedForeground");
        #endregion

        #region Pressed
        public static ComponentResourceKey PressedBackgroundKey => _PressedBackgroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "PressedBackground");
        public static ComponentResourceKey PressedBorderBrushKey => _PressedBorderBrushKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "PressedBorderBrush");
        public static ComponentResourceKey PressedForegroundKey => _PressedForegroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "PressedForeground");
        #endregion

        #region Disabled
        public static ComponentResourceKey DisabledBackgroundKey => _DisabledBackgroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "DisabledBackground");
        public static ComponentResourceKey DisabledBorderBrushKey => _DisabledBorderBrushKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "DisabledBorderBrush");
        public static ComponentResourceKey DisabledForegroundKey => _DisabledForegroundKey.GetRegisteredKey(typeof(ZapMenuItemResourceKeys), "DisabledForeground");
        #endregion
        #endregion
    }
}

using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapButtonResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _ButtonTemplateKey = null;
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
        public static ComponentResourceKey ButtonTemplateKey
        {
            get { return _ButtonTemplateKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "ButtonTemplate"); }
        }

        public static ComponentResourceKey BorderThicknessKey
        {
            get { return _BorderThicknessKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderThickness"); }
        }

        public static ComponentResourceKey PaddingKey
        {
            get { return _PaddingKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Padding"); }
        }
        #endregion

        #region Normal
        public static ComponentResourceKey BackgroundKey
        {
            get { return _BackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Background"); }
        }

        public static ComponentResourceKey BorderBrushKey
        {
            get { return _BorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "BorderBrush"); }
        }

        public static ComponentResourceKey ForegroundKey
        {
            get { return _ForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "Foreground"); }
        }
        #endregion

        #region Checked
        public static ComponentResourceKey CheckedBackgroundKey
        {
            get { return _CheckedBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "CheckedBackground"); }
        }

        public static ComponentResourceKey CheckedBorderBrushKey
        {
            get { return _CheckedBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "CheckedBorderBrush"); }
        }

        public static ComponentResourceKey CheckedForegroundKey
        {
            get { return _CheckedForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "CheckedForeground"); }
        }
        #endregion

        #region Focused
        public static ComponentResourceKey FocusedBackgroundKey
        {
            get { return _FocusedBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "FocusedBackground"); }
        }

        public static ComponentResourceKey FocusedBorderBrushKey
        {
            get { return _FocusedBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "FocusedBorderBrush"); }
        }

        public static ComponentResourceKey FocusedForegroundKey
        {
            get { return _FocusedForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "FocusedForeground"); }
        }
        #endregion

        #region Pressed
        public static ComponentResourceKey PressedBackgroundKey
        {
            get { return _PressedBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "PressedBackground"); }
        }

        public static ComponentResourceKey PressedBorderBrushKey
        {
            get { return _PressedBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "PressedBorderBrush"); }
        }

        public static ComponentResourceKey PressedForegroundKey
        {
            get { return _PressedForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "PressedForeground"); }
        }
        #endregion

        #region Disabled
        public static ComponentResourceKey DisabledBackgroundKey
        {
            get { return _DisabledBackgroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "DisabledBackground"); }
        }

        public static ComponentResourceKey DisabledBorderBrushKey
        {
            get { return _DisabledBorderBrushKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "DisabledBorderBrush"); }
        }

        public static ComponentResourceKey DisabledForegroundKey
        {
            get { return _DisabledForegroundKey.GetRegisteredKey(typeof(ZapButtonResourceKeys), "DisabledForeground"); }
        }
        #endregion
        #endregion
    }
}

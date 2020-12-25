using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapDatePickerResourceKeys
    {
        #region Fields
        // Control
        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;

        // Button background 
        private static readonly ComponentResourceKey _ButtonBackgroundKey = null;
        private static readonly ComponentResourceKey _ButtonBackgroundHoverKey = null;
        private static readonly ComponentResourceKey _ButtonBackgroundPressedKey = null;
        // Button background 
        private static readonly ComponentResourceKey _ButtonBorderKey = null;
        private static readonly ComponentResourceKey _ButtonBorderHoverKey = null;
        private static readonly ComponentResourceKey _ButtonBorderPressedKey = null;

        // Icon 
        private static readonly ComponentResourceKey _IconNormalKey = null;
        private static readonly ComponentResourceKey _IconHoverKey = null;
        private static readonly ComponentResourceKey _IconPressedKey = null;

        #endregion

        #region Resource Keys

        #region Control
        public static ComponentResourceKey BackgroundKey
        {
            get => _BackgroundKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "Background");
        }
        public static ComponentResourceKey BorderBrushKey
        {
            get => _BorderBrushKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "BorderBrush");
        }
        #endregion

        #region Button
        public static ComponentResourceKey ButtonBackgroundKey
        {
            get => _ButtonBackgroundKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "ButtonBackground");
        }
        public static ComponentResourceKey ButtonBackgroundHoverKey
        {
            get => _ButtonBackgroundHoverKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "ButtonBackgroundHover");
        }
        public static ComponentResourceKey ButtonBackgroundPressedKey
        {
            get => _ButtonBackgroundPressedKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "ButtonBackgroundPressed");
        }
        public static ComponentResourceKey ButtonBorderKey
        {
            get => _ButtonBorderKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "ButtonBorder");
        }
        public static ComponentResourceKey ButtonBorderHoverKey
        {
            get => _ButtonBorderHoverKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "ButtonBorderHover");
        }
        public static ComponentResourceKey ButtonBorderPressedKey
        {
            get => _ButtonBorderPressedKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "ButtonBorderPressed");
        }
        #endregion

        #region Icon
        public static ComponentResourceKey IconNormalKey
        {
            get => _IconNormalKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "IconNormal");
        }

        public static ComponentResourceKey IconHoverKey
        {
            get => _IconHoverKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "IconHover");
        }

        public static ComponentResourceKey IconPressedKey
        {
            get => _IconPressedKey.GetRegisteredKey(typeof(ZapDatePickerResourceKeys), "IconPressed");
        }
        #endregion

        #endregion
    }
}

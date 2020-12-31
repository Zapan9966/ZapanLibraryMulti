using System.Windows;

namespace ZapanControls.Controls.ResourceKeys
{
    public static class ZapTabControlResourceKeys
    {
        #region Fields
        private static readonly ComponentResourceKey _TabsTemplateKey = null;
        private static readonly ComponentResourceKey _TabAddTemplateKey = null;

        private static readonly ComponentResourceKey _BackgroundKey = null;
        private static readonly ComponentResourceKey _BorderBrushKey = null;
        private static readonly ComponentResourceKey _ForegroundKey = null;
        private static readonly ComponentResourceKey _BorderThicknessKey = null;
        private static readonly ComponentResourceKey _PaddingKey = null;

        private static readonly ComponentResourceKey _AddContentKey = null;

        private static readonly ComponentResourceKey _ItemBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemForegroundKey = null;

        private static readonly ComponentResourceKey _ItemFocusedBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemFocusedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemFocusedForegroundKey = null;

        private static readonly ComponentResourceKey _ItemSelectedBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemSelectedBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemSelectedForegroundKey = null;

        private static readonly ComponentResourceKey _ItemDisabledBackgroundKey = null;
        private static readonly ComponentResourceKey _ItemDisabledBorderBrushKey = null;
        private static readonly ComponentResourceKey _ItemDisabledForegroundKey = null;
        #endregion

        #region Resource Keys
        #region Control
        public static ComponentResourceKey TabsTemplateKey => _TabsTemplateKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "TabsTemplate");
        public static ComponentResourceKey TabAddTemplateKey => _TabAddTemplateKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "TabAddTemplate");
        public static ComponentResourceKey BackgroundKey => _BackgroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "Background");
        public static ComponentResourceKey BorderBrushKey => _BorderBrushKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "BorderBrush");
        public static ComponentResourceKey ForegroundKey  => _ForegroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "Foreground");
        public static ComponentResourceKey BorderThicknessKey => _BorderThicknessKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "BorderThickness");
        public static ComponentResourceKey PaddingKey => _PaddingKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "Padding");
        #endregion

        #region Add Tab
        public static ComponentResourceKey AddContentKey => _AddContentKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "AddContent");
        #endregion

        #region ItemNormal
        public static ComponentResourceKey ItemBackgroundKey => _ItemBackgroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemBackground");
        public static ComponentResourceKey ItemBorderBrushKey => _ItemBorderBrushKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemBorderBrush");
        public static ComponentResourceKey ItemForegroundKey => _ItemForegroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemForeground");
        #endregion

        #region Focused
        public static ComponentResourceKey ItemFocusedBackgroundKey => _ItemFocusedBackgroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemFocusedBackground");
        public static ComponentResourceKey ItemFocusedBorderBrushKey => _ItemFocusedBorderBrushKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemFocusedBorderBrush");
        public static ComponentResourceKey ItemFocusedForegroundKey => _ItemFocusedForegroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemFocusedForeground");
        #endregion

        #region Selected
        public static ComponentResourceKey ItemSelectedBackgroundKey => _ItemSelectedBackgroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemSelectedBackground");
        public static ComponentResourceKey ItemSelectedBorderBrushKey => _ItemSelectedBorderBrushKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemSelectedBorderBrush");
        public static ComponentResourceKey ItemSelectedForegroundKey => _ItemSelectedForegroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemSelectedForeground");
        #endregion

        #region Disabled
        public static ComponentResourceKey ItemDisabledBackgroundKey => _ItemDisabledBackgroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemDisabledBackground");
        public static ComponentResourceKey ItemDisabledBorderBrushKey => _ItemDisabledBorderBrushKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemDisabledBorderBrush");
        public static ComponentResourceKey ItemDisabledForegroundKey => _ItemDisabledForegroundKey.GetRegisteredKey(typeof(ZapTabControlResourceKeys), "ItemDisabledForeground");
        #endregion
        #endregion
    }
}

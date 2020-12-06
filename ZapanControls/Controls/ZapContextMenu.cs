using System.Windows;
using System.Windows.Controls;

namespace ZapanControls.Controls
{
    public sealed class ZapContextMenu : ContextMenu
    {
        #region Dependancy Properties

        /// <summary>
        /// Identifies the <see cref="ZapContextMenu.CornerRadiusProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(ZapContextMenu), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// Get/set the ContextMenu corner radius.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion

        #region Constructors 

        static ZapContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapContextMenu), new FrameworkPropertyMetadata(typeof(ZapContextMenu)));
        }

        #endregion
    }
}

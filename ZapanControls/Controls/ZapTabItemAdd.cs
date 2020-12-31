using System.Windows;
using System.Windows.Controls;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Helpers;

namespace ZapanControls.Controls
{
    internal sealed class ZapTabItemAdd : TabItem
    {
        #region Internal Event Handlers
        internal void OnTabAddClick(object sender, RoutedEventArgs e)
        {
            if (Parent is ZapTabControl tc)
            {
                var eventArgs = new TabAddEventArgs(ZapTabControl.TabAddEvent, this);
                RaiseEvent(eventArgs);

                if (!eventArgs.Handled)
                    eventArgs.Handled = true;

                if (eventArgs.NewTabItem != null)
                {
                    tc.Items.Add(eventArgs.NewTabItem);

                    if (eventArgs.NewTabItem is Control ctrl)
                        ctrl.Focus();

                    tc.SelectedItem = eventArgs.NewTabItem;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructeur de la classe <see cref="ZapTabItemAdd"/>
        /// </summary>
        static ZapTabItemAdd()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapTabItemAdd), new FrameworkPropertyMetadata(typeof(ZapTabItemAdd)));
        }

        public ZapTabItemAdd()
        {

        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (VisualTreeHelpers.FindChild(this, "tabAdd") is ZapButton tabAdd)
            {
                tabAdd.Click -= OnTabAddClick;
                tabAdd.Click += OnTabAddClick;
            }
        }
        #endregion

    }
}

using System.Windows;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Helpers;

namespace ZapanControls.Controls
{
    public sealed class ListViewItem : System.Windows.Controls.ListViewItem
    {
        #region Internal Event Handlers
        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var listView = VisualTreeHelpers.FindParent<ListView>(this);
            if (listView != null)
            {
                listView.RaiseEvent(new ListViewItemDoubleClickEventArgs(ListView.ItemDoubleClickEvent, listView, this));
            }
        }
        #endregion

        #region Constructors
        static ListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewItem), new FrameworkPropertyMetadata(typeof(ListViewItem)));
        }

        public ListViewItem()
        {
            MouseDoubleClick += OnMouseDoubleClick;
        }
        #endregion
    }
}

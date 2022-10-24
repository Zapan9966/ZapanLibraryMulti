using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Helpers;

namespace ZapanControls.Controls
{
    public class ListViewItem : System.Windows.Controls.ListViewItem
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

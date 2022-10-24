using System.Windows;

namespace ZapanControls.Controls.ControlEventArgs
{
    public class ListViewItemDoubleClickEventArgs : RoutedEventArgs
    {
        public ListViewItem Item { get; private set; }

        public ListViewItemDoubleClickEventArgs(RoutedEvent routedEvent, object source, ListViewItem item)
        {
            RoutedEvent = routedEvent;
            Source = source;
            Handled = false;
            Item = item;
        }
    }
}

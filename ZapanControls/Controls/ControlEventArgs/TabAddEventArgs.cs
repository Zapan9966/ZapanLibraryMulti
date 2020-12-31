using System.Windows;

namespace ZapanControls.Controls.ControlEventArgs
{
    public class TabAddEventArgs : RoutedEventArgs
    {
        public object NewTabItem { get; set; }
        public object Parameter { get; set; }

        public TabAddEventArgs(RoutedEvent routedEvent, object source, object parameter = null)
        {
            RoutedEvent = routedEvent;
            Source = source;
            Handled = false;
            NewTabItem = new ZapTabItem();
            Parameter = parameter;
        }
    }
}

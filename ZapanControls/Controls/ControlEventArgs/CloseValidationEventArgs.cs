using System.Windows;

namespace ZapanControls.Controls.ControlEventArgs
{
    public class CloseValidationEventArgs : RoutedEventArgs
    {
        public bool CanClose { get; set; }

        public CloseValidationEventArgs(RoutedEvent routedEvent, object source)
        {
            RoutedEvent = routedEvent;
            Source = source;
            Handled = false;
            CanClose = true;
        }
    }
}

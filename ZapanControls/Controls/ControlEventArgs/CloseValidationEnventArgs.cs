using System.Windows;

namespace ZapanControls.Controls.ControlEventArgs
{
    public class CloseValidationEnventArgs : RoutedEventArgs
    {
        public bool CanClose { get; set; }

        public CloseValidationEnventArgs(RoutedEvent routedEvent, object source)
        {
            RoutedEvent = routedEvent;
            Source = source;
            Handled = false;
            CanClose = true;
        }
    }
}

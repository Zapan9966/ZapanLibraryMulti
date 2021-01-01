using System.Windows;

namespace ZapanControls.Controls.ControlEventArgs
{
    public class TemplateChangedEventArgs : RoutedEventArgs
    {
        public string NewTemplateKey { get; private set; }
        public string OldTemplateKey { get; private set; }

        public TemplateChangedEventArgs(RoutedEvent routedEvent, object source, string oldTemplateKey, string newTemplateKey)
        {
            RoutedEvent = routedEvent;
            Source = source;
            Handled = false;
            OldTemplateKey = oldTemplateKey;
            NewTemplateKey = newTemplateKey;
        }
    }
}

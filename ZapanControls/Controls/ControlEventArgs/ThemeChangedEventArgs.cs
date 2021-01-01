using System.Windows;

namespace ZapanControls.Controls.ControlEventArgs
{
    public class ThemeChangedEventArgs : RoutedEventArgs
    {
        public string NewThemeKey { get; private set; }
        public string OldThemeKey { get; private set; }

        public ThemeChangedEventArgs(RoutedEvent routedEvent, object source, string oldThemeKey, string newThemeKey)
        {
            RoutedEvent = routedEvent;
            Source = source;
            Handled = false;
            OldThemeKey = oldThemeKey;
            NewThemeKey = newThemeKey;
        }
    }
}

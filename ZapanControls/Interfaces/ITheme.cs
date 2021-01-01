using System.Collections.Generic;
using System.Windows;
using ZapanControls.Controls.ControlEventArgs;

namespace ZapanControls.Interfaces
{
    public interface ITheme : IObservableObject
    {
        Dictionary<DependencyProperty, object> DefaultThemeProperties { get; }
        Dictionary<string, ResourceDictionary> ThemeDictionaries { get; }
        string Theme { get; set; }

        delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);
        event ThemeChangedEventHandler ThemeChanged;
    }
}

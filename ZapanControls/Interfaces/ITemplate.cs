using System.Collections.Generic;
using System.Windows;
using ZapanControls.Controls.ControlEventArgs;

namespace ZapanControls.Interfaces
{
    public interface ITemplate : ITheme
    {
        Dictionary<string, ResourceDictionary> TemplateDictionaries { get; }
        bool HasInitialized { get; }
        string ZapTemplate { get; set; }

        delegate void TemplateChangedEventHandler(object sender, TemplateChangedEventArgs e);
        event TemplateChangedEventHandler TemplateChanged;
    }
}

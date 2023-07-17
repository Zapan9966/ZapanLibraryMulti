using System.Collections.Generic;
using System.Windows;
using ZapanControls.Controls.ControlEventArgs;

namespace ZapanControls.Interfaces
{
    public interface ITemplate<TTemplate> : ITheme
    {
        Dictionary<string, ResourceDictionary> TemplateDictionaries { get; }
        bool HasInitialized { get; }
        TTemplate ZapTemplate { get; set; }

        delegate void TemplateChangedEventHandler(object sender, TemplateChangedEventArgs e);
        event TemplateChangedEventHandler TemplateChanged;
    }
}

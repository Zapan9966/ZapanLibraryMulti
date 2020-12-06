using System;
using System.Windows.Markup;

namespace ZapanControls.Converters
{
    /// <summary>
    /// Classe de base pour la création d'un convertisseur WPF.
    /// </summary>
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

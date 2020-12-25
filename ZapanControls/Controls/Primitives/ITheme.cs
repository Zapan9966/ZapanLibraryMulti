using System;

namespace ZapanControls.Controls.Primitives
{
    public interface ITheme
    {
        string Theme { get; set; }

        void RegisterTheme(ThemePath theme, Type ownerType);

        string GetRegistrationName(string themeName, Type ownerType);

        string GetThemeName(string key);
    }
}

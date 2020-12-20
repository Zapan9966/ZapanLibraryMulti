using System;

namespace ZapanControls.Controls.Primitives
{
    /// <summary>Theme name/path pairing</summary>
    public struct ThemePath
    {
        public string Name;
        public string DictionaryPath;

        /// <summary>
        /// ThemePath Constructor: add a theme to the control
        /// </summary>
        /// <param name="name">Theme name</param>
        /// <param name="dictionaryPath">Theme path</param>
        public ThemePath(string name, string dictionaryPath)
        {
            Name = name;
            DictionaryPath = dictionaryPath;
        }

        /// <summary>
        /// ThemePath Constructor: add a theme to the control
        /// </summary>
        /// <param name="theme">Theme enumerable value</param>
        /// <param name="dictionaryPath">Theme path</param>
        public ThemePath(Enum theme, string dictionaryPath)
        {
            Name = theme.ToString();
            DictionaryPath = dictionaryPath;
        }
    }
}

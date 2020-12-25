using System;

namespace ZapanControls.Controls.Primitives
{
    public struct TemplatePath
    {
        public string Name;
        public string DictionaryPath;

        /// <summary>
        /// TemplatePath Constructor: add a template to the control
        /// </summary>
        /// <param name="name">Theme name</param>
        /// <param name="dictionaryPath">Theme path</param>
        public TemplatePath(string name, string dictionaryPath)
        {
            Name = name;
            DictionaryPath = dictionaryPath;
        }

        /// <summary>
        /// TemplatePath Constructor: add a template to the control
        /// </summary>
        /// <param name="theme">Theme enumerable value</param>
        /// <param name="dictionaryPath">Theme path</param>
        public TemplatePath(Enum theme, string dictionaryPath)
        {
            Name = theme.ToString();
            DictionaryPath = dictionaryPath;
        }
    }
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Interfaces;

namespace ZapanControls.Controls.Templates
{
    internal static class TemplateExtensions
    {
        private const string ZapTemplatePropName = "ZapTemplate";

        public static string TemplateRegistrationName(this TemplatePath template, Type ownerType) 
            => template.Name.TemplateRegistrationName(ownerType);

        public static string TemplateRegistrationName(this string template, Type ownerType)
            => $"{ownerType};{template}";

        public static string GetTemplateName(this string key)
            => key?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1];

        public static void LoadDefaultTemplate<TTemplate>(this FrameworkElement f, DependencyProperty p)
        {
            if (f is ITemplate<TTemplate> t)
            {
                if (t.TemplateDictionaries.Any())
                {
                    object template = t.TemplateDictionaries.First().Key.GetTemplateName();
                    if (typeof(TTemplate).IsEnum)
                    {
                        template = (TTemplate)Enum.Parse(typeof(TTemplate), template.ToString());
                    }

                    f.SetCurrentValue(p, template);
                }
            }
        }

        public static void RegisterAttachedTemplates<TTemplate>(this DependencyObject o, Type type)
        {
            if (o is ITemplate<TTemplate> t)
            {
                var templateFields = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.FieldType == typeof(TemplatePath));

                foreach (var field in templateFields)
                {
                    t.RegisterTemplate((TemplatePath)field.GetValue(o), o.GetType());
                }
            }
        }

        public static void RegisterTemplate<TTemplate>(this ITemplate<TTemplate> t, TemplatePath template, Type ownerType)
        {
            // test args
            if (template.Name == null || template.DictionaryPath == null)
                throw new ArgumentNullException("Theme name/path is null");

            if (ownerType == null)
                throw new ArgumentNullException("Invalid ownerType");

            string registrationName = template.Name.TemplateRegistrationName(ownerType);

            try
            {
                if (!t.TemplateDictionaries.ContainsKey(registrationName))
                {
                    // create the Uri
                    Uri themeUri = new Uri(template.DictionaryPath, UriKind.Relative);
                    // register the new theme
                    t.TemplateDictionaries[registrationName] = Application.LoadComponent(themeUri) as ResourceDictionary;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void TemplateChanged<TTemplate>(this DependencyObject o, DependencyPropertyChangedEventArgs e, RoutedEvent successEvent)
        {
            // test args
            if (!(o is ITemplate<TTemplate> t) || !(o is FrameworkElement fe) || e == null)
                throw new ArgumentNullException("Invalid ZapTemplate property");

            string curTemplateName = e.OldValue as string;
            string curRegisteredTemplateName = curTemplateName.TemplateRegistrationName(o.GetType());

            if (t.TemplateDictionaries.ContainsKey(curRegisteredTemplateName))
            {
                // remove current template
                ResourceDictionary curTemplateDictionary = t.TemplateDictionaries[curRegisteredTemplateName];
                fe.Resources.MergedDictionaries.Remove(curTemplateDictionary);
            }

            // new template name
            string newTemplateName = e.NewValue as string;
            string newRegisteredTemplateName = !string.IsNullOrEmpty(newTemplateName) 
                ? newTemplateName.TemplateRegistrationName(o.GetType())
                : t.TemplateDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!t.TemplateDictionaries.ContainsKey(newRegisteredTemplateName))
            {
                throw new ArgumentNullException("Invalid ZapTemplate property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newTemplateDictionary = t.TemplateDictionaries[newRegisteredTemplateName];
                fe.Resources.MergedDictionaries.Add(newTemplateDictionary);

                if (t.HasInitialized)
                {
                    fe.OnApplyTemplate();
                }

                // Raise theme successfully changed event
                fe.RaiseEvent(new TemplateChangedEventArgs(successEvent, o, curRegisteredTemplateName, newRegisteredTemplateName));
            }

            t.RaisePropertyChanged(new PropertyChangedEventArgs(ZapTemplatePropName));
        }
    }
}

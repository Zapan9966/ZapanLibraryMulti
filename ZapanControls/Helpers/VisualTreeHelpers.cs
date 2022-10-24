using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ZapanControls.Helpers
{
    public static class VisualTreeHelpers
    {
        /// <summary>
        /// Méthode permettant de récupérer les contrôles enfant d'un object de dépendance.
        /// </summary>
        /// <typeparam name="T">Type de contrôle à rechercher.</typeparam>
        /// <param name="depObj">Objet de dépendance dans lequel effectuer la recherche.</param>
        /// <returns>Renvoi une collection de contrôles.</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                if (depObj is FrameworkElement) (depObj as FrameworkElement).ApplyTemplate();

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static IEnumerable<FrameworkElement> FindVisualChildrenByName(DependencyObject depObj, string name)
        {
            if (depObj != null)
            {
                if (depObj is FrameworkElement) (depObj as FrameworkElement).ApplyTemplate();

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is FrameworkElement element && element.Name == name)
                    {
                        yield return child as FrameworkElement;
                    }

                    foreach (FrameworkElement childOfChild in FindVisualChildrenByName(child, name))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static FrameworkElement FindVisualChild(DependencyObject depObj, string name)
        {
            if (depObj != null)
            {
                if (depObj is FrameworkElement) (depObj as FrameworkElement).ApplyTemplate();

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is FrameworkElement element && element.Name == name)
                    {
                        return child as FrameworkElement;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Recherche le contrôle parent d'un objet.
        /// </summary>
        /// <typeparam name="T">Type du parent à rechercher.</typeparam>
        /// <param name="child">Contrôle enfant utilisé comme point de départ.</param>
        /// <returns>Renvoi le contrôle parent.</returns>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        /// <summary>
        /// Recherche le contrôle parent d'un objet dans le template.
        /// </summary>
        public static T FindTemplatedParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = LogicalTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            if (parentObject is T parent)
                return parent;
            else
                return FindTemplatedParent<T>(parentObject);
        }

        /// <summary>
        /// Looks for a child control within a parent by name.
        /// </summary>
        public static DependencyObject FindChild(DependencyObject parent, string name)
        {
            // confirm parent and name are valid.
            if (parent == null || string.IsNullOrEmpty(name)) return null;

            if (parent is FrameworkElement && (parent as FrameworkElement).Name == name) return parent;

            DependencyObject result = null;

            if (parent is FrameworkElement) (parent as FrameworkElement).ApplyTemplate();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                result = FindChild(child, name);
                if (result != null) break;
            }

            return result;
        }

        /// <summary>
        /// Looks for a child control within a parent by type.
        /// </summary>
        public static T FindChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            // confirm parent is valid.
            if (parent == null) return null;
            if (parent is T) return parent as T;

            DependencyObject foundChild = null;

            if (parent is FrameworkElement) (parent as FrameworkElement).ApplyTemplate();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                foundChild = FindChild<T>(child);
                if (foundChild != null) break;
            }

            return foundChild as T;
        }

        /// <summary>
        /// Looks for a child control within a parent by name.
        /// </summary>
        public static FrameworkElement GetTemplateChildByName(DependencyObject parent, string name)
        {
            if (parent is FrameworkElement) (parent as FrameworkElement).ApplyTemplate();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement element && element.Name == name)
                {
                    return child as FrameworkElement;
                }
                else
                {
                    var s = GetTemplateChildByName(child, name);
                    if (s != null)
                        return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Défini un <see cref="Binding"/> sur un contrôle.
        /// </summary>
        /// <param name="parent">Contrôle sur lequel appliqué le <see cref="Binding"/>.</param>
        /// <param name="elements">Propriété à définir.</param>
        /// <param name="data">Données a affecter au <see cref="Binding"/>.</param>
        public static void SetBindedItem(object parent, string[] elements, object data)
        {
            if (parent != null)
            {
                if ((elements?.Length ?? 0) == 0)
                    return;

                if (elements.Length == 1)
                {
                    var accesor = parent.GetType().GetProperty(elements[0]);

                    if (accesor?.CanWrite ?? false)
                        accesor.SetValue(parent, data);
                }
                string[] innerElements = elements.Skip(1).ToArray();
                object innerProperty = parent.GetType().GetProperty(elements[0]).GetValue(parent);
                SetBindedItem(innerProperty, innerElements, data);
            }
        }

        /// <summary>
        /// Renvoi la valeur d'un <see cref="Binding"/>.
        /// </summary>
        public static object GetBindedItem(object data, string[] elements)
        {
            if (data != null)
            {
                if ((elements?.Length ?? 0) == 0)
                    return data;

                if (elements.Length == 1)
                {
                    var accesor = data.GetType().GetProperty(elements[0]);
                    return accesor.GetValue(data, null);
                }
                string[] innerElements = elements.Skip(1).ToArray();
                object innerData = data.GetType().GetProperty(elements[0]).GetValue(data);
                return GetBindedItem(innerData, innerElements);
            }
            return null;
        }

        /// <summary>
        /// Met à jour un binding contenu dans la fenêtre parente du contrôle.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public static void UpdateBinding(DependencyObject obj, DependencyProperty prop, object value)
        {
            Binding binding = BindingOperations.GetBinding(obj, prop);
            UpdateBinding(obj, binding, value);
        }

        /// <summary>
        /// Met à jour un binding contenu dans la fenêtre parente du contrôle.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="binding"></param>
        /// <param name="value"></param>
        public static void UpdateBinding(DependencyObject obj, Binding binding, object value)
        {
            string bindingPath = binding.Path.Path;
            string[] elements = bindingPath.Split('.');

            Window parentWindow = Window.GetWindow(obj);

            object parent = parentWindow;
            if (binding.Source != null)
            {
                parent = binding.Source;
            }
            else if (!string.IsNullOrEmpty(binding.ElementName))
            {
                parent = FindChild(parentWindow, binding.ElementName);
            }

            SetBindedItem(parent, elements, value);
        }

    }
}

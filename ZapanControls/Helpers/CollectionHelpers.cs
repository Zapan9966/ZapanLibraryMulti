using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Data;
using ZapanControls.Libraries;

namespace ZapanControls.Helpers
{
    public static class CollectionHelpers
    {
        public static object SortWithConverter(IEnumerable items, Binding binding, ListSortDirection? direction, bool isParallel = true)
        {
            IEnumerable<dynamic> source = items?.Cast<dynamic>();
            IEnumerable<dynamic> sortedItems = null;

            if (isParallel)
            {
                if (direction == ListSortDirection.Ascending)
                {
                    sortedItems = source.AsParallel().AsOrdered()
                        .OrderBy(o => binding.Converter.Convert(
                                GetPropertyValue(o, binding.Path.Path),
                                typeof(string), binding.ConverterParameter,
                                Thread.CurrentThread.CurrentCulture
                            )
                        );
                }
                else
                {
                    sortedItems = source.AsParallel().AsOrdered()
                        .OrderByDescending(o => binding.Converter.Convert(
                                GetPropertyValue(o, binding.Path.Path),
                                typeof(string), binding.ConverterParameter,
                                Thread.CurrentThread.CurrentCulture
                            )
                        );
                }
            }
            else
            {
                if (direction == ListSortDirection.Ascending)
                {
                    sortedItems = source.OrderBy(o => binding.Converter.Convert(
                        GetPropertyValue(o, binding.Path.Path),
                        typeof(string), binding.ConverterParameter,
                        Thread.CurrentThread.CurrentCulture)
                    );
                }
                else
                {
                    sortedItems = source.OrderByDescending(o => binding.Converter.Convert(
                        GetPropertyValue(o, binding.Path.Path), 
                        typeof(string), binding.ConverterParameter, 
                        Thread.CurrentThread.CurrentCulture)
                    );
                }
            }

            return EnumerableDynamicCast(sortedItems, items);
        }

        private static object GetPropertyValue(object prop, string path)
        {
            object propValue = prop;

            var properties = path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyDescriptor property = TypeDescriptor.GetProperties(propValue).Find(properties[i], true);
                try
                {
                    if (property != null)
                        propValue = property.GetValue(propValue);
                }
                catch { }
            }
            return propValue;
        }

        /// <summary>
        /// Converti le type d'une collection vers un autre type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static object EnumerableDynamicCast(IEnumerable source, IEnumerable target)
        {
            object result = source;

            if (source != null && target != null)
            {
                if (source.GetType() != target.GetType())
                {
                    MethodInfo methodInfo = typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public);
                    Type[] genericArguments = new Type[] { target.GetType().GenericTypeArguments[0] };
                    MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(genericArguments);

                    result = genericMethodInfo.Invoke(null, new object[] { result });
                    result = Activator.CreateInstance(target.GetType(), result);
                }
            }
            return result;
        }

    }
}

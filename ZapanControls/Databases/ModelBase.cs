using System;
using System.Data.Common;
using System.Reflection;
using System.Windows.Media;
using ZapanControls.Libraries;

namespace ZapanControls.Databases
{
    public abstract class ModelBase<T> : ObservableObject
    {
        #region Constructors

        /// <summary>
        /// Constructeur de la class <see cref="ModelBase{T}"/>.
        /// </summary>
        protected ModelBase()
        { }

        /// <summary>
        /// Constructeur de la class <see cref="ModelBase{T}"/>.
        /// </summary>
        /// <param name="reader"><see cref="DbDataReader"/> utilisé pour définir les propriété de la classe.</param>
        protected ModelBase(DbDataReader reader)
        {
            if (reader != null)
            {
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    if (prop.Name == "Error" || prop.Name == "Item" || prop.Name == "CanFreeze" || prop.Name == "IsFrozen"
                        || prop.Name == "DependencyObjectType" || prop.Name == "IsSealed" || prop.Name == "Dispatcher")
                        continue;

                    Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    object value = t == typeof(int) && string.IsNullOrEmpty(reader[prop.Name].ToString()) ? 0 : reader[prop.Name];
                    value = (t == typeof(DateTime) || t == typeof(DateTime?)) && string.IsNullOrEmpty(reader[prop.Name].ToString()) ? Convert.DBNull : value;
                    value = t == typeof(Brush) && !string.IsNullOrEmpty(reader[prop.Name].ToString()) ?
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString(value.ToString())) : value;

                    object safeValue;
                    if (!(value is SolidColorBrush))
                    {
                        safeValue = value == null || Convert.IsDBNull(value) ? null : Convert.ChangeType(value, t);
                    }
                    else
                    {
                        safeValue = value;
                        ((SolidColorBrush)safeValue).Freeze();
                    }

                    prop.SetValue(this, safeValue);
                }
            }
        }

        #endregion
    }
}

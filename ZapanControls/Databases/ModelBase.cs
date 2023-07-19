using System;
using System.Data.Common;
using System.Linq;
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
                var objectMemberAccessor = FastMember.TypeAccessor.Create(typeof(T));
                var propertiesDict = objectMemberAccessor
                    .GetMembers()
                    .ToDictionary(
                        m => m.Name.ToUpper(),
                        m => m.Type
                    );

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var nameNormalized = reader.GetName(i).ToUpper();
                    if (propertiesDict.ContainsKey(nameNormalized))
                    {
                        Type t = Nullable.GetUnderlyingType(propertiesDict[nameNormalized]) ?? propertiesDict[nameNormalized];

                        object value = reader.GetValue(i);
                        if (string.IsNullOrEmpty(value?.ToString()))
                        {
                            if (t == typeof(int))
                            {
                                value = 0;
                            }
                        }
                        else if (t == typeof(Brush))
                        {
                            value = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value.ToString()));
                        }

                        object safeValue;
                        if (value is SolidColorBrush brush)
                        {
                            safeValue = value;
                            ((SolidColorBrush)safeValue).Freeze();
                        }
                        else
                        {
                            safeValue = value == null || Convert.IsDBNull(value) ? null : Convert.ChangeType(value, t);
                        }
                        objectMemberAccessor[this, reader.GetName(i)] = safeValue;
                    }


                }
            }
        }

        #endregion
    }
}

﻿using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ZapanControls.Libraries
{
    // Source : https://github.com/RSuter/MyToolkit/blob/master/src/MyToolkit/Utilities/ExpressionUtilities.cs

    /// <summary>Provides methods to handle lambda expressions. </summary>
    public static class ExpressionUtilities
    {
        /// <summary>Returns the property name of the property specified in the given lambda (e.g. GetPropertyName(i => i.MyProperty)). </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <typeparam name="TProperty">The property type. </typeparam>
        /// <param name="expression">The lambda with the property. </param>
        /// <returns>The name of the property in the lambda. </returns>
#if !LEGACY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetPropertyName<TClass, TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            if (expression?.Body is UnaryExpression)
                return ((MemberExpression)(((UnaryExpression)expression.Body).Operand)).Member.Name;
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>Returns the property name of the property specified in the given lambda (e.g. GetPropertyName(i => i.MyProperty)). </summary>
        /// <typeparam name="TProperty">The property type. </typeparam>
        /// <param name="expression">The lambda with the property. </param>
        /// <returns>The name of the property in the lambda. </returns>
#if !LEGACY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> expression)
        {
            if (expression?.Body is UnaryExpression)
                return ((MemberExpression)(((UnaryExpression)expression.Body).Operand)).Member.Name;
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>Returns the property name of the property specified in the given lambda (e.g. GetPropertyName(i => i.MyProperty)). </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <param name="expression">The lambda with the property. </param>
        /// <returns>The name of the property in the lambda. </returns>
#if !LEGACY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetPropertyName<TClass>(Expression<Func<TClass, object>> expression)
        {
            if (expression?.Body is UnaryExpression)
                return ((MemberExpression)(((UnaryExpression)expression.Body).Operand)).Member.Name;
            return ((MemberExpression)expression.Body).Member.Name;
        }
    }
}

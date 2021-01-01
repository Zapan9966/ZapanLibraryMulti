using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ZapanControls.Interfaces
{
    public interface IObservableObject : INotifyPropertyChanged
    {
        bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null);
        bool Set<T>(Expression<Func<T>> propertyNameExpression, ref T oldValue, T newValue);
        bool Set<TClass, TProp>(Expression<Func<TClass, TProp>> propertyNameExpression, ref TProp oldValue, TProp newValue);
        bool Set<T>(string propertyName, ref T oldValue, T newValue);

        void RaisePropertyChanged([CallerMemberName] string propertyName = null);
        void RaisePropertyChanged(Expression<Func<object>> propertyNameExpression);
        void RaisePropertyChanged<TClass>(Expression<Func<TClass, object>> propertyNameExpression);
        void RaisePropertyChanged(PropertyChangedEventArgs args);

        void RaiseAllPropertiesChanged();
    }
}

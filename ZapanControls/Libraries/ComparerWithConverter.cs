using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Data;

namespace ZapanControls.Libraries
{
    public class ComparerWithConverter : IComparer
    {
        private readonly IValueConverter _converter;
        private readonly ListSortDirection _direction;
        private readonly object _parameter;

        public ComparerWithConverter(IValueConverter converter, ListSortDirection direction, object parameter)
        {
            _converter = converter;
            _direction = direction;
            _parameter = parameter;
        }

        public int Compare(object x, object y)
        {
            object transx = _converter.Convert(x, typeof(string), _parameter, Thread.CurrentThread.CurrentCulture);
            object transy = _converter.Convert(y, typeof(string), _parameter, Thread.CurrentThread.CurrentCulture);

            return _direction == ListSortDirection.Ascending ? 
                Comparer.Default.Compare(transx, transy) 
                : Comparer.Default.Compare(transx, transy) * (-1);
        }
    }
}

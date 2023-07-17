using System.Windows;
using System.Windows.Controls;

namespace ZapanControls.Behaviours
{
    // Source : https://apaers.com/wpf-listview-access-gridviewcolumnheader-from-gridviewcolumn/
    public static class BoundGridViewColumn
    {
        /// <summary> 
        /// Synthetic attached property for wiring up GridViewColumn with it's GridViewColumnHeader.
        /// Serves for catching the moment when GridViewColumnHeader.Column has changed.
        /// </summary>
        public static readonly DependencyProperty BoundGridViewColumnProperty =
            DependencyProperty.RegisterAttached("BoundGridViewColumn", typeof(GridViewColumn), typeof(BoundGridViewColumn),
                new UIPropertyMetadata(null, BoundGridViewColumnPropertyChanged));

        private static void BoundGridViewColumnPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is GridViewColumnHeader gridViewColumnHeader) || gridViewColumnHeader.Column == null)
            {
                return;
            }
            // Set attached property Column.BoundGridViewColumnHeader
            SetBoundGridViewColumnHeader(gridViewColumnHeader.Column, gridViewColumnHeader);
        }

        /// <summary> 
        /// This property "attached" to GridViewColumn and contains GridViewColumnHeader instance
        /// </summary>
        public static readonly DependencyProperty BoundGridViewColumnHeaderProperty =
            DependencyProperty.RegisterAttached("BoundGridViewColumnHeader", typeof(GridViewColumnHeader),
                typeof(BoundGridViewColumn), new PropertyMetadata(null));

        public static GridViewColumnHeader GetBoundGridViewColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj?.GetValue(BoundGridViewColumnHeaderProperty);
        }

        public static void SetBoundGridViewColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj?.SetValue(BoundGridViewColumnHeaderProperty, value);
        }
    }
}

using System.Windows;
using ZapanControls.Controls.ResourceKeys;

namespace ZapanControls.Controls
{
    public sealed class GridView : System.Windows.Controls.GridView
    {
        #region Constructors
        static GridView()
        {
            ColumnHeaderContainerStyleProperty.OverrideMetadata(typeof(GridView),
                new FrameworkPropertyMetadata(Application.Current.TryFindResource(ListViewResourceKeys.ColumnHeaderStyleKey)));
        }

        public GridView()
        {

        }
        #endregion

        #region Overrides
        protected override object DefaultStyleKey => ListViewResourceKeys.GridViewStyleKey;
        protected override object ItemContainerDefaultStyleKey => ListViewResourceKeys.ItemContainerStyleKey;
        #endregion
    }
}

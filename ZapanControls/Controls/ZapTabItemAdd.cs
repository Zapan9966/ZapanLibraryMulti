using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZapanControls.Helpers;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    internal sealed class ZapTabItemAdd : TabItem
    {
        #region Commands

        /// <summary>
        /// Commande qui gère l'ajout d'un onglet
        /// </summary>
        public ICommand AddTabCommand { get; }

        //private void OnAddTabClick(RoutedEventArgs e)
        private void OnAddTabClick(object parameters)
        {
            ZapTabControl tc = VisualTreeHelpers.FindParent<ZapTabControl>(this);

            if (tc != null)
            {
                if (tc.SelectedItem == this)
                    tc.SelectedItem = tc.LastSelectedTab ?? tc.Items[tc.Items.Count - 1];

                if (tc.OnAddTab(parameters) is ZapTabItem newTab)
                {
                    tc.Items.Add(newTab);
                    newTab.Focus();

                    tc.SelectedItem = newTab;
                }
            }
        }

        #endregion

        /// <summary>
        /// Constructeur de la classe <see cref="ZapTabItemAdd"/>
        /// </summary>
        static ZapTabItemAdd()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapTabItemAdd), new FrameworkPropertyMetadata(typeof(ZapTabItemAdd)));
        }

        public ZapTabItemAdd()
        {
            AddTabCommand = new RelayCommand<object>(
                param => OnAddTabClick(param),
                param => true);
        }

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion

    }
}

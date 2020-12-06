using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /// <summary>
    /// Classe représentant un onglet qui peut être fermé.
    /// </summary>
    public sealed class ZapTabItem : TabItem
    {

        #region Dependancy Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabItem.IsClosable"/>.
        /// </summary>
        private static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
            "IsClosable", typeof(bool), typeof(ZapTabItem), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si l'onglet peut être fermé.
        /// </summary>
        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Commande qui gère la fermeture d'un onglet
        /// </summary>
        public ICommand CloseTabCommand { get; }

        private void OnCloseTab(RoutedEventArgs e)
        {
            if (Parent is ZapTabControl zapTabControl)
                if (zapTabControl.OnCloseTab(e ?? new RoutedEventArgs()))
                    zapTabControl.Items.Remove(this);

            if (e.RoutedEvent != null)
                e.Handled = true;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur de la classe <see cref="ZapTabItem"/>
        /// </summary>
        static ZapTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapTabItem), new FrameworkPropertyMetadata(typeof(ZapTabItem)));
        }

        public ZapTabItem()
        {
            CloseTabCommand = new RelayCommand<RoutedEventArgs>(
                param => OnCloseTab(param),
                param => true);
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion

    }
}

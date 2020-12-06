using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ZapanControls.Controls
{
    /// <summary>
    /// TabControl qui prend en charge l'ajout/suppression d'onglet.
    /// </summary>
    public sealed class ZapTabControl : TabControl
    {
        public object LastSelectedTab { get; private set; }

        #region Dependency Properties

        #region Add Tabs Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.CanAddItem"/>.
        /// </summary>
        private static readonly DependencyProperty CanAddItemProperty = DependencyProperty.Register(
            "CanAddItem", typeof(bool), typeof(ZapTabControl), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si des onglets peuvent être ajoutés/supprimés.
        /// </summary>
        public bool CanAddItem
        {
            get { return (bool)GetValue(CanAddItemProperty); }
            set { SetValue(CanAddItemProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.AddButtonTooltip"/>.
        /// </summary>
        private static readonly DependencyProperty AddButtonTooltipProperty = DependencyProperty.Register(
            "AddButtonTooltip", typeof(string), typeof(ZapTabControl), new FrameworkPropertyMetadata("Ajouter un onglet"));

        /// <summary>
        /// Obtient ou défini le texte de l'info-bulle du bouton Fermer.
        /// </summary>
        public string AddButtonTooltip
        {
            get { return (string)GetValue(AddButtonTooltipProperty); }
            set { SetValue(AddButtonTooltipProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.AddButtonHeader"/>.
        /// </summary>
        private static readonly DependencyProperty AddButtonHeaderProperty = DependencyProperty.Register(
            "AddButtonHeader", typeof(object), typeof(ZapTabControl), new FrameworkPropertyMetadata("Ê"));

        /// <summary>
        /// Obtient ou défini l'entête de l'onglet d'ajout.
        /// </summary>
        public object AddButtonHeader
        {
            get { return (object)GetValue(AddButtonHeaderProperty); }
            set { SetValue(AddButtonHeaderProperty, value); }
        }

        #endregion

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.BorderBrush"/>.
        /// </summary>
        private static readonly new DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.DarkOrange));

        /// <summary>
        /// Obtient ou défini la couleur de la bordure.
        /// </summary>
        public new Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        #region TabItem Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemBackground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.Register(
            "ItemBackground", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.DarkOrchid));

        /// <summary>
        /// Obtient ou défini la couleur de fond des onglets.
        /// </summary>
        public Brush ItemBackground
        {
            get { return (Brush)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemBorderBrush"/>.
        /// </summary>
        private static readonly DependencyProperty ItemBorderBrushProperty = DependencyProperty.Register(
            "ItemBorderBrush", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.Purple));

        /// <summary>
        /// Obtient ou défini la couleur de la bordure des onglets.
        /// </summary>
        public Brush ItemBorderBrush
        {
            get { return (Brush)GetValue(ItemBorderBrushProperty); }
            set { SetValue(ItemBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemForeground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemForegroundProperty = DependencyProperty.Register(
            "ItemForeground", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini la couleur de la police des onglets.
        /// </summary>
        public Brush ItemForeground
        {
            get { return (Brush)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        #region IsSelected Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemIsSelectedBackground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemIsSelectedBackgroundProperty = DependencyProperty.Register(
            "ItemIsSelectedBackground", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.Orange));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'onglet sélectionné.
        /// </summary>
        public Brush ItemIsSelectedBackground
        {
            get { return (Brush)GetValue(ItemIsSelectedBackgroundProperty); }
            set { SetValue(ItemIsSelectedBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemIsSelectedForeground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemIsSelectedForegroundProperty = DependencyProperty.Register(
            "ItemIsSelectedForeground", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini la couleur de la police de l'onglets sélectionné.
        /// </summary>
        public Brush ItemIsSelectedForeground
        {
            get { return (Brush)GetValue(ItemIsSelectedForegroundProperty); }
            set { SetValue(ItemIsSelectedForegroundProperty, value); }
        }

        #endregion

        #region MouseOver Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemMouseOverBackground"/>.
        /// </summary>
        public static readonly DependencyProperty ItemMouseOverBackgroundProperty = DependencyProperty.Register(
            "ItemMouseOverBackground", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.Purple));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'onglet lors du survole de la souris.
        /// </summary>
        public Brush ItemMouseOverBackground
        {
            get { return (Brush)GetValue(ItemMouseOverBackgroundProperty); }
            set { SetValue(ItemMouseOverBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemMouseOverBorderBrush"/>.
        /// </summary>
        public static readonly DependencyProperty ItemMouseOverBorderBrushProperty = DependencyProperty.Register(
            "ItemMouseOverBorderBrush", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.BlueViolet));

        /// <summary>
        /// Obtient ou défini la couleur de la bordure de l'onglet lors du survole de la souris.
        /// </summary>
        public Brush TabItemMouseOverBorderBrush
        {
            get { return (Brush)GetValue(ItemMouseOverBorderBrushProperty); }
            set { SetValue(ItemMouseOverBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapTabControl.ItemMouseOverForeground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemMouseOverForegroundProperty = DependencyProperty.Register(
            "ItemMouseOverForeground", typeof(Brush), typeof(ZapTabControl), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini la couleur de la police de l'onglet lors du survole de la souris.
        /// </summary>
        public Brush ItemMouseOverForeground
        {
            get { return (Brush)GetValue(ItemMouseOverForegroundProperty); }
            set { SetValue(ItemMouseOverForegroundProperty, value); }
        }

        #endregion

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Représente la méthode qui gère la validation de fermeture d'un onglet.
        /// </summary>
        /// <returns>Renvoi <see cref="True"/> si l'onglet doit être fermé, sinon <see cref="False"/></returns>
        public delegate bool CloseTabEventHandler(object sender, RoutedEventArgs e);

        /// <summary>
        /// Se produit lors de la validation de la fermeture d'un onglet.
        /// </summary>
        public event CloseTabEventHandler CloseTab;

        /// <summary>
        /// Méthode interne qui gère la validation de fermeture d'un onglet.
        /// </summary>
        /// <param name="e">Informations d'état et données d'évènement associés à l'évènement routé.</param>
        /// <returns>Renvoi <see cref="True"/> si la fenêtre doit être fermée, sinon <see cref="False"/></returns>
        internal bool OnCloseTab(RoutedEventArgs e)
        {
            bool result = true;

            if (CloseTab != null)
                result = CloseTab(this, e);

            if (e?.RoutedEvent != null)
                e.Handled = true;

            return result;
        }

        /// <summary>
        /// Représente la méthode qui gère l'ajount d'un onglet.
        /// </summary>
        /// <returns>Renvoi un objet dérivée de la classe <see cref="ZapTabItem"/>.</returns>
        public delegate ZapTabItem AddTabEventHandler(object sender, object parameters);

        /// <summary>
        /// Se produit lors de l'ajout d'un onglet.
        /// </summary>
        public event AddTabEventHandler AddTab;

        /// <summary>
        /// Méthode interne qui gère l'ajout d'un onglet.
        /// </summary>
        /// <param name="e">Informations d'état et données d'évènement associés à l'évènement routé.</param>
        /// <returns>Renvoi un objet dérivée de la classe <see cref="ZapTabItem"/>.</returns>
        internal ZapTabItem OnAddTab(object parameters)
        {
            ZapTabItem tab = new ZapTabItem();

            if (AddTab != null)
                tab = AddTab(this, parameters);

            return tab;
        }

        /// <summary>
        /// Se produit lorsque la collection est modifié.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> ItemsChanged;

        #endregion

        /// <summary>
        /// Constructeur de la classe <see cref="ZapTabControl"/>.
        /// </summary>
        static ZapTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(typeof(ZapTabControl)));
        }

        /// <summary>
        /// Méthode qui gère le changement d'onglet.
        /// </summary>
        /// <param name="sender">Object qui a généré le changement d'onglet.</param>
        /// <param name="e">Informations d'état et données d'évènement associés à l'évènement routé.</param>
        private void InternalZapTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LastSelectedTab = (e?.RemovedItems?.Count ?? 0) > 0 ? e.RemovedItems[0] : Items?[(Items?.Count ?? 0) > 0 ? Items.Count - 1 : 0];

            if ((e?.AddedItems?.Count ?? 0) > 0)
            {
                if (e.AddedItems[0] is ZapTabItemAdd)
                    SelectedItem = LastSelectedTab;
            }

            if (e?.RoutedEvent != null)
                e.Handled = true;
        }

        public void PerformAddTab(object parameters = null)
        {
            if (CanAddItem)
                if (Items.GetItemAt(0) is ZapTabItemAdd addTab)
                    addTab.AddTabCommand.Execute(parameters);
        }

        #region Overrides

        public override void OnApplyTemplate()
        {
            SetBinding(Control.BorderBrushProperty, new Binding("BorderBrush") { Source = this });

            SelectionChanged += InternalZapTabControl_SelectionChanged;

            base.OnApplyTemplate();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            ItemsChanged?.Invoke(this, e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (CanAddItem)
            {
                Items.Insert(0, new ZapTabItemAdd() { ToolTip = AddButtonTooltip, Header = AddButtonHeader });

                if (Items.Count > 0)
                    SelectedIndex = 1;
            }
            base.OnInitialized(e);
        }

        #endregion

    }
}

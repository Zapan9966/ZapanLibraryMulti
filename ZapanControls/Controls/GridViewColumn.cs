using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ZapanControls.Controls.ResourceKeys;

namespace ZapanControls.Controls
{
    /// <summary>
    /// Définition d'une colonne sur laquelle un champ de recherche est intégré.
    /// </summary>
    public sealed class GridViewColumn : System.Windows.Controls.GridViewColumn
    {
        private ListSortDirection? _sortDirection;

        #region Properties
        #region IsVisible
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="IsVisible"/>.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible", typeof(bool), typeof(GridViewColumn), 
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                new PropertyChangedCallback(OnIsVisibleChanged)));

        /// <summary>
        /// Obtient ou défini la valeur indiquant le type de recherche qui peut être effectué sur la colonne.
        /// </summary>
        public bool IsVisible { get => (bool)GetValue(IsVisibleProperty); set => SetValue(IsVisibleProperty, value); }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridViewColumn column && e.NewValue is bool value)
            {
                column.Width = !value ? 0 : double.NaN;
            }
        }
        #endregion

        #region SearchType
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="SearchType"/>.
        /// </summary>
        public static readonly DependencyProperty SearchTypeProperty = DependencyProperty.Register(
            "SearchType", typeof(ColumnSearchTypes), typeof(GridViewColumn), 
            new FrameworkPropertyMetadata(
                ColumnSearchTypes.None,
                FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                new PropertyChangedCallback(OnSearchTypeChanged)));

        /// <summary>
        /// Obtient ou défini la valeur indiquant le type de recherche qui peut être effectué sur la colonne.
        /// </summary>
        public ColumnSearchTypes SearchType 
        { 
            get => (ColumnSearchTypes)GetValue(SearchTypeProperty); 
            set => SetValue(SearchTypeProperty, value); 
        }

        private static void OnSearchTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridViewColumn column && e.NewValue is ColumnSearchTypes type)
            {
                object template = type switch
                {
                    ColumnSearchTypes.Date => Application.Current.TryFindResource(ListViewResourceKeys.HeaderTemplateDateKey),
                    ColumnSearchTypes.Text => Application.Current.TryFindResource(ListViewResourceKeys.HeaderTemplateTextKey),
                    ColumnSearchTypes.ComboBox => Application.Current.TryFindResource(ListViewResourceKeys.HeaderTemplateComboBoxKey),
                    ColumnSearchTypes.None => Application.Current.TryFindResource(ListViewResourceKeys.HeaderTemplateDefaultKey),
                    _ => null,
                };
                column.SetValue(HeaderTemplateProperty, template);
            }
        }
        #endregion

        #region ComboBoxSource
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ComboBoxSource"/>.
        /// </summary>
        private static readonly DependencyProperty ComboBoxSourceProperty = DependencyProperty.Register(
            "ComboBoxSource", typeof(string), typeof(GridViewColumn), 
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Obtient ou défini la propriété utilisée comme source pour la <see cref="ComboBox"/> de recherche.
        /// </summary>
        public string ComboBoxSource 
        { 
            get => (string)GetValue(ComboBoxSourceProperty); 
            set => SetValue(ComboBoxSourceProperty, value); 
        }
        #endregion

        #region ComboBoxDisplayMemberPath
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ComboBoxDisplayMemberPath"/>.
        /// </summary>
        private static readonly DependencyProperty ComboBoxDisplayMemberPathProperty = DependencyProperty.Register(
            "ComboBoxDisplayMemberPath", typeof(string), typeof(GridViewColumn), 
            new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Obtient ou défini le nom du champ pour afficher la valeur de la <see cref="ComboBox"/>.
        /// </summary>
        public string ComboBoxDisplayMemberPath 
        { 
            get => (string)GetValue(ComboBoxDisplayMemberPathProperty); 
            set => SetValue(ComboBoxDisplayMemberPathProperty, value); 
        }
        #endregion

        #region SortBinding
        /// <summary>
        /// Obtient ou défini le nom de la propriété utilisée pour trier la colonne.
        /// </summary>
        public Binding SortBinding { get; set; }
        #endregion

        #region SortDirection
        /// <summary>
        /// Obtient ou défini le sens de tri de la colonne.
        /// </summary>
        public ListSortDirection? SortDirection
        {
            get => _sortDirection;
            set
            {
                _sortDirection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SortDirection"));
            }
        }
        #endregion
        #endregion

        #region Constructors
        static GridViewColumn()
        {
            HeaderTemplateProperty.OverrideMetadata(
                typeof(GridViewColumn),
                new FrameworkPropertyMetadata(
                    Application.Current.TryFindResource(
                        ListViewResourceKeys.HeaderTemplateDefaultKey)));
        }

        /// <summary>
        /// Constructeur de la classe <see cref="GridViewColumn"/>.
        /// </summary>
        public GridViewColumn()
        { 
            
        }
        #endregion
    }
}

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZapanControls.Controls
{
    /// <summary>
    /// Définition d'une colonne sur laquelle un champ de recherche est intégré.
    /// </summary>
    public sealed class ZapGridViewColumn : GridViewColumn
    {
        private ListSortDirection? _sortDirection;

        #region Dependency Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapGridViewColumn.IsVisible"/>.
        /// </summary>
        private static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible", typeof(bool), typeof(ZapGridViewColumn), new FrameworkPropertyMetadata(true, (o, e) => 
            {
                if (e.NewValue is bool value)
                {
                    ZapGridViewColumn column = (ZapGridViewColumn)o;
                    column.Width = !value ? 0 : double.NaN;
                }
            }));

        /// <summary>
        /// Obtient ou défini la valeur indiquant le type de recherche qui peut être effectué sur la colonne.
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapGridViewColumn.SearchType"/>.
        /// </summary>
        private static readonly DependencyProperty SearchTypeProperty = DependencyProperty.Register(
            "SearchType", typeof(ColumnSearchType), typeof(ZapGridViewColumn), new FrameworkPropertyMetadata(ColumnSearchType.None));

        /// <summary>
        /// Obtient ou défini la valeur indiquant le type de recherche qui peut être effectué sur la colonne.
        /// </summary>
        public ColumnSearchType SearchType
        {
            get { return (ColumnSearchType)GetValue(SearchTypeProperty); }
            set { SetValue(SearchTypeProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapGridViewColumn.ComboBoxSource"/>.
        /// </summary>
        private static readonly DependencyProperty ComboBoxSourceProperty = DependencyProperty.Register(
            "ComboBoxSource", typeof(string), typeof(ZapGridViewColumn), new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Obtient ou défini la propriété utilisée comme source pour la <see cref="ComboBox"/> de recherche.
        /// </summary>
        public string ComboBoxSource
        {
            get { return (string)GetValue(ComboBoxSourceProperty); }
            set { SetValue(ComboBoxSourceProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapGridViewColumn.ComboBoxDisplayMemberPath"/>.
        /// </summary>
        private static readonly DependencyProperty ComboBoxDisplayMemberPathProperty = DependencyProperty.Register(
            "ComboBoxDisplayMemberPath", typeof(string), typeof(ZapGridViewColumn), new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Obtient ou défini le nom du champ pour afficher la valeur de la <see cref="ComboBox"/>.
        /// </summary>
        public string ComboBoxDisplayMemberPath
        {
            get { return (string)GetValue(ComboBoxDisplayMemberPathProperty); }
            set { SetValue(ComboBoxDisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Obtient ou défini le nom de la propriété utilisée pour trier la colonne.
        /// </summary>
        public Binding SortBinding { get; set; }

        /// <summary>
        /// Obtient ou défini le sens de tri de la colonne.
        /// </summary>
        public ListSortDirection? SortDirection
        {
            get { return _sortDirection; }
            set
            {
                _sortDirection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SortDirection"));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur de la classe <see cref="ZapGridViewColumn"/>.
        /// </summary>
        public ZapGridViewColumn()
        { }

        #endregion

    }
}

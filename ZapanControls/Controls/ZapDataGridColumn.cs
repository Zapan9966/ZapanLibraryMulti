using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ZapanControls.Controls
{
    public sealed class ZapDataGridColumn : DataGridColumn
    {
        #region Dependancy Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGridColumn.DataType"/>.
        /// </summary>
        public static readonly DependencyProperty DataTypeProperty = DependencyProperty.Register(
            "DataType", typeof(DataGridColumnType), typeof(ZapDataGridColumn), new PropertyMetadata(DataGridColumnType.Text));

        /// <summary>
        /// Obtient ou défini le type de données contenu dans la colonne.
        /// </summary>
        public DataGridColumnType DataType
        {
            get { return (DataGridColumnType)GetValue(DataTypeProperty); }
            set { SetValue(DataTypeProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGridColumn.ComboBoxSource"/>.
        /// </summary>
        public static readonly DependencyProperty ComboBoxSourceProperty = DependencyProperty.Register(
            "ComboBoxSource", typeof(string), typeof(ZapDataGridColumn), new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Obtient ou défini la propriété utilisée comme source pour la <see cref="ComboBox"/> de recherche.
        /// </summary>
        public string ComboBoxSource
        {
            get { return (string)GetValue(ComboBoxSourceProperty); }
            set { SetValue(ComboBoxSourceProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGridColumn.ComboBoxDisplayMemberPath"/>.
        /// </summary>
        public static readonly DependencyProperty ComboBoxDisplayMemberPathProperty = DependencyProperty.Register(
            "ComboBoxDisplayMemberPath", typeof(string), typeof(ZapDataGridColumn), new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Obtient ou défini le nom du champ pour afficher la valeur de la <see cref="ComboBox"/>.
        /// </summary>
        public string ComboBoxDisplayMemberPath
        {
            get { return (string)GetValue(ComboBoxDisplayMemberPathProperty); }
            set { SetValue(ComboBoxDisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGridColumn.CanFilterProperty"/>.
        /// </summary>
        public static readonly DependencyProperty CanFilterProperty = DependencyProperty.Register(
            "CanFilter", typeof(bool), typeof(ZapDataGridColumn), new PropertyMetadata(false));

        /// <summary>
        /// Obtient ou défini la valeur indiquant que la colonne peut être filtrée.
        /// </summary>
        public bool CanFilter
        {
            get { return (bool)GetValue(CanFilterProperty); }
            set { SetValue(CanFilterProperty, value); }
        }

        #endregion

        #region Properties

        public Binding Binding { get; set; }

        #endregion

        #region Constructors

        public ZapDataGridColumn()
        {
        }

        #endregion

        #region Overrides

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement elem;
            switch (DataType)
            {
                case DataGridColumnType.ComboBox:
                    elem = new ComboBox()
                    {
                        Margin = new Thickness(-2, -1, -2, -1),
                        DisplayMemberPath = ComboBoxDisplayMemberPath,
                        SelectedValuePath = ComboBoxDisplayMemberPath,
                    };
                    elem.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(ComboBoxSource)
                    {
                        RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = typeof(Window) }
                    });
                    elem.SetBinding(Selector.SelectedValueProperty, Binding);
                    break;
                default:
                    elem = new TextBox()
                    {
                        Margin = new Thickness(-2, -1, -2, -1)
                    };
                    elem.SetBinding(TextBox.TextProperty, Binding);
                    break;
            }

            return elem;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement elem = new TextBlock { Margin = new Thickness(2, 0, 2, 0) };
            elem.SetBinding(TextBlock.TextProperty, Binding);

            return elem;
        }

        #endregion

    }

    public enum DataGridColumnType
    {
        Text = 1,
        ComboBox = 2,
        DateTime = 3,
    }
}

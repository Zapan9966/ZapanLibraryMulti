using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ZapanControls.Controls;
using ZapanControls.Helpers;
using ZapanControls.Libraries;

namespace ZapanControls.DataTemplateSelectors
{
    public class ZapDataGrid : DataGrid, INotifyPropertyChanged, IDisposable
    {
        private readonly DeferredAction _deferredAction;

        private double _buttonColumnsActualWidth;
        private bool _disposed;
        private bool _isFiltering;
        private BackgroundWorker _worker;
        private ZapButton _btnRemoveFilters;
        private IEnumerable<dynamic> _fullItems;

        private Grid _loadingGrid;
        private ProgressBar _loadingProgress;
        private ZapLoadingIndicator _loadingIndicator;

        private ZapDataGridColumn _sortedColumn;
        private ListSortDirection? _sortDirection;

        private Dictionary<string, KeyValuePair<object, IValueConverter>> _filtersDictionary;

        #region Default Static Properties

        private static readonly SolidColorBrush _scrollBarThumbInnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#505050"));
        private static readonly SolidColorBrush _scrollBarDisabledButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE8EDF9"));
        private static readonly SolidColorBrush _rowMouseOverBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC99FF"));
        private static readonly SolidColorBrush _rowSelectionActiveBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9933FF"));
        //private static readonly SolidColorBrush _rowSelectionInactiveBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9933FF"));


        private static readonly GradientStopCollection _columnHeaderBackground_GradientStopCollectionDefault =
            new GradientStopCollection()
            {
                        new GradientStop() { Color = Brushes.DimGray.Color, Offset = 1 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#292C31"), Offset = 0.5 },
                        new GradientStop() { Color = Brushes.DimGray.Color, Offset = 0.15 }
            };

        private static readonly LinearGradientBrush _columnHeaderBackground_LinearGradientBrushDefault =
            new LinearGradientBrush(_columnHeaderBackground_GradientStopCollectionDefault)
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

        private static readonly GradientStopCollection _headerBackgroundMouseOver_GradientStopCollectionDefault =
            new GradientStopCollection()
            {
                        new GradientStop() { Color = Brushes.DarkViolet.Color, Offset = 1 },
                        new GradientStop() { Color = Brushes.Indigo.Color, Offset = 0.5 },
                        new GradientStop() { Color = Brushes.DarkViolet.Color, Offset = 0.15 }
            };

        private static readonly LinearGradientBrush _headerBackgroundMouseOver_LinearGradientBrushDefault =
            new LinearGradientBrush(_headerBackgroundMouseOver_GradientStopCollectionDefault)
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

        private static readonly GradientStopCollection _headerBackgroundIsPressed_GradientStopCollectionDefault =
            new GradientStopCollection()
            {
                        new GradientStop() { Color = Brushes.Indigo.Color, Offset = 1 },
                        new GradientStop() { Color = Brushes.DarkViolet.Color, Offset = 0.5 },
                        new GradientStop() { Color = Brushes.Indigo.Color, Offset = 0.15 }
            };

        private static readonly LinearGradientBrush _headerBackgroundIsPressed_LinearGradientBrushDefault =
            new LinearGradientBrush(_headerBackgroundIsPressed_GradientStopCollectionDefault)
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

        private static readonly GradientStopCollection _scrollBarBackground_GradientStopCollection =
            new GradientStopCollection()
            {
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#363636"), Offset = 0 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#505050"), Offset = 0.5 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#363636"), Offset = 1 },
            };

        private static readonly LinearGradientBrush _scrollBarBackground =
            new LinearGradientBrush(_scrollBarBackground_GradientStopCollection)
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.CanSelectAll"/>.
        /// </summary>
        private static readonly DependencyProperty CanSelectAllProperty = DependencyProperty.Register(
            "CanSelectAll", typeof(bool), typeof(ZapDataGrid), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant s'il est possible de sélectionner toutes les lignes. 
        /// </summary>
        public bool CanSelectAll
        {
            get { return (bool)GetValue(CanSelectAllProperty); }
            set { SetValue(CanSelectAllProperty, value); }
        }

        #region Header Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.ColumnHeaderBackground"/>.
        /// </summary>
        private static readonly DependencyProperty ColumnHeaderBackgroundProperty = DependencyProperty.Register(
            "ColumnHeaderBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_columnHeaderBackground_LinearGradientBrushDefault));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'en-tête des colonnes. 
        /// </summary>
        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(ColumnHeaderBackgroundProperty); }
            set { SetValue(ColumnHeaderBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.ColumnHeaderBorderBrush"/>.
        /// </summary>
        private static readonly DependencyProperty ColumnHeaderBorderBrushProperty = DependencyProperty.Register(
            "ColumnHeaderBorderBrush", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.Indigo));

        /// <summary>
        /// Obtient ou défini la couleur des bordures de l'en-tête des colonnes. 
        /// </summary>
        public Brush ColumnHeaderBorderBrush
        {
            get { return (Brush)GetValue(ColumnHeaderBorderBrushProperty); }
            set { SetValue(ColumnHeaderBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.ColumnHeaderForeground"/>.
        /// </summary>
        private static readonly DependencyProperty ColumnHeaderForegroundProperty = DependencyProperty.Register(
            "ColumnHeaderForeground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.FloralWhite));

        /// <summary>
        /// Obtient ou défini la couleur de la police de l'en-tête des colonnes. 
        /// </summary>
        public Brush ColumnHeaderForeground
        {
            get { return (Brush)GetValue(ColumnHeaderForegroundProperty); }
            set { SetValue(ColumnHeaderForegroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.ColumnHeaderBackgroundMouseOver"/>.
        /// </summary>
        private static readonly DependencyProperty ColumnHeaderBackgroundMouseOverProperty = DependencyProperty.Register(
            "ColumnHeaderBackgroundMouseOver", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_headerBackgroundMouseOver_LinearGradientBrushDefault));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'en-tête des colonnes quand la souris est au dessus. 
        /// </summary>
        public Brush ColumnHeaderBackgroundMouseOver
        {
            get { return (Brush)GetValue(ColumnHeaderBackgroundMouseOverProperty); }
            set { SetValue(ColumnHeaderBackgroundMouseOverProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.ColumnHeaderBackgroundIsPressed"/>.
        /// </summary>
        private static readonly DependencyProperty ColumnHeaderBackgroundIsPressedProperty = DependencyProperty.Register(
            "ColumnHeaderBackgroundIsPressed", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_headerBackgroundIsPressed_LinearGradientBrushDefault));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'en-tête des colonnes quand la souris est au dessus. 
        /// </summary>
        public Brush ColumnHeaderBackgroundIsPressed
        {
            get { return (Brush)GetValue(ColumnHeaderBackgroundIsPressedProperty); }
            set { SetValue(ColumnHeaderBackgroundIsPressedProperty, value); }
        }

        #endregion

        #region Row Properties

        #region Normal

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.RowBorderBrush"/>.
        /// </summary>
        private static readonly DependencyProperty RowBorderBrushProperty = DependencyProperty.Register(
            "RowBorderBrush", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Obtient ou défini la couleur des bordures des éléments du <see cref="System.Windows.Controls.ListView"/>. 
        /// </summary>
        public Brush RowBorderBrush
        {
            get { return (Brush)GetValue(RowBorderBrushProperty); }
            set { SetValue(RowBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.RowForeground"/>.
        /// </summary>
        private static readonly DependencyProperty RowForegroundProperty = DependencyProperty.Register(
            "RowForeground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini la couleur de la police des éléments du <see cref="System.Windows.Controls.ListView"/>. 
        /// </summary>
        public Brush RowForeground
        {
            get { return (Brush)GetValue(RowForegroundProperty); }
            set { SetValue(RowForegroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.RowBorderThickness"/>.
        /// </summary>
        private static readonly DependencyProperty RowBorderThicknessProperty = DependencyProperty.Register(
            "RowBorderThickness", typeof(Thickness), typeof(ZapDataGrid), new FrameworkPropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Obtient ou défini la taille des bordures des éléments du <see cref="System.Windows.Controls.ListView"/>. 
        /// </summary>
        public Thickness RowBorderThickness
        {
            get { return (Thickness)GetValue(RowBorderThicknessProperty); }
            set { SetValue(RowBorderThicknessProperty, value); }
        }

        #endregion

        #region MouseOver

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.RowMouseOverBackground"/>.
        /// </summary>
        private static readonly DependencyProperty RowMouseOverBackgroundProperty = DependencyProperty.Register(
            "RowMouseOverBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_rowMouseOverBackground));

        /// <summary>
        /// Obtient ou défini la couleur de fond des éléments du <see cref="System.Windows.Controls.ListView"/> lors du survol de la souris. 
        /// </summary>
        public Brush RowMouseOverBackground
        {
            get { return (Brush)GetValue(RowMouseOverBackgroundProperty); }
            set { SetValue(RowMouseOverBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.RowMouseOverForeground"/>.
        /// </summary>
        private static readonly DependencyProperty RowMouseOverForegroundProperty = DependencyProperty.Register(
            "RowMouseOverForeground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.BlueViolet));

        /// <summary>
        /// Obtient ou défini la couleur de la police des éléments du <see cref="System.Windows.Controls.ListView"/> lors du survol de la souris. 
        /// </summary>
        public Brush RowMouseOverForeground
        {
            get { return (Brush)GetValue(RowMouseOverForegroundProperty); }
            set { SetValue(RowMouseOverForegroundProperty, value); }
        }

        #endregion

        #region Selected

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.RowSelectionBackground"/>.
        /// </summary>
        private static readonly DependencyProperty RowSelectionBackgroundProperty = DependencyProperty.Register(
            "RowSelectionBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_rowSelectionActiveBackground));

        /// <summary>
        /// Obtient ou défini la couleur de fond des éléments sélectionnés lorsque le <see cref="System.Windows.Controls.ListView"/> a le focus. 
        /// </summary>
        public Brush RowSelectionBackground
        {
            get { return (Brush)GetValue(RowSelectionBackgroundProperty); }
            set { SetValue(RowSelectionBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.RowSelectionForeground"/>.
        /// </summary>
        private static readonly DependencyProperty RowSelectionForegroundProperty = DependencyProperty.Register(
            "RowSelectionForeground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini la couleur de la police des éléments sélectionnés du <see cref="System.Windows.Controls.ListView"/>. 
        /// </summary>
        public Brush RowSelectionForeground
        {
            get { return (Brush)GetValue(RowSelectionForegroundProperty); }
            set { SetValue(RowSelectionForegroundProperty, value); }
        }

        #endregion

        #endregion

        #region Cell Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.FocusedCellBackground"/>.
        /// </summary>
        private static readonly DependencyProperty FocusedCellBackgroundProperty = DependencyProperty.Register(
            "FocusedCellBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Obtient ou défini la couleur de fond de la cellule sélectionnée. 
        /// </summary>
        public Brush FocusedCellBackground
        {
            get { return (Brush)GetValue(FocusedCellBackgroundProperty); }
            set { SetValue(FocusedCellBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.FocusedCellBorderBrush"/>.
        /// </summary>
        private static readonly DependencyProperty FocusedCellBorderBrushProperty = DependencyProperty.Register(
            "FocusedCellBorderBrush", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.Indigo));

        /// <summary>
        /// Obtient ou défini la couleur de la bordure de la cellule sélectionnée. 
        /// </summary>
        public Brush FocusedCellBorderBrush
        {
            get { return (Brush)GetValue(FocusedCellBorderBrushProperty); }
            set { SetValue(FocusedCellBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.FocusedCellBorderThickness"/>.
        /// </summary>
        private static readonly DependencyProperty FocusedCellBorderThicknessProperty = DependencyProperty.Register(
            "FocusedCellBorderThickness", typeof(Thickness), typeof(ZapDataGrid), new FrameworkPropertyMetadata(new Thickness(1)));

        /// <summary>
        /// Obtient ou défini l'épaisseur de la bordure de la cellule sélectionnée. 
        /// </summary>
        public Thickness FocusedCellBorderThickness
        {
            get { return (Thickness)GetValue(FocusedCellBorderThicknessProperty); }
            set { SetValue(FocusedCellBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapDataGrid.FocusedCellForeground"/>.
        /// </summary>
        private static readonly DependencyProperty FocusedCellForegroundProperty = DependencyProperty.Register(
            "FocusedCellForeground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini l'épaisseur de la bordure de la cellule sélectionnée. 
        /// </summary>
        public Brush FocusedCellForeground
        {
            get { return (Brush)GetValue(FocusedCellForegroundProperty); }
            set { SetValue(FocusedCellForegroundProperty, value); }
        }

        #endregion

        #region ScrollBar

        private static readonly DependencyProperty ScrollBarBackgroundProperty = DependencyProperty.Register(
            "ScrollBarBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_scrollBarBackground));

        public Brush ScrollBarBackground
        {
            get { return (Brush)GetValue(ScrollBarBackgroundProperty); }
            set { SetValue(ScrollBarBackgroundProperty, value); }
        }

        #region ScrollBar Thumb properties

        private static readonly DependencyProperty ScrollBarThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarThumbInnerBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_scrollBarThumbInnerBackground));

        public Brush ScrollBarThumbInnerBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbInnerBackgroundProperty); }
            set { SetValue(ScrollBarThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBackgroundProperty = DependencyProperty.Register(
            "ScrollBarThumbBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ScrollBarThumbBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbBackgroundProperty); }
            set { SetValue(ScrollBarThumbBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarThumbBorderBrush", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(null));

        public Brush ScrollBarThumbBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarThumbBorderBrushProperty); }
            set { SetValue(ScrollBarThumbBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarThumbBorderThickness", typeof(Thickness), typeof(ZapDataGrid), new FrameworkPropertyMetadata(null));

        public Thickness ScrollBarThumbBorderThickness
        {
            get { return (Thickness)GetValue(ScrollBarThumbBorderThicknessProperty); }
            set { SetValue(ScrollBarThumbBorderThicknessProperty, value); }
        }

        #endregion

        #region ScrollBar Buttons properties

        private static readonly DependencyProperty ScrollBarButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarButtonBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ScrollBarButtonBackgroundProperty); }
            set { SetValue(ScrollBarButtonBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarButtonBorderBrush", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(null));

        public Brush ButtonBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarButtonBorderBrushProperty); }
            set { SetValue(ScrollBarButtonBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarButtonBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarButtonBorderThickness", typeof(double), typeof(ZapDataGrid), new FrameworkPropertyMetadata(null));

        public double ButtonBorderThickness
        {
            get { return (double)GetValue(ScrollBarButtonBorderThicknessProperty); }
            set { SetValue(ScrollBarButtonBorderThicknessProperty, value); }
        }

        #endregion

        #region ScrollBar Disabled properties

        private static readonly DependencyProperty ScrollBarDisabledThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarDisabledThumbInnerBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_scrollBarDisabledButtonBackground));

        public Brush ScrollBarDisabledThumbInnerBackground
        {
            get { return (Brush)GetValue(ScrollBarDisabledThumbInnerBackgroundProperty); }
            set { SetValue(ScrollBarDisabledThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBackground", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(_scrollBarDisabledButtonBackground));

        public Brush ScrollBarDisabledButtonBackground
        {
            get { return (Brush)GetValue(ScrollBarDisabledButtonBackgroundProperty); }
            set { SetValue(ScrollBarDisabledButtonBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBorderBrush", typeof(Brush), typeof(ZapDataGrid), new FrameworkPropertyMetadata(null));

        public Brush ScrollBarDisabledButtonBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarDisabledButtonBorderBrushProperty); }
            set { SetValue(ScrollBarDisabledButtonBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBorderThickness", typeof(double), typeof(ZapDataGrid), new FrameworkPropertyMetadata(null));

        public double ScrollBarDisabledButtonBorderThickness
        {
            get { return (double)GetValue(ScrollBarDisabledButtonBorderThicknessProperty); }
            set { SetValue(ScrollBarDisabledButtonBorderThicknessProperty, value); }
        }

        #endregion

        #endregion

        #endregion

        #region Observable Properties

        public double ButtonColumnsActualWidth
        {
            get { return _buttonColumnsActualWidth; }
            private set { Set(ref _buttonColumnsActualWidth, value); }
        }

        public bool IsFiltering
        {
            get { return _isFiltering; }
            private set { Set(ref _isFiltering, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Commande qui traite le clique sur l'entête des lignes
        /// </summary>
        public ICommand ToggleButtonCommand { get; }

        private static void ToggleButtonClick(ZapToggleButton toggleButton)
        { }

        /// <summary>
        /// Commande qui gère la suppression de la date d'un champ de filtrage de date
        /// </summary>
        public ICommand RemoveDateCommand { get; }

        private static void RemoveDateClick(DatePicker datePicker)
        {
            if (datePicker != null)
                datePicker.SelectedDate = null;
        }

        /// <summary>
        /// Commande qui gère la suppression de tous les filtres
        /// </summary>
        public ICommand RemoveFiltersCommand { get; }

        private void RemoveFiltersClick()
        {
            if (VisualTreeHelpers.FindChild(this, "PART_ColumnHeadersPresenter") is DataGridColumnHeadersPresenter headerPresenter)
            {
                Parallel.ForEach(VisualTreeHelpers.FindVisualChildren<Control>(headerPresenter), ctrl => 
                {
                    if (ctrl.Name == "search")
                    {
                        if (ctrl is DatePicker dp)
                        {
                            if (dp.SelectedDate.HasValue)
                                dp.SelectedDate = null;
                        }
                        else if (ctrl is ComboBox cb)
                        {
                            if (cb.SelectedIndex > -1)
                                cb.SelectedIndex = -1;
                        }
                        else if (ctrl is TextBox tb)
                        {
                            if (!string.IsNullOrEmpty(tb.Text))
                                tb.Text = string.Empty;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Commande qui gère les changements dans les champs de filtrage
        /// </summary>
        public ICommand FilterChangedCommand { get; }

        private void OnFilterChanged()
        {
            _deferredAction.Defer(TimeSpan.FromMilliseconds(350));
        }

        /// <summary>
        /// Commande qui gère le clique sur l'entête des colonnes
        /// </summary>
        public ICommand ColumnHeaderClickCommand { get; }

        private static void OnColumnHeaderClick(DataGridColumnHeader header)
        { }

        /// <summary>
        /// Commande qui gère le double click sur une ligne
        /// </summary>
        public ICommand RowDoubleClickCommand { get; }

        private void OnRowDoubleClick(DataGridRow row)
        {
            if (IsReadOnly)
                RowMouseDoubleClick?.Invoke(row, new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = MouseDoubleClickEvent,
                    Source = this,
                });
        }

        #endregion

        #region Events

        /// <summary>
        /// Se produit lors du double clique sur une ligne du <see cref="ZapDataGrid"/>.
        /// </summary>
        public event MouseButtonEventHandler RowMouseDoubleClick;

        /// <summary>
        /// Se produit avant le chargement des données suite à une recherche.
        /// </summary>
        public event EventHandler<RunWorkerCompletedEventArgs> BeforeFilteringFinished;

        /// <summary>
        /// Se produit après le chargement des données suite à une recherche.
        /// </summary>
        public event EventHandler<RunWorkerCompletedEventArgs> AfterFilteringFinished;

        /// <summary>
        /// Représente la méthode qui gère l'évènement de filtrage des éléments.
        /// </summary>
        /// <returns>Renvoi le <see cref="BackgroundWorker"/> utilisé pour effectuer le filtrage des éléments.</returns>
        public delegate BackgroundWorker FilteringEventHandler();

        /// <summary>
        /// Se produit lors du filtrage des éléments
        /// </summary>
        public event FilteringEventHandler Filtering;

        /// <summary>
        /// Méthode interne gère la construction du <see cref="BackgroundWorker"/> utilisé pour effectuer le filtrage des éléments.
        /// </summary>
        /// <returns>Renvoi le <see cref="BackgroundWorker"/> utilisé pour effectuer le filtrage des éléments.</returns>
        internal BackgroundWorker OnFiltering()
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
                while (_worker.IsBusy)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new Action(delegate { System.Threading.Thread.Sleep(10); }));
                }
            }

            if (Filtering != null)
            {
                _worker = Filtering();
                _worker.WorkerSupportsCancellation = true;

                Filtering().CancelAsync();
                while (Filtering().IsBusy)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new Action(delegate { System.Threading.Thread.Sleep(10); }));
                }
            }
            else if (_worker == null)
            {
                _worker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                _worker.DoWork += Worker_DoWork;
                _worker.ProgressChanged += Worker_ProgressChanged;
            }
            _worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            return _worker;
        }

        /// <summary>
        /// Représente la méthode qui gère le filtrage avancé.
        /// </summary>
        /// <param name="filtersDictionary">Dictionnaire contenant les éléments de filtrage.</param>
        public delegate void AdvancedFilteringEventHandler(ref Dictionary<string, KeyValuePair<object, IValueConverter>> filtersDictionary);

        /// <summary>
        /// Se produit lorsque la construction du dictionnaire de filtrage est terminé.
        /// </summary>
        public event AdvancedFilteringEventHandler AdvancedFiltering;

        #endregion

        #region Constructors

        static ZapDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapDataGrid), new FrameworkPropertyMetadata(typeof(ZapDataGrid)));
        }

        public ZapDataGrid()
        {
            ToggleButtonCommand = new RelayCommand<ZapToggleButton>(
                param => ToggleButtonClick(param),
                param => true);

            RemoveDateCommand = new RelayCommand<DatePicker>(
                param => RemoveDateClick(param),
                param => true);

            RemoveFiltersCommand = new RelayCommand(
                RemoveFiltersClick,
                true);

            FilterChangedCommand = new RelayCommand(
                OnFilterChanged,
                true);

            ColumnHeaderClickCommand = new RelayCommand<DataGridColumnHeader>(
                param => OnColumnHeaderClick(param),
                param => true);

            RowDoubleClickCommand = new RelayCommand<DataGridRow>(
                param => OnRowDoubleClick(param),
                param => true);

            this.Loaded += ZapDataGrid_Loaded;
            this.Sorting += ZapDataGrid_Sorting;

            _deferredAction = DeferredAction.Create(() => FilterDataGrid());

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Méthode qui gère le chargement du <see cref="ZapDataGrid"/>.
        /// </summary>
        private void ZapDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (VisualTreeHelpers.GetTemplateChildByName(this, "ButtonsColumn") is Grid buttonsColumnGrid)
                ButtonColumnsActualWidth = buttonsColumnGrid.ActualWidth;

            if (VisualTreeHelpers.FindChild(this, "PART_ColumnHeadersPresenter") is DataGridColumnHeadersPresenter headerPresenter)
            {
                IEnumerable<Control> controls = VisualTreeHelpers.FindVisualChildren<Control>(headerPresenter);

                if (controls.Any(c => c.Name == "search"))
                    _btnRemoveFilters.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Méthode qui gère le tri des colonnes.
        /// </summary>
        private void ZapDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column is ZapDataGridColumn column)
            {
                IValueConverter converter = null;
                if (column.Binding != null)
                {
                    converter = column.Binding.Converter;
                    _ = column.Binding.ConverterParameter;
                }

                if (converter != null)
                {
                    e.Handled = true;
                    ListSortDirection direction = column.SortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                    object items = CollectionHelpers.SortWithConverter(ItemsSource, column.Binding, direction);
                    VisualTreeHelpers.UpdateBinding(this, ItemsSourceProperty, items);

                    column.SortDirection = direction;
                }

                _sortedColumn = column;
                _sortDirection = column.SortDirection;
            }
        }

        #endregion

        #region Filter Methods

        public void FilterDataGrid(bool forceLoadingIndicatorStyle = false)
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
                while (_worker.IsBusy)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new Action(delegate { System.Threading.Thread.Sleep(100); }));
                }
            }
            ToggleLoadingScreen(forceLoadingIndicatorStyle);

            _filtersDictionary = new Dictionary<string, KeyValuePair<object, IValueConverter>>();

            IEnumerable<DataGridColumnHeader> headers = VisualTreeHelpers.FindVisualChildren<DataGridColumnHeader>(this);

            foreach (DataGridColumnHeader header in VisualTreeHelpers.FindVisualChildren<DataGridColumnHeader>(this))
            {
                if (string.IsNullOrEmpty(header?.Content?.ToString())) continue;

                if (header.Column is ZapDataGridColumn column)
                {
                    string columnName = column.Binding.Path.Path;

                    DependencyObject dpo = VisualTreeHelpers.FindChild(header, "search");

                    // Vérification de la Textbox
                    if (dpo is TextBox tb)
                    {
                        if (!string.IsNullOrEmpty(tb.Text))
                        {
                            if (string.IsNullOrEmpty(tb.Text)) continue;

                            if (_filtersDictionary.ContainsKey(columnName))
                            {
                                if (_filtersDictionary[columnName].Value == column.Binding.Converter)
                                    _filtersDictionary.Remove(columnName);
                                else
                                    columnName += _filtersDictionary.Where(f => f.Key == columnName).Count() + 1;
                            }
                            _filtersDictionary.Add(columnName, new KeyValuePair<object, IValueConverter>(tb.Text, column.Binding.Converter));
                        }
                        else
                            _filtersDictionary.Remove(columnName);
                    }
                    // Vérification de la ComboBox
                    else if (dpo is ComboBox cb)
                    {
                        if (!string.IsNullOrEmpty(cb.SelectedValue?.ToString()))
                        {
                            if (string.IsNullOrEmpty(cb?.SelectedValue?.ToString())) continue;

                            if (_filtersDictionary.ContainsKey(columnName))
                                _filtersDictionary.Remove(columnName);

                            _filtersDictionary.Add(columnName, new KeyValuePair<object, IValueConverter>(cb.SelectedValue, column.Binding.Converter));
                        }
                        else
                            _filtersDictionary.Remove(columnName);
                    }
                    // Vérification du DatePicker
                    if (dpo is DatePicker dp)
                    {
                        if (dp.SelectedDate.HasValue)
                        {
                            if (_filtersDictionary.ContainsKey(columnName))
                                _filtersDictionary.Remove(columnName);

                            _filtersDictionary.Add(columnName, new KeyValuePair<object, IValueConverter>(dp.SelectedDate?.ToString("dd/MM/yyyy"), column.Binding.Converter));
                        }
                        else
                            _filtersDictionary.Remove(columnName);
                    }
                }
            }

            IsFiltering = true;
            AdvancedFiltering?.Invoke(ref _filtersDictionary);
            OnFiltering().RunWorkerAsync(new List<object>() { _filtersDictionary, true });
        }

        /// <summary>
        /// Méthode qui gère l'affichage de l'indicateur de progression du filtrage des éléments.
        /// </summary>
        /// <param name="forceLoadingIndicatorStyle">Permet de forcer l'affichage du chargement sans barre de progression</param>
        public void ToggleLoadingScreen(bool forceLoadingIndicatorStyle = false)
        {
            _loadingGrid.Visibility = _loadingGrid.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            _loadingProgress.Visibility = Filtering == null && !forceLoadingIndicatorStyle ? Visibility.Visible : Visibility.Collapsed;
            _loadingIndicator.Visibility = Filtering == null && !forceLoadingIndicatorStyle ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Méthode qui gère le filtrage des éléments en tâche de fond.
        /// </summary>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            List<dynamic> items = _filtersDictionary.Count > 0 ? new List<dynamic>() : new List<dynamic>(_fullItems);

            Parallel.ForEach(_fullItems, (item, state) =>
            {
                if (_worker.CancellationPending)
                    state.Break();

                List<bool> fieldsValidation = new List<bool>();

                foreach (KeyValuePair<string, KeyValuePair<object, IValueConverter>> filter in _filtersDictionary)
                {
                    if (_worker.CancellationPending)
                        state.Break();

                    if (filter.Value.Key != null)
                    {
                        if (!string.IsNullOrEmpty(filter.Value.Key.ToString()))
                        {
                            PropertyDescriptor property = TypeDescriptor.GetProperties(item).Find(Regex.Replace(filter.Key, "[0-9]", string.Empty), true);
                            if (property != null)
                            {
                                string itemValue = string.Empty;
                                if (filter.Value.Value == null)
                                    itemValue = property.GetValue(item) != null ? property.GetValue(item).ToString() : string.Empty;
                                else
                                    itemValue = filter.Value.Value.Convert(property.GetValue(item), typeof(String), null, CultureInfo.CurrentCulture).ToString();

                                fieldsValidation.Add(Regex.IsMatch(itemValue, filter.Value.Key.ToString(), RegexOptions.IgnoreCase));
                            }
                        }
                    }
                }

                if (!fieldsValidation.All(v => v == true))
                    items.Add(item);

                System.Threading.Thread.Sleep(1);

                _worker.ReportProgress(i);
                i++;
            });

            if (_worker.CancellationPending)
                e.Cancel = true;
            else
                e.Result = CollectionHelpers.EnumerableDynamicCast(items, ItemsSource);
        }

        /// <summary>
        /// Méthode qui gère la progression du filtrage des éléments en tâche de fond.
        /// </summary>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _loadingProgress.Value = e.ProgressPercentage * 100 / _fullItems.Count();
        }

        /// <summary>
        /// Méthode qui gère la fin du filtrage des élements en tâche de fond.
        /// </summary>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                BeforeFilteringFinished?.Invoke(this, e);

                if (e.Result != null)
                {
                    object items = e.Result;

                    // Récupération du binding de la dernière colonne triée
                    if (_sortedColumn?.Binding is Binding binding)
                    {
                        // Si le binding de la colonne à un convertisseur
                        if (binding?.Converter != null)
                            items = CollectionHelpers.SortWithConverter((IEnumerable)items, binding, _sortDirection);
                    }

                    VisualTreeHelpers.UpdateBinding(this, ItemsSourceProperty, items);

                    if (_sortedColumn != null)
                        _sortedColumn.SortDirection = _sortDirection;
                }
                AfterFilteringFinished?.Invoke(this, e);
            }
            ToggleLoadingScreen();
            IsFiltering = false;
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            // Application du template des entêtes
            foreach (DataGridColumn column in this.Columns)
            {
                string template = string.Empty;
                string templateFormat = @"<DataTemplate>
                    <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"" Margin=""0"">
                        <ContentPresenter x:Name=""headerContent"" Content=""{{TemplateBinding Content}}"" />
                        <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                        <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                    </StackPanel>
                    <DataTemplate.Triggers>
                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Ascending"">
                            <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                        </DataTrigger>
                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Descending"">
                            <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                        </DataTrigger>
                    </DataTemplate.Triggers>
                </DataTemplate>";

                if (column is ZapDataGridColumn col)
                {
                    if (string.IsNullOrEmpty(col.SortMemberPath))
                        col.SortMemberPath = col.Binding.Path.Path;

                    if (col.CanFilter)
                    {
                        switch (col.DataType)
                        {
                            case DataGridColumnType.Text:
                                templateFormat = @"<DataTemplate>
                                    <StackPanel x:Name=""panel"" Margin=""0"">
                                        <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"" Margin=""0,0,0,2"">
                                            <ContentPresenter x:Name=""headerContent"" Content=""{{TemplateBinding Content}}"" />
                                            <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                                  Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                                            <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                                  Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                                        </StackPanel>
                                        <TextBox x:Name=""search"" Visibility=""Visible"">
                                            <i:Interaction.Triggers>
                                                <i:EventTrigger EventName=""TextChanged"">
                                                    <i:InvokeCommandAction Command=""{{Binding FilterChangedCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapDataGrid}}}}}}"" />
                                                </i:EventTrigger>
                                            </i:Interaction.Triggers>
                                        </TextBox>
                                    </StackPanel>
                                    <DataTemplate.Triggers>
                                        <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value="""">
                                            <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />
                                        </DataTrigger>
                                        <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value=""{{x:Null}}"">
                                            <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />
                                        </DataTrigger>
                                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Ascending"">
                                            <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                                        </DataTrigger>
                                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Descending"">
                                            <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                                        </DataTrigger>
                                    </DataTemplate.Triggers>
                                </DataTemplate>";

                                template = String.Format(templateFormat);
                                break;
                            case DataGridColumnType.ComboBox:
                                if (!string.IsNullOrEmpty(col.ComboBoxSource))
                                {
                                    templateFormat = @"<DataTemplate>
                                        <StackPanel Margin=""0"">
                                            <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"" Margin=""0,0,0,2"">
                                                <ContentPresenter x:Name=""headerContent"" Content=""{{TemplateBinding Content}}"" />
                                                <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                                        Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                                                <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                                        Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                                            </StackPanel>
                                            <ComboBox x:Name=""search"" Visibility=""Visible"" Padding=""4,1,1,1"" DisplayMemberPath=""{0}"" SelectedValuePath=""{0}""
                                                      ItemsSource=""{{Binding {1}, RelativeSource={{RelativeSource AncestorType={{x:Type Window}}}}}}"">
                                                <i:Interaction.Triggers>
                                                    <i:EventTrigger EventName=""SelectionChanged"">
                                                        <i:InvokeCommandAction Command=""{{Binding FilterChangedCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapDataGrid}}}}}}"" />
                                                    </i:EventTrigger>
                                                </i:Interaction.Triggers>
                                            </ComboBox>
                                        </StackPanel>
                                        <DataTemplate.Triggers>             
                                            <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value="""">                
                                                <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />                     
                                            </DataTrigger>                     
                                            <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value=""{{x:Null}}"">                
                                                <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />                     
                                            </DataTrigger>        
                                            <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Ascending"">
                                                <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                                            </DataTrigger>
                                            <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Descending"">
                                                <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                                            </DataTrigger>
                                        </DataTemplate.Triggers>                     
                                    </DataTemplate>";

                                    template = String.Format(templateFormat, col.ComboBoxDisplayMemberPath, col.ComboBoxSource);
                                }
                                else
                                {
                                    // Application du template par défaut
                                    template = String.Format(templateFormat);
                                }
                                break;
                            case DataGridColumnType.DateTime:
                                templateFormat = @"<DataTemplate>
                                    <StackPanel Margin=""0"">
                                        <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"" Margin=""0,0,0,2"">
                                            <ContentPresenter x:Name=""headerContent"" Content=""{{TemplateBinding Content}}"" />
                                            <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                                  Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                                            <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""  
                                                  Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                                        </StackPanel>
                                        <DockPanel Visibility=""Visible"">
                                            <z:ZapButtonFlat x:Name=""clearDatePickerButton""
                                                             FontFamily=""Webdings""
                                                             Content=""r""
                                                             Foreground=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}""     
                                                             MouseOverBackground=""{{x:Null}}""
                                                             ToolTip=""Supprimer la date""
                                                             Visibility=""Visible""
                                                             Command=""{{Binding RemoveDateCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapDataGrid}}}}}}""   
                                                             CommandParameter=""{{Binding ElementName=search}}"" />
                                            <DatePicker x:Name=""search"" BorderBrush=""{{x:Null}}"" BorderThickness=""0"" Visibility=""Visible"">
                                                <DatePicker.Resources>
                                                    <Style TargetType=""{{x:Type DatePickerTextBox}}"" >
                                                        <Setter Property=""Control.Template"">
                                                            <Setter.Value>
                                                                <ControlTemplate TargetType=""{{x:Type DatePickerTextBox}}"">           
                                                                    <TextBox x:Name=""PART_TextBox""
                                                                                Text=""{{Binding Path=SelectedDate, RelativeSource={{RelativeSource AncestorType={{x:Type DatePicker}}}}, 
                                                                                        StringFormat='dd/MM/yyyy'}}""
                                                                                IsReadOnly=""True""
                                                                                HorizontalContentAlignment=""Center"" />
                                                                </ControlTemplate>
                                                            </Setter.Value>
                                                        </Setter>
                                                    </Style>
                                                </DatePicker.Resources>
                                                <i:Interaction.Triggers>
                                                    <i:EventTrigger EventName=""SelectedDateChanged"">
                                                        <i:InvokeCommandAction Command=""{{Binding FilterChangedCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapDataGrid}}}}}}"" />
                                                    </i:EventTrigger>
                                                </i:Interaction.Triggers>
                                            </DatePicker>
                                        </DockPanel>
                                    </StackPanel>
                                    <DataTemplate.Triggers>
                                        <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value="""" >   
                                               <Setter TargetName=""clearDatePickerButton"" Property=""Visibility"" Value=""Collapsed"" />        
                                               <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />             
                                        </DataTrigger>             
                                        <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value=""{{x:Null}}"" >                
                                            <Setter TargetName=""clearDatePickerButton"" Property=""Visibility"" Value=""Collapsed"" />                     
                                            <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />                          
                                        </DataTrigger>         
                                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Ascending"">
                                            <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                                        </DataTrigger>
                                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type DataGridColumnHeader}}}}}}"" Value=""Descending"">
                                            <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                                        </DataTrigger>
                                    </DataTemplate.Triggers>                          
                                </DataTemplate>";

                                template = string.Format(templateFormat);
                                break;
                        }
                    }
                    else
                    {
                        // Application du template par défaut
                        template = string.Format(templateFormat);
                    }

                    ParserContext context = new ParserContext { XamlTypeMapper = new XamlTypeMapper(Array.Empty<string>()) };
                    context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                    context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                    context.XmlnsDictionary.Add("i", "clr-namespace:Microsoft.Xaml.Behaviors;assembly=Microsoft.Xaml.Behaviors");
                    context.XmlnsDictionary.Add("z", "http://schemas.zapan.com/wpf/controls/2020");

                    col.HeaderTemplate = XamlReader.Parse(template, context) as DataTemplate;
                }
            }

            _loadingGrid = (Grid)VisualTreeHelpers.FindChild(this, "loadingGrid");
            _loadingProgress = (ProgressBar)VisualTreeHelpers.FindChild(this, "loadingProgress");
            _loadingIndicator = (ZapLoadingIndicator)VisualTreeHelpers.FindChild(this, "loadingIndicator");

            _btnRemoveFilters = (ZapButton)VisualTreeHelpers.FindChild(this, "RemoveFiltersButton");

            base.OnApplyTemplate();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (!IsFiltering)
                _fullItems = new AsyncObservableCollection<dynamic>((IEnumerable<dynamic>)ItemsSource);
        }

        protected override void OnAutoGeneratedColumns(EventArgs e)
        {
            base.OnAutoGeneratedColumns(e);
        }

        protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
        {
            base.OnAutoGeneratingColumn(e);
        }

        protected override void OnRowDetailsVisibilityChanged(DataGridRowDetailsEventArgs e)
        {
            base.OnRowDetailsVisibilityChanged(e);
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        /// <summary>Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        private bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] String propertyName = null)
        {
            return Set(propertyName, ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <typeparam name="T">The type of the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        private bool Set<T>(Expression<Func<T>> propertyNameExpression, ref T oldValue, T newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <typeparam name="TProp">The type of the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        private bool Set<TClass, TProp>(Expression<Func<TClass, TProp>> propertyNameExpression, ref TProp oldValue,
            TProp newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        private bool Set<T>(String propertyName, ref T oldValue, T newValue)
        {
            if (Equals(oldValue, newValue))
                return false;

            oldValue = newValue;
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
            return true;
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyName">The property name. </param>
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        private void RaisePropertyChanged(Expression<Func<object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        private void RaisePropertyChanged<TClass>(Expression<Func<TClass, object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="args">The arguments. </param>
        private void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>Raises the property changed event for all properties (string.Empty). </summary>
        private void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(string.Empty));
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _worker?.Dispose();
                _deferredAction?.Dispose();
            }

            _disposed = true;
        }

    }
}

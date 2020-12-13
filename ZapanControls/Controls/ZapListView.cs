using System;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ZapanControls.Helpers;
using ZapanControls.Libraries;
using Ctrl = System.Windows.Controls;

namespace ZapanControls.Controls
{
    public sealed class ZapListView : ListView, INotifyPropertyChanged, IDisposable
    {
        private static readonly ResourceDictionary _dict = new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/ZapanControls;component/Themes/ZapListView.xaml")
        };
        private readonly DeferredAction _deferredAction;

        private bool _disposed;
        private bool _isFiltering;
        private BackgroundWorker _worker;
        private ZapButtonFlat _btnRemoveFilters;
        private Grid _loadingGrid;
        private ProgressBar _loadingProgress;
        private ZapLoadingIndicator _loadingIndicator;
        private Dictionary<string, KeyValuePair<object, IValueConverter>> _filtersDictionary;

        private ZapGridViewColumn _sortedColumn;
        private ListSortDirection? _sortDirection;

        public Dictionary<string, KeyValuePair<object, IValueConverter>> FiltersDictionary
        {
            get { return _filtersDictionary; }
            private set { Set(ref _filtersDictionary, value); }
        }

        public bool IsFiltering
        {
            get { return _isFiltering; }
            set { Set(ref _isFiltering, value); }
        }

        public AsyncObservableCollection<dynamic> Alltems { get; private set; }

        #region Default Static Properties

        //private static readonly SolidColorBrush _backgroundProperty = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#505050"));
        private static readonly SolidColorBrush _disabledBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE8EDF9"));
        private static readonly SolidColorBrush _itemMouseOverBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CC99FF"));
        private static readonly SolidColorBrush _itemSelectionActiveBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9933FF"));
        private static readonly SolidColorBrush _itemSelectionInactiveBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9933FF"));
        private static readonly SolidColorBrush _scrollBarThumbInnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#505050"));
        private static readonly SolidColorBrush _scrollBarDisabledButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE8EDF9"));

        private static readonly GradientStopCollection _headerBackground_GradientStopCollectionDefault =
            new GradientStopCollection()
            {
                        new GradientStop() { Color = Brushes.DimGray.Color, Offset = 1 },
                        new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#292C31"), Offset = 0.5 },
                        new GradientStop() { Color = Brushes.DimGray.Color, Offset = 0.15 }
            };

        private static readonly LinearGradientBrush _headerBackground_LinearGradientBrushDefault =
            new LinearGradientBrush(_headerBackground_GradientStopCollectionDefault)
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

        #region ListView Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.IsSearchAsync"/>.
        /// </summary>
        private static readonly DependencyProperty IsSearchAsyncProperty = DependencyProperty.Register(
            "IsSearchAsync", typeof(bool), typeof(ZapListView), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si la recherche dans le listview est asynchrone. 
        /// </summary>
        public bool IsSearchAsync
        {
            get { return (bool)GetValue(IsSearchAsyncProperty); }
            set { SetValue(IsSearchAsyncProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.IsAutoResizeColumns"/>.
        /// </summary>
        private static readonly DependencyProperty IsAutoResizeColumnsProperty = DependencyProperty.Register(
            "IsAutoResizeColumns", typeof(bool), typeof(ZapListView), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si les colonnes doivent être redimenssionnées automatiquement. 
        /// </summary>
        public bool IsAutoResizeColumns
        {
            get { return (bool)GetValue(IsAutoResizeColumnsProperty); }
            set { SetValue(IsAutoResizeColumnsProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.HeaderVisibility"/>.
        /// </summary>
        private static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(
            "HeaderVisibility", typeof(Visibility), typeof(ZapListView), new FrameworkPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Obtient ou défini de la visibilité des entêtes de colonne. 
        /// </summary>
        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.DisabledBackground"/>.
        /// </summary>
        private static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_disabledBackground));

        /// <summary>
        /// Obtient ou défini la couleur de fond lorsque le ListView est désactivé. 
        /// </summary>
        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        #endregion

        #region Header Properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.HeaderBackground"/>.
        /// </summary>
        private static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(
            "HeaderBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_headerBackground_LinearGradientBrushDefault));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'en-tête des colonnes. 
        /// </summary>
        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.HeaderBorderBrush"/>.
        /// </summary>
        private static readonly DependencyProperty HeaderBorderBrushProperty = DependencyProperty.Register(
            "HeaderBorderBrush", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.Indigo));

        /// <summary>
        /// Obtient ou défini la couleur des bordures de l'en-tête des colonnes. 
        /// </summary>
        public Brush HeaderBorderBrush
        {
            get { return (Brush)GetValue(HeaderBorderBrushProperty); }
            set { SetValue(HeaderBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.HeaderForeground"/>.
        /// </summary>
        private static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register(
            "HeaderForeground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.FloralWhite));

        /// <summary>
        /// Obtient ou défini la couleur de la police de l'en-tête des colonnes. 
        /// </summary>
        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.HeaderBackgroundMouseOver"/>.
        /// </summary>
        private static readonly DependencyProperty HeaderBackgroundMouseOverProperty = DependencyProperty.Register(
            "HeaderBackgroundMouseOver", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_headerBackgroundMouseOver_LinearGradientBrushDefault));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'en-tête des colonnes quand la souris est au dessus. 
        /// </summary>
        public Brush HeaderBackgroundMouseOver
        {
            get { return (Brush)GetValue(HeaderBackgroundMouseOverProperty); }
            set { SetValue(HeaderBackgroundMouseOverProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.HeaderBackgroundIsPressed"/>.
        /// </summary>
        private static readonly DependencyProperty HeaderBackgroundIsPressedProperty = DependencyProperty.Register(
            "HeaderBackgroundIsPressed", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_headerBackgroundIsPressed_LinearGradientBrushDefault));

        /// <summary>
        /// Obtient ou défini la couleur de fond de l'en-tête des colonnes quand la souris est au dessus. 
        /// </summary>
        public Brush HeaderBackgroundIsPressed
        {
            get { return (Brush)GetValue(HeaderBackgroundIsPressedProperty); }
            set { SetValue(HeaderBackgroundIsPressedProperty, value); }
        }

        #endregion

        #region ItemContainer

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemBackground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.Register(
            "ItemBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Obtient ou défini la couleur de fond des éléments du <see cref="ListView"/>. 
        /// </summary>
        public Brush ItemBackground
        {
            get { return (Brush)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemBorderBrush"/>.
        /// </summary>
        private static readonly DependencyProperty ItemBorderBrushProperty = DependencyProperty.Register(
            "ItemBorderBrush", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Obtient ou défini la couleur des bordures des éléments du <see cref="ListView"/>. 
        /// </summary>
        public Brush ItemBorderBrush
        {
            get { return (Brush)GetValue(ItemBorderBrushProperty); }
            set { SetValue(ItemBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemForeground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemForegroundProperty = DependencyProperty.Register(
            "ItemForeground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini la couleur de la police des éléments du <see cref="ListView"/>. 
        /// </summary>
        public Brush ItemForeground
        {
            get { return (Brush)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemBorderThickness"/>.
        /// </summary>
        private static readonly DependencyProperty ItemBorderThicknessProperty = DependencyProperty.Register(
            "ItemBorderThickness", typeof(Thickness), typeof(ZapListView), new FrameworkPropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Obtient ou défini la taille des bordures des éléments du <see cref="ListView"/>. 
        /// </summary>
        public Thickness ItemBorderThickness
        {
            get { return (Thickness)GetValue(ItemBorderThicknessProperty); }
            set { SetValue(ItemBorderThicknessProperty, value); }
        }

        #endregion

        #region Item MouseOver

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemMouseOverBackground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemMouseOverBackgroundProperty = DependencyProperty.Register(
            "ItemMouseOverBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_itemMouseOverBackground));

        /// <summary>
        /// Obtient ou défini la couleur de fond des éléments du <see cref="ListView"/> lors du survol de la souris. 
        /// </summary>
        public Brush ItemMouseOverBackground
        {
            get { return (Brush)GetValue(ItemMouseOverBackgroundProperty); }
            set { SetValue(ItemMouseOverBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemMouseOverForeground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemMouseOverForegroundProperty = DependencyProperty.Register(
            "ItemMouseOverForeground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.BlueViolet));

        /// <summary>
        /// Obtient ou défini la couleur de la police des éléments du <see cref="ListView"/> lors du survol de la souris. 
        /// </summary>
        public Brush ItemMouseOverForeground
        {
            get { return (Brush)GetValue(ItemMouseOverForegroundProperty); }
            set { SetValue(ItemMouseOverForegroundProperty, value); }
        }

        #endregion

        #region ListViewItem Selection

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemSelectionActiveBackground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemSelectionActiveBackgroundProperty = DependencyProperty.Register(
            "ItemSelectionActiveBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_itemSelectionActiveBackground));

        /// <summary>
        /// Obtient ou défini la couleur de fond des éléments sélectionnés lorsque le <see cref="ListView"/> a le focus. 
        /// </summary>
        public Brush ItemSelectionActiveBackground
        {
            get { return (Brush)GetValue(ItemSelectionActiveBackgroundProperty); }
            set { SetValue(ItemSelectionActiveBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemSelectionInactiveBackground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemSelectionInactiveBackgroundProperty = DependencyProperty.Register(
            "ItemSelectionInactiveBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_itemSelectionInactiveBackground));

        /// <summary>
        /// Obtient ou défini la couleur de fond des éléments sélectionnés lorsque le <see cref="ListView"/> n'a pas le focus. 
        /// </summary>
        public Brush ItemSelectionInactiveBackground
        {
            get { return (Brush)GetValue(ItemSelectionInactiveBackgroundProperty); }
            set { SetValue(ItemSelectionInactiveBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapListView.ItemSelectionForeground"/>.
        /// </summary>
        private static readonly DependencyProperty ItemSelectionForegroundProperty = DependencyProperty.Register(
            "ItemSelectionForeground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.LavenderBlush));

        /// <summary>
        /// Obtient ou défini la couleur de la police des éléments sélectionnés du <see cref="ListView"/>. 
        /// </summary>
        public Brush ItemSelectionForeground
        {
            get { return (Brush)GetValue(ItemSelectionForegroundProperty); }
            set { SetValue(ItemSelectionForegroundProperty, value); }
        }

        #endregion

        #region ScrollBar

        private static readonly DependencyProperty ScrollBarBackgroundProperty = DependencyProperty.Register(
            "ScrollBarBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_scrollBarBackground));

        public Brush ScrollBarBackground
        {
            get { return (Brush)GetValue(ScrollBarBackgroundProperty); }
            set { SetValue(ScrollBarBackgroundProperty, value); }
        }

        #region ScrollBar Thumb properties

        private static readonly DependencyProperty ScrollBarThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarThumbInnerBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_scrollBarThumbInnerBackground));

        public Brush ScrollBarThumbInnerBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbInnerBackgroundProperty); }
            set { SetValue(ScrollBarThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBackgroundProperty = DependencyProperty.Register(
            "ScrollBarThumbBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ScrollBarThumbBackground
        {
            get { return (Brush)GetValue(ScrollBarThumbBackgroundProperty); }
            set { SetValue(ScrollBarThumbBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarThumbBorderBrush", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(null));

        public Brush ScrollBarThumbBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarThumbBorderBrushProperty); }
            set { SetValue(ScrollBarThumbBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarThumbBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarThumbBorderThickness", typeof(Thickness), typeof(ZapListView), new FrameworkPropertyMetadata(null));

        public Thickness ScrollBarThumbBorderThickness
        {
            get { return (Thickness)GetValue(ScrollBarThumbBorderThicknessProperty); }
            set { SetValue(ScrollBarThumbBorderThicknessProperty, value); }
        }

        #endregion

        #region ScrollBar Buttons properties

        private static readonly DependencyProperty ScrollBarButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarButtonBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush ScrollBarButtonBackground
        {
            get { return (Brush)GetValue(ScrollBarButtonBackgroundProperty); }
            set { SetValue(ScrollBarButtonBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarButtonBorderBrush", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(null));

        public Brush ScrollBarButtonBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarButtonBorderBrushProperty); }
            set { SetValue(ScrollBarButtonBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarButtonBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarButtonBorderThickness", typeof(double), typeof(ZapListView), new FrameworkPropertyMetadata(null));

        public double ScrollBarButtonBorderThickness
        {
            get { return (double)GetValue(ScrollBarButtonBorderThicknessProperty); }
            set { SetValue(ScrollBarButtonBorderThicknessProperty, value); }
        }

        #endregion

        #region ScrollBar Disabled properties

        private static readonly DependencyProperty ScrollBarDisabledThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarDisabledThumbInnerBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_scrollBarDisabledButtonBackground));

        public Brush ScrollBarDisabledThumbInnerBackground
        {
            get { return (Brush)GetValue(ScrollBarDisabledThumbInnerBackgroundProperty); }
            set { SetValue(ScrollBarDisabledThumbInnerBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(_scrollBarDisabledButtonBackground));

        public Brush ScrollBarDisabledButtonBackground
        {
            get { return (Brush)GetValue(ScrollBarDisabledButtonBackgroundProperty); }
            set { SetValue(ScrollBarDisabledButtonBackgroundProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBorderBrush", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(null));

        public Brush ScrollBarDisabledButtonBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarDisabledButtonBorderBrushProperty); }
            set { SetValue(ScrollBarDisabledButtonBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty ScrollBarDisabledButtonBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarDisabledButtonBorderThickness", typeof(double), typeof(ZapListView), new FrameworkPropertyMetadata(null));

        public double ScrollBarDisabledButtonBorderThickness
        {
            get { return (double)GetValue(ScrollBarDisabledButtonBorderThicknessProperty); }
            set { SetValue(ScrollBarDisabledButtonBorderThicknessProperty, value); }
        }

        #endregion

        #endregion

        #region Loading Indicator

        private static readonly DependencyProperty LoadingIndicatorBackgroundProperty = DependencyProperty.Register(
            "LoadingIndicatorBackground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        public Brush LoadingIndicatorBackground
        {
            get { return (Brush)GetValue(LoadingIndicatorBackgroundProperty); }
            set { SetValue(LoadingIndicatorBackgroundProperty, value); }
        }

        private static readonly DependencyProperty LoadingIndicatorForegroundProperty = DependencyProperty.Register(
            "LoadingIndicatorForeground", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.White));

        public Brush LoadingIndicatorForeground
        {
            get { return (Brush)GetValue(LoadingIndicatorForegroundProperty); }
            set { SetValue(LoadingIndicatorForegroundProperty, value); }
        }

        private static readonly DependencyProperty LoadingIndicatorBorderBrushProperty = DependencyProperty.Register(
            "LoadingIndicatorBorderBrush", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.Indigo));

        public Brush LoadingIndicatorBorderBrush
        {
            get { return (Brush)GetValue(LoadingIndicatorBorderBrushProperty); }
            set { SetValue(LoadingIndicatorBorderBrushProperty, value); }
        }

        private static readonly DependencyProperty LoadingIndicatorAccentColorProperty = DependencyProperty.Register(
            "LoadingIndicatorAccentColor", typeof(Brush), typeof(ZapListView), new FrameworkPropertyMetadata(Brushes.Indigo));

        public Brush LoadingIndicatorAccentColor
        {
            get { return (Brush)GetValue(LoadingIndicatorAccentColorProperty); }
            set { SetValue(LoadingIndicatorAccentColorProperty, value); }
        }

        private static readonly DependencyProperty LoadingIndicatorBorderThicknessProperty = DependencyProperty.Register(
            "LoadingIndicatorBorderThickness", typeof(Thickness), typeof(ZapListView), new FrameworkPropertyMetadata(new Thickness(2)));

        public Thickness LoadingIndicatorBorderThickness
        {
            get { return (Thickness)GetValue(LoadingIndicatorBorderThicknessProperty); }
            set { SetValue(LoadingIndicatorBorderThicknessProperty, value); }
        }

        private static readonly DependencyProperty LoadingIndicatorCornerRadiusProperty = DependencyProperty.Register(
            "LoadingIndicatorCornerRadius", typeof(CornerRadius), typeof(ZapListView), new FrameworkPropertyMetadata(new CornerRadius(6)));

        public CornerRadius LoadingIndicatorCornerRadius
        {
            get { return (CornerRadius)GetValue(LoadingIndicatorCornerRadiusProperty); }
            set { SetValue(LoadingIndicatorCornerRadiusProperty, value); }
        }

        private static readonly DependencyProperty LoadingIndicatorTextProperty = DependencyProperty.Register(
            "LoadingIndicatorText", typeof(string), typeof(ZapListView), new FrameworkPropertyMetadata("Chargement des données en cours, patienter..."));

        public string LoadingIndicatorText
        {
            get { return (string)GetValue(LoadingIndicatorTextProperty); }
            set { SetValue(LoadingIndicatorTextProperty, value); }
        }



        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// Commande qui gère la suppression de la date d'un champ de filtrage de date
        /// </summary>
        public ICommand RemoveDateCommand { get; }

        private static void RemoveDateClick(Ctrl.DatePicker datePicker)
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
            if (VisualTreeHelpers.FindChild(this, "headerRowPresenter") is GridViewHeaderRowPresenter headerRowPresenter)
            {
                foreach (Control ctrl in VisualTreeHelpers.FindVisualChildren<Control>(headerRowPresenter))
                {
                    if (ctrl.Name == "search")
                    {
                        if (ctrl is Ctrl.DatePicker dp)
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
                }
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

        private void OnColumnHeaderClick(GridViewColumnHeader header)
        {
            if ((Items?.Count ?? 0) > 0)
            {
                IsFiltering = true;

                // Suppression de toutes le description de tri
                Items.SortDescriptions.Clear();

                // Réinitialidation des indicateurs de tri sur les colonnes
                if (View is GridView gridView)
                {
                    foreach (ZapGridViewColumn col in gridView.Columns)
                        col.SortDirection = null;
                }

                if (header.Column is ZapGridViewColumn column)
                {
                    ListSortDirection direction = _sortedColumn == column && _sortDirection == ListSortDirection.Ascending ?
                        ListSortDirection.Descending : ListSortDirection.Ascending;

                    if (!(column.DisplayMemberBinding is Binding binding))
                        binding = column.SortBinding;

                    if (binding != null)
                    {
                        if (binding?.Converter != null)
                        {
                            object items = CollectionHelpers.SortWithConverter(ItemsSource, binding, direction, IsSearchAsync);
                            VisualTreeHelpers.UpdateBinding(this, ItemsSourceProperty, items);
                            Refresh();
                        }
                        else
                        {
                            Items.SortDescriptions.Add(new SortDescription(binding.Path.Path, direction));
                            Items.Refresh();
                            AutoResizeColumns();
                        }
                        _sortedColumn = column;
                        _sortDirection = direction;
                        column.SortDirection = direction;
                    }
                }
                IsFiltering = false;
            }
        }

        /// <summary>
        /// Commande qui gère le double click sur une ligne
        /// </summary>
        public ICommand ItemMouseDoubleClickCommand { get; }

        private void OnItemMouseDoubleClick(ListViewItem row)
        {
            ItemMouseDoubleClick?.Invoke(row, new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = MouseDoubleClickEvent,
                Source = this,
            });
        }

        #endregion

        #region Events

        /// <summary>
        /// Se produit lorsque la collection est modifié.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> ItemsChanged;

        /// <summary>
        /// Se produit lors du double clique sur un élément du <see cref="ZapListView"/>.
        /// </summary>
        public event MouseButtonEventHandler ItemMouseDoubleClick;

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
        /// <returns>Renvoi le <see cref="BackgroundWorker"/> utilisé pour efectuer le filtrage des éléments.</returns>
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

            if (Filtering == null)
            {
                _worker = new BackgroundWorker()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                _worker.DoWork += Worker_DoWork;
                _worker.ProgressChanged += Worker_ProgressChanged;
            }
            else
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
            _worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            return _worker;
        }

        /// <summary>
        /// Représente la méthode qui gère le filtrage avancé.
        /// </summary>
        /// <param name="filtersDictionary">Dictionnaire contenant les éléments de filtrage.</param>
        public delegate void AdvancedFilteringEventHandler(in Dictionary<string, KeyValuePair<object, IValueConverter>> filtersDictionary);

        /// <summary>
        /// Se produit lorsque la construction du dictionnaire de filtrage est terminé.
        /// </summary>
        public event AdvancedFilteringEventHandler AdvancedFiltering;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur de la classe <see cref="ZapListView"/>.
        /// </summary>
        public ZapListView()
        {
            RemoveDateCommand = new RelayCommand<Ctrl.DatePicker>(
                param => RemoveDateClick(param),
                param => true);

            RemoveFiltersCommand = new RelayCommand(
                RemoveFiltersClick,
                true);

            FilterChangedCommand = new RelayCommand(
                OnFilterChanged,
                true);

            ColumnHeaderClickCommand = new RelayCommand<GridViewColumnHeader>(
                param => OnColumnHeaderClick(param),
                param => true);

            ItemMouseDoubleClickCommand = new RelayCommand<ListViewItem>(
                param => OnItemMouseDoubleClick(param),
                param => true);

            Loaded += InternalZapListView_Loaded;

            Alltems = new AsyncObservableCollection<dynamic>();
            FiltersDictionary = new Dictionary<string, KeyValuePair<object, IValueConverter>>();
            _deferredAction = DeferredAction.Create(() => FilterListView());
        }

        #endregion

        /// <summary>
        /// Méthode qui gère le chargement du <see cref="ZapListView"/>.
        /// </summary>
        private void InternalZapListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (VisualTreeHelpers.FindChild(this, "headerRowPresenter") is GridViewHeaderRowPresenter headerRowPresenter)
            {
                IEnumerable<Control> controls = VisualTreeHelpers.FindVisualChildren<Control>(headerRowPresenter);

                if (controls.Any(c => c.Name == "search"))
                    _btnRemoveFilters.Visibility = HeaderVisibility;
            }

            _sortedColumn = VisualTreeHelpers.FindVisualChildren<GridViewColumnHeader>(this)
                .Where(h => h.Column is ZapGridViewColumn)
                .Select(h => h.Column)
                .Cast<ZapGridViewColumn>()
                .FirstOrDefault(c => c.SortDirection != null);
            _sortDirection = _sortedColumn?.SortDirection;

            if (!(_sortedColumn?.DisplayMemberBinding is Binding binding))
                binding = _sortedColumn?.SortBinding;

            if (binding?.Converter == null && _sortDirection != null)
                Items.SortDescriptions.Add(new SortDescription(binding.Path.Path, _sortDirection.Value));

            AutoResizeColumns();
        }

        public void ResizeColumns()
        {
            if (View is GridView gridView)
            {
                foreach (GridViewColumn col in gridView.Columns)
                {
                    if (col is ZapGridViewColumn zCol)
                    {
                        if (zCol.IsVisible)
                        {
                            zCol.Width = zCol.ActualWidth;
                            zCol.Width = double.NaN;
                        }
                    }
                    else
                    {
                        col.Width = col.ActualWidth;
                        col.Width = double.NaN;
                    }
                }
            }
        }

        /// <summary>
        /// Méthode qui gère l'affichage la désactivation/activation des champs de recherche
        /// </summary>
        /// <param name="isEnabled"></param>
        private void DisableEnableSearchFields(bool isEnabled)
        {
            if (VisualTreeHelpers.FindChild(this, "headerRowPresenter") is GridViewHeaderRowPresenter headerRowPresenter)
            {
                foreach (Control ctrl in VisualTreeHelpers.FindVisualChildren<Control>(headerRowPresenter))
                    ctrl.IsEnabled = isEnabled;
            }
        }

        /// <summary>
        /// Méthode qui gère le redimmensionnement automatique des colonnes.
        /// </summary>
        private void AutoResizeColumns()
        {
            if (IsAutoResizeColumns)
                ResizeColumns();
        }

        /// <summary>
        /// Méthode qui gère l'actualisation du <see cref="ZapListView"/>.
        /// </summary>
        public void Refresh()
        {
            GetBindingExpression(ItemsSourceProperty).UpdateTarget();
            AutoResizeColumns();
        }

        #region Filter Methods

        /// <summary>
        /// Méthode qui gère le filtrage des éléments du <see cref="ZapListView"/>.
        /// </summary>
        public async void FilterListView(bool forceLoadingIndicatorStyle = false, bool showLoadingIndicator = true)
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
                while (IsFiltering)
                {
                    await Task.Delay(100);
                }
            }

            if (showLoadingIndicator)
                ToggleLoadingScreen(forceLoadingIndicatorStyle);

            FiltersDictionary.Clear();

            foreach (GridViewColumnHeader header in VisualTreeHelpers.FindVisualChildren<GridViewColumnHeader>(this))
            {
                if (header.Content == null) continue;

                if (header.Column is ZapGridViewColumn column)
                {
                    if (column.IsVisible)
                    {
                        Binding displayBinding = column.DisplayMemberBinding as Binding;
                        Binding sortBinding = column.SortBinding as Binding;

                        if (displayBinding != null || sortBinding != null)
                        {
                            Binding binding = displayBinding ?? sortBinding;

                            string columnName = binding.Path.Path;

                            DependencyObject dpo = VisualTreeHelpers.FindChild(header, "search");

                            // Vérification du DatePicker
                            if (dpo is Ctrl.DatePicker dp)
                            {
                                if (dp.SelectedDate.HasValue)
                                {
                                    if (FiltersDictionary.ContainsKey(columnName))
                                        FiltersDictionary.Remove(columnName);

                                    FiltersDictionary.Add(columnName, new KeyValuePair<object, IValueConverter>(dp.SelectedDate.Value.ToString("dd/MM/yyyy"), binding.Converter));
                                }
                                else
                                    FiltersDictionary.Remove(columnName);
                            }
                            // Vérification de la ComboBox
                            else if (dpo is ComboBox cb)
                            {
                                if (!string.IsNullOrEmpty(cb.SelectedValue?.ToString()))
                                {
                                    if (FiltersDictionary.ContainsKey(columnName))
                                        FiltersDictionary.Remove(columnName);

                                    FiltersDictionary.Add(columnName, new KeyValuePair<object, IValueConverter>(cb.SelectedValue, binding.Converter));
                                }
                                else
                                    FiltersDictionary.Remove(columnName);
                            }
                            // Vérification de la Textbox
                            else if (dpo is TextBox tb)
                            {
                                if (!string.IsNullOrEmpty(tb.Text))
                                {
                                    if (FiltersDictionary.ContainsKey(columnName))
                                    {
                                        if (FiltersDictionary[columnName].Value == binding.Converter)
                                            FiltersDictionary.Remove(columnName);
                                        else
                                            columnName += FiltersDictionary.Where(f => f.Key == columnName).Count() + 1;
                                    }
                                    FiltersDictionary.Add(columnName, new KeyValuePair<object, IValueConverter>(tb.Text, binding.Converter));
                                }
                                else
                                    FiltersDictionary.Remove(columnName);
                            }
                        }
                    }
                }
            }

            IsFiltering = true;
            AdvancedFiltering?.Invoke(FiltersDictionary);
            RaisePropertyChanged(nameof(FiltersDictionary));

            if (IsSearchAsync)
                OnFiltering().RunWorkerAsync(showLoadingIndicator);
            else
                FilterSynchrone(showLoadingIndicator);
        }

        /// <summary>
        /// Méthode qui gère le filtrage des éléments
        /// </summary>
        private void FilterSynchrone(bool showLoadingIndicator)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                int i = 1;
                List<dynamic> items = FiltersDictionary.Count > 0 ? new List<dynamic>() : new List<dynamic>(Alltems);

                if (!items.Any())
                {
                    lock (Alltems)
                    {
                        foreach (var item in Alltems)
                        {
                            List<bool> fieldsValidation = new List<bool>();

                            foreach (KeyValuePair<string, KeyValuePair<object, IValueConverter>> filter in FiltersDictionary)
                            {
                                if (!string.IsNullOrEmpty(filter.Value.Key?.ToString()))
                                {
                                    string itemValue;
                                    object propValue = item;

                                    if (filter.Key != ".")
                                    {
                                        var properties = filter.Key.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                        for (int j = 0; j < properties.Length; j++)
                                        {
                                            propValue = TypeDescriptor.GetProperties(propValue)
                                                .Find(Regex.Replace(properties[j], "[0-9]", string.Empty), true)?
                                                .GetValue(propValue);
                                        }
                                    }

                                    if (filter.Value.Value == null)
                                        itemValue = propValue?.ToString();
                                    else
                                        itemValue = filter.Value.Value.Convert(propValue, typeof(string), null, CultureInfo.CurrentCulture)?.ToString();

                                    fieldsValidation.Add(Regex.IsMatch($"{itemValue}", $"({filter.Value.Key})+", RegexOptions.IgnoreCase | RegexOptions.Multiline));
                                }
                            }

                            if (fieldsValidation.All(v => v == true))
                                items.Add(item);

                            _loadingProgress.Value = i * 100 / Alltems.Count();
                            i++;
                        }
                    }
                }

                object result = CollectionHelpers.EnumerableDynamicCast(items, ItemsSource);

                BeforeFilteringFinished?.Invoke(this, new RunWorkerCompletedEventArgs(null, null, false));

                // Récupération du binding de la dernière colonne triée
                if (!(_sortedColumn?.DisplayMemberBinding is Binding binding))
                    binding = _sortedColumn?.SortBinding;

                // Si le binding de la colonne à un convertisseur
                if (binding?.Converter != null)
                    result = CollectionHelpers.SortWithConverter((IEnumerable<dynamic>)result, binding, _sortDirection);

                VisualTreeHelpers.UpdateBinding(this, ItemsSourceProperty, result);
                Refresh();

                AfterFilteringFinished?.Invoke(this, new RunWorkerCompletedEventArgs(null, null, false));

                if (showLoadingIndicator)
                    ToggleLoadingScreen();

                IsFiltering = false;
            }), DispatcherPriority.Background);
        }

        /// <summary>
        /// Méthode qui gère le filtrage des éléments en tâche de fond.
        /// </summary>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            List<dynamic> items = FiltersDictionary.Count > 0 ? new List<dynamic>() : new List<dynamic>(Alltems);

            if (!items.Any())
            {
                Parallel.ForEach(Alltems, (item, state) =>
                {
                    lock (Alltems)
                    {
                        if (_worker.CancellationPending)
                            state.Break();

                        List<bool> fieldsValidation = new List<bool>();

                        foreach (KeyValuePair<string, KeyValuePair<object, IValueConverter>> filter in FiltersDictionary)
                        {
                            if (_worker.CancellationPending)
                                state.Break();

                            if (!string.IsNullOrEmpty(filter.Value.Key?.ToString()))
                            {
                                string itemValue;
                                object propValue = item;

                                if (filter.Key != ".")
                                {
                                    var properties = filter.Key.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                                    for (int j = 0; j < properties.Length; j++)
                                    {
                                        propValue = TypeDescriptor.GetProperties(propValue)
                                            .Find(Regex.Replace(properties[j], "[0-9]", string.Empty), true)?
                                            .GetValue(propValue);
                                    }
                                }

                                if (filter.Value.Value == null)
                                    itemValue = propValue?.ToString();
                                else
                                    itemValue = filter.Value.Value.Convert(propValue, typeof(string), null, CultureInfo.CurrentCulture)?.ToString();

                                fieldsValidation.Add(Regex.IsMatch($"{itemValue}", $"({filter.Value.Key})+", RegexOptions.IgnoreCase | RegexOptions.Multiline));
                            }
                        }

                        if (fieldsValidation.All(v => v == true))
                            items.Add(item);

                        System.Threading.Thread.Sleep(1);

                        _worker.ReportProgress(i);
                        i++;
                    }
                });
            }

            if (_worker.CancellationPending)
            {
                e.Cancel = true;
                e.Result = new object[] { null, e.Argument };
            }
            else
                e.Result = new object[] { CollectionHelpers.EnumerableDynamicCast(items, ItemsSource), e.Argument };
        }

        /// <summary>
        /// Méthode qui gère la progression du filtrage des éléments en tâche de fond.
        /// </summary>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _loadingProgress.Value = e.ProgressPercentage * 100 / Alltems.Count();
        }

        /// <summary>
        /// Méthode qui gère la fin du filtrage des élements en tâche de fond.
        /// </summary>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Result is object[] objects)
            {
                BeforeFilteringFinished?.Invoke(this, e);

                if (objects[0] != null)
                {
                    object items = objects[0];

                    // Récupération du binding de la dernière colonne triée
                    if (!(_sortedColumn?.DisplayMemberBinding is Binding binding))
                        binding = _sortedColumn?.SortBinding;

                    // Si le binding de la colonne à un convertisseur
                    if (binding?.Converter != null)
                        items = CollectionHelpers.SortWithConverter((IEnumerable<dynamic>)items, binding, _sortDirection);

                    VisualTreeHelpers.UpdateBinding(this, ItemsSourceProperty, items);
                }
                else
                    DisableEnableSearchFields(_filtersDictionary?.Any() ?? false);

                Refresh();
                AfterFilteringFinished?.Invoke(this, e);
            }

            if (_loadingGrid.Visibility == Visibility.Visible)
                ToggleLoadingScreen();

            IsFiltering = false;
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

        #endregion

        #region Overrides

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        public override void OnApplyTemplate()
        {
            Style = (Style)_dict["ZapListViewStyle"];

            if (View is GridView gv)
            {
                gv.ColumnHeaderContainerStyle = (Style)_dict["ZapGridViewColumnHeaderStyle"];

                foreach (ZapGridViewColumn column in gv.Columns)
                {
                    string templateFormat;
                    string template = string.Empty;

                    switch (column.SearchType)
                    {
                        case ColumnSearchType.Date:
                            templateFormat = @"<DataTemplate>
                                <StackPanel Margin=""0"">
                                    <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"">
                                        <ContentPresenter x:Name=""headerContent"" Content=""{{TemplateBinding Content}}"" />
                                        <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                                Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                                        <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                                Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                                    </StackPanel>
                                    <DockPanel Visibility=""Visible"">
                                        <z:ZapButtonFlat x:Name=""clearDatePickerButton""
                                                         FontFamily=""Webdings""
                                                         Content=""r""
                                                         Foreground=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""   
                                                         MouseOverBackground=""{{x:Null}}""
                                                         ToolTip=""Supprimer la date""
                                                         Visibility=""Visible""
                                                         Command=""{{Binding RemoveDateCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}""
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
                                                    <i:InvokeCommandAction Command=""{{Binding FilterChangedCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" />
                                                </i:EventTrigger>
                                            </i:Interaction.Triggers>
                                            <DatePicker.Style>
                                                <Style TargetType=""{{x:Type DatePicker}}"">
                                                    <Style.Triggers>
                                                        <MultiDataTrigger>
                                                            <MultiDataTrigger.Conditions>
                                                                <Condition Binding=""{{Binding ItemsSource, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""{{x:Null}}"" />
                                                                <Condition Binding=""{{Binding FiltersDictionary.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                            </MultiDataTrigger.Conditions>
                                                            <Setter Property=""IsEnabled"" Value=""False"" />
                                                        </MultiDataTrigger>
                                                        <MultiDataTrigger>
                                                            <MultiDataTrigger.Conditions>
                                                                <Condition Binding=""{{Binding ItemsSource.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                                <Condition Binding=""{{Binding FiltersDictionary.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                            </MultiDataTrigger.Conditions>
                                                            <Setter Property=""IsEnabled"" Value=""False"" />
                                                        </MultiDataTrigger>
                                                    </Style.Triggers>
                                                </Style>
                                            </DatePicker.Style>
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
                                    <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Ascending"">
                                        <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                                    </DataTrigger>
                                    <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Descending"">
                                        <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                                    </DataTrigger>
                                </DataTemplate.Triggers>                          
                            </DataTemplate>";

                            template = string.Format(templateFormat);
                            break;
                        case ColumnSearchType.Text:
                            templateFormat = @"<DataTemplate>
                                <StackPanel x:Name=""panel"" Margin=""0"">
                                    <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"" >
                                        <ContentPresenter x:Name=""headerContent"" Content=""{{TemplateBinding Content}}"" />
                                        <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                                Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                                        <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                                Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                                    </StackPanel>
                                    <TextBox x:Name=""search"" Visibility=""Visible"">
                                        <i:Interaction.Triggers>
                                            <i:EventTrigger EventName=""TextChanged"">
                                                <i:InvokeCommandAction Command=""{{Binding FilterChangedCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" />
                                            </i:EventTrigger>
                                        </i:Interaction.Triggers>
                                        <TextBox.Style>
                                            <Style TargetType=""{{x:Type TextBox}}"">
                                                <Style.Triggers>
                                                    <MultiDataTrigger>
                                                        <MultiDataTrigger.Conditions>
                                                            <Condition Binding=""{{Binding ItemsSource, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""{{x:Null}}"" />
                                                            <Condition Binding=""{{Binding FiltersDictionary.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                        </MultiDataTrigger.Conditions>
                                                        <Setter Property=""IsEnabled"" Value=""False"" />
                                                    </MultiDataTrigger>
                                                    <MultiDataTrigger>
                                                        <MultiDataTrigger.Conditions>
                                                            <Condition Binding=""{{Binding ItemsSource.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                            <Condition Binding=""{{Binding FiltersDictionary.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                        </MultiDataTrigger.Conditions>
                                                        <Setter Property=""IsEnabled"" Value=""False"" />
                                                    </MultiDataTrigger>
                                                </Style.Triggers>
                                            </Style>
                                        </TextBox.Style>
                                    </TextBox>
                                </StackPanel>
                                <DataTemplate.Triggers>
                                    <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value="""">
                                        <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />
                                    </DataTrigger>
                                    <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value=""{{x:Null}}"">
                                        <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />
                                    </DataTrigger>
                                    <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Ascending"">
                                        <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                                    </DataTrigger>
                                    <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Descending"">
                                        <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                                    </DataTrigger>
                                </DataTemplate.Triggers>
                            </DataTemplate>";

                            template = string.Format(templateFormat);
                            break;
                        case ColumnSearchType.ComboBox:
                            if (!string.IsNullOrEmpty(column.ComboBoxSource))
                            {
                                templateFormat = @"<DataTemplate>
                                    <StackPanel Margin=""0"">
                                        <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"">
                                            <ContentPresenter x:Name=""headerContent"" Content=""{{TemplateBinding Content}}"" />
                                            <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                                    Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                                            <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                                    Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                                        </StackPanel>
                                        <ComboBox x:Name=""search"" Visibility=""Visible"" Padding=""4,1,1,1"" DisplayMemberPath=""{0}"" SelectedValuePath=""{0}""
                                                  ItemsSource=""{{Binding {1}, RelativeSource={{RelativeSource AncestorType={{x:Type Window}}}}}}"">
                                            <i:Interaction.Triggers>
                                                <i:EventTrigger EventName=""SelectionChanged"">
                                                    <i:InvokeCommandAction Command=""{{Binding FilterChangedCommand, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" />
                                                </i:EventTrigger>
                                            </i:Interaction.Triggers>
                                            <ComboBox.Style>
                                                <Style TargetType=""{{x:Type ComboBox}}"">
                                                    <Style.Triggers>
                                                        <MultiDataTrigger>
                                                            <MultiDataTrigger.Conditions>
                                                                <Condition Binding=""{{Binding ItemsSource, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""{{x:Null}}"" />
                                                                <Condition Binding=""{{Binding FiltersDictionary.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                            </MultiDataTrigger.Conditions>
                                                            <Setter Property=""IsEnabled"" Value=""False"" />
                                                        </MultiDataTrigger>
                                                        <MultiDataTrigger>
                                                            <MultiDataTrigger.Conditions>
                                                                <Condition Binding=""{{Binding ItemsSource.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                                <Condition Binding=""{{Binding FiltersDictionary.Count, RelativeSource={{RelativeSource AncestorType={{x:Type z:ZapListView}}}}}}"" Value=""0"" />
                                                            </MultiDataTrigger.Conditions>
                                                            <Setter Property=""IsEnabled"" Value=""False"" />
                                                        </MultiDataTrigger>
                                                    </Style.Triggers>
                                                </Style>
                                            </ComboBox.Style>
                                        </ComboBox>
                                    </StackPanel>
                                    <DataTemplate.Triggers>             
                                        <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value="""">                
                                            <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />                     
                                        </DataTrigger>                     
                                        <DataTrigger Binding=""{{Binding Content, ElementName=headerContent}}"" Value=""{{x:Null}}"">                
                                            <Setter TargetName=""search"" Property=""Visibility"" Value=""Collapsed"" />                     
                                        </DataTrigger>          
                                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Ascending"">
                                            <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                                        </DataTrigger>
                                        <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Descending"">
                                            <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                                        </DataTrigger>
                                    </DataTemplate.Triggers>                     
                                </DataTemplate>";

                                template = string.Format(templateFormat, column.ComboBoxDisplayMemberPath, column.ComboBoxSource);
                            }
                            break;
                        default:
                            templateFormat = @"<DataTemplate>
                                <StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Center"" Margin=""0"">
                                    <ContentPresenter x:Name=""headerContent"" Tag=""{0}"" Content=""{{TemplateBinding Content}}"" />
                                    <Path x:Name=""pathSortAsc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                            Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 4 L 3.5 0 L 7 4 Z"" Visibility=""Collapsed"" />
                                    <Path x:Name=""pathSortDesc"" Fill=""{{Binding Foreground, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}""  
                                            Margin=""5,0,0,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" Data=""M 0 0 L 3.5 4 L 7 0 Z"" Visibility=""Collapsed"" />
                                </StackPanel>
                                <DataTemplate.Triggers>             
                                    <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Ascending"">
                                        <Setter TargetName=""pathSortAsc"" Property=""Visibility"" Value=""Visible"" />
                                    </DataTrigger>
                                    <DataTrigger Binding=""{{Binding Column.SortDirection, RelativeSource={{RelativeSource AncestorType={{x:Type GridViewColumnHeader}}}}}}"" Value=""Descending"">
                                        <Setter TargetName=""pathSortDesc"" Property=""Visibility"" Value=""Visible"" />
                                    </DataTrigger>
                                </DataTemplate.Triggers>                     
                            </DataTemplate>";

                            template = string.Format(templateFormat, column.SortBinding);
                            break;
                    }

                    ParserContext context = new ParserContext { XamlTypeMapper = new XamlTypeMapper(Array.Empty<string>()) };
                    context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                    context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                    context.XmlnsDictionary.Add("i", "clr-namespace:Microsoft.Xaml.Behaviors;assembly=Microsoft.Xaml.Behaviors");
                    context.XmlnsDictionary.Add("z", "http://schemas.zapan.com/wpf/controls/2020");

                    column.HeaderTemplate = XamlReader.Parse(template, context) as DataTemplate;
                }
            }

            _loadingGrid = (Grid)VisualTreeHelpers.FindChild(this, "loadingGrid");
            _loadingProgress = (ProgressBar)VisualTreeHelpers.FindChild(this, "loadingProgress");
            _loadingIndicator = (ZapLoadingIndicator)VisualTreeHelpers.FindChild(this, "loadingIndicator");

            _btnRemoveFilters = (ZapButtonFlat)VisualTreeHelpers.FindChild(this, "btnRemoveFilters");

            base.OnApplyTemplate();
        }

        public void Add(object item)
        {
            lock (Alltems)
            {
                Alltems.Add(item);
                FilterListView(showLoadingIndicator: false);
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (!IsFiltering)
            {
                lock (Alltems)
                {
                    Alltems = new AsyncObservableCollection<dynamic>((IEnumerable<dynamic>)ItemsSource);
                    AutoResizeColumns();
                }
            }
            ItemsChanged?.Invoke(this, e);
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

        public void Dispose(bool disposing)
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

    #region Enums

    /// <summary>
    /// Types de recherche pouvant être utilisé sur une colonne.
    /// </summary>
    public enum ColumnSearchType
    {
        None = 0,
        Date = 1,
        Text = 2,
        ComboBox = 3
    }

    #endregion
}

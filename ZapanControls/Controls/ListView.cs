using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Helpers;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListViewItem))]
    public sealed class ListView : System.Windows.Controls.ListView, ITheme, IDisposable
    {
        #region Fields
        private readonly DeferredAction _deferredAction;

        private bool _disposed;
        private bool _isFiltering;
        private ZapButton _btnClear;
        private GridViewHeaderRowPresenter _headerRowPresenter;
        private BackgroundWorker _worker;
        private Dictionary<string, KeyValuePair<object, IValueConverter>> _filterDictionary;
        #endregion

        #region Properties
        #region AllItems
        public AsyncObservableCollection<dynamic> AllItems { get; private set; }
        #endregion

        #region FilterDictionary
        public Dictionary<string, KeyValuePair<object, IValueConverter>> FilterDictionary { get => _filterDictionary; private set => Set(ref _filterDictionary, value); }
        #endregion

        #region IsFiltering
        public bool IsFiltering { get => _isFiltering; set => Set(ref _isFiltering, value); }
        #endregion

        #region IsSearchAsync
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="IsSearchAsync"/>.
        /// </summary>
        private static readonly DependencyProperty IsSearchAsyncProperty = DependencyProperty.Register(
            "IsSearchAsync", typeof(bool), typeof(ListView), new PropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si la recherche dans le listview est asynchrone. 
        /// </summary>
        public bool IsSearchAsync { get => (bool)GetValue(IsSearchAsyncProperty); set => SetValue(IsSearchAsyncProperty, value); }
        #endregion

        #region IsAutoResizeColumns
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="IsAutoResizeColumns"/>.
        /// </summary>
        private static readonly DependencyProperty IsAutoResizeColumnsProperty = DependencyProperty.Register(
            "IsAutoResizeColumns", typeof(bool), typeof(ListView),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si les colonnes doivent être redimenssionnées automatiquement. 
        /// </summary>
        public bool IsAutoResizeColumns 
        { 
            get => (bool)GetValue(IsAutoResizeColumnsProperty); 
            set => SetValue(IsAutoResizeColumnsProperty, value); 
        }
        #endregion

        #region HeaderVisibility
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="HeaderVisibility"/>.
        /// </summary>
        private static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(
            "HeaderVisibility", typeof(Visibility), typeof(ListView),
            new FrameworkPropertyMetadata(
                Visibility.Visible,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Obtient ou défini de la visibilité des entêtes de colonne. 
        /// </summary>
        public Visibility HeaderVisibility { get => (Visibility)GetValue(HeaderVisibilityProperty); set => SetValue(HeaderVisibilityProperty, value); }
        #endregion

        #region Control
        #region DisabledBackground
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBackgroundChanged));

        public Brush DisabledBackground { get => (Brush)GetValue(DisabledBackgroundProperty); set => SetValue(DisabledBackgroundProperty, value); }

        private static void OnDisabledBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledBackgroundProperty, e.NewValue);
        #endregion

        #region DisabledBorderBrush
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDisabledBorderBrushChanged));

        public Brush DisabledBorderBrush { get => (Brush)GetValue(DisabledBorderBrushProperty); set => SetValue(DisabledBorderBrushProperty, value); }

        private static void OnDisabledBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(DisabledBorderBrushProperty, e.NewValue);
        #endregion
        #endregion

        #region Header
        #region Normal
        #region HeaderBackground
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(
            "HeaderBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderBackgroundChanged));

        public Brush HeaderBackground { get => (Brush)GetValue(HeaderBackgroundProperty); set => SetValue(HeaderBackgroundProperty, value); }

        private static void OnHeaderBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(HeaderBackgroundProperty, e.NewValue);
        #endregion

        #region HeaderBorderBrush
        public static readonly DependencyProperty HeaderBorderBrushProperty = DependencyProperty.Register(
            "HeaderBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderBorderBrushChanged));

        public Brush HeaderBorderBrush { get => (Brush)GetValue(HeaderBorderBrushProperty); set => SetValue(HeaderBorderBrushProperty, value); }

        private static void OnHeaderBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(HeaderBorderBrushProperty, e.NewValue);
        #endregion

        #region HeaderForeground
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register(
            "HeaderForeground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderForegroundChanged));

        public Brush HeaderForeground { get => (Brush)GetValue(HeaderForegroundProperty); set => SetValue(HeaderForegroundProperty, value); }

        private static void OnHeaderForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(HeaderForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Focused
        #region HeaderFocusedBackground
        public static readonly DependencyProperty HeaderFocusedBackgroundProperty = DependencyProperty.Register(
            "HeaderFocusedBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderFocusedBackgroundChanged));

        public Brush HeaderFocusedBackground { get => (Brush)GetValue(HeaderFocusedBackgroundProperty); set => SetValue(HeaderFocusedBackgroundProperty, value); }

        private static void OnHeaderFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(HeaderFocusedBackgroundProperty, e.NewValue);
        #endregion

        #region HeaderFocusedForeground
        public static readonly DependencyProperty HeaderFocusedForegroundProperty = DependencyProperty.Register(
            "HeaderFocusedForeground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderFocusedForegroundChanged));

        public Brush HeaderFocusedForeground { get => (Brush)GetValue(HeaderFocusedForegroundProperty); set => SetValue(HeaderFocusedForegroundProperty, value); }

        private static void OnHeaderFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(HeaderFocusedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Pressed
        #region HeaderPressedBackground
        public static readonly DependencyProperty HeaderPressedBackgroundProperty = DependencyProperty.Register(
            "HeaderPressedBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderPressedBackgroundChanged));

        public Brush HeaderPressedBackground { get => (Brush)GetValue(HeaderPressedBackgroundProperty); set => SetValue(HeaderPressedBackgroundProperty, value); }

        private static void OnHeaderPressedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(HeaderPressedBackgroundProperty, e.NewValue);
        #endregion

        #region HeaderPressedForeground
        public static readonly DependencyProperty HeaderPressedForegroundProperty = DependencyProperty.Register(
            "HeaderPressedForeground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderPressedForegroundChanged));

        public Brush HeaderPressedForeground { get => (Brush)GetValue(HeaderPressedForegroundProperty); set => SetValue(HeaderPressedForegroundProperty, value); }

        private static void OnHeaderPressedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(HeaderPressedForegroundProperty, e.NewValue);
        #endregion
        #endregion
        #endregion

        #region Items
        #region Normal
        #region ItemsBackground
        public static readonly DependencyProperty ItemsBackgroundProperty = DependencyProperty.Register(
            "ItemsBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsBackgroundChanged));

        public Brush ItemsBackground { get => (Brush)GetValue(ItemsBackgroundProperty); set => SetValue(ItemsBackgroundProperty, value); }

        private static void OnItemsBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsBackgroundProperty, e.NewValue);
        #endregion

        #region ItemsBorderBrush
        public static readonly DependencyProperty ItemsBorderBrushProperty = DependencyProperty.Register(
            "ItemsBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsBorderBrushChanged));

        public Brush ItemsBorderBrush { get => (Brush)GetValue(ItemsBorderBrushProperty); set => SetValue(ItemsBorderBrushProperty, value); }

        private static void OnItemsBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemsBorderThickness
        public static readonly DependencyProperty ItemsBorderThicknessProperty = DependencyProperty.Register(
            "ItemsBorderThickness", typeof(Thickness), typeof(ListView),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsBorderThicknessChanged));

        public Thickness ItemsBorderThickness { get => (Thickness)GetValue(ItemsBorderThicknessProperty); set => SetValue(ItemsBorderThicknessProperty, value); }

        private static void OnItemsBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsBorderThicknessProperty, e.NewValue);
        #endregion

        #region ItemsForeground
        public static readonly DependencyProperty ItemsForegroundProperty = DependencyProperty.Register(
            "ItemsForeground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsForegroundChanged));

        public Brush ItemsForeground { get => (Brush)GetValue(ItemsForegroundProperty); set => SetValue(ItemsForegroundProperty, value); }

        private static void OnItemsForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Focused
        #region ItemsFocusedBackground
        public static readonly DependencyProperty ItemsFocusedBackgroundProperty = DependencyProperty.Register(
            "ItemsFocusedBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsFocusedBackgroundChanged));

        public Brush ItemsFocusedBackground { get => (Brush)GetValue(ItemsFocusedBackgroundProperty); set => SetValue(ItemsFocusedBackgroundProperty, value); }

        private static void OnItemsFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsFocusedBackgroundProperty, e.NewValue);
        #endregion

        #region ItemsFocusedBorderBrush
        public static readonly DependencyProperty ItemsFocusedBorderBrushProperty = DependencyProperty.Register(
            "ItemsFocusedBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsFocusedBorderBrushChanged));

        public Brush ItemsFocusedBorderBrush { get => (Brush)GetValue(ItemsFocusedBorderBrushProperty); set => SetValue(ItemsFocusedBorderBrushProperty, value); }

        private static void OnItemsFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsFocusedBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemsFocusedForeground
        public static readonly DependencyProperty ItemsFocusedForegroundProperty = DependencyProperty.Register(
            "ItemsFocusedForeground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsFocusedForegroundChanged));

        public Brush ItemsFocusedForeground { get => (Brush)GetValue(ItemsFocusedForegroundProperty); set => SetValue(ItemsFocusedForegroundProperty, value); }

        private static void OnItemsFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsFocusedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Selected
        #region ItemsSelectedActiveBackground
        public static readonly DependencyProperty ItemsSelectedActiveBackgroundProperty = DependencyProperty.Register(
            "ItemsSelectedActiveBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsSelectedActiveBackgroundChanged));

        public Brush ItemsSelectedActiveBackground { get => (Brush)GetValue(ItemsSelectedActiveBackgroundProperty); set => SetValue(ItemsSelectedActiveBackgroundProperty, value); }

        private static void OnItemsSelectedActiveBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsSelectedActiveBackgroundProperty, e.NewValue);
        #endregion

        #region ItemsSelectedInactiveBackground
        public static readonly DependencyProperty ItemsSelectedInactiveBackgroundProperty = DependencyProperty.Register(
            "ItemsSelectedInactiveBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsSelectedInactiveBackgroundChanged));

        public Brush ItemsSelectedInactiveBackground { get => (Brush)GetValue(ItemsSelectedInactiveBackgroundProperty); set => SetValue(ItemsSelectedInactiveBackgroundProperty, value); }

        private static void OnItemsSelectedInactiveBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsSelectedInactiveBackgroundProperty, e.NewValue);
        #endregion

        #region ItemsSelectedActiveBorderBrush
        public static readonly DependencyProperty ItemsSelectedActiveBorderBrushProperty = DependencyProperty.Register(
            "ItemsSelectedActiveBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsSelectedActiveBorderBrushChanged));

        public Brush ItemsSelectedActiveBorderBrush { get => (Brush)GetValue(ItemsSelectedActiveBorderBrushProperty); set => SetValue(ItemsSelectedActiveBorderBrushProperty, value); }

        private static void OnItemsSelectedActiveBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsSelectedActiveBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemsSelectedInactiveBorderBrush
        public static readonly DependencyProperty ItemsSelectedInactiveBorderBrushProperty = DependencyProperty.Register(
            "ItemsSelectedInactiveBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsSelectedInactiveBorderBrushChanged));

        public Brush ItemsSelectedInactiveBorderBrush { get => (Brush)GetValue(ItemsSelectedInactiveBorderBrushProperty); set => SetValue(ItemsSelectedInactiveBorderBrushProperty, value); }

        private static void OnItemsSelectedInactiveBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsSelectedInactiveBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemsSelectedForeground
        public static readonly DependencyProperty ItemsSelectedForegroundProperty = DependencyProperty.Register(
            "ItemsSelectedForeground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemsSelectedForegroundChanged));

        public Brush ItemsSelectedForeground { get => (Brush)GetValue(ItemsSelectedForegroundProperty); set => SetValue(ItemsSelectedForegroundProperty, value); }

        private static void OnItemsSelectedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemsSelectedForegroundProperty, e.NewValue);
        #endregion
        #endregion
        #endregion

        #region ScrollBars
        #region ScrollBarsBackground
        public static readonly DependencyProperty ScrollBarsBackgroundProperty = DependencyProperty.Register(
            "ScrollBarsBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsBackgroundChanged));

        public Brush ScrollBarsBackground { get => (Brush)GetValue(ScrollBarsBackgroundProperty); set => SetValue(ScrollBarsBackgroundProperty, value); }

        private static void OnScrollBarsBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsBackgroundProperty, e.NewValue);
        #endregion

        #region ScrollBarsBorderBrush
        public static readonly DependencyProperty ScrollBarsBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarsBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsBorderBrushChanged));

        public Brush ScrollBarsBorderBrush { get => (Brush)GetValue(ScrollBarsBorderBrushProperty); set => SetValue(ScrollBarsBorderBrushProperty, value); }

        private static void OnScrollBarsBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsBorderBrushProperty, e.NewValue);
        #endregion

        #region ScrollBarsBorderThickness
        public static readonly DependencyProperty ScrollBarsBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarsBorderThickness", typeof(Thickness), typeof(ListView),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsBorderThicknessChanged));

        public Thickness ScrollBarsBorderThickness { get => (Thickness)GetValue(ScrollBarsBorderThicknessProperty); set => SetValue(ScrollBarsBorderThicknessProperty, value); }

        private static void OnScrollBarsBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsBorderThicknessProperty, e.NewValue);
        #endregion

        #region ScrollBarsButton
        #region ScrollBarsButtonBackground
        public static readonly DependencyProperty ScrollBarsButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarsButtonBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsButtonBackgroundChanged));

        public Brush ScrollBarsButtonBackground { get => (Brush)GetValue(ScrollBarsButtonBackgroundProperty); set => SetValue(ScrollBarsButtonBackgroundProperty, value); }

        private static void OnScrollBarsButtonBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsButtonBackgroundProperty, e.NewValue);
        #endregion

        #region ScrollBarsButtonBorderBrush
        public static readonly DependencyProperty ScrollBarsButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarsButtonBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsButtonBorderBrushChanged));

        public Brush ScrollBarsButtonBorderBrush { get => (Brush)GetValue(ScrollBarsButtonBorderBrushProperty); set => SetValue(ScrollBarsButtonBorderBrushProperty, value); }

        private static void OnScrollBarsButtonBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsButtonBorderBrushProperty, e.NewValue);
        #endregion

        #region ScrollBarsButtonBorderThickness
        public static readonly DependencyProperty ScrollBarsButtonBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarsButtonBorderThickness", typeof(Thickness), typeof(ListView),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsButtonBorderThicknessChanged));

        public Thickness ScrollBarsButtonBorderThickness { get => (Thickness)GetValue(ScrollBarsButtonBorderThicknessProperty); set => SetValue(ScrollBarsButtonBorderThicknessProperty, value); }

        private static void OnScrollBarsButtonBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsButtonBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region ScrollBarsThumb
        #region ScrollBarsThumbInnerBackground
        public static readonly DependencyProperty ScrollBarsThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarsThumbInnerBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsThumbInnerBackgroundChanged));

        public Brush ScrollBarsThumbInnerBackground { get => (Brush)GetValue(ScrollBarsThumbInnerBackgroundProperty); set => SetValue(ScrollBarsThumbInnerBackgroundProperty, value); }

        private static void OnScrollBarsThumbInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsThumbInnerBackgroundProperty, e.NewValue);
        #endregion

        #region ScrollBarsThumbBackground
        public static readonly DependencyProperty ScrollBarsThumbBackgroundProperty = DependencyProperty.Register(
            "ScrollBarsThumbBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsThumbBackgroundChanged));

        public Brush ScrollBarsThumbBackground { get => (Brush)GetValue(ScrollBarsThumbBackgroundProperty); set => SetValue(ScrollBarsThumbBackgroundProperty, value); }

        private static void OnScrollBarsThumbBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsThumbBackgroundProperty, e.NewValue);
        #endregion

        #region ScrollBarsThumbBorderBrush
        public static readonly DependencyProperty ScrollBarsThumbBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarsThumbBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsThumbBorderBrushChanged));

        public Brush ScrollBarsThumbBorderBrush { get => (Brush)GetValue(ScrollBarsThumbBorderBrushProperty); set => SetValue(ScrollBarsThumbBorderBrushProperty, value); }

        private static void OnScrollBarsThumbBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsThumbBorderBrushProperty, e.NewValue);
        #endregion

        #region ScrollBarsThumbBorderThickness
        public static readonly DependencyProperty ScrollBarsThumbBorderThicknessProperty = DependencyProperty.Register(
            "ScrollBarsThumbBorderThickness", typeof(Thickness), typeof(ListView),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsThumbBorderThicknessChanged));

        public Thickness ScrollBarsThumbBorderThickness { get => (Thickness)GetValue(ScrollBarsThumbBorderThicknessProperty); set => SetValue(ScrollBarsThumbBorderThicknessProperty, value); }

        private static void OnScrollBarsThumbBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsThumbBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region ScrollBarsDisabled
        #region ScrollBarsDisabledThumbInnerBackground
        public static readonly DependencyProperty ScrollBarsDisabledThumbInnerBackgroundProperty = DependencyProperty.Register(
            "ScrollBarsDisabledThumbInnerBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsDisabledThumbInnerBackgroundChanged));

        public Brush ScrollBarsDisabledThumbInnerBackground { get => (Brush)GetValue(ScrollBarsDisabledThumbInnerBackgroundProperty); set => SetValue(ScrollBarsDisabledThumbInnerBackgroundProperty, value); }

        private static void OnScrollBarsDisabledThumbInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsDisabledThumbInnerBackgroundProperty, e.NewValue);
        #endregion

        #region ScrollBarsDisabledButtonBackground
        public static readonly DependencyProperty ScrollBarsDisabledButtonBackgroundProperty = DependencyProperty.Register(
            "ScrollBarsDisabledButtonBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsDisabledButtonBackgroundChanged));

        public Brush ScrollBarsDisabledButtonBackground { get => (Brush)GetValue(ScrollBarsDisabledButtonBackgroundProperty); set => SetValue(ScrollBarsDisabledButtonBackgroundProperty, value); }

        private static void OnScrollBarsDisabledButtonBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsDisabledButtonBackgroundProperty, e.NewValue);
        #endregion

        #region ScrollBarsDisabledButtonBorderBrush
        public static readonly DependencyProperty ScrollBarsDisabledButtonBorderBrushProperty = DependencyProperty.Register(
            "ScrollBarsDisabledButtonBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnScrollBarsDisabledButtonBorderBrushChanged));

        public Brush ScrollBarsDisabledButtonBorderBrush { get => (Brush)GetValue(ScrollBarsDisabledButtonBorderBrushProperty); set => SetValue(ScrollBarsDisabledButtonBorderBrushProperty, value); }

        private static void OnScrollBarsDisabledButtonBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ScrollBarsDisabledButtonBorderBrushProperty, e.NewValue);
        #endregion
        #endregion
        #endregion

        #region Progress
        #region Control
        #region ProgressBackground
        public static readonly DependencyProperty ProgressBackgroundProperty = DependencyProperty.Register(
            "ProgressBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressBackgroundChanged));

        public Brush ProgressBackground { get => (Brush)GetValue(ProgressBackgroundProperty); set => SetValue(ProgressBackgroundProperty, value); }

        private static void OnProgressBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressBackgroundProperty, e.NewValue);
        #endregion

        #region ProgressBorderBrush
        public static readonly DependencyProperty ProgressBorderBrushProperty = DependencyProperty.Register(
            "ProgressBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressBorderBrushChanged));

        public Brush ProgressBorderBrush { get => (Brush)GetValue(ProgressBorderBrushProperty); set => SetValue(ProgressBorderBrushProperty, value); }

        private static void OnProgressBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressBorderBrushProperty, e.NewValue);
        #endregion

        #region ProgressBorderThickness
        public static readonly DependencyProperty ProgressBorderThicknessProperty = DependencyProperty.Register(
            "ProgressBorderThickness", typeof(Thickness), typeof(ListView),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressBorderThicknessChanged));

        public Thickness ProgressBorderThickness { get => (Thickness)GetValue(ProgressBorderThicknessProperty); set => SetValue(ProgressBorderThicknessProperty, value); }

        private static void OnProgressBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressBorderThicknessProperty, e.NewValue);
        #endregion

        #region ProgressCornerRadius
        public static readonly DependencyProperty ProgressCornerRadiusProperty = DependencyProperty.Register(
            "ProgressCornerRadius", typeof(CornerRadius), typeof(ListView),
            new FrameworkPropertyMetadata(new CornerRadius(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressCornerRadiusChanged));

        public CornerRadius ProgressCornerRadius { get => (CornerRadius)GetValue(ProgressCornerRadiusProperty); set => SetValue(ProgressCornerRadiusProperty, value); }

        private static void OnProgressCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressCornerRadiusProperty, e.NewValue);
        #endregion

        #region ProgressForeground
        public static readonly DependencyProperty ProgressForegroundProperty = DependencyProperty.Register(
            "ProgressForeground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressForegroundChanged));

        public Brush ProgressForeground { get => (Brush)GetValue(ProgressForegroundProperty); set => SetValue(ProgressForegroundProperty, value); }

        private static void OnProgressForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressForegroundProperty, e.NewValue);
        #endregion

        #region ProgressPadding
        public static readonly DependencyProperty ProgressPaddingProperty = DependencyProperty.Register(
            "ProgressPadding", typeof(Thickness), typeof(ListView),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressPaddingChanged));

        public Thickness ProgressPadding { get => (Thickness)GetValue(ProgressPaddingProperty); set => SetValue(ProgressPaddingProperty, value); }

        private static void OnProgressPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressPaddingProperty, e.NewValue);
        #endregion
        #endregion

        #region ProgressBar
        #region ProgressBarInnerBackground
        public static readonly DependencyProperty ProgressBarInnerBackgroundProperty = DependencyProperty.Register(
            "ProgressBarInnerBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressBarInnerBackgroundChanged));

        public Brush ProgressBarInnerBackground { get => (Brush)GetValue(ProgressBarInnerBackgroundProperty); set => SetValue(ProgressBarInnerBackgroundProperty, value); }

        private static void OnProgressBarInnerBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressBarInnerBackgroundProperty, e.NewValue);
        #endregion

        #region ProgressBarBackground
        public static readonly DependencyProperty ProgressBarBackgroundProperty = DependencyProperty.Register(
            "ProgressBarBackground", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressBarBackgroundChanged));

        public Brush ProgressBarBackground { get => (Brush)GetValue(ProgressBarBackgroundProperty); set => SetValue(ProgressBarBackgroundProperty, value); }

        private static void OnProgressBarBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressBarBackgroundProperty, e.NewValue);
        #endregion

        #region ProgressBarBorderBrush
        public static readonly DependencyProperty ProgressBarBorderBrushProperty = DependencyProperty.Register(
            "ProgressBarBorderBrush", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressBarBorderBrushChanged));

        public Brush ProgressBarBorderBrush { get => (Brush)GetValue(ProgressBarBorderBrushProperty); set => SetValue(ProgressBarBorderBrushProperty, value); }

        private static void OnProgressBarBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressBarBorderBrushProperty, e.NewValue);
        #endregion

        #region ProgressBarBorderThickness
        public static readonly DependencyProperty ProgressBarBorderThicknessProperty = DependencyProperty.Register(
            "ProgressBarBorderThickness", typeof(Thickness), typeof(ListView),
            new FrameworkPropertyMetadata(new Thickness(0),
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnProgressBarBorderThicknessChanged));

        public Thickness ProgressBarBorderThickness { get => (Thickness)GetValue(ProgressBarBorderThicknessProperty); set => SetValue(ProgressBarBorderThicknessProperty, value); }

        private static void OnProgressBarBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ProgressBarBorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Indicator
        #region IndicatorTemplate
        public static readonly DependencyProperty IndicatorTemplateProperty = DependencyProperty.Register(
            "IndicatorTemplate", typeof(ZapLoadingIndicatorTemplates), typeof(ListView),
            new FrameworkPropertyMetadata(ZapLoadingIndicatorTemplates.ThreeDots,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnIndicatorTemplateChanged)));

        public ZapLoadingIndicatorTemplates IndicatorTemplate { get => (ZapLoadingIndicatorTemplates)GetValue(IndicatorTemplateProperty); set => SetValue(IndicatorTemplateProperty, value); }

        private static void OnIndicatorTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(IndicatorTemplateProperty, e.NewValue);
        #endregion

        #region IndicatorAccentColor
        public static readonly DependencyProperty IndicatorAccentColorProperty = DependencyProperty.Register(
            "IndicatorAccentColor", typeof(Brush), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnIndicatorAccentColorChanged)));

        public Brush IndicatorAccentColor { get => (Brush)GetValue(IndicatorAccentColorProperty); set => SetValue(IndicatorAccentColorProperty, value); }

        private static void OnIndicatorAccentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(IndicatorAccentColorProperty, e.NewValue);
        #endregion

        #region IndicatorHeight
        public static readonly DependencyProperty IndicatorHeightProperty = DependencyProperty.Register(
            "IndicatorHeight", typeof(double), typeof(ListView),
            new FrameworkPropertyMetadata(40d,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                new PropertyChangedCallback(OnIndicatorHeightChanged)));

        public double IndicatorHeight { get => (double)GetValue(IndicatorHeightProperty); set => SetValue(IndicatorHeightProperty, value); }

        private static void OnIndicatorHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(IndicatorHeightProperty, e.NewValue);
        #endregion

        #region IndicatorSpeedRatio
        /// <summary>
        /// Identifies the <see cref="IndicatorSpeedRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndicatorSpeedRatioProperty = DependencyProperty.Register(
            "IndicatorSpeedRatio", typeof(double), typeof(ListView), new PropertyMetadata(1d, new PropertyChangedCallback(OnSpeedRatioChanged)));

        /// <summary>
        /// Get/set the speed ratio of the animation.
        /// </summary>
        public double IndicatorSpeedRatio { get => (double)GetValue(IndicatorSpeedRatioProperty); set => SetValue(IndicatorSpeedRatioProperty, value); }

        private static void OnSpeedRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(IndicatorSpeedRatioProperty, e.NewValue);
        #endregion

        #region IndicatorWidth
        public static readonly DependencyProperty IndicatorWidthProperty = DependencyProperty.Register(
            "IndicatorWidth", typeof(double), typeof(ListView),
            new FrameworkPropertyMetadata(40d,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                new PropertyChangedCallback(OnIndicatorWidthChanged)));

        public double IndicatorWidth { get => (double)GetValue(IndicatorWidthProperty); set => SetValue(IndicatorWidthProperty, value); }

        private static void OnIndicatorWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(IndicatorWidthProperty, e.NewValue);
        #endregion
        #endregion
        #endregion
        #endregion

        #region Theme Properties
        #region DefaultThemeProperties
        public Dictionary<DependencyProperty, object> DefaultThemeProperties { get; internal set; } = new Dictionary<DependencyProperty, object>();
        #endregion

        #region Theme
        /// <summary>
        /// Get/Sets the theme
        /// </summary>
        public static DependencyProperty ThemeProperty = DependencyProperty.Register(
            "Theme", typeof(string), typeof(ListView),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }
        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.ThemeChanged(e, ThemeChangedEvent);

        private static object CoerceThemeChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion

        #region ThemeDictionaries
        public Dictionary<string, ResourceDictionary> ThemeDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion
        #endregion

        #region Native Properties Changed
        #region Background
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BorderBrushProperty, e.NewValue);
        #endregion

        #region BorderThickness
        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Events
        #region ItemDoubleClickEvent
        public delegate void ItemDoubleClickEventHandler(object sender, ListViewItemDoubleClickEventArgs e);

        public static readonly RoutedEvent ItemDoubleClickEvent = EventManager.RegisterRoutedEvent(
            "ItemDoubleClick", RoutingStrategy.Bubble, typeof(ItemDoubleClickEventHandler), typeof(ListView));

        public event ItemDoubleClickEventHandler ItemDoubleClick 
        { 
            add => AddHandler(ItemDoubleClickEvent, value); 
            remove => RemoveHandler(ItemDoubleClickEvent, value); 
        }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ListView));

        public event ITheme.ThemeChangedEventHandler ThemeChanged 
        { 
            add => AddHandler(ThemeChangedEvent, value); 
            remove => RemoveHandler(ThemeChangedEvent, value); 
        }

        private void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();
            // Control
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ListViewResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ListViewResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ListViewResourceKeys.BorderThicknessKey);

            this.SetThemePropertyDefault(DisabledBackgroundProperty, ResourceKeys.ListViewResourceKeys.DisabledBackgroundKey);
            this.SetThemePropertyDefault(DisabledBorderBrushProperty, ResourceKeys.ListViewResourceKeys.DisabledBorderBrushKey);
            // Header
            this.SetThemePropertyDefault(HeaderBackgroundProperty, ResourceKeys.ListViewResourceKeys.HeaderBackgroundKey);
            this.SetThemePropertyDefault(HeaderBorderBrushProperty, ResourceKeys.ListViewResourceKeys.HeaderBorderBrushKey);
            this.SetThemePropertyDefault(HeaderForegroundProperty, ResourceKeys.ListViewResourceKeys.HeaderForegroundKey);

            this.SetThemePropertyDefault(HeaderFocusedBackgroundProperty, ResourceKeys.ListViewResourceKeys.HeaderFocusedBackgroundKey);
            this.SetThemePropertyDefault(HeaderFocusedForegroundProperty, ResourceKeys.ListViewResourceKeys.HeaderFocusedForegroundKey);

            this.SetThemePropertyDefault(HeaderPressedBackgroundProperty, ResourceKeys.ListViewResourceKeys.HeaderPressedBackgroundKey);
            this.SetThemePropertyDefault(HeaderPressedForegroundProperty, ResourceKeys.ListViewResourceKeys.HeaderPressedForegroundKey);

            // Items
            this.SetThemePropertyDefault(ItemsBackgroundProperty, ResourceKeys.ListViewResourceKeys.ItemsBackgroundKey);
            this.SetThemePropertyDefault(ItemsBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ItemsBorderBrushKey);
            this.SetThemePropertyDefault(ItemsBorderThicknessProperty, ResourceKeys.ListViewResourceKeys.ItemsBorderThicknessKey);
            this.SetThemePropertyDefault(ItemsForegroundProperty, ResourceKeys.ListViewResourceKeys.ItemsForegroundKey);

            this.SetThemePropertyDefault(ItemsFocusedBackgroundProperty, ResourceKeys.ListViewResourceKeys.ItemsFocusedBackgroundKey);
            this.SetThemePropertyDefault(ItemsFocusedBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ItemsFocusedBorderBrushKey);
            this.SetThemePropertyDefault(ItemsFocusedForegroundProperty, ResourceKeys.ListViewResourceKeys.ItemsFocusedForegroundKey);

            this.SetThemePropertyDefault(ItemsSelectedActiveBackgroundProperty, ResourceKeys.ListViewResourceKeys.ItemsSelectedActiveBackgroundKey);
            this.SetThemePropertyDefault(ItemsSelectedInactiveBackgroundProperty, ResourceKeys.ListViewResourceKeys.ItemsSelectedInactiveBackgroundKey);
            this.SetThemePropertyDefault(ItemsSelectedActiveBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ItemsSelectedActiveBorderBrushKey);
            this.SetThemePropertyDefault(ItemsSelectedInactiveBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ItemsSelectedInactiveBorderBrushKey);
            this.SetThemePropertyDefault(ItemsSelectedForegroundProperty, ResourceKeys.ListViewResourceKeys.ItemsSelectedForegroundKey);

            // ScrollBars
            this.SetThemePropertyDefault(ScrollBarsBackgroundProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsBackgroundKey);
            this.SetThemePropertyDefault(ScrollBarsBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsBorderBrushKey);
            this.SetThemePropertyDefault(ScrollBarsBorderThicknessProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsBorderThicknessKey);

            this.SetThemePropertyDefault(ScrollBarsButtonBackgroundProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsButtonBackgroundKey);
            this.SetThemePropertyDefault(ScrollBarsButtonBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsButtonBorderBrushKey);
            this.SetThemePropertyDefault(ScrollBarsButtonBorderThicknessProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsButtonBorderThicknessKey);

            this.SetThemePropertyDefault(ScrollBarsThumbInnerBackgroundProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsThumbInnerBackgroundKey);
            this.SetThemePropertyDefault(ScrollBarsThumbBackgroundProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsThumbBackgroundKey);
            this.SetThemePropertyDefault(ScrollBarsThumbBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsThumbBorderBrushKey);
            this.SetThemePropertyDefault(ScrollBarsThumbBorderThicknessProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsThumbBorderThicknessKey);

            this.SetThemePropertyDefault(ScrollBarsDisabledThumbInnerBackgroundProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsDisabledThumbInnerBackgroundKey);
            this.SetThemePropertyDefault(ScrollBarsDisabledButtonBackgroundProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsDisabledButtonBackgroundKey);
            this.SetThemePropertyDefault(ScrollBarsDisabledButtonBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ScrollBarsDisabledButtonBorderBrushKey);

            // Progress
            this.SetThemePropertyDefault(ProgressBackgroundProperty, ResourceKeys.ListViewResourceKeys.ProgressBackgroundKey);
            this.SetThemePropertyDefault(ProgressBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ProgressBorderBrushKey);
            this.SetThemePropertyDefault(ProgressBorderThicknessProperty, ResourceKeys.ListViewResourceKeys.ProgressBorderThicknessKey);
            this.SetThemePropertyDefault(ProgressCornerRadiusProperty, ResourceKeys.ListViewResourceKeys.ProgressCornerRadiusKey);
            this.SetThemePropertyDefault(ProgressForegroundProperty, ResourceKeys.ListViewResourceKeys.ProgressForegroundKey);
            this.SetThemePropertyDefault(ProgressPaddingProperty, ResourceKeys.ListViewResourceKeys.ProgressPaddingKey);

            this.SetThemePropertyDefault(ProgressBarInnerBackgroundProperty, ResourceKeys.ListViewResourceKeys.ProgressBarInnerBackgroundKey);
            this.SetThemePropertyDefault(ProgressBarBackgroundProperty, ResourceKeys.ListViewResourceKeys.ProgressBarBackgroundKey);
            this.SetThemePropertyDefault(ProgressBarBorderBrushProperty, ResourceKeys.ListViewResourceKeys.ProgressBarBorderBrushKey);
            this.SetThemePropertyDefault(ProgressBarBorderThicknessProperty, ResourceKeys.ListViewResourceKeys.ProgressBarBorderThicknessKey);

            this.SetThemePropertyDefault(IndicatorTemplateProperty, ResourceKeys.ListViewResourceKeys.IndicatorTemplateKey);
            this.SetThemePropertyDefault(IndicatorAccentColorProperty, ResourceKeys.ListViewResourceKeys.IndicatorAccentColorKey);
            this.SetThemePropertyDefault(IndicatorHeightProperty, ResourceKeys.ListViewResourceKeys.IndicatorHeightKey);
            this.SetThemePropertyDefault(IndicatorSpeedRatioProperty, ResourceKeys.ListViewResourceKeys.IndicatorSpeedRatioKey);
            this.SetThemePropertyDefault(IndicatorWidthProperty, ResourceKeys.ListViewResourceKeys.IndicatorWidthKey);
        }
        #endregion
        #endregion

        #region Internal Event Handlers
        private void OnBtnClearClick(object sender, RoutedEventArgs e)
        {
            if (_headerRowPresenter != null)
            {
                foreach (var ctrl in VisualTreeHelpers.FindVisualChildrenByName(_headerRowPresenter, "search"))
                {
                    if (ctrl is System.Windows.Controls.DatePicker dp)
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

        #endregion

        #region Commands
        #region ColumnHeaderClickCommand
        /// <summary>
        /// Commande qui gère le clique sur l'entête des colonnes
        /// </summary>
        public ICommand ColumnHeaderClickCommand { get; }

        private void OnColumnHeaderClick(GridViewColumnHeader header)
        {

        }
        #endregion
        #endregion

        #region Constructors
        static ListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(typeof(ListView)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;
            // Control
            BackgroundProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
        }

        public ListView()
        {
            ColumnHeaderClickCommand = new RelayCommand<GridViewColumnHeader>(
                param => OnColumnHeaderClick(param),
                param => true);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterInternalThemes<ListViewThemes>();
            this.LoadDefaultTheme(ThemeProperty);

            Loaded += InternalListView_Loaded;
        }

        private void InternalListView_Loaded(object sender, RoutedEventArgs e)
        {
            AutoResizeColumns();
        }
        #endregion

        /// <summary>
        /// Méthode qui adapate la largeur des colonnes à leur contenu.
        /// </summary>
        public void ResizeColumns()
        {
            var sv = GetTemplateChild("scrollViewer") as ScrollViewer;
            double offset = sv?.HorizontalOffset ?? 0;

            if (View is GridView gridView)
            {
                foreach (var col in gridView.Columns)
                {
                    if (col is GridViewColumn zCol)
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
            sv?.ScrollToHorizontalOffset(offset);
        }

        /// <summary>
        /// Méthode qui adapate automatiquement la largeur des colonnes à leur contenu.
        /// </summary>
        private void AutoResizeColumns()
        {
            if (IsAutoResizeColumns)
            {
                ResizeColumns();
            }
        }

        #region Overrides
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_btnClear != null)
            {
                _btnClear.Click -= OnBtnClearClick;
                _btnClear = null;
            }

            if (GetTemplateChild("scrollViewer") is ScrollViewer viewer)
            {
                if (viewer.ApplyTemplate())
                {
                    if (viewer.Template.FindName("btnClearFilters", viewer) is ZapButton btn)
                    {
                        _btnClear = btn;
                        _btnClear.Click += OnBtnClearClick;
                    }

                    _headerRowPresenter = viewer.Template.FindName("headerRowPresenter", viewer) as GridViewHeaderRowPresenter;
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ListViewItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
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
        public bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            return Set(propertyName, ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <typeparam name="T">The type of the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(Expression<Func<T>> propertyNameExpression, ref T oldValue, T newValue)
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
        public bool Set<TClass, TProp>(Expression<Func<TClass, TProp>> propertyNameExpression, ref TProp oldValue, TProp newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (Equals(oldValue, newValue))
                return false;

            oldValue = newValue;
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
            return true;
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyName">The property name. </param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        public void RaisePropertyChanged(Expression<Func<object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        public void RaisePropertyChanged<TClass>(Expression<Func<TClass, object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="args">The arguments. </param>
        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>Raises the property changed event for all properties (string.Empty). </summary>
        public void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(string.Empty));
        }
        #endregion

        #region IDisposable implementation
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
        #endregion
    }

    #region Enums
    /// <summary>
    /// Types de recherche pouvant être utilisé sur une colonne.
    /// </summary>
    public enum ColumnSearchTypes
    {
        None = 0,
        Date = 1,
        Text = 2,
        ComboBox = 3
    }
    #endregion
}

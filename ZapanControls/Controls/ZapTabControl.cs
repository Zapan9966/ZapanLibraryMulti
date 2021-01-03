using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /// <summary>
    /// TabControl qui prend en charge l'ajout/suppression d'onglet.
    /// </summary>
    public sealed class ZapTabControl : TabControl, ITemplate
    {
        #region Fields
        private readonly ZapTabItemAdd _tabAdd;
        private bool _hasInitialized;
        #endregion

        #region Theme Declarations
        public static ThemePath Oceatech = new ThemePath(ZapTabControlThemes.Oceatech, "/ZapanControls;component/Themes/ZapTabControl/Oceatech.xaml");
        public static ThemePath Contactel = new ThemePath(ZapTabControlThemes.Contactel, "/ZapanControls;component/Themes/ZapTabControl/Contactel.xaml");
        #endregion

        #region Template Declarations
        public static TemplatePath Flat = new TemplatePath(ZapTabControlTemplates.Flat, "/ZapanControls;component/Themes/ZapTabControl/Template.Flat.xaml");
        public static TemplatePath Glass = new TemplatePath(ZapTabControlTemplates.Glass, "/ZapanControls;component/Themes/ZapTabControl/Template.Glass.xaml");
        #endregion

        #region Properties
        #region AddContent
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="AddButtonHeader"/>.
        /// </summary>
        private static readonly DependencyProperty AddContentProperty = DependencyProperty.Register(
            "AddContent", typeof(object), typeof(ZapTabControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini l'entête de l'onglet d'ajout.
        /// </summary>
        public object AddContent { get => GetValue(AddContentProperty); set => SetValue(AddContentProperty, value); }
        #endregion

        #region AddTooltip
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="AddTooltip"/>.
        /// </summary>
        private static readonly DependencyProperty AddTooltipProperty = DependencyProperty.Register(
            "AddTooltip", typeof(string), typeof(ZapTabControl), new FrameworkPropertyMetadata("Ajouter un onglet", FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini le texte de l'info-bulle du bouton Fermer.
        /// </summary>
        public string AddTooltip { get => (string)GetValue(AddTooltipProperty); set => SetValue(AddTooltipProperty, value); }
        #endregion

        #region CanAddItem
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="CanAddItem"/>.
        /// </summary>
        public static readonly DependencyProperty CanAddItemProperty = DependencyProperty.Register(
            "CanAddItem", typeof(bool), typeof(ZapTabControl), 
            new FrameworkPropertyMetadata(false, 
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender,
                OnCanAddItemChanged));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si des onglets peuvent être ajoutés/supprimés.
        /// </summary>
        public bool CanAddItem { get => (bool)GetValue(CanAddItemProperty); set => SetValue(CanAddItemProperty, value); }

        private static void OnCanAddItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
            if (d is ZapTabControl tc && e.NewValue is bool canAddItem)
            {
                if (canAddItem)
                {
                    tc.Items.Insert(0, tc._tabAdd);
                }
                else
                {
                    tc.Items.Remove(tc._tabAdd);
                }
            }
        }
        #endregion

        #region ItemNormal
        #region ItemBackground
        public static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.Register(
            "ItemBackground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemBackgroundChanged));

        public Brush ItemBackground { get => (Brush)GetValue(ItemBackgroundProperty); set => SetValue(ItemBackgroundProperty, value); }

        private static void OnItemBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemBackgroundProperty, e.NewValue);
        #endregion

        #region ItemBorderBrush
        public static readonly DependencyProperty ItemBorderBrushProperty = DependencyProperty.Register(
            "ItemBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemBorderBrushChanged));

        public Brush ItemBorderBrush { get => (Brush)GetValue(ItemBorderBrushProperty); set => SetValue(ItemBorderBrushProperty, value); }

        private static void OnItemBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemForeground
        public static readonly DependencyProperty ItemForegroundProperty = DependencyProperty.Register(
            "ItemForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemForegroundChanged));

        public Brush ItemForeground { get => (Brush)GetValue(ItemForegroundProperty); set => SetValue(ItemForegroundProperty, value); }

        private static void OnItemForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region ItemFocused
        #region ItemFocusedForeground
        public static readonly DependencyProperty ItemFocusedBackgroundProperty = DependencyProperty.Register(
            "ItemFocusedBackground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemFocusedBackgroundChanged));

        public Brush ItemFocusedBackground { get => (Brush)GetValue(ItemFocusedBackgroundProperty); set => SetValue(ItemFocusedBackgroundProperty, value); }

        private static void OnItemFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemFocusedBackgroundProperty, e.NewValue);
        #endregion

        #region ItemFocusedForeground
        public static readonly DependencyProperty ItemFocusedBorderBrushProperty = DependencyProperty.Register(
            "ItemFocusedBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemFocusedBorderBrushChanged));

        public Brush ItemFocusedBorderBrush { get => (Brush)GetValue(ItemFocusedBorderBrushProperty); set => SetValue(ItemFocusedBorderBrushProperty, value); }

        private static void OnItemFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemFocusedBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemFocusedForeground
        public static readonly DependencyProperty ItemFocusedForegroundProperty = DependencyProperty.Register(
            "ItemFocusedForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemFocusedForegroundChanged));

        public Brush ItemFocusedForeground { get => (Brush)GetValue(ItemFocusedForegroundProperty); set => SetValue(ItemFocusedForegroundProperty, value); }

        private static void OnItemFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemFocusedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region ItemSelected
        #region ItemSelectedForeground
        public static readonly DependencyProperty ItemSelectedBackgroundProperty = DependencyProperty.Register(
            "ItemSelectedBackground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemSelectedBackgroundChanged));

        public Brush ItemSelectedBackground { get => (Brush)GetValue(ItemSelectedBackgroundProperty); set => SetValue(ItemSelectedBackgroundProperty, value); }

        private static void OnItemSelectedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemSelectedBackgroundProperty, e.NewValue);
        #endregion

        #region ItemSelectedForeground
        public static readonly DependencyProperty ItemSelectedBorderBrushProperty = DependencyProperty.Register(
            "ItemSelectedBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemSelectedBorderBrushChanged));

        public Brush ItemSelectedBorderBrush { get => (Brush)GetValue(ItemSelectedBorderBrushProperty); set => SetValue(ItemSelectedBorderBrushProperty, value); }

        private static void OnItemSelectedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemSelectedBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemSelectedForeground
        public static readonly DependencyProperty ItemSelectedForegroundProperty = DependencyProperty.Register(
            "ItemSelectedForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemSelectedForegroundChanged));

        public Brush ItemSelectedForeground { get => (Brush)GetValue(ItemSelectedForegroundProperty); set => SetValue(ItemSelectedForegroundProperty, value); }

        private static void OnItemSelectedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemSelectedForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region ItemDisabled
        #region ItemDisabledForeground
        public static readonly DependencyProperty ItemDisabledBackgroundProperty = DependencyProperty.Register(
            "ItemDisabledBackground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemDisabledBackgroundChanged));

        public Brush ItemDisabledBackground { get => (Brush)GetValue(ItemDisabledBackgroundProperty); set => SetValue(ItemDisabledBackgroundProperty, value); }

        private static void OnItemDisabledBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemDisabledBackgroundProperty, e.NewValue);
        #endregion

        #region ItemDisabledForeground
        public static readonly DependencyProperty ItemDisabledBorderBrushProperty = DependencyProperty.Register(
            "ItemDisabledBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemDisabledBorderBrushChanged));

        public Brush ItemDisabledBorderBrush { get => (Brush)GetValue(ItemDisabledBorderBrushProperty); set => SetValue(ItemDisabledBorderBrushProperty, value); }

        private static void OnItemDisabledBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemDisabledBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemDisabledForeground
        public static readonly DependencyProperty ItemDisabledForegroundProperty = DependencyProperty.Register(
            "ItemDisabledForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemDisabledForegroundChanged));

        public Brush ItemDisabledForeground { get => (Brush)GetValue(ItemDisabledForegroundProperty); set => SetValue(ItemDisabledForegroundProperty, value); }

        private static void OnItemDisabledForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ItemDisabledForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region LastSelectedTab
        public object LastSelectedTab { get; private set; }
        #endregion
        #endregion

        #region Template Properties
        #region HasInitialized
        public bool HasInitialized { get => _hasInitialized; private set => Set(ref _hasInitialized, value); }
        #endregion

        #region TemplateDictionaries
        public Dictionary<string, ResourceDictionary> TemplateDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion

        #region ZapTemplate
        public static readonly DependencyProperty ZapTemplateProperty = DependencyProperty.Register(
            "ZapTemplate", typeof(string), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnZapTemplateChanged),
                new CoerceValueCallback(CoerceZapTemplateChange)));

        public string ZapTemplate { get => (string)GetValue(ZapTemplateProperty); set => SetValue(ZapTemplateProperty, value); }

        private static void OnZapTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.TemplateChanged(e, TemplateChangedEvent);

        private static object CoerceZapTemplateChange(DependencyObject d, object o)
        {
            return o;
        }
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
            "Theme", typeof(string), typeof(ZapTabControl),
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

        #region Foreground
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ForegroundProperty, e.NewValue);
        #endregion

        #region Padding
        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(PaddingProperty, e.NewValue);
        #endregion
        #endregion

        #region Events
        #region AddTab
        /// <summary>
        /// Représente la méthode qui gère l'ajout d'un nouvel onglet.
        /// </summary>
        public delegate void TabAddEventHandler(object sender, TabAddEventArgs e);

        public static readonly RoutedEvent TabAddEvent = EventManager.RegisterRoutedEvent(
            "TabAdd", RoutingStrategy.Bubble, typeof(TabAddEventHandler), typeof(ZapTabControl));

        /// <summary>
        /// Se produit lors de l'ajout d'un nouvel onglet.
        /// </summary>
        public event TabAddEventHandler TabAdd { add => AddHandler(TabAddEvent, value); remove => RemoveHandler(TabAddEvent, value); }
        #endregion

        #region CloseValidation
        /// <summary>
        /// Représente la méthode qui gère la validation de fermeture de l'onglet.
        /// </summary>
        /// <returns>Renvoi <see cref="True"/> si l'onglet doit être fermé, sinon <see cref="False"/></returns>
        public delegate void CloseValidationEventHandler(object sender, CloseValidationEventArgs e);

        public static readonly RoutedEvent CloseValidationEvent = EventManager.RegisterRoutedEvent(
            "CloseValidation", RoutingStrategy.Bubble, typeof(CloseValidationEventHandler), typeof(ZapTabControl));

        /// <summary>
        /// Se produit lors de la validation de la fermeture de l'onglet.
        /// </summary>
        public event CloseValidationEventHandler CloseValidation { add => AddHandler(CloseValidationEvent, value); remove => RemoveHandler(CloseValidationEvent, value); }
        #endregion

        #region ItemsChanged
        /// <summary>
        /// Se produit lorsque la collection est modifié.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> ItemsChanged;
        #endregion

        #region TemplateChanged
        public static readonly RoutedEvent TemplateChangedEvent = EventManager.RegisterRoutedEvent(
            "TemplateChanged", RoutingStrategy.Bubble, typeof(ITemplate.TemplateChangedEventHandler), typeof(ZapTabControl));

        public event ITemplate.TemplateChangedEventHandler TemplateChanged { add => AddHandler(TemplateChangedEvent, value); remove => RemoveHandler(TemplateChangedEvent, value); }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ZapTabControl));

        public event ITheme.ThemeChangedEventHandler ThemeChanged { add => AddHandler(ThemeChangedEvent, value); remove => RemoveHandler(ThemeChangedEvent, value); }

        private void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();
            // Control
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapTabControlResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapTabControlResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(ForegroundProperty, ResourceKeys.ZapTabControlResourceKeys.ForegroundKey);
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ZapTabControlResourceKeys.BorderThicknessKey);
            this.SetThemePropertyDefault(PaddingProperty, ResourceKeys.ZapTabControlResourceKeys.PaddingKey);
            this.SetThemePropertyDefault(AddContentProperty, ResourceKeys.ZapTabControlResourceKeys.AddContentKey);
            // ItemNormal
            this.SetThemePropertyDefault(ItemBackgroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemBackgroundKey);
            this.SetThemePropertyDefault(ItemBorderBrushProperty, ResourceKeys.ZapTabControlResourceKeys.ItemBorderBrushKey);
            this.SetThemePropertyDefault(ItemForegroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemForegroundKey);
            // ItemFocused
            this.SetThemePropertyDefault(ItemFocusedBackgroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemFocusedBackgroundKey);
            this.SetThemePropertyDefault(ItemFocusedBorderBrushProperty, ResourceKeys.ZapTabControlResourceKeys.ItemFocusedBorderBrushKey);
            this.SetThemePropertyDefault(ItemFocusedForegroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemFocusedForegroundKey);
            // ItemSelected
            this.SetThemePropertyDefault(ItemSelectedBackgroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemSelectedBackgroundKey);
            this.SetThemePropertyDefault(ItemSelectedBorderBrushProperty, ResourceKeys.ZapTabControlResourceKeys.ItemSelectedBorderBrushKey);
            this.SetThemePropertyDefault(ItemSelectedForegroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemSelectedForegroundKey);
            // ItemDisabled
            this.SetThemePropertyDefault(ItemDisabledBackgroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemDisabledBackgroundKey);
            this.SetThemePropertyDefault(ItemDisabledBorderBrushProperty, ResourceKeys.ZapTabControlResourceKeys.ItemDisabledBorderBrushKey);
            this.SetThemePropertyDefault(ItemDisabledForegroundProperty, ResourceKeys.ZapTabControlResourceKeys.ItemDisabledForegroundKey);
        }
        #endregion
        #endregion

        #region Internal Event Handlers
        private void InternalOnSelectionChanged(object sender, SelectionChangedEventArgs e)
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
        #endregion

        #region Constructors
        static ZapTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(typeof(ZapTabControl)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;
            // Control
            BackgroundProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            ForegroundProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(null, options, OnForegroundChanged) { Inherits = false });
            BorderThicknessProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
            PaddingProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(new Thickness(0), options, OnPaddingChanged));
        }

        public ZapTabControl()
        {
            _tabAdd = new ZapTabItemAdd();

            // Load Templates
            this.RegisterAttachedTemplates(typeof(ZapTabControl));
            this.LoadDefaultTemplate(ZapTemplateProperty);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterAttachedThemes(typeof(ZapTabControl));
            this.LoadDefaultTheme(ThemeProperty);

            SelectionChanged += InternalOnSelectionChanged;
        }
        #endregion

        #region Control Methods
        public void PerformAddTab(object parameter)
        {
            if (CanAddItem)
            {
                var eventArgs = new TabAddEventArgs(TabAddEvent, this, parameter);
                RaiseEvent(eventArgs);

                if (!eventArgs.Handled)
                    eventArgs.Handled = true;

                if (eventArgs.NewTabItem != null)
                {
                    Items.Add(eventArgs.NewTabItem);

                    if (eventArgs.NewTabItem is Control ctrl)
                        ctrl.Focus();

                    SelectedItem = eventArgs.NewTabItem;
                }
            }
        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;
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
                if (Items.Count > 0)
                    SelectedIndex = 1;
            }
            base.OnInitialized(e);
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
        public bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] String propertyName = null)
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
        public bool Set<TClass, TProp>(Expression<Func<TClass, TProp>> propertyNameExpression, ref TProp oldValue,
            TProp newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(String propertyName, ref T oldValue, T newValue)
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
    }
}

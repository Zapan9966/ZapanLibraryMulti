using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Themes;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /// <summary>
    /// TabControl qui prend en charge l'ajout/suppression d'onglet.
    /// </summary>
    public sealed class ZapTabControl : TabControl, ITheme, INotifyPropertyChanged
    {
        #region Property Name Constants
        private const string ThemePropName = "Theme";
        private const string TabsTemplatePropName = "TabsTemplate";
        #endregion

        #region Fields
        private readonly Dictionary<string, ResourceDictionary> _rdThemeDictionaries;
        private readonly Dictionary<string, ResourceDictionary> _rdTemplateDictionaries;
        private readonly Dictionary<DependencyProperty, object> _defaultThemeProperties;
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
                    if (!tc.Items.Cast<object>().Any(i => i is ZapTabItemAdd))
                        tc.Items.Insert(0, new ZapTabItemAdd());
                }
                else
                {
                    tc.Items.Cast<object>()
                        .Where(i => i is ZapTabItemAdd)
                        .ToList()
                        .ForEach(i => tc.Items.Remove(i));
                }
            }
        }
        #endregion

        #region HasInitialized
        public bool HasInitialized { get => _hasInitialized; private set => Set(ref _hasInitialized, value); }
        #endregion

        #region ItemNormal
        #region ItemBackground
        public static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.Register(
            "ItemBackground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemBackgroundChanged));

        public Brush ItemBackground { get => (Brush)GetValue(ItemBackgroundProperty); set => SetValue(ItemBackgroundProperty, value); }

        private static void OnItemBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemBackgroundProperty, e.NewValue);
        #endregion

        #region ItemBorderBrush
        public static readonly DependencyProperty ItemBorderBrushProperty = DependencyProperty.Register(
            "ItemBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemBorderBrushChanged));

        public Brush ItemBorderBrush { get => (Brush)GetValue(ItemBorderBrushProperty); set => SetValue(ItemBorderBrushProperty, value); }

        private static void OnItemBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemForeground
        public static readonly DependencyProperty ItemForegroundProperty = DependencyProperty.Register(
            "ItemForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemForegroundChanged));

        public Brush ItemForeground { get => (Brush)GetValue(ItemForegroundProperty); set => SetValue(ItemForegroundProperty, value); }

        private static void OnItemForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemForegroundProperty, e.NewValue);
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

        private static void OnItemFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemFocusedBackgroundProperty, e.NewValue);
        #endregion

        #region ItemFocusedForeground
        public static readonly DependencyProperty ItemFocusedBorderBrushProperty = DependencyProperty.Register(
            "ItemFocusedBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemFocusedBorderBrushChanged));

        public Brush ItemFocusedBorderBrush { get => (Brush)GetValue(ItemFocusedBorderBrushProperty); set => SetValue(ItemFocusedBorderBrushProperty, value); }

        private static void OnItemFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemFocusedBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemFocusedForeground
        public static readonly DependencyProperty ItemFocusedForegroundProperty = DependencyProperty.Register(
            "ItemFocusedForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemFocusedForegroundChanged));

        public Brush ItemFocusedForeground { get => (Brush)GetValue(ItemFocusedForegroundProperty); set => SetValue(ItemFocusedForegroundProperty, value); }

        private static void OnItemFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemFocusedForegroundProperty, e.NewValue);
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

        private static void OnItemSelectedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemSelectedBackgroundProperty, e.NewValue);
        #endregion

        #region ItemSelectedForeground
        public static readonly DependencyProperty ItemSelectedBorderBrushProperty = DependencyProperty.Register(
            "ItemSelectedBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemSelectedBorderBrushChanged));

        public Brush ItemSelectedBorderBrush { get => (Brush)GetValue(ItemSelectedBorderBrushProperty); set => SetValue(ItemSelectedBorderBrushProperty, value); }

        private static void OnItemSelectedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemSelectedBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemSelectedForeground
        public static readonly DependencyProperty ItemSelectedForegroundProperty = DependencyProperty.Register(
            "ItemSelectedForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemSelectedForegroundChanged));

        public Brush ItemSelectedForeground { get => (Brush)GetValue(ItemSelectedForegroundProperty); set => SetValue(ItemSelectedForegroundProperty, value); }

        private static void OnItemSelectedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemSelectedForegroundProperty, e.NewValue);
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

        private static void OnItemDisabledBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemDisabledBackgroundProperty, e.NewValue);
        #endregion

        #region ItemDisabledForeground
        public static readonly DependencyProperty ItemDisabledBorderBrushProperty = DependencyProperty.Register(
            "ItemDisabledBorderBrush", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemDisabledBorderBrushChanged));

        public Brush ItemDisabledBorderBrush { get => (Brush)GetValue(ItemDisabledBorderBrushProperty); set => SetValue(ItemDisabledBorderBrushProperty, value); }

        private static void OnItemDisabledBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemDisabledBorderBrushProperty, e.NewValue);
        #endregion

        #region ItemDisabledForeground
        public static readonly DependencyProperty ItemDisabledForegroundProperty = DependencyProperty.Register(
            "ItemDisabledForeground", typeof(Brush), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnItemDisabledForegroundChanged));

        public Brush ItemDisabledForeground { get => (Brush)GetValue(ItemDisabledForegroundProperty); set => SetValue(ItemDisabledForegroundProperty, value); }

        private static void OnItemDisabledForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ItemDisabledForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region LastSelectedTab
        public object LastSelectedTab { get; private set; }
        #endregion

        #region TabsTemplate
        public static readonly DependencyProperty TabsTemplateProperty = DependencyProperty.Register(
            "TabsTemplate", typeof(string), typeof(ZapTabControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnWindowTemplateChanged),
                new CoerceValueCallback(CoerceWindowTemplateChange)));

        public string TabsTemplate { get => (string)GetValue(TabsTemplateProperty); set => SetValue(TabsTemplateProperty, value); }

        private static void OnWindowTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapTabControl tc) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            string curTemplateName = e.OldValue as string;
            string curRegisteredTemplateName = tc.GetRegistrationName(curTemplateName, tc.GetType());

            if (tc._rdTemplateDictionaries.ContainsKey(curRegisteredTemplateName))
            {
                // remove current template
                ResourceDictionary curTemplateDictionary = tc._rdTemplateDictionaries[curRegisteredTemplateName];
                tc.Resources.MergedDictionaries.Remove(curTemplateDictionary);

                foreach (var item in tc.Items)
                {
                    if (item is ZapTabItem tab)
                        tab.Resources.MergedDictionaries.Remove(curTemplateDictionary);
                }
            }

            // new template name
            string newTemplateName = e.NewValue as string;
            string newRegisteredTemplateName = !string.IsNullOrEmpty(newTemplateName) ?
                tc.GetRegistrationName(newTemplateName, tc.GetType())
                : tc._rdTemplateDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!tc._rdTemplateDictionaries.ContainsKey(newRegisteredTemplateName))
            {
                throw new ArgumentNullException("Invalid Template property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newTemplateDictionary = tc._rdTemplateDictionaries[newRegisteredTemplateName];
                tc.Resources.MergedDictionaries.Add(newTemplateDictionary);

                foreach (var item in tc.Items)
                {
                    if (item is ZapTabItem tab)
                        tab.Resources.MergedDictionaries.Add(newTemplateDictionary);
                }

                if (tc.HasInitialized)
                {
                    tc.OnApplyTemplate();
                    foreach (var tab in tc.Items)
                    {
                        if (tab is ZapTabItem zTab)
                            zTab.OnApplyTemplate();
                    }
                }
            }

            tc.RaisePropertyChanged(new PropertyChangedEventArgs(TabsTemplatePropName));
        }

        private static object CoerceWindowTemplateChange(DependencyObject d, object o)
        {
            return o;
        }
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

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapTabControl tc) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            // current theme
            string curThemeName = e.OldValue as string;
            string curRegisteredThemeName = tc.GetRegistrationName(curThemeName, tc.GetType());

            if (tc._rdThemeDictionaries.ContainsKey(curRegisteredThemeName))
            {
                // remove current theme
                ResourceDictionary curThemeDictionary = tc._rdThemeDictionaries[curRegisteredThemeName];
                tc.Resources.MergedDictionaries.Remove(curThemeDictionary);
            }

            // new theme name
            string newThemeName = e.NewValue as string;
            string newRegisteredThemeName = !string.IsNullOrEmpty(newThemeName) ?
                tc.GetRegistrationName(newThemeName, tc.GetType())
                : tc._rdThemeDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!tc._rdThemeDictionaries.ContainsKey(newRegisteredThemeName))
            {
                throw new ArgumentNullException("Invalid Theme property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newThemeDictionary = tc._rdThemeDictionaries[newRegisteredThemeName];
                tc.Resources.MergedDictionaries.Add(newThemeDictionary);
                // Raise theme successfully changed event
                tc.RaiseEvent(new RoutedEventArgs(ThemeChangedSuccessEvent, tc));
            }

            tc.RaisePropertyChanged(new PropertyChangedEventArgs(ThemePropName));
        }

        private static object CoerceThemeChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion
        #endregion

        #region Native Properties Changed
        #region Background
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, BorderBrushProperty, e.NewValue);
        #endregion

        #region BorderThickness
        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, BorderThicknessProperty, e.NewValue);
        #endregion

        #region Foreground
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, ForegroundProperty, e.NewValue);
        #endregion

        #region Padding
        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, PaddingProperty, e.NewValue);
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
        public delegate void CloseValidationEventHandler(object sender, CloseValidationEnventArgs e);

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

        #region ThemeChangedSuccess
        public static readonly RoutedEvent ThemeChangedSuccessEvent = EventManager.RegisterRoutedEvent(
            "ThemeChangedSuccess", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapTabControl));

        public event RoutedEventHandler ThemeChangedSuccess { add => AddHandler(ThemeChangedSuccessEvent, value); remove => RemoveHandler(ThemeChangedSuccessEvent, value); }

        private void OnThemeChangedSuccess(object sender, RoutedEventArgs e)
        {
            _defaultThemeProperties.Clear();
            // Control
            SetThemePropertyDefault(BackgroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.BackgroundKey));
            SetThemePropertyDefault(BorderBrushProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.BorderBrushKey));
            SetThemePropertyDefault(ForegroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ForegroundKey));
            SetThemePropertyDefault(BorderThicknessProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.BorderThicknessKey));
            SetThemePropertyDefault(PaddingProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.PaddingKey));
            SetThemePropertyDefault(AddContentProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.AddContentKey));
            // ItemNormal
            SetThemePropertyDefault(ItemBackgroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemBackgroundKey));
            SetThemePropertyDefault(ItemBorderBrushProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemBorderBrushKey));
            SetThemePropertyDefault(ItemForegroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemForegroundKey));
            // ItemFocused
            SetThemePropertyDefault(ItemFocusedBackgroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemFocusedBackgroundKey));
            SetThemePropertyDefault(ItemFocusedBorderBrushProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemFocusedBorderBrushKey));
            SetThemePropertyDefault(ItemFocusedForegroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemFocusedForegroundKey));
            // ItemSelected
            SetThemePropertyDefault(ItemSelectedBackgroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemSelectedBackgroundKey));
            SetThemePropertyDefault(ItemSelectedBorderBrushProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemSelectedBorderBrushKey));
            SetThemePropertyDefault(ItemSelectedForegroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemSelectedForegroundKey));
            // ItemDisabled
            SetThemePropertyDefault(ItemDisabledBackgroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemDisabledBackgroundKey));
            SetThemePropertyDefault(ItemDisabledBorderBrushProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemDisabledBorderBrushKey));
            SetThemePropertyDefault(ItemDisabledForegroundProperty, TryFindResource(ResourceKeys.ZapTabControlResourceKeys.ItemDisabledForegroundKey));
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
            ForegroundProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(null, options, OnForegroundChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
            PaddingProperty.OverrideMetadata(typeof(ZapTabControl), new FrameworkPropertyMetadata(new Thickness(0), options, OnPaddingChanged));
        }

        public ZapTabControl()
        {
            _defaultThemeProperties = new Dictionary<DependencyProperty, object>();

            _rdTemplateDictionaries = new Dictionary<string, ResourceDictionary>();
            RegisterAttachedTemplates();

            // Load first template
            if (_rdTemplateDictionaries.Any())
                SetCurrentValue(TabsTemplateProperty, GetThemeName(_rdTemplateDictionaries.FirstOrDefault().Key));

            _rdThemeDictionaries = new Dictionary<string, ResourceDictionary>();
            RegisterAttachedThemes();

            ThemeChangedSuccess += OnThemeChangedSuccess;

            // Load first theme
            if (_rdThemeDictionaries.Any())
                SetCurrentValue(ThemeProperty, GetThemeName(_rdThemeDictionaries.FirstOrDefault().Key));

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

            foreach (var property in _defaultThemeProperties)
            {
                // Update bindings with theme default values
                if (BindingOperations.GetBinding(this, property.Key) is BindingBase binding)
                {
                    var newBinding = binding.Clone();

                    if (binding.FallbackValue == null || binding.FallbackValue == DependencyProperty.UnsetValue)
                        newBinding.FallbackValue = property.Value;

                    if (binding.TargetNullValue == null || binding.TargetNullValue == DependencyProperty.UnsetValue)
                        newBinding.TargetNullValue = property.Value;

                    SetBinding(property.Key, newBinding);
                }
            }
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

        #region Theming
        /// <summary>
        /// Register a theme with internal dictionary
        /// </summary>
        public void RegisterTheme(ThemePath theme, Type ownerType)
        {
            // test args
            if (theme.Name == null || theme.DictionaryPath == null)
                throw new ArgumentNullException("Theme name/path is null");

            if (ownerType == null)
                throw new ArgumentNullException("Invalid ownerType");

            string registrationName = GetRegistrationName(theme, ownerType);

            try
            {
                if (!_rdThemeDictionaries.ContainsKey(registrationName))
                {
                    // create the Uri
                    Uri themeUri = new Uri(theme.DictionaryPath, UriKind.Relative);
                    // register the new theme
                    _rdThemeDictionaries[registrationName] = Application.LoadComponent(themeUri) as ResourceDictionary;
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Instance theme dictionary and add themes
        /// </summary>
        private void RegisterAttachedThemes()
        {
            // Attach control attached themes
            var themeFields = GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(ThemePath));

            foreach (var field in themeFields)
            {
                RegisterTheme((ThemePath)field.GetValue(this), GetType());
            }
        }

        /// <summary>
        /// Load the default theme
        /// </summary>
        internal void LoadDefaultTheme(ZapTabControlThemes theme, Type ownerType)
        {
            string registrationName = GetRegistrationName(theme, ownerType);
            Resources.MergedDictionaries.Add(_rdThemeDictionaries[registrationName]);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        private string GetRegistrationName(ZapTabControlThemes theme, Type ownerType)
        {
            return GetRegistrationName(theme.ToString(), ownerType);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        private string GetRegistrationName(ThemePath theme, Type ownerType)
        {
            return GetRegistrationName(theme.Name, ownerType);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        public string GetRegistrationName(string themeName, Type ownerType)
        {
            return $"{ownerType};{themeName}";
        }

        /// <summary>
        /// Get themes name from formal registration name
        /// </summary>
        public string GetThemeName(string key)
        {
            return key?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        /// <summary>
        /// Set themes default propertie value
        /// </summary>
        internal void SetThemePropertyDefault(DependencyProperty p, object value)
        {
            _defaultThemeProperties.Add(p, value);
            SetCurrentValue(p, value);
        }

        /// <summary>
        /// Set dependency property default theme value if value is null
        /// </summary>
        private static void SetValueCommon(DependencyObject o, DependencyProperty p, object value)
        {
            if (o is ZapTabControl tc)
            {
                if (!(BindingOperations.GetBinding(tc, p) is Binding))
                {
                    if (value is Thickness t)
                    {
                        if (t == new Thickness(0))
                            value = null;
                    }
                    else if (value is double d)
                    {
                        if (d == 0d)
                            value = null;
                    }

                    if (value == null)
                    {
                        if (tc._defaultThemeProperties.ContainsKey(p))
                            value = tc._defaultThemeProperties[p];

                        tc.SetCurrentValue(p, value);
                    }
                }
            }
        }
        #endregion

        #region Templating
        public void RegisterTemplate(TemplatePath template, Type ownerType)
        {
            // test args
            if (template.Name == null || template.DictionaryPath == null)
                throw new ArgumentNullException("Template name/path is null");

            if (ownerType == null)
                throw new ArgumentNullException("Invalid ownerType");

            string registrationName = GetRegistrationName(template.Name, ownerType);

            try
            {
                if (!_rdTemplateDictionaries.ContainsKey(registrationName))
                {
                    // create the Uri
                    Uri templateUri = new Uri(template.DictionaryPath, UriKind.Relative);
                    // register the new template
                    _rdTemplateDictionaries[registrationName] = Application.LoadComponent(templateUri) as ResourceDictionary;
                }
            }
            catch (Exception)
            { }
        }

        private void RegisterAttachedTemplates()
        {
            var templateFields = GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(TemplatePath));

            foreach (var field in templateFields)
            {
                RegisterTemplate((TemplatePath)field.GetValue(this), GetType());
            }
        }

        /// <summary>
        /// Load the default template
        /// </summary>
        internal void LoadDefaultTemplate(string registrationName)
        {
            Resources.MergedDictionaries.Add(_rdTemplateDictionaries[registrationName]);
        }

        private string GetTemplateName(string key)
        {
            return key?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1];
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

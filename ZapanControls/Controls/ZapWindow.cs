using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Themes;
using ZapanControls.Helpers;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /// <summary>
    /// Fenêtre personnalisée
    /// </summary>
    public partial class ZapWindow : Window, ITheme, INotifyPropertyChanged
    {
        #region Property Name Constants
        private const string ThemePropName = "Theme";
        private const string WindowTemplatePropName = "WindowTemplate";
        #endregion

        #region Fields
        private readonly Dictionary<string, ResourceDictionary> _rdThemeDictionaries;
        private readonly Dictionary<string, ResourceDictionary> _rdTemplateDictionaries;
        private readonly Dictionary<DependencyProperty, object> _defaultThemeProperties;
        private readonly DeferredAction _centerDeferred;

        private HwndSource _hwndSource;
        private bool _startupResized;
        private bool _hasInitialized;
        private bool _isReady;
        #endregion

        #region Theme Declarations
        public static ThemePath Oceatech = new ThemePath(ZapWindowThemes.Oceatech, "/ZapanControls;component/Themes/ZapWindow/Oceatech.xaml");
        public static ThemePath Contactel = new ThemePath(ZapWindowThemes.Contactel, "/ZapanControls;component/Themes/ZapWindow/Contactel.xaml");
        #endregion

        #region Template Declarations
        public static TemplatePath Flat = new TemplatePath(ZapWindowTemplates.Flat, "/ZapanControls;component/Themes/ZapWindow/Template.Flat.xaml");
        public static TemplatePath Glass = new TemplatePath(ZapWindowTemplates.Glass, "/ZapanControls;component/Themes/ZapWindow/Template.Glass.xaml");
        #endregion

        #region Properties
        #region GridDim
        /// <summary>
        /// Obtient la grille qui grise la fenêtre.
        /// </summary>
        public Grid GridDim { get; private set; }
        #endregion

        #region HasInitialized
        public bool HasInitialized { get => _hasInitialized; private set => Set(ref _hasInitialized, value); }
        #endregion

        #region IsReady
        /// <summary>
        /// Obtient une valeur permettant de savoir si la fenêtre est active.
        /// </summary>
        public bool IsReady { get => _isReady; internal set => Set(ref _isReady, value); }
        #endregion

        #region Buttons
        #region CanBeClosed
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="CanClosed"/>.
        /// </summary>
        public static readonly DependencyProperty CanBeClosedProperty = DependencyProperty.Register(
            "CanBeClosed", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Fermer est activé.
        /// </summary>
        public bool CanBeClosed { get => (bool)GetValue(CanBeClosedProperty); set => SetValue(CanBeClosedProperty, value); }
        #endregion

        #region ShowCloseButton
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ShowCloseButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            "ShowCloseButton", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Fermer est visible.
        /// </summary>
        public bool ShowCloseButton { get => (bool)GetValue(ShowCloseButtonProperty); set => SetValue(ShowCloseButtonProperty, value); }
        #endregion

        #region ShowMaximizeButton
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ShowMaximizeButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register(
            "ShowMaximizeButton", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Maximiser est visible.
        /// </summary>
        public bool ShowMaximizeButton { get => (bool)GetValue(ShowMaximizeButtonProperty); set => SetValue(ShowMaximizeButtonProperty, value); }
        #endregion

        #region ShowMinimizeButton
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ShowMinimizeButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register(
            "ShowMinimizeButton", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Minimiser est visible.
        /// </summary>
        public bool ShowMinimizeButton { get => (bool)GetValue(ShowMinimizeButtonProperty); set => SetValue(ShowMinimizeButtonProperty, value); }
        #endregion
        #endregion

        #region TitleBar
        #region TitleBarBackground
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="TitleBarBackground"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(
            "TitleBarBackground", typeof(Brush), typeof(ZapWindow), 
            new FrameworkPropertyMetadata(null, 
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnTitleBarBackgroundChanged));

        /// <summary>
        /// Obtient ou défini la couleur de fond de la barre de titre.
        /// </summary>
        public Brush TitleBarBackground { get => (Brush)GetValue(TitleBarBackgroundProperty); set => SetValue(TitleBarBackgroundProperty, value); }

        private static void OnTitleBarBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, TitleBarBackgroundProperty, e.NewValue);
        #endregion

        #region TitleBarForeground
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="TitleBarForeground"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register(
            "TitleBarForeground", typeof(Brush), typeof(ZapWindow), 
            new FrameworkPropertyMetadata(null, 
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnTitleBarForegroundChanged));

        /// <summary>
        /// Obtient ou défini la couleur de la police de la barre de titre.
        /// </summary>
        public Brush TitleBarForeground { get => (Brush)GetValue(TitleBarForegroundProperty); set => SetValue(TitleBarForegroundProperty, value); }

        private static void OnTitleBarForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, TitleBarForegroundProperty, e.NewValue);
        #endregion

        #region TitleBarHeight
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="TitleBarHeight"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(
            "TitleBarHeight", typeof(double), typeof(ZapWindow), 
            new FrameworkPropertyMetadata(30d, 
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnTitleBarHeightChanged));

        /// <summary>
        /// Obtient ou défini la hauteur de la barre de titre.
        /// </summary>
        public double TitleBarHeight { get => (double)GetValue(TitleBarHeightProperty); set => SetValue(TitleBarHeightProperty, value); }

        private static void OnTitleBarHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => SetValueCommon(d, TitleBarHeightProperty, e.NewValue);
        #endregion
        #endregion

        #region Theme
        /// <summary>
        /// Get/Sets the theme
        /// </summary>
        public static DependencyProperty ThemeProperty = DependencyProperty.Register(
            "Theme", typeof(string), typeof(ZapWindow),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapWindow w) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            // current theme
            string curThemeName = e.OldValue as string;
            string curRegisteredThemeName = w.GetRegistrationName(curThemeName, w.GetType());

            if (w._rdThemeDictionaries.ContainsKey(curRegisteredThemeName))
            {
                // remove current theme
                ResourceDictionary curThemeDictionary = w._rdThemeDictionaries[curRegisteredThemeName];
                w.Resources.MergedDictionaries.Remove(curThemeDictionary);
            }

            // new theme name
            string newThemeName = e.NewValue as string;
            string newRegisteredThemeName = !string.IsNullOrEmpty(newThemeName) ?
                w.GetRegistrationName(newThemeName, w.GetType())
                : w._rdThemeDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!w._rdThemeDictionaries.ContainsKey(newRegisteredThemeName))
            {
                throw new ArgumentNullException("Invalid Theme property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newThemeDictionary = w._rdThemeDictionaries[newRegisteredThemeName];
                w.Resources.MergedDictionaries.Add(newThemeDictionary);
                // Raise theme successfully changed event
                w.RaiseEvent(new RoutedEventArgs(ThemeChangedSuccessEvent, w));
            }

            w.RaisePropertyChanged(new PropertyChangedEventArgs(ThemePropName));
        }

        private static object CoerceThemeChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion

        #region WindowTemplate
        public static readonly DependencyProperty WindowTemplateProperty = DependencyProperty.Register(
            "WindowTemplate", typeof(string), typeof(ZapWindow),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnWindowTemplateChanged),
                new CoerceValueCallback(CoerceWindowTemplateChange)));

        public string WindowTemplate { get => (string)GetValue(WindowTemplateProperty); set => SetValue(WindowTemplateProperty, value); }

        private static void OnWindowTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // test args
            if (!(d is ZapWindow w) || e == null)
                throw new ArgumentNullException("Invalid Theme property");

            string curTemplateName = e.OldValue as string;
            string curRegisteredTemplateName = w.GetRegistrationName(curTemplateName, w.GetType());

            if (w._rdTemplateDictionaries.ContainsKey(curRegisteredTemplateName))
            {
                // remove current template
                ResourceDictionary curTemplateDictionary = w._rdTemplateDictionaries[curRegisteredTemplateName];
                w.Resources.MergedDictionaries.Remove(curTemplateDictionary);
            }

            // new template name
            string newTemplateName = e.NewValue as string;
            string newRegisteredTemplateName = !string.IsNullOrEmpty(newTemplateName) ? 
                w.GetRegistrationName(newTemplateName, w.GetType())
                : w._rdTemplateDictionaries.FirstOrDefault().Key;

            // add the resource
            if (!w._rdTemplateDictionaries.ContainsKey(newRegisteredTemplateName))
            {
                throw new ArgumentNullException("Invalid Template property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newTemplateDictionary = w._rdTemplateDictionaries[newRegisteredTemplateName];
                w.Resources.MergedDictionaries.Add(newTemplateDictionary);

                if (w.HasInitialized)
                    w.OnApplyTemplate();
            }

            w.RaisePropertyChanged(new PropertyChangedEventArgs(WindowTemplatePropName));
        }

        private static object CoerceWindowTemplateChange(DependencyObject d, object o)
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
        #endregion

        #region Events
        #region Minimizing
        public static readonly RoutedEvent MinimizingEvent = EventManager.RegisterRoutedEvent(
            "Minimizing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lorsque la fenêtre est minimisée.
        /// </summary>
        public event RoutedEventHandler Minimizing { add => AddHandler(MinimizingEvent, value); remove => RemoveHandler(MinimizingEvent, value); }
        #endregion

        #region Maximizing
        public static readonly RoutedEvent MaximizingEvent = EventManager.RegisterRoutedEvent(
            "Maximizing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lorsque la fenêtre est maximisée.
        /// </summary>
        public event RoutedEventHandler Maximizing { add => AddHandler(MaximizingEvent, value); remove => RemoveHandler(MaximizingEvent, value); }
        #endregion

        #region Restoring
        public static readonly RoutedEvent RestoringEvent = EventManager.RegisterRoutedEvent(
            "Restoring", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lorsque la fenêtre est restaurée.
        /// </summary>
        public event RoutedEventHandler Restoring { add => AddHandler(RestoringEvent, value); remove => RemoveHandler(RestoringEvent, value); }
        #endregion

        #region CloseValidation
        /// <summary>
        /// Représente la méthode qui gère la validation de fermeture de la fenêtre.
        /// </summary>
        /// <returns>Renvoi <see cref="True"/> si la fenêtre doit être fermée, sinon <see cref="False"/></returns>
        public delegate void CloseValidationEventHandler(object sender, WindowCloseValidationEnventArgs e);

        public static readonly RoutedEvent CloseValidationEvent = EventManager.RegisterRoutedEvent(
            "CloseValidation", RoutingStrategy.Bubble, typeof(CloseValidationEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lors de la validation de la fermeture de la fenêtre.
        /// </summary>
        public event CloseValidationEventHandler CloseValidation { add => AddHandler(CloseValidationEvent, value); remove => RemoveHandler(CloseValidationEvent, value); }
        #endregion

        #region ThemeChangedSuccessEvent
        public static readonly RoutedEvent ThemeChangedSuccessEvent = EventManager.RegisterRoutedEvent(
            "ThemeChangedSuccess", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapWindow));

        public event RoutedEventHandler ThemeChangedSuccess { add => AddHandler(ThemeChangedSuccessEvent, value); remove => RemoveHandler(ThemeChangedSuccessEvent, value); }

        protected virtual void OnThemeChangedSuccess(object sender, RoutedEventArgs e)
        {
            _defaultThemeProperties.Clear();
            // Control
            SetThemePropertyDefault(BackgroundProperty, TryFindResource(ResourceKeys.ZapWindowResourceKeys.BackgroundKey));
            SetThemePropertyDefault(BorderBrushProperty, TryFindResource(ResourceKeys.ZapWindowResourceKeys.BorderBrushKey));
            SetThemePropertyDefault(BorderThicknessProperty, TryFindResource(ResourceKeys.ZapWindowResourceKeys.BorderThicknessKey));
            // TitleBar
            SetThemePropertyDefault(TitleBarBackgroundProperty, TryFindResource(ResourceKeys.ZapWindowResourceKeys.TitleBarBackgroundKey));
            SetThemePropertyDefault(TitleBarForegroundProperty, TryFindResource(ResourceKeys.ZapWindowResourceKeys.TitleBarForegroundKey));
            SetThemePropertyDefault(TitleBarHeightProperty, TryFindResource(ResourceKeys.ZapWindowResourceKeys.TitleBarHeightKey));
        }
        #endregion
        #endregion

        #region Internal Event Handlers
        /// <summary>
        /// Méthode qui gère le déplacement de la fenêtre.
        /// </summary>
        /// <param name="sender">Object qui a généré le déplacement.</param>
        /// <param name="e">Fournit les données sur le déplacement de la souris.</param>
        private void OnMoveBorderClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (ShowMaximizeButton)
                {
                    // Raise maximizing event
                    RaiseEvent(new RoutedEventArgs(MaximizingEvent, this));
                    WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
                }
                else
                {
                    CenterWindow();
                }
            }
            else if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void OnBtnCloseClick(object sender, RoutedEventArgs e)
        {
            var eventArgs = new WindowCloseValidationEnventArgs(CloseValidationEvent, this);
            RaiseEvent(eventArgs);

            if (!eventArgs.Handled)
                eventArgs.Handled = true;

            if (eventArgs.CanClose)
                Close();
        }

        private void OnBtnMinimizeClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MinimizingEvent, this));
            WindowState = WindowState.Minimized;
        }

        private void OnBtnRestoreClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                RaiseEvent(new RoutedEventArgs(MaximizingEvent, this));
                WindowState = WindowState.Maximized;
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(RestoringEvent, this));
                WindowState = WindowState.Normal;
            }
        }
        #endregion

        #region Constructors
        static ZapWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapWindow), new FrameworkPropertyMetadata(typeof(ZapWindow)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;

            BackgroundProperty.OverrideMetadata(typeof(ZapWindow), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapWindow), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapWindow), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
        }

        public ZapWindow()
            : base()
        {
            _startupResized = false;
            _centerDeferred = DeferredAction.Create(() => CenterWindowDefered());
            _defaultThemeProperties = new Dictionary<DependencyProperty, object>();

            _rdTemplateDictionaries = new Dictionary<string, ResourceDictionary>();
            RegisterAttachedTemplates();

            // Load first template
            if (_rdTemplateDictionaries.Any())
                SetCurrentValue(WindowTemplateProperty, GetThemeName(_rdTemplateDictionaries.FirstOrDefault().Key));

            _rdThemeDictionaries = new Dictionary<string, ResourceDictionary>();
            RegisterAttachedThemes();

            ThemeChangedSuccess += OnThemeChangedSuccess;

            // Load first theme
            if (_rdThemeDictionaries.Any())
                SetCurrentValue(ThemeProperty, GetThemeName(_rdThemeDictionaries.FirstOrDefault().Key));
        }

        #endregion

        #region Control Methods
        /// <summary>
        /// Méthode permettant de centrer la fenêtre sur la fenêtre parent ou dans l'écran.
        /// </summary>
        public void CenterWindow(int milliseconds = 250)
        {
            CenterWindow(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Méthode permettant de centrer la fenêtre sur la fenêtre parent ou dans l'écran.
        /// </summary>
        public void CenterWindow(TimeSpan timeSpan)
        {
            _centerDeferred.Defer(timeSpan);
        }

        /// <summary>
        /// Méthode déférée utilisée pour centrer la fenêtre sur la fenêtre parent ou dans l'écran.
        /// </summary>
        private void CenterWindowDefered()
        {
            if (WindowStartupLocation == WindowStartupLocation.CenterScreen || Owner?.WindowState == WindowState.Maximized)
            {
                System.Windows.Forms.Screen currentScreen = ScreenHelpers.GetCurrentScreen(this);

                Left = ((currentScreen.WorkingArea.Width - ActualWidth) / 2) + currentScreen.WorkingArea.X;
                Top = ((currentScreen.WorkingArea.Height - ActualHeight) / 2) + currentScreen.WorkingArea.Y;
            }
            else if (WindowStartupLocation == WindowStartupLocation.CenterOwner)
            {
                if (Owner != null)
                {
                    Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
                    Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
                }
            }
        }
        #endregion

        #region Overrides
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            // Permet d'éviter les bordures noires lors de la création d'une nouvelle fenêtre
            InvalidateVisual();

            if (ComponentDispatcher.IsThreadModal)
                if (Owner is ZapWindow win)
                    win.GridDim.Visibility = Visibility.Visible;

            base.OnContentRendered(e);

            if (!_startupResized)
            {
                CenterWindow();
                _startupResized = true;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
            // Pour le redimensionnement de la fenêtre en plein écran
            _hwndSource.AddHook(NativeMethods.MaximizedSizeFixWindowProc);

            base.OnSourceInitialized(e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            IsReady = true;
            base.OnActivated(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (ComponentDispatcher.IsThreadModal)
                if (Owner is ZapWindow win)
                    win.GridDim.Visibility = Visibility.Collapsed;

            base.OnClosed(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;

            if (GetTemplateChild("gridDim") is Grid gridDim)
                GridDim = gridDim;

            if (GetTemplateChild("moveBorder") is Border moveBorder)
            {
                moveBorder.MouseLeftButtonDown -= OnMoveBorderClick;
                moveBorder.MouseLeftButtonDown += OnMoveBorderClick;
            }

            if (VisualTreeHelpers.FindChild(this, "btnMinimize") is ZapButton btnMinimize)
            {
                btnMinimize.Click -= OnBtnMinimizeClick;
                btnMinimize.Click += OnBtnMinimizeClick;
            }

            if (VisualTreeHelpers.FindChild(this, "btnRestore") is ZapButton btnRestore)
            {
                btnRestore.Click -= OnBtnRestoreClick;
                btnRestore.Click += OnBtnRestoreClick;
            }

            if (VisualTreeHelpers.FindChild(this, "btnClose") is ZapButton btnClose)
            {
                btnClose.Click -= OnBtnCloseClick;
                btnClose.Click += OnBtnCloseClick;
            }

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
            var themeFields = typeof(ZapWindow).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(ThemePath));

            foreach (var field in themeFields)
            {
                RegisterTheme((ThemePath)field.GetValue(this), GetType());
            }
        }

        /// <summary>
        /// Load the default theme
        /// </summary>
        internal void LoadDefaultTheme(ZapWindowThemes theme, Type ownerType)
        {
            string registrationName = GetRegistrationName(theme, ownerType);
            Resources.MergedDictionaries.Add(_rdThemeDictionaries[registrationName]);
        }

        /// <summary>
        /// Get themes formal registration name
        /// </summary>
        private string GetRegistrationName(ZapWindowThemes theme, Type ownerType)
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
        internal virtual void SetThemePropertyDefault(DependencyProperty p, object value)
        {
            _defaultThemeProperties.Add(p, value);
            SetCurrentValue(p, value);
        }

        /// <summary>
        /// Set dependency property default theme value if value is null
        /// </summary>
        private static void SetValueCommon(DependencyObject o, DependencyProperty p, object value)
        {
            if (o is ZapWindow w)
            {
                if (!(BindingOperations.GetBinding(w, p) is Binding))
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
                        if (w._defaultThemeProperties.ContainsKey(p))
                            value = w._defaultThemeProperties[p];

                        w.SetCurrentValue(p, value);
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
            var templateFields = typeof(ZapWindow).GetFields(BindingFlags.Public | BindingFlags.Static)
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

    public class WindowCloseValidationEnventArgs : RoutedEventArgs
    {
        public bool CanClose { get; set; }

        public WindowCloseValidationEnventArgs(RoutedEvent routedEvent, object source)
        {
            RoutedEvent = routedEvent;
            Source = source;
            Handled = false;
            CanClose = true;
        }
    }
}

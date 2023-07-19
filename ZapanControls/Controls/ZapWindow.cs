using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Helpers;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /// <summary>
    /// Fenêtre personnalisée
    /// </summary>
    public partial class ZapWindow : Window, ITemplate<string>
    {
        #region Fields
        private readonly DeferredAction _centerDeferred;

        private HwndSource _hwndSource;
        private bool _startupResized;
        private bool _hasInitialized;
        private bool _isReady;
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

        #region IsReady
        /// <summary>
        /// Obtient une valeur permettant de savoir si la fenêtre est active.
        /// </summary>
        public bool IsReady 
        { 
            get => _isReady; 
            internal set => Set(ref _isReady, value); 
        }
        #endregion

        #region Buttons
        #region CanBeClosed
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="CanClosed"/>.
        /// </summary>
        public static readonly DependencyProperty CanBeClosedProperty = DependencyProperty.Register(
            "CanBeClosed", typeof(bool), typeof(ZapWindow), 
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Fermer est activé.
        /// </summary>
        public bool CanBeClosed 
        { 
            get => (bool)GetValue(CanBeClosedProperty); 
            set => SetValue(CanBeClosedProperty, value); 
        }
        #endregion

        #region ShowCloseButton
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ShowCloseButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            "ShowCloseButton", typeof(bool), typeof(ZapWindow), 
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Fermer est visible.
        /// </summary>
        public bool ShowCloseButton 
        { 
            get => (bool)GetValue(ShowCloseButtonProperty); 
            set => SetValue(ShowCloseButtonProperty, value); 
        }
        #endregion

        #region ShowMaximizeButton
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ShowMaximizeButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register(
            "ShowMaximizeButton", typeof(bool), typeof(ZapWindow), 
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Maximiser est visible.
        /// </summary>
        public bool ShowMaximizeButton 
        { 
            get => (bool)GetValue(ShowMaximizeButtonProperty); 
            set => SetValue(ShowMaximizeButtonProperty, value); 
        }
        #endregion

        #region ShowMinimizeButton
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ShowMinimizeButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register(
            "ShowMinimizeButton", typeof(bool), typeof(ZapWindow), 
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Minimiser est visible.
        /// </summary>
        public bool ShowMinimizeButton 
        { 
            get => (bool)GetValue(ShowMinimizeButtonProperty);
            set => SetValue(ShowMinimizeButtonProperty, value); 
        }
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
        public Brush TitleBarBackground 
        { 
            get => (Brush)GetValue(TitleBarBackgroundProperty); 
            set => SetValue(TitleBarBackgroundProperty, value); 
        }

        private static void OnTitleBarBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(TitleBarBackgroundProperty, e.NewValue);
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
        public Brush TitleBarForeground 
        { 
            get => (Brush)GetValue(TitleBarForegroundProperty); 
            set => SetValue(TitleBarForegroundProperty, value); 
        }

        private static void OnTitleBarForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(TitleBarForegroundProperty, e.NewValue);
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
        public double TitleBarHeight 
        { 
            get => (double)GetValue(TitleBarHeightProperty); 
            set => SetValue(TitleBarHeightProperty, value); 
        }

        private static void OnTitleBarHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(TitleBarHeightProperty, e.NewValue);
        #endregion
        #endregion
        #endregion

        #region Template Properties
        #region HasInitialized
        public bool HasInitialized 
        { 
            get => _hasInitialized; 
            private set => Set(ref _hasInitialized, value); 
        }
        #endregion

        #region TemplateDictionaries
        public Dictionary<string, ResourceDictionary> TemplateDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion

        #region ZapTemplate
        public static readonly DependencyProperty ZapTemplateProperty = DependencyProperty.Register(
            "ZapTemplate", typeof(string), typeof(ZapWindow),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnZapTemplateChanged),
                new CoerceValueCallback(CoerceZapTemplateChange)));

        public string ZapTemplate 
        { 
            get => (string)GetValue(ZapTemplateProperty); 
            set => SetValue(ZapTemplateProperty, value); 
        }

        private static void OnZapTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.TemplateChanged<string>(e, TemplateChangedEvent);

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
            "Theme", typeof(string), typeof(ZapWindow),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme 
        { 
            get => (string)GetValue(ThemeProperty); 
            set => SetValue(ThemeProperty, value);
        }

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.ThemeChanged(e, ThemeChangedEvent);

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
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(BorderBrushProperty, e.NewValue);
        #endregion

        #region BorderThickness
        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => d.SetValueCommon(BorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Events
        #region Minimizing
        public static readonly RoutedEvent MinimizingEvent = EventManager.RegisterRoutedEvent(
            "Minimizing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lorsque la fenêtre est minimisée.
        /// </summary>
        public event RoutedEventHandler Minimizing 
        { 
            add => AddHandler(MinimizingEvent, value); 
            remove => RemoveHandler(MinimizingEvent, value); 
        }
        #endregion

        #region Maximizing
        public static readonly RoutedEvent MaximizingEvent = EventManager.RegisterRoutedEvent(
            "Maximizing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lorsque la fenêtre est maximisée.
        /// </summary>
        public event RoutedEventHandler Maximizing 
        { 
            add => AddHandler(MaximizingEvent, value); 
            remove => RemoveHandler(MaximizingEvent, value); 
        }
        #endregion

        #region Restoring
        public static readonly RoutedEvent RestoringEvent = EventManager.RegisterRoutedEvent(
            "Restoring", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lorsque la fenêtre est restaurée.
        /// </summary>
        public event RoutedEventHandler Restoring 
        { 
            add => AddHandler(RestoringEvent, value); 
            remove => RemoveHandler(RestoringEvent, value); 
        }
        #endregion

        #region CloseValidation
        /// <summary>
        /// Représente la méthode qui gère la validation de fermeture de la fenêtre.
        /// </summary>
        /// <returns>Renvoi <see cref="True"/> si la fenêtre doit être fermée, sinon <see cref="False"/></returns>
        public delegate void CloseValidationEventHandler(object sender, CloseValidationEventArgs e);

        public static readonly RoutedEvent CloseValidationEvent = EventManager.RegisterRoutedEvent(
            "CloseValidation", RoutingStrategy.Bubble, typeof(CloseValidationEventHandler), typeof(ZapWindow));

        /// <summary>
        /// Se produit lors de la validation de la fermeture de la fenêtre.
        /// </summary>
        public event CloseValidationEventHandler CloseValidation 
        {
            add => AddHandler(CloseValidationEvent, value); 
            remove => RemoveHandler(CloseValidationEvent, value); 
        }
        #endregion

        #region TemplateChanged
        public static readonly RoutedEvent TemplateChangedEvent = EventManager.RegisterRoutedEvent(
            "TemplateChanged", RoutingStrategy.Bubble, typeof(ITemplate<string>.TemplateChangedEventHandler), typeof(ZapWindow));

        public event ITemplate<string>.TemplateChangedEventHandler TemplateChanged 
        { 
            add => AddHandler(TemplateChangedEvent, value); 
            remove => RemoveHandler(TemplateChangedEvent, value); 
        }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ZapWindow));

        public event ITheme.ThemeChangedEventHandler ThemeChanged 
        { 
            add => AddHandler(ThemeChangedEvent, value); 
            remove => RemoveHandler(ThemeChangedEvent, value); 
        }

        protected virtual void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();
            // Control
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapWindowResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapWindowResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ZapWindowResourceKeys.BorderThicknessKey);
            // TitleBar
            this.SetThemePropertyDefault(TitleBarBackgroundProperty, ResourceKeys.ZapWindowResourceKeys.TitleBarBackgroundKey);
            this.SetThemePropertyDefault(TitleBarForegroundProperty, ResourceKeys.ZapWindowResourceKeys.TitleBarForegroundKey);
            this.SetThemePropertyDefault(TitleBarHeightProperty, ResourceKeys.ZapWindowResourceKeys.TitleBarHeightKey);
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
            var eventArgs = new CloseValidationEventArgs(CloseValidationEvent, this);
            RaiseEvent(eventArgs);

            if (!eventArgs.Handled)
            {
                eventArgs.Handled = true;
            }

            if (eventArgs.CanClose)
            {
                Close();
            }
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

            // Load Templates
            this.RegisterAttachedTemplates<string>(typeof(ZapWindow));
            this.LoadDefaultTemplate<string>(ZapTemplateProperty);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterInternalThemes<ZapWindowThemes>("ZapWindow");
            this.LoadDefaultTheme(ThemeProperty);
        }
        #endregion

        #region Control Methods
        /// <summary>
        /// Méthode permettant de centrer la fenêtre sur la fenêtre parent ou dans l'écran.
        /// </summary>
        public void CenterWindow(int milliseconds = 250)
            => CenterWindow(TimeSpan.FromMilliseconds(milliseconds));

        /// <summary>
        /// Méthode permettant de centrer la fenêtre sur la fenêtre parent ou dans l'écran.
        /// </summary>
        public void CenterWindow(TimeSpan timeSpan)
            => _centerDeferred.Defer(timeSpan);

        /// <summary>
        /// Méthode déférée utilisée pour centrer la fenêtre sur la fenêtre parent ou dans l'écran.
        /// </summary>
        private void CenterWindowDefered()
        {
            if (WindowStartupLocation == WindowStartupLocation.CenterScreen 
                || Owner?.WindowState == WindowState.Maximized)
            {
                System.Windows.Forms.Screen currentScreen = ScreenHelpers.GetCurrentScreen(this);

                Left = ((currentScreen.WorkingArea.Width - ActualWidth) / 2) + currentScreen.WorkingArea.X;
                Top = ((currentScreen.WorkingArea.Height - ActualHeight) / 2) + currentScreen.WorkingArea.Y;
            }
            else if (WindowStartupLocation == WindowStartupLocation.CenterOwner
                && Owner != null)
            {
                Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
                Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
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

            if (ComponentDispatcher.IsThreadModal && Owner is ZapWindow win)
            {
                win.GridDim.Visibility = Visibility.Visible;
            }

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
            if (ComponentDispatcher.IsThreadModal && Owner is ZapWindow win)
            {
                win.GridDim.Visibility = Visibility.Collapsed;
            }

            base.OnClosed(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;

            if (GetTemplateChild("gridDim") is Grid gridDim)
            {
                GridDim = gridDim;
            }

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

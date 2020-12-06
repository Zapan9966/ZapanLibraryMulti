using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using ZapanControls.Helpers;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /// <summary>
    /// Fenêtre personnalisée
    /// </summary>
    public partial class ZapWindow : Window, INotifyPropertyChanged
    {
        private readonly DeferredAction _centerDeferred;

        private HwndSource _hwndSource;
        private bool _startupResized;
        private bool _isReady;

        /// <summary>
        /// Obtient la grille qui grise la fenêtre.
        /// </summary>
        public Grid GridDim { get; private set; }

        /// <summary>
        /// Obtient une valeur permettant de savoir si la fenêtre est active.
        /// </summary>
        public bool IsReady
        {
            get { return _isReady; }
            internal set { Set(ref _isReady, value); }
        }

        #region Dependency Properties

        #region Buttons properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.CanClosed"/>.
        /// </summary>
        public static readonly DependencyProperty CanClosedProperty = DependencyProperty.Register(
            "CanClosed", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Fermer est activé.
        /// </summary>
        public bool CanClosed
        {
            get { return (bool)GetValue(CanClosedProperty); }
            set { SetValue(CanClosedProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.ShowCloseButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            "ShowCloseButton", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Fermer est visible.
        /// </summary>
        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.ShowMaximizeButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register(
            "ShowMaximizeButton", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Maximiser est visible.
        /// </summary>
        public bool ShowMaximizeButton
        {
            get { return (bool)GetValue(ShowMaximizeButtonProperty); }
            set { SetValue(ShowMaximizeButtonProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.ShowMinimizeButton"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register(
            "ShowMinimizeButton", typeof(bool), typeof(ZapWindow), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si le bouton Minimiser est visible.
        /// </summary>
        public bool ShowMinimizeButton
        {
            get { return (bool)GetValue(ShowMinimizeButtonProperty); }
            set { SetValue(ShowMinimizeButtonProperty, value); }
        }

        #endregion

        #region TitleBar properties

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.TitleBarBackground"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(
            "TitleBarBackground", typeof(Brush), typeof(ZapWindow), new FrameworkPropertyMetadata(Brushes.DarkViolet));

        /// <summary>
        /// Obtient ou défini la couleur de fond de la barre de titre.
        /// </summary>
        public Brush TitleBarBackground
        {
            get { return (Brush)GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.TitleBarForeground"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register(
            "TitleBarForeground", typeof(Brush), typeof(ZapWindow), new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// Obtient ou défini la couleur de la police de la barre de titre.
        /// </summary>
        public Brush TitleBarForeground
        {
            get { return (Brush)GetValue(TitleBarForegroundProperty); }
            set { SetValue(TitleBarForegroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.TitleBarRoundedButtonBackground"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarRoundedButtonBackgroundProperty = DependencyProperty.Register(
            "TitleBarRoundedButtonBackground", typeof(Brush), typeof(ZapWindow), new FrameworkPropertyMetadata(
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F000000"))));

        /// <summary>
        /// Obtient ou défini la couleur de fond des boutons de la barre de titre arrondie.
        /// </summary>
        public Brush TitleBarRoundedButtonBackground
        {
            get { return (Brush)GetValue(TitleBarRoundedButtonBackgroundProperty); }
            set { SetValue(TitleBarRoundedButtonBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.TitleBarRoundedButtonBorderBrush"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarRoundedButtonBorderBrushProperty = DependencyProperty.Register(
            "TitleBarRoundedButtonBorderBrush", typeof(Brush), typeof(ZapWindow), new FrameworkPropertyMetadata(Brushes.MediumPurple));

        /// <summary>
        /// Obtient ou défini la couleur de la bordure des boutons de la barre de titre arrondie.
        /// </summary>
        public Brush TitleBarRoundedButtonBorderBrush
        {
            get { return (Brush)GetValue(TitleBarRoundedButtonBorderBrushProperty); }
            set { SetValue(TitleBarRoundedButtonBorderBrushProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.TitleBarStyle"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarStyleProperty = DependencyProperty.Register(
            "TitleBarStyle", typeof(TitleBarStyleEnum), typeof(ZapWindow), new FrameworkPropertyMetadata(TitleBarStyleEnum.Flat));

        /// <summary>
        /// Obtient ou défini le style de la barre de titre.
        /// </summary>
        public TitleBarStyleEnum TitleBarStyle
        {
            get { return (TitleBarStyleEnum)GetValue(TitleBarStyleProperty); }
            set { SetValue(TitleBarStyleProperty, value); }
        }

        /// <summary>
        /// Identifie la propriété de dépendance <see cref="ZapWindow.TitleBarHeight"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(
            "TitleBarHeight", typeof(double), typeof(ZapWindow), new FrameworkPropertyMetadata(Convert.ToDouble(30)));

        /// <summary>
        /// Obtient ou défini la hauteur de la barre de titre.
        /// </summary>
        public double TitleBarHeight
        {
            get { return (double)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Se produit lorsque la fenêtre est minimisée.
        /// </summary>
        public event RoutedEventHandler Minimizing;

        /// <summary>
        /// Se produit lorsque la fenêtre est maximisée.
        /// </summary>
        public event RoutedEventHandler Maximizing;

        /// <summary>
        /// Représente la méthode qui gère la validation de fermeture de la fenêtre.
        /// </summary>
        /// <returns>Renvoi <see cref="True"/> si la fenêtre doit être fermée, sinon <see cref="False"/></returns>
        public delegate bool CloseValidationEventHandler(object sender, RoutedEventArgs e);

        /// <summary>
        /// Se produit lors de la validation de la fermeture de la fenêtre.
        /// </summary>
        public event CloseValidationEventHandler CloseValidation;

        /// <summary>
        /// Méthode interne qui gère la validation de fermeture de la fenêtre.
        /// </summary>
        /// <param name="e">Informations d'état et données d'évènement associés à l'évènement routé.</param>
        /// <returns>Renvoi <see cref="True"/> si la fenêtre doit être fermée, sinon <see cref="False"/></returns>
        internal bool OnCloseValidation(RoutedEventArgs e)
        {
            bool result = true;

            if (CloseValidation != null)
                result = CloseValidation(this, e);

            return result;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Commande qui gère la réduction de la fenêtre.
        /// </summary>
        public ICommand MinimizeCommand { get; }

        private void OnMinimizeClick(RoutedEventArgs e)
        {
            if (e != null)
            {
                Minimizing?.Invoke(this, e);
                WindowState = WindowState.Minimized;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Commande qui gère l'agrandissement de la fenêtre.
        /// </summary>
        public ICommand RestoreCommand { get; }

        private void OnRestoreClick()
        {
            if (ShowMaximizeButton)
            {
                Maximizing?.Invoke(this, null);
                WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            }
            else
            {
                CenterWindow();
            }
        }

        /// <summary>
        /// Commande qui gère la fermeture de la fenêtre.
        /// </summary>
        public ICommand CloseCommand { get; }

        private void OnCloseClick(RoutedEventArgs e)
        {
            if (e != null)
            {
                if (e.RoutedEvent != null)
                    e.Handled = true;

                if (OnCloseValidation(e))
                    Close();
            }
        }

        /// <summary>
        /// Commande qui gère le déplacement de la fenêtre.
        /// </summary>
        public ICommand DragMoveCommand { get; }

        private void OnDragMove()
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        #endregion

        #region Constructors

        static ZapWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapWindow), new FrameworkPropertyMetadata(typeof(ZapWindow)));
        }

        public ZapWindow()
            : base()
        {
            _centerDeferred = DeferredAction.Create(() => this.CenterWindowDefered());
            _startupResized = false;

            MinimizeCommand = new RelayCommand<RoutedEventArgs>(
                param => OnMinimizeClick(param),
                param => true);

            RestoreCommand = new RelayCommand(
                OnRestoreClick,
                true);

            CloseCommand = new RelayCommand<RoutedEventArgs>(
                param => OnCloseClick(param),
                param => true);

            DragMoveCommand = new RelayCommand(
                OnDragMove,
                true);
        }

        #endregion

        /// <summary>
        /// Méthode qui gère le déplacement de la fenêtre.
        /// </summary>
        /// <param name="sender">Object qui a généré le déplacement.</param>
        /// <param name="e">Fournit les données sur le déplacement de la souris.</param>
        private void DragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

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
                Screen currentScreen = ScreenHelpers.GetCurrentScreen(this);

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
            if (GetTemplateChild("gridDim") is Grid gridDim)
                GridDim = gridDim;

            base.OnApplyTemplate(); 
        }

        #endregion

        #region Enums

        /// <summary>
        /// Styles de la barre de titre.
        /// </summary>
        public enum TitleBarStyleEnum
        {
            Flat = 0,
            Rounded = 1,
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

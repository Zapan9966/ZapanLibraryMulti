using System.Windows;
using System.Windows.Controls;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Helpers;

namespace ZapanControls.Controls
{
    /// <summary>
    /// Classe représentant un onglet qui peut être fermé.
    /// </summary>
    public sealed class ZapTabItem : TabItem
    {
        #region Properties
        #region IsClosable
        /// <summary>
        /// Identifie la propriété de dépendance <see cref="IsClosable"/>.
        /// </summary>
        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
            "IsClosable", typeof(bool), typeof(ZapTabItem),
            new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender 
                | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Obtient ou défini la valeur indiquant si l'onglet peut être fermé.
        /// </summary>
        public bool IsClosable 
        { 
            get => (bool)GetValue(IsClosableProperty); 
            set => SetValue(IsClosableProperty, value); 
        }
        #endregion
        #endregion

        #region Internal Event Handlers
        private void OnBtnCloseClick(object sender, RoutedEventArgs e)
        {
            if (Parent is ZapTabControl tc)
            {
                var eventArgs = new CloseValidationEventArgs(ZapTabControl.CloseValidationEvent, this);
                RaiseEvent(eventArgs);

                if (!eventArgs.Handled)
                {
                    eventArgs.Handled = true;
                }

                if (eventArgs.CanClose)
                {
                    tc.Items.Remove(this);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructeur de la classe <see cref="ZapTabItem"/>
        /// </summary>
        static ZapTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapTabItem), new FrameworkPropertyMetadata(typeof(ZapTabItem)));
        }

        public ZapTabItem()
        {

        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (VisualTreeHelpers.FindChild(this, "btnCloseTab") is ZapButton btnCloseTab)
            {
                btnCloseTab.Click -= OnBtnCloseClick;
                btnCloseTab.Click += OnBtnCloseClick;
            }
        }
        #endregion
    }
}

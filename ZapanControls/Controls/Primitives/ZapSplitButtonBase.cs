using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using ZapanControls.Libraries;

namespace ZapanControls.Controls.Primitives
{
    [TemplatePart(Name = "PART_DropDown", Type = typeof(ZapButtonBaseOld))]
    [ContentProperty("Items")]
    [DefaultProperty("Items")]
    public class ZapSplitButtonBase : ZapButtonBaseOld
    {
        #region Dependancy properties

        /// <summary>
        /// Identifies the <see cref="ZapSplitButtonBase.Mode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof(SplitButtonMode), typeof(ZapSplitButtonBase), new FrameworkPropertyMetadata(SplitButtonMode.Split));

        /// <summary>
        /// Defines the Mode of operation of the Button
        /// </summary>
        /// <remarks>
        ///     The SplitButton two Modes are
        ///     Split (default),    - the button has two parts, a normal button and a dropdown which exposes the ContextMenu
        ///     Dropdown            - the button acts like a combobox, clicking anywhere on the button opens the Context Menu
        /// </remarks>
        public SplitButtonMode Mode
        {
            get { return (SplitButtonMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapSplitButtonBase.IsContextMenuOpen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsContextMenuOpenProperty = DependencyProperty.Register(
            "IsContextMenuOpen", typeof(bool), typeof(ZapSplitButtonBase), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsContextMenuOpenChanged)));

        /// <summary>
        /// Gets or sets the IsContextMenuOpen property. 
        /// </summary>
        public bool IsContextMenuOpen
        {
            get { return (bool)GetValue(IsContextMenuOpenProperty); }
            set { SetValue(IsContextMenuOpenProperty, value); }
        }

        private static void OnIsContextMenuOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZapSplitButtonBase s = (ZapSplitButtonBase)d;
            s.EnsureContextMenuIsValid();

            if (!s.ContextMenu.HasItems)
                return;

            bool value = (bool)e.NewValue;

            if (value && !s.ContextMenu.IsOpen)
                s.ContextMenu.IsOpen = true;
            else if (!value && s.ContextMenu.IsOpen)
                s.ContextMenu.IsOpen = false;
        }

        /// <summary>
        /// Identifies the <see cref="ZapSplitButtonBase.IsContextMenuRound"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsContextMenuRoundProperty = DependencyProperty.Register(
            "IsContextMenuRound", typeof(bool), typeof(ZapSplitButtonBase), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsContextMenuRoundChanged)));

        /// <summary>
        /// Gets or sets the IsContextMenuRound property. 
        /// </summary>
        public bool IsContextMenuRound
        {
            get { return (bool)GetValue(IsContextMenuRoundProperty); }
            set { SetValue(IsContextMenuRoundProperty, value); }
        }

        private static void OnIsContextMenuRoundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapSplitButtonBase s)
                s.EnsureContextMenuIsValid();
        }

        /// <summary>
        /// Identifies the <see cref="ZapSplitButtonBase.Placement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = ContextMenuService.PlacementProperty.AddOwner(
            typeof(ZapSplitButtonBase), new FrameworkPropertyMetadata(PlacementMode.Bottom, new PropertyChangedCallback(OnPlacementChanged)));

        /// <summary>
        /// Placement of the Context menu
        /// </summary>
        public PlacementMode Placement
        {
            get { return (PlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Placement Property changed callback, pass the value through to the buttons context menu
        /// </summary>
        private static void OnPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapSplitButtonBase s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.Placement = (PlacementMode)e.NewValue;
            }
        }

        /// <summary>
        /// Identifies the <see cref="ZapSplitButtonBase.PlacementRectangle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlacementRectangleProperty = ContextMenuService.PlacementRectangleProperty.AddOwner(
            typeof(ZapSplitButtonBase), new FrameworkPropertyMetadata(Rect.Empty, new PropertyChangedCallback(OnPlacementRectangleChanged)));

        /// <summary>
        /// PlacementRectangle of the Context menu
        /// </summary>
        public Rect PlacementRectangle
        {
            get { return (Rect)GetValue(PlacementRectangleProperty); }
            set { SetValue(PlacementRectangleProperty, value); }
        }

        /// <summary>
        /// PlacementRectangle Property changed callback, pass the value through to the buttons context menu
        /// </summary>
        private static void OnPlacementRectangleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapSplitButtonBase s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.PlacementRectangle = (Rect)e.NewValue;
            }
        }

        /// <summary>
        /// Identifies the <see cref="ZapSplitButtonBase.HorizontalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = ContextMenuService.HorizontalOffsetProperty.AddOwner(
            typeof(ZapSplitButtonBase), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnHorizontalOffsetChanged)));

        /// <summary>
        /// HorizontalOffset of the Context menu
        /// </summary>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// HorizontalOffset Property changed callback, pass the value through to the buttons context menu
        /// </summary>
        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapSplitButtonBase s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.HorizontalOffset = (double)e.NewValue;
            }
        }

        /// <summary>
        /// Identifies the <see cref="ZapSplitButtonBase.VerticalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = ContextMenuService.VerticalOffsetProperty.AddOwner(
            typeof(ZapSplitButtonBase), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnVerticalOffsetChanged)));

        /// <summary>
        /// VerticalOffset of the Context menu
        /// </summary>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// VerticalOffset Property changed callback, pass the value through to the buttons context menu
        /// </summary>
        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapSplitButtonBase s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.VerticalOffset = (double)e.NewValue;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command Handler for the Drop Down Button's Click event
        /// </summary>
        public ICommand DropDownCommand { get; }

        private void OnDropDownClick(RoutedEventArgs e)
        {
            OnDropdown();
            e.Handled = true;
        }

        #endregion

        /// <summary>
        /// The Split Button's Items property maps to the base classes ContextMenu.Items property
        /// </summary>
        public ItemCollection Items
        {
            get
            {
                EnsureContextMenuIsValid();
                return this.ContextMenu.Items;
            }
        }

        #region Constructors

        static ZapSplitButtonBase()
        { }

        public ZapSplitButtonBase()
        {
            DropDownCommand = new RelayCommand<RoutedEventArgs>(
                param => OnDropDownClick(param),
                param => true);
        }

        #endregion

        #region Overrides

        /// <summary>
        ///     Handles the Base Buttons OnClick event
        /// </summary>
        protected override void OnClick()
        {
            switch (Mode)
            {
                case SplitButtonMode.Dropdown:
                    OnDropdown();
                    break;

                default:
                    base.OnClick(); // forward on the Click event to the user
                    break;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Make sure the Context menu is not null
        /// </summary>
        private void EnsureContextMenuIsValid()
        {
            if (this.ContextMenu == null)
            {
                this.ContextMenu = new ZapContextMenu
                {
                    PlacementTarget = this,
                    Placement = Placement,
                    CornerRadius = IsContextMenuRound ? new CornerRadius(0, 0, 4, 4) : new CornerRadius(0),                    
                };
                this.ContextMenu.Opened += ((sender, routedEventArgs) => IsContextMenuOpen = true);
                this.ContextMenu.Closed += ((sender, routedEventArgs) => IsContextMenuOpen = false);
            }
        }

        private void OnDropdown()
        {
            EnsureContextMenuIsValid();
            if (!this.ContextMenu.HasItems)
                return;

            this.ContextMenu.IsOpen = !IsContextMenuOpen; // open it if closed, close it if open
        }

        #endregion

    }

    public enum SplitButtonMode
    {
        Split = 0,
        Dropdown = 1
    }
}

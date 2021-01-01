using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Converters;
using ZapanControls.Helpers;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    [TemplatePart(Name = "PART_DropDown", Type = typeof(ZapButtonBase))]
    [ContentProperty("Items")]
    [DefaultProperty("Items")]
    public sealed class ZapSplitButton : ZapButtonBase
    {
        #region Theme Declarations

        #endregion

        #region Template Declarations
        public static TemplatePath Flat = new TemplatePath(ZapSplitButtonTemplates.Flat, "/ZapanControls;component/Themes/ZapButton/Template.Split.Flat.xaml");
        public static TemplatePath Glass = new TemplatePath(ZapSplitButtonTemplates.Glass, "/ZapanControls;component/Themes/ZapButton/Template.Split.Glass.xaml");
        #endregion

        #region Properties
        #region Mode
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof(SplitButtonModes), typeof(ZapSplitButton), 
            new FrameworkPropertyMetadata(SplitButtonModes.Split, 
                FrameworkPropertyMetadataOptions.AffectsArrange 
                | FrameworkPropertyMetadataOptions.AffectsMeasure 
                | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Defines the Mode of operation of the button
        /// </summary>
        /// <remarks>
        ///     The SplitButton two Modes are
        ///     Split (default),    - the button has two parts, a normal button and a dropdown which exposes the ContextMenu
        ///     Dropdown            - the button acts like a combobox, clicking anywhere on the button opens the Context Menu
        /// </remarks>
        public SplitButtonModes Mode
        {
            get => (SplitButtonModes)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        #endregion

        #region IsContextMenuOpen
        public static readonly DependencyProperty IsContextMenuOpenProperty = DependencyProperty.Register(
            "IsContextMenuOpen", typeof(bool), typeof(ZapSplitButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsContextMenuOpenChanged)));

        /// <summary>
        /// Gets or sets the IsContextMenuOpen property. 
        /// </summary>
        public bool IsContextMenuOpen
        {
            get => (bool)GetValue(IsContextMenuOpenProperty);
            set => SetValue(IsContextMenuOpenProperty, value);
        }

        private static void OnIsContextMenuOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZapSplitButton s = (ZapSplitButton)d;
            s.EnsureContextMenuIsValid();

            if (!s.ContextMenu.HasItems)
                return;

            bool value = (bool)e.NewValue;

            if (value && !s.ContextMenu.IsOpen)
                s.ContextMenu.IsOpen = true;
            else if (!value && s.ContextMenu.IsOpen)
                s.ContextMenu.IsOpen = false;
        }
        #endregion

        #region IsContextMenuRound
        /// <summary>
        /// Identifies the <see cref="IsContextMenuRound"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsContextMenuRoundProperty = DependencyProperty.Register(
            "IsContextMenuRound", typeof(bool), typeof(ZapSplitButton), 
            new FrameworkPropertyMetadata(false, 
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnIsContextMenuRoundChanged)));

        /// <summary>
        /// Gets or sets the IsContextMenuRound property. 
        /// </summary>
        public bool IsContextMenuRound
        {
            get => (bool)GetValue(IsContextMenuRoundProperty);
            set => SetValue(IsContextMenuRoundProperty, value);
        }

        private static void OnIsContextMenuRoundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapSplitButton s)
                s.EnsureContextMenuIsValid();
        }
        #endregion

        #region Items
        /// <summary>
        /// The Split Button's Items property maps to the base classes ContextMenu.Items property
        /// </summary>
        public ItemCollection Items
        {
            get
            {
                EnsureContextMenuIsValid();
                return ContextMenu.Items;
            }
        }
        #endregion

        #region Placement
        /// <summary>
        /// Identifies the <see cref="ZapSplitButto.Placement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = ContextMenuService.PlacementProperty.AddOwner(
            typeof(ZapSplitButton), new FrameworkPropertyMetadata(PlacementMode.Bottom, new PropertyChangedCallback(OnPlacementChanged)));

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
            if (d is ZapSplitButton s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.Placement = (PlacementMode)e.NewValue;
            }
        }
        #endregion

        #region PlacementRectangle
        /// <summary>
        /// Identifies the <see cref="PlacementRectangle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlacementRectangleProperty = ContextMenuService.PlacementRectangleProperty.AddOwner(
            typeof(ZapSplitButton), new FrameworkPropertyMetadata(Rect.Empty, new PropertyChangedCallback(OnPlacementRectangleChanged)));

        /// <summary>
        /// PlacementRectangle of the Context menu
        /// </summary>
        public Rect PlacementRectangle
        {
            get => (Rect)GetValue(PlacementRectangleProperty);
            set => SetValue(PlacementRectangleProperty, value);
        }

        /// <summary>
        /// PlacementRectangle Property changed callback, pass the value through to the buttons context menu
        /// </summary>
        private static void OnPlacementRectangleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapSplitButton s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.PlacementRectangle = (Rect)e.NewValue;
            }
        }
        #endregion

        #region HorizontalOffset
        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = ContextMenuService.HorizontalOffsetProperty.AddOwner(
            typeof(ZapSplitButton), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnHorizontalOffsetChanged)));

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
            if (d is ZapSplitButton s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.HorizontalOffset = (double)e.NewValue;
            }
        }
        #endregion

        #region VerticalOffset
        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = ContextMenuService.VerticalOffsetProperty.AddOwner(
            typeof(ZapSplitButton), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnVerticalOffsetChanged)));

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
            if (d is ZapSplitButton s)
            {
                s.EnsureContextMenuIsValid();
                s.ContextMenu.VerticalOffset = (double)e.NewValue;
            }
        }
        #endregion
        #endregion

        #region Internal Event Handlers
        private void OnDropDownClick(object sender, RoutedEventArgs e)
        {
            EnsureContextMenuIsValid();
            if (!ContextMenu.HasItems)
                return;

            ContextMenu.IsOpen = !IsContextMenuOpen; // open it if closed, close it if open
        }

        private void OnContextMenuOpened(object sender, RoutedEventArgs e)
        {
            ZapContextMenu ctx = sender as ZapContextMenu;
            ctx.Placement = Placement;
            ctx.CornerRadius = IsContextMenuRound ? new CornerRadius(0, 0, 4, 4) : new CornerRadius(0);

            if (Placement.In(PlacementMode.Bottom, PlacementMode.Top))
            {
                if (ContextMenu.ActualWidth < ActualWidth)
                {
                    ContextMenu.Width = ActualWidth + BorderThickness.Left * 4 + 1;
                }
            }
            else
            {
                ContextMenu.Width = double.NaN;
            }

            SetBindingsOnItems(Items, 1);

            if (ZapTemplate == "Glass")
            {
                if (Template.FindName("PART_DropDown", this) is ZapButton dropdown)
                {
                    if (VisualTreeHelpers.FindChild(dropdown, "PART_Border") is Border border)
                        border.CornerRadius = new CornerRadius(0, 4, 0, 0);
                }
            }
            IsContextMenuOpen = true;
        }
        private void OnContextMenuClosed(object sender, RoutedEventArgs e)
        {
            if (ZapTemplate == "Glass")
            {
                if (Template.FindName("PART_DropDown", this) is ZapButton dropdown)
                {
                    if (VisualTreeHelpers.FindChild(dropdown, "PART_Border") is Border border)
                        border.CornerRadius = new CornerRadius(0, 4, 4, 0);
                }
            }
            IsContextMenuOpen = false;
        }
        #endregion

        #region Constructors
        static ZapSplitButton()
        {

        }

        public ZapSplitButton()
        {

        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Template.FindName("PART_DropDown", this) is ZapButton dropdown)
            {
                #region Bindings
                dropdown.SetBinding(ZapTemplateProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("ZapTemplate"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(ThemeProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("Theme"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(BackgroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("Background"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(BorderBrushProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("BorderBrush"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(ForegroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("Foreground"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(BorderThicknessProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("BorderThickness"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(FocusedBackgroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("FocusedBackground"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(FocusedBorderBrushProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("FocusedBorderBrush"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(FocusedForegroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ZapSplitButton), 1),
                    Path = new PropertyPath("FocusedForeground"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(PressedBackgroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("PressedBackground"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(PressedBorderBrushProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("PressedBorderBrush"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(PressedForegroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("PressedForeground"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(DisabledBackgroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("DisabledBackground"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(DisabledBorderBrushProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("DisabledBorderBrush"),
                    Mode = BindingMode.OneWay
                });

                dropdown.SetBinding(DisabledForegroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("DisabledForeground"),
                    Mode = BindingMode.OneWay
                });
                #endregion

                dropdown.Click -= OnDropDownClick;
                dropdown.Click += OnDropDownClick;

                if (ZapTemplate == "Glass")
                {
                    if (VisualTreeHelpers.FindChild(dropdown, "PART_Border") is Border border)
                        border.CornerRadius = new CornerRadius(0, 4, 4, 0);

                    if (VisualTreeHelpers.FindChild(dropdown, "glow") is Border glow)
                        glow.CornerRadius = new CornerRadius(0, 3, 0, 0);

                    if (VisualTreeHelpers.FindChild(dropdown, "shine") is Border shine)
                        shine.CornerRadius = new CornerRadius(0, 4, 0, 0);

                    IsContextMenuRound = true;
                }
            }
        }

        protected override void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            base.OnThemeChanged(sender, e);
        }

        /// <summary>
        /// Handles the Base Buttons OnClick event
        /// </summary>
        protected override void OnClick()
        {
            switch (Mode)
            {
                case SplitButtonModes.Dropdown:
                    if (Template.FindName("PART_DropDown", this) is ZapButton dropdown)
                    {
                        ButtonAutomationPeer peer = new ButtonAutomationPeer(dropdown);
                        IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                        invokeProv.Invoke();
                    }
                    break;

                default:
                    base.OnClick(); // forward on the Click event to the user
                    break;
            }
        }
        #endregion

        #region Control Methods
        private void SetBindingsOnItems(ItemCollection itemCollection, int level)
        {
            var relativeSource = level == 1 ?
                new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ZapContextMenu), 1)
                : new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ZapMenuItem), 1);

            foreach (var item in itemCollection)
            {
                if (item is ZapMenuItem menuItem)
                {
                    #region Normal
                    menuItem.SetBinding(BackgroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}Background"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(BorderBrushProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}BorderBrush"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(ZapMenuItem.SubMenuForegroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : "SubMenu")}Foreground"),
                        Mode = BindingMode.OneWay
                    });
                    #endregion

                    #region Focused
                    menuItem.SetBinding(ZapMenuItem.FocusedBackgroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}FocusedBackground"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(ZapMenuItem.FocusedBorderBrushProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}FocusedBackground"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(ZapMenuItem.FocusedForegroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}FocusedForeground"),
                        Mode = BindingMode.OneWay
                    });
                    #endregion

                    #region Pressed
                    menuItem.SetBinding(ZapMenuItem.PressedBackgroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}PressedBackground"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(ZapMenuItem.PressedBorderBrushProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}PressedBackground"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(ZapMenuItem.PressedForegroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}PressedForeground"),
                        Mode = BindingMode.OneWay
                    });
                    #endregion

                    #region Disabled
                    menuItem.SetBinding(ZapMenuItem.DisabledBackgroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}DisabledBackground"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(ZapMenuItem.DisabledBorderBrushProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}DisabledBackground"),
                        Mode = BindingMode.OneWay
                    });

                    menuItem.SetBinding(ZapMenuItem.DisabledForegroundProperty, new Binding
                    {
                        RelativeSource = relativeSource,
                        Path = new PropertyPath($"{(level == 1 ? "PlacementTarget." : null)}DisabledForeground"),
                        Mode = BindingMode.OneWay
                    });
                    #endregion

                    if (menuItem.Items.Count > 0)
                        SetBindingsOnItems(menuItem.Items, level + 1);
                }
            }
        }

        /// <summary>
        /// Make sure the Context menu is not null
        /// </summary>
        private void EnsureContextMenuIsValid()
        {
            if (ContextMenu == null)
            {
                ZapContextMenu contextMenu = new ZapContextMenu { PlacementTarget = this };

                #region Bindings
                #region ContextMenu
                contextMenu.SetBinding(BackgroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                    Path = new PropertyPath("PlacementTarget.Background"),
                    Converter = new ColorBrithnessConverter(),
                    ConverterParameter = -15,
                    Mode = BindingMode.OneWay
                });

                contextMenu.SetBinding(BorderBrushProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                    Path = new PropertyPath("PlacementTarget.BorderBrush"),
                    Mode = BindingMode.OneWay
                });

                contextMenu.SetBinding(ForegroundProperty, new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                    Path = new PropertyPath("PlacementTarget.Foreground"),
                    Mode = BindingMode.OneWay
                });
                #endregion
                #endregion

                ContextMenu = contextMenu;
                ContextMenu.Opened += OnContextMenuOpened;
                ContextMenu.Closed += OnContextMenuClosed;
            }
        }
        #endregion
    }

    public enum SplitButtonModes
    {
        Split = 0,
        Dropdown = 1
    }
}

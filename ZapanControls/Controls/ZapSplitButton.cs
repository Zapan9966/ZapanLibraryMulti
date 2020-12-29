﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Themes;
using ZapanControls.Helpers;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    [TemplatePart(Name = "PART_DropDown", Type = typeof(ZapButtonBase))]
    [ContentProperty("Items")]
    [DefaultProperty("Items")]
    public class ZapSplitButton : ZapButtonBase
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
            "Mode", typeof(SplitButtonMode), typeof(ZapSplitButton), 
            new FrameworkPropertyMetadata(SplitButtonMode.Split, 
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
        public SplitButtonMode Mode
        {
            get => (SplitButtonMode)GetValue(ModeProperty);
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
                    ContextMenu.Width = ActualWidth + BorderThickness.Left * 4;
                }
            }
            else
            {
                ContextMenu.Width = double.NaN;
            }

            if (ButtonTemplate == "Glass")
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
            if (ButtonTemplate == "Glass")
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
                var buttonTemplateBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("ButtonTemplate"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(ButtonTemplateProperty, buttonTemplateBinding);

                var themeBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("Theme"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(ThemeProperty, themeBinding);

                var backgroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("Background"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(BackgroundProperty, backgroundBinding);

                var borderBrushBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("BorderBrush"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(BorderBrushProperty, borderBrushBinding);

                var foregroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("Foreground"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(ForegroundProperty, foregroundBinding);

                var borderThicknessBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("BorderThickness"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(BorderThicknessProperty, borderThicknessBinding);

                var focusedBackgroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("FocusedBackground"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(FocusedBackgroundProperty, focusedBackgroundBinding);

                var focusedBorderBrushBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("FocusedBorderBrush"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(FocusedBorderBrushProperty, focusedBorderBrushBinding);

                var focusedForegroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ZapSplitButton), 1),
                    Path = new PropertyPath("FocusedForeground"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(FocusedForegroundProperty, focusedForegroundBinding);

                var pressedBackgroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("PressedBackground"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(PressedBackgroundProperty, pressedBackgroundBinding);

                var pressedBorderBrushBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("PressedBorderBrush"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(PressedBorderBrushProperty, pressedBorderBrushBinding);

                var pressedForegroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("PressedForeground"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(PressedForegroundProperty, pressedForegroundBinding);

                var disabledBackgroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("DisabledBackground"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(DisabledBackgroundProperty, disabledBackgroundBinding);

                var disabledBorderBrushBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("DisabledBorderBrush"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(DisabledBorderBrushProperty, disabledBorderBrushBinding);

                var disabledForegroundBinding = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("DisabledForeground"),
                    Mode = BindingMode.OneWay
                };
                dropdown.SetBinding(DisabledForegroundProperty, disabledForegroundBinding);
                #endregion

                dropdown.Click -= OnDropDownClick;
                dropdown.Click += OnDropDownClick;

                if (ButtonTemplate == "Glass")
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

        protected override void OnThemeChangedSuccess(object sender, RoutedEventArgs e)
        {
            base.OnThemeChangedSuccess(sender, e);
        }

        /// <summary>
        /// Handles the Base Buttons OnClick event
        /// </summary>
        protected override void OnClick()
        {
            switch (Mode)
            {
                case SplitButtonMode.Dropdown:
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

        #region Templating
        /// <summary>
        /// Load the default template
        /// </summary>
        private void LoadDefaultTemplate(ZapSplitButtonTemplates template, Type ownerType)
        {
            string registrationName = GetRegistrationName(template, ownerType);
            LoadDefaultTemplate(registrationName);
        }

        /// <summary>
        /// Get template formal registration name
        /// </summary>
        private string GetRegistrationName(ZapSplitButtonTemplates template, Type ownerType)
        {
            return GetRegistrationName(template.ToString(), ownerType);
        }
        #endregion

        #region Control Methods
        /// <summary>
        /// Make sure the Context menu is not null
        /// </summary>
        private void EnsureContextMenuIsValid()
        {
            if (ContextMenu == null)
            {
                ContextMenu = new ZapContextMenu { PlacementTarget = this };
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
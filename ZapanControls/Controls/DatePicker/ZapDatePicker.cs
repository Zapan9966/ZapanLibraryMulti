#region Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Themes;
#endregion

namespace ZapanControls.Controls.DatePicker
{
    [DefaultEvent("SelectedDateChanged"),
    DefaultProperty("SelectedDate"),
    TemplatePart(Name = "Part_DateCheckBox", Type = typeof(CheckBox)),
    TemplatePart(Name = "Part_DateTextBox", Type = typeof(TextBox)),
    TemplatePart(Name = "Part_CalendarButton", Type = typeof(Button)),
    TemplatePart(Name = "Part_CalendarGrid", Type = typeof(Grid)),
    TemplatePart(Name = "Part_CalendarPopup", Type = typeof(Popup))]
    public sealed class ZapDatePicker : ThemableControl<ZapDatePickerThemes>
    {
        #region Constants
        private const string DefaultThemeName = "Oceatech";
        private const string ImageUri = "pack://application:,,,/ZapanControls;component/Controls/DatePicker/Images/outlook_calendar_day.png";
        /// <summary>
        /// Property names as string constants
        /// </summary>
        private const string FooterVisibilityPropName = "FooterVisibility";
        private const string WeekColumnVisibilityPropName = "WeekColumnVisibility";
        private const string CalendarHeightPropName = "CalendarHeight";
        private const string CalendarWidthPropName = "CalendarWidth";
        private const string SelectedDatePropName = "SelectedDate";
        private const string DisplayDatePropName = "DisplayDate";
        private const string DisplayDateStartPropName = "DisplayDateStart";
        private const string DisplayDateEndPropName = "DisplayDateEnd";
        /// <summary>
        /// Parts names as string constants
        /// </summary>
        private const string PartCalendarButton = "Part_CalendarButton";
        private const string PartCalendarGrid = "Part_CalendarGrid";
        private const string PartDateCheckBox = "Part_DateCheckBox";
        private const string PartDateTextBox = "Part_DateTextBox";
        private const string PartButtonImage = "Part_ButtonImage";
        /// <summary>
        /// Styles names as string constants
        /// </summary>
        private const string ButtonImageStyleName = "ButtonImageStyle";
        private const string ButtonBrushStyleName = "ButtonBrushStyle";
        private const string ButtonFlatStyleName = "ButtonFlatStyle";
        #endregion

        #region Fields
        private string FormatString = "{0:dd/MM/yyyy}";
        #endregion

        #region Local Properties
        private bool HasInitialized { get; set; }
        #endregion

        #region Local Event Handlers
        /// <summary>
        /// Handles button click, launches Calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = true;
        }

        /// <summary>
        /// Handles lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateText();
        }

        /// <summary>
        /// Handles KeyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ValidateText();
            }
        }
        #endregion

        #region Native Properties Changed
        #region Background
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BorderBrushProperty, e.NewValue);
        #endregion

        #region Foreground
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(ForegroundProperty, e.NewValue);
        #endregion
        #endregion

        #region Properties
        #region Calendar Object
        /// <summary>
        /// Gets/Sets Calendar object
        /// </summary>
        public CalendarPicker.Calendar Calendar { get; set; }
        #endregion

        #region Popup Object
        /// <summary>
        /// Gets/Sets Calendar object
        /// </summary>
        public Popup Popup { get; set; }

        private CustomPopupPlacement[] PopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
        {
            var placements = new List<CustomPopupPlacement>();

            if (Popup != null && FindElement(PartCalendarButton) is Button button)
            {
                if (CalendarPlacement == PlacementType.Left)
                {
                    placements.Add(new CustomPopupPlacement(
                        new Point(
                            button.ActualWidth - Calendar.ActualWidth + 2,
                            button.ActualHeight + 2),
                        PopupPrimaryAxis.Vertical));
                }
                else
                {
                    placements.Add(new CustomPopupPlacement(
                        new Point(
                            button.ActualWidth + 2, 
                            -2), 
                        PopupPrimaryAxis.Vertical));
                }
            }
            return placements.ToArray();
        }

        #endregion

        #region ButtonStyle
        /// <summary>
        /// Gets/Sets the button appearence
        /// </summary>
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(
            "ButtonStyle", typeof(ButtonType), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(ButtonType.Flat,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnButtonStyleChanged)));

        public ButtonType ButtonStyle
        {
            get { return (ButtonType)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        private static void OnButtonStyleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ZapDatePicker dp)
            {
                dp.SetButtonStyle();
            }
        }

        private void SetButtonStyle()
        {
            if (Template != null)
            {
                Button button = FindElement(PartCalendarButton) as Button;
                Image buttonImage = FindElement(PartButtonImage) as Image;
                if (button != null || buttonImage != null)
                {
                    if (ButtonStyle == ButtonType.Image)
                    {
                        button.Style = (Style)TryFindResource(ButtonImageStyleName);
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.UriSource = new Uri(ImageUri, UriKind.Absolute);
                        img.EndInit();
                        buttonImage.Source = img;
                    }
                    else
                    {
                        buttonImage.Source = null;
                        button.Style = ButtonStyle == ButtonType.Brush
                            ? (Style)TryFindResource(ButtonBrushStyleName)
                            : (Style)TryFindResource(ButtonFlatStyleName);
                    }
                }
            }
        }
        #endregion

        #region ButtonBackground
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register(
            "ButtonBackground", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonBackground
        {
            get => (Brush)GetValue(ButtonBackgroundProperty);
            set => SetValue(ButtonBackgroundProperty, value);
        }
        #endregion

        #region ButtonBackgroundHover
        public static readonly DependencyProperty ButtonBackgroundHoverProperty = DependencyProperty.Register(
            "ButtonBackgroundHover", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonBackgroundHover
        {
            get => (Brush)GetValue(ButtonBackgroundHoverProperty);
            set => SetValue(ButtonBackgroundHoverProperty, value);
        }
        #endregion

        #region ButtonBackgroundPressed
        public static readonly DependencyProperty ButtonBackgroundPressedProperty = DependencyProperty.Register(
            "ButtonBackgroundPressed", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonBackgroundPressed
        {
            get => (Brush)GetValue(ButtonBackgroundPressedProperty);
            set => SetValue(ButtonBackgroundPressedProperty, value);
        }
        #endregion

        #region ButtonBorder
        public static readonly DependencyProperty ButtonBorderProperty = DependencyProperty.Register(
            "ButtonBorder", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonBorder
        {
            get => (Brush)GetValue(ButtonBorderProperty);
            set => SetValue(ButtonBorderProperty, value);
        }
        #endregion

        #region ButtonBorderHover
        public static readonly DependencyProperty ButtonBorderHoverProperty = DependencyProperty.Register(
            "ButtonBorderHover", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonBorderHover
        {
            get => (Brush)GetValue(ButtonBorderHoverProperty);
            set => SetValue(ButtonBorderHoverProperty, value);
        }
        #endregion

        #region ButtonBorderPressed
        public static readonly DependencyProperty ButtonBorderPressedProperty = DependencyProperty.Register(
            "ButtonBorderPressed", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonBorderPressed
        {
            get => (Brush)GetValue(ButtonBorderPressedProperty);
            set => SetValue(ButtonBorderPressedProperty, value);
        }
        #endregion

        #region IconNormal
        public static readonly DependencyProperty IconNormalProperty = DependencyProperty.Register(
            "IconNormal", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush IconNormal
        {
            get => (Brush)GetValue(IconNormalProperty);
            set => SetValue(IconNormalProperty, value);
        }
        #endregion

        #region IconHover
        public static readonly DependencyProperty IconHoverProperty = DependencyProperty.Register(
            "IconHover", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush IconHover
        {
            get => (Brush)GetValue(IconHoverProperty);
            set => SetValue(IconHoverProperty, value);
        }
        #endregion

        #region IconPressed
        public static readonly DependencyProperty IconPressedProperty = DependencyProperty.Register(
            "IconPressed", typeof(Brush), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush IconPressed
        {
            get => (Brush)GetValue(IconPressedProperty);
            set => SetValue(IconPressedProperty, value);
        }
        #endregion

        #region CalendarHeight
        /// <summary>
        /// Gets/Sets calendar height
        /// </summary>
        public static readonly DependencyProperty CalendarHeightProperty = DependencyProperty.Register(
            "CalendarHeight", typeof(double), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata
            {
                DefaultValue = 175d,
                CoerceValueCallback = new CoerceValueCallback(CoerceCalendarSize),
                AffectsRender = true
            });

        public double CalendarHeight
        {
            get { return (double)GetValue(CalendarHeightProperty); }
            set { SetValue(CalendarHeightProperty, value); }
        }

        private static object CoerceCalendarSize(DependencyObject d, object o)
        {
            double value = (double)o;

            if (value < 160)
            {
                return 160;
            }
            else if (value > 350)
            {
                return 350;
            }

            return o;
        }
        #endregion

        #region CalendarPlacement
        /// <summary>
        /// Gets/Sets calendar relative position
        /// </summary>
        public static readonly DependencyProperty CalendarPlacementProperty = DependencyProperty.Register(
            "CalendarPlacement", typeof(PlacementType), typeof(ZapDatePicker), new PropertyMetadata(PlacementType.Right));

        public PlacementType CalendarPlacement
        {
            get { return (PlacementType)GetValue(CalendarPlacementProperty); }
            set { SetValue(CalendarPlacementProperty, value); }
        }
        #endregion

        #region CalendarWidth
        /// <summary>Gets/Sets calendar width</summary>
        public static readonly DependencyProperty CalendarWidthProperty = DependencyProperty.Register(
            "CalendarWidth", typeof(double), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata
            {
                DefaultValue = 175d,
                CoerceValueCallback = new CoerceValueCallback(CoerceCalendarSize),
                AffectsRender = true
            });

        public double CalendarWidth
        {
            get { return (double)GetValue(CalendarWidthProperty); }
            set { SetValue(CalendarWidthProperty, value); }
        }
        #endregion

        #region CalendarTheme
        /// <summary>
        /// Change the Calendar element theme
        /// </summary>
        public static readonly DependencyProperty CalendarThemeProperty = DependencyProperty.Register(
            "CalendarTheme", typeof(string), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata
            {
                DefaultValue = DefaultThemeName,
                CoerceValueCallback = new CoerceValueCallback(CoerceCalendarTheme),
                PropertyChangedCallback = new PropertyChangedCallback(OnCalendarThemeChanged),
                AffectsRender = true
            });

        public string CalendarTheme
        {
            get { return (string)GetValue(CalendarThemeProperty); }
            set { SetValue(CalendarThemeProperty, value); }
        }

        private static object CoerceCalendarTheme(DependencyObject d, object o)
        {
            string value = o.ToString();
            return string.IsNullOrEmpty(value) ? DefaultThemeName : o;
        }

        private static void OnCalendarThemeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ZapDatePicker dp && dp.Template != null)
            {
                if (dp.GetTemplateChild(PartCalendarGrid) is Grid grid)
                {
                    if (grid.Children[0] is CalendarPicker.Calendar cld)
                    {
                        // test against legit value
                        var themes = Enum.GetNames(typeof(CalendarPickerThemes));
                        if (themes.Contains(e.NewValue))
                        {
                            cld.Theme = e.NewValue.ToString();
                        }
                    }
                }
            }
        }
        #endregion

        #region SelectedDate
        /// <summary>
        /// Gets/Sets currently selected date
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
            "SelectedDate", typeof(DateTime), typeof(ZapDatePicker),
            new PropertyMetadata(
                DateTime.Today,
                new PropertyChangedCallback(OnSelectedDateChanged),
                CoerceDateToBeInRange));

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        private static object CoerceDateToBeInRange(DependencyObject d, object o)
        {
            ZapDatePicker dp = d as ZapDatePicker;
            DateTime value = (DateTime)o;

            if (value < dp.DisplayDateStart)
            {
                return dp.DisplayDateStart;
            }
            else if (value > dp.DisplayDateEnd)
            {
                return dp.DisplayDateEnd;
            }
            return value;
        }
        #endregion

        #region DateFormat
        /// <summary>
        /// Gets/Sets date display format
        /// </summary>
        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.Register(
            "DateFormat", typeof(DateFormatType), typeof(ZapDatePicker), new FrameworkPropertyMetadata
            {
                DefaultValue = DateFormatType.ddMMyyyy,
                PropertyChangedCallback = new PropertyChangedCallback(OnDateFormatChanged),
                AffectsRender = true
            });

        public DateFormatType DateFormat
        {
            get { return (DateFormatType)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        private static void OnDateFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
            DateFormatType df = (DateFormatType)e.NewValue;

            switch (df)
            {
                case DateFormatType.MMMMddddyyyy:
                    dp.FormatString = "{0:dddd MMMM dd, yyyy}";
                    break;
                case DateFormatType.ddMMMyy:
                    dp.FormatString = "{0:dd MMM, yy}";
                    break;
                case DateFormatType.ddMMMyyyy:
                    dp.FormatString = "{0:dd MMM, yyyy}";
                    break;
                case DateFormatType.Mddyyyy:
                    dp.FormatString = "{0:M d, yyyy}";
                    break;
                case DateFormatType.Mdyy:
                    dp.FormatString = "{0:M d, yy}";
                    break;
                case DateFormatType.Mdyyyy:
                    dp.FormatString = "{0:M d, yyyy}";
                    break;
                case DateFormatType.MMMddyyyy:
                    dp.FormatString = "{0:MMM dd, yyyy}";
                    break;
                case DateFormatType.yyMMdd:
                    dp.FormatString = "{0:yy MM, dd}";
                    break;
                case DateFormatType.yyyyMMdd:
                    dp.FormatString = "{0:yyyy MM, dd}";
                    break;
                case DateFormatType.ddddddMMMMyyyy:
                    dp.FormatString = "{0:dddd dd MMMM yyyy}";
                    break;
                case DateFormatType.ddMMyyyy:
                    dp.FormatString = "{0:dd/MM/yyyy}";
                    break;
            }
            dp.Text = string.Format(dp.FormatString, dp.DisplayDate);
        }
        #endregion

        #region DisplayDate
        /// <summary>
        /// Gets/Sets currently displayed date
        /// </summary>
        public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register(
            "DisplayDate", typeof(DateTime), typeof(ZapDatePicker),
            new PropertyMetadata(
                DateTime.Today,
                new PropertyChangedCallback(OnDisplayDateChanged),
                new CoerceValueCallback(CoerceDateToBeInRange)));

        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        private static void OnDisplayDateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            //ZapDatePicker dp = o as ZapDatePicker;
        }
        #endregion

        #region DisplayDateStart
        /// <summary>
        /// Gets/Sets the minimum date that can be displayed
        /// </summary>
        public static readonly DependencyProperty DisplayDateStartProperty = DependencyProperty.Register(
            "DisplayDateStart", typeof(DateTime), typeof(ZapDatePicker),
            new PropertyMetadata(
                new DateTime(1, 1, 1),
                null,
                new CoerceValueCallback(CoerceDisplayDateStart)));

        public DateTime DisplayDateStart
        {
            get { return (DateTime)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }

        private static object CoerceDisplayDateStart(DependencyObject d, object o)
        {
            ZapDatePicker dp = d as ZapDatePicker;
            DateTime value = (DateTime)o;
            return value > dp.DisplayDateEnd ? dp.DisplayDateEnd : value;
        }

        #endregion

        #region DisplayDateEnd
        /// <summary>
        /// Gets/Sets the maximum date that is displayed, and can be selected
        /// </summary>
        public static readonly DependencyProperty DisplayDateEndProperty = DependencyProperty.Register(
            "DisplayDateEnd", typeof(DateTime), typeof(ZapDatePicker),
            new PropertyMetadata(
                new DateTime(2199, 1, 1),
                null,
                new CoerceValueCallback(CoerceDisplayDateEnd)));

        public DateTime DisplayDateEnd
        {
            get { return (DateTime)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }

        private static object CoerceDisplayDateEnd(DependencyObject d, object o)
        {
            ZapDatePicker dp = d as ZapDatePicker;
            DateTime value = (DateTime)o;
            return value < dp.DisplayDateStart ? dp.DisplayDateStart : o;
        }
        #endregion

        #region FooterVisibility
        /// <summary>
        /// Gets/Sets the footer visibility</summary>
        public static readonly DependencyProperty FooterVisibilityProperty = DependencyProperty.Register(
            "FooterVisibility", typeof(Visibility), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata
            {
                DefaultValue = Visibility.Collapsed,
                AffectsRender = true
            });

        public Visibility FooterVisibility
        {
            get { return (Visibility)GetValue(FooterVisibilityProperty); }
            set { SetValue(FooterVisibilityProperty, value); }
        }
        #endregion

        #region IsCheckable
        /// <summary>
        /// Gets/Sets control contains checkbox control
        /// </summary>
        public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(
            "IsCheckable", typeof(bool), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata
            {
                DefaultValue = false,
                PropertyChangedCallback = new PropertyChangedCallback(OnIsCheckableChanged),
                AffectsRender = true
            });

        public bool IsCheckable
        {
            get { return (bool)GetValue(IsCheckableProperty); }
            set { SetValue(IsCheckableProperty, value); }
        }

        private static void OnIsCheckableChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
            bool value = (bool)e.NewValue;

            if (dp.GetTemplateChild(PartDateCheckBox) is CheckBox cb)
            {
                cb.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        #region IsChecked
        /// <summary>OnIsCheckedChanged
        /// Gets/Sets checkbox control check state
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked", typeof(bool), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsCheckedChanged)));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private static void OnIsCheckedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ZapDatePicker dp)
            {
                bool value = (bool)e.NewValue;
                if (dp.GetTemplateChild(PartDateCheckBox) is CheckBox cb)
                {
                    cb.IsChecked = value;
                }
            }
        }
        #endregion

        #region IsReadOnly
        /// <summary>
        /// Gets/Sets calendar text edit
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly", typeof(bool), typeof(ZapDatePicker),
            new PropertyMetadata
            {
                DefaultValue = true,
                PropertyChangedCallback = new PropertyChangedCallback(OnReadOnlyChanged)
            });

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        private static void OnReadOnlyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ZapDatePicker dp 
                && dp.Template != null 
                && dp.GetTemplateChild(PartDateTextBox) is TextBox tb)
            {
                tb.IsReadOnly = (bool)e.NewValue;
            }
        }
        #endregion

        #region Text
        /// <summary>
        /// Gets/Sets text displayed in textbox
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ZapDatePicker),
            new PropertyMetadata(
                DateTime.Today.ToString(),
                new PropertyChangedCallback(OnTextChanged)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            //ZapDatePicker dp = o as ZapDatePicker;
        }
        #endregion

        #region WeekColumnVisibility
        /// <summary>
        /// Gets/Sets the week column visibility
        /// </summary>
        public static readonly DependencyProperty WeekColumnVisibilityProperty = DependencyProperty.Register(
            "WeekColumnVisibility", typeof(Visibility), typeof(ZapDatePicker),
            new FrameworkPropertyMetadata
            {
                DefaultValue = Visibility.Collapsed,
                AffectsRender = true
            });

        public Visibility WeekColumnVisibility
        {
            get { return (Visibility)GetValue(WeekColumnVisibilityProperty); }
            set { SetValue(WeekColumnVisibilityProperty, value); }
        }
        #endregion
        #endregion

        #region SelectedDateChangedEvent
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedDateChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapDatePicker));

        public event RoutedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }

        private static void OnSelectedDateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
            dp.Text = string.Format(dp.FormatString, e.NewValue);
            dp.RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent, dp));
        }
        #endregion

        #region ThemableControl implementation
        internal override string ThemeFolder => "ZapDatePicker";

        public override void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            base.OnThemeChanged(sender, e);

            // Control
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapDatePickerResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapDatePickerResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(ForegroundProperty, ResourceKeys.ZapDatePickerResourceKeys.ForegroundKey);
            // Button background
            this.SetThemePropertyDefault(ButtonBackgroundProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBackgroundKey);
            this.SetThemePropertyDefault(ButtonBackgroundHoverProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBackgroundHoverKey);
            this.SetThemePropertyDefault(ButtonBackgroundPressedProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBackgroundPressedKey);
            // Button borderbrush
            this.SetThemePropertyDefault(ButtonBorderProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBorderKey);
            this.SetThemePropertyDefault(ButtonBorderHoverProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBorderHoverKey);
            this.SetThemePropertyDefault(ButtonBorderPressedProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBorderPressedKey);
            // Icon
            this.SetThemePropertyDefault(IconNormalProperty, ResourceKeys.ZapDatePickerResourceKeys.IconNormalKey);
            this.SetThemePropertyDefault(IconHoverProperty, ResourceKeys.ZapDatePickerResourceKeys.IconHoverKey);
            this.SetThemePropertyDefault(IconPressedProperty, ResourceKeys.ZapDatePickerResourceKeys.IconPressedKey);

            var calendarThemes = Enum.GetNames(typeof(CalendarPickerThemes));
            if (calendarThemes.Any(t => t == Theme))
            {
                CalendarTheme = Theme;
            }
        }
        #endregion

        #region Constructors
        public ZapDatePicker()
        {
            // defaults
            MinHeight = 24;
            MinWidth = 90;
        }

        static ZapDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapDatePicker), new FrameworkPropertyMetadata(typeof(ZapDatePicker)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;
            // Normal
            BackgroundProperty.OverrideMetadata(typeof(ZapDatePicker), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapDatePicker), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            ForegroundProperty.OverrideMetadata(typeof(ZapDatePicker), new FrameworkPropertyMetadata(null, options, OnForegroundChanged) { Inherits = false });
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Apply template and bindings
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // templates was applied
            HasInitialized = true;

            // initialize style
            SetButtonStyle();

            if (FindElement("Part_CalendarGrid") is Grid grdCalendar)
            {
                if (grdCalendar.Children[0] is CalendarPicker.Calendar calendar)
                {
                    Calendar = calendar;
                    calendar.Theme = CalendarTheme;
                    calendar.Height = CalendarHeight;
                    calendar.Width = CalendarWidth;
                    calendar.DisplayDateStart = DisplayDateStart;
                    calendar.DisplayDateEnd = DisplayDateEnd;
                    calendar.SelectedDate = SelectedDate;
                }
            }

            if (FindElement("Part_CalendarPopup") is Popup popup)
            {
                Popup = popup;
                Popup.Placement = PlacementMode.Custom;
                Popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(PopupPlacementCallback);
                Popup.Opened += (s, e) => Calendar.RefreshSelected();
            }

            // set element bindings
            SetBindings();

            // startup date
            Text = string.Format(FormatString, SelectedDate);
        }
        #endregion

        #region Control Methods
        /// <summary>
        /// Find element in the template
        /// </summary>
        private object FindElement(string name)
        {
            try
            {
                return HasInitialized ? Template.FindName(name, this) : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Bind elements to control
        /// </summary>
        private void SetBindings()
        {
            if (FindElement(PartDateTextBox) is TextBox textbox)
            {
                textbox.LostFocus += new RoutedEventHandler(DateTextBox_LostFocus);
                textbox.KeyUp += new KeyEventHandler(DateTextBox_KeyUp);
            }

            if (FindElement(PartCalendarButton) is Button btn)
            {
                btn.Click += new RoutedEventHandler(CalendarButton_Click);
            }

            if (!(BindingOperations.GetBinding(this, FooterVisibilityProperty) is Binding calendarFooterBinding))
            {
                calendarFooterBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(FooterVisibilityPropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.FooterVisibilityProperty, calendarFooterBinding);

            if (!(BindingOperations.GetBinding(this, WeekColumnVisibilityProperty) is Binding calendarWeekColumnBinding))
            {
                calendarWeekColumnBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(WeekColumnVisibilityPropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.WeekColumnVisibilityProperty, calendarWeekColumnBinding);

            if (!(BindingOperations.GetBinding(this, CalendarHeightProperty) is Binding calendarHeightBinding))
            {
                calendarHeightBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(CalendarHeightPropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(HeightProperty, calendarHeightBinding);

            if (!(BindingOperations.GetBinding(this, CalendarWidthProperty) is Binding calendarWidthBinding))
            {
                calendarWidthBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(CalendarWidthPropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(WidthProperty, calendarWidthBinding);

            if (!(BindingOperations.GetBinding(this, SelectedDateProperty) is Binding selectedDateBinding))
            {
                selectedDateBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(SelectedDatePropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.SelectedDateProperty, selectedDateBinding);

            if (!(BindingOperations.GetBinding(this, DisplayDateProperty) is Binding displayDateBinding))
            {
                displayDateBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(DisplayDatePropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.DisplayDateProperty, displayDateBinding);

            if (!(BindingOperations.GetBinding(this, DisplayDateStartProperty) is Binding displayDateStartBinding))
            {
                displayDateStartBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(DisplayDateStartPropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.DisplayDateStartProperty, displayDateStartBinding);

            if (!(BindingOperations.GetBinding(this, DisplayDateEndProperty) is Binding displayDateEndBinding))
            {
                displayDateEndBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(DisplayDateEndPropName),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.DisplayDateEndProperty, displayDateEndBinding);
        }

        /// <summary>
        /// Convert date to text
        /// </summary>
        private void ValidateText()
        {
            if (FindElement(PartDateTextBox) is TextBox textbox)
            {
                if (DateTime.TryParse(textbox.Text, out DateTime date))
                {
                    SelectedDate = date;
                    DisplayDate = date;
                }
                Text = string.Format(FormatString, SelectedDate);
            }
        }

        #endregion
    }
}

#region Author/About
/************************************************************************************
*  vhDatePicker 1.2                                                                 *
*                                                                                   *
*  Created:     June 20, 2010                                                       *
*  Built on:    Vista/Win7                                                          *
*  Purpose:     Date Picker Control                                                 *
*  Revision:    1.2                                                                 *
*  Tested On:   Win7 32bit                                                          *
*  IDE:         C# 2008 SP1 FW 3.5                                                  *
*  Referenced:  VTD Freeware                                                        *
*  Author:      John Underhill (Steppenwolfe)                                       *
*                                                                                   *
*************************************************************************************

You can not:
-Sell or redistribute this code or the binary for profit. This is freeware.
-Use this control, or porions of this code in spyware, malware, or any generally acknowledged form of malicious software.
-Remove or alter the above author accreditation, or this disclaimer.

You can:
-Use this control in private and commercial applications.
-Use portions of this code in your own projects or commercial applications.

I will not:
-Except any responsibility for this code whatsoever.
-Modify on demand.. you have the source code, read it, learn from it, write it.
-There is no guarantee of fitness, nor should you have any expectation of support. 
-I further renounce any and all responsibilities for this code, in every way conceivable, 
now, and for the rest of time.

Cheers,
John
steppenwolfe_2000@yahoo.com
*/
#endregion

#region Directives
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using ZapanControls.Controls.CalendarPicker;
using ZapanControls.Helpers;
using ZapanControls.Controls.Themes;
using System.Collections.Generic;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.ControlEventArgs;
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
    public class ZapDatePicker : ThemableControl
    {
        #region Fields
        private string FormatString = "{0:dd/MM/yyyy}";
        #endregion

        #region Theme Declarations
        public static ThemePath Oceatech = new ThemePath(ZapDatePickerThemes.Oceatech, "/ZapanControls;component/Themes/ZapDatePicker/Oceatech.xaml");
        public static ThemePath Contactel = new ThemePath(ZapDatePickerThemes.Contactel, "/ZapanControls;component/Themes/ZapDatePicker/Contactel.xaml");
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
            if (Popup != null)
            {
                if (CalendarPlacement != PlacementType.Left)
                {
                    Button button = (Button)FindElement("Part_CalendarButton");
                    double btnWidth = button?.ActualWidth ?? 0.0;

                    Popup.Placement = PlacementMode.RelativePoint;
                    Popup.HorizontalOffset = btnWidth - CalendarWidth;
                    Popup.VerticalOffset = ActualHeight;
                }
                else
                {
                    Popup.HorizontalOffset = 0;
                    Popup.VerticalOffset = 0;
                    Popup.Placement = PlacementMode.Bottom;
                }
                Popup.IsOpen = true;
            }
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
                ValidateText();
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

            Grid grdCalendar = (Grid)FindElement("Part_CalendarGrid");
            if (grdCalendar != null)
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

            Popup popCalendarPopup = (Popup)FindElement("Part_CalendarPopup");
            if (popCalendarPopup != null)
            {
                Popup = popCalendarPopup;
                Popup.Opened += (s, e) => Calendar.RefreshSelected();
            }

            // set element bindings
            SetBindings();

            // startup date
            Text = string.Format(FormatString, SelectedDate);
        }
        #endregion

        #region ThemableControl implementation
        public override void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            // Control
            this.SetValueCommon(BackgroundProperty, ResourceKeys.ZapDatePickerResourceKeys.BackgroundKey);
            this.SetValueCommon(BorderBrushProperty, ResourceKeys.ZapDatePickerResourceKeys.BorderBrushKey);
            // Button background
            this.SetValueCommon(ButtonBackgroundProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBackgroundKey);
            this.SetValueCommon(ButtonBackgroundHoverProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBackgroundHoverKey);
            this.SetValueCommon(ButtonBackgroundPressedProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBackgroundPressedKey);
            // Button borderbrush
            this.SetValueCommon(ButtonBorderProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBorderKey);
            this.SetValueCommon(ButtonBorderHoverProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBorderHoverKey);
            this.SetValueCommon(ButtonBorderPressedProperty, ResourceKeys.ZapDatePickerResourceKeys.ButtonBorderPressedKey);
            // Icon
            this.SetValueCommon(IconNormalProperty, ResourceKeys.ZapDatePickerResourceKeys.IconNormalKey);
            this.SetValueCommon(IconHoverProperty, ResourceKeys.ZapDatePickerResourceKeys.IconHoverKey);
            this.SetValueCommon(IconPressedProperty, ResourceKeys.ZapDatePickerResourceKeys.IconPressedKey);
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
        }

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
            ZapDatePicker dp = o as ZapDatePicker;
            dp.SetButtonStyle();
        }

        private void SetButtonStyle()
        {
            if (Template != null)
            {
                Button button = (Button)FindElement("Part_CalendarButton");
                Image buttonImage = (Image)FindElement("Part_ButtonImage");
                if (button != null || buttonImage != null)
                {
                    if (ButtonStyle == ButtonType.Image)
                    {
                        button.Style = (Style)TryFindResource("ButtonImageStyle");
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.UriSource = new Uri("pack://application:,,,/ZapanControls;component/Controls/DatePicker/Images/outlook_calendar_day.png", UriKind.Absolute);
                        img.EndInit();
                        buttonImage.Source = img;
                    }
                    else
                    {
                        buttonImage.Source = null;
                        if (ButtonStyle == ButtonType.Brush)
                        {
                            button.Style = (Style)TryFindResource("ButtonBrushStyle");
                        }
                        else
                        {
                            button.Style = (Style)TryFindResource("ButtonFlatStyle");
                        }
                    }
                }
            }
        }
        #endregion

        #region ButtonBackground
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register(
            "ButtonBackground", typeof(Brush), typeof(ZapDatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

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
            "ButtonBorder", typeof(Brush), typeof(ZapDatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

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
            new FrameworkPropertyMetadata (null, FrameworkPropertyMetadataOptions.AffectsRender));

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
                DefaultValue = (double)175,
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
            //ZapDatePicker pdp = d as ZapDatePicker;
            double value = (double)o;

            if (value < 160)
                return 160;

            if (value > 350)
                return 350;

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
                DefaultValue = (double)175,
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
                DefaultValue = "Oceatech",
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
            string value = (string)o;
            if (string.IsNullOrEmpty(value))
                return "Oceatech";
            return o;
        }

        private static void OnCalendarThemeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;

            if (dp.Template != null)
            {
                if (dp.FindElement("Part_CalendarGrid") is Grid grid)
                {
                    if (grid.Children[0] is CalendarPicker.Calendar cld)
                    {
                        // test against legit value
                        ArrayList themes = GetEnumArray(typeof(CalendarPickerThemes));
                        foreach (string ot in themes)
                        {
                            if ((string)e.NewValue == ot)
                            {
                                cld.Theme = (string)e.NewValue;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts enum members to string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static ArrayList GetEnumArray(Type type)
        {
            FieldInfo[] info = type.GetFields();
            ArrayList fields = new ArrayList();

            foreach (FieldInfo fInfo in info)
            {
                fields.Add(fInfo.Name);
            }

            return fields;
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
                return dp.DisplayDateStart;

            if (value > dp.DisplayDateEnd)
                return dp.DisplayDateEnd;

            return o;
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

            if (value > dp.DisplayDateEnd)
                return dp.DisplayDateEnd;

            return o;
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

            if (value < dp.DisplayDateStart)
                return dp.DisplayDateStart;

            return o;
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

            if (dp.FindElement("Part_DateCheckBox") is CheckBox cb)
                cb.Visibility = value == false ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion

        #region IsChecked
        /// <summary>OnIsCheckedChanged
        /// Gets/Sets checkbox control check state
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked", typeof(bool), typeof(ZapDatePicker), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsCheckedChanged)));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private static void OnIsCheckedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
            bool value = (bool)e.NewValue;
            CheckBox cb = (CheckBox)dp.FindElement("Part_DateCheckBox");

            if (cb != null)
                cb.IsChecked = value;
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
            ZapDatePicker dp = o as ZapDatePicker;
            if (dp.Template != null)
            {
                if (dp.FindElement("Part_DateTextBox") is TextBox tb)
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
            TextBox textbox = (TextBox)FindElement("Part_DateTextBox");
            if (textbox != null)
            {
                Binding textBinding = new Binding
                {
                    Source = textbox,
                    Path = new PropertyPath("Text"),
                    Mode = BindingMode.TwoWay,
                };
                SetBinding(TextProperty, textBinding);

                textbox.LostFocus += new RoutedEventHandler(DateTextBox_LostFocus);
                textbox.KeyUp += new KeyEventHandler(DateTextBox_KeyUp);
                textbox.Foreground = Foreground;
            }

            Button button = (Button)FindElement("Part_CalendarButton");
            if (button != null)
                button.Click += new RoutedEventHandler(CalendarButton_Click);

            CheckBox checkbox = (CheckBox)FindElement("Part_DateCheckBox");
            if (checkbox != null)
            {
                if (IsCheckable)
                    checkbox.Visibility = Visibility.Visible;
                else
                    checkbox.Visibility = Visibility.Collapsed;

               /* Binding isCheckedBinding = new Binding
                {
                    Source = checkbox,
                    Path = new PropertyPath("IsChecked"),
                    Mode = BindingMode.TwoWay,
                };
                this.SetBinding(IsCheckedProperty, isCheckedBinding);*/
            }

            if (!(BindingOperations.GetBinding(this, FooterVisibilityProperty) is Binding calendarFooterBinding))
            {
                calendarFooterBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("FooterVisibility"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.FooterVisibilityProperty, calendarFooterBinding);

            if (!(BindingOperations.GetBinding(this, WeekColumnVisibilityProperty) is Binding calendarWeekColumnBinding))
            {
                calendarWeekColumnBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("WeekColumnVisibility"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.WeekColumnVisibilityProperty, calendarWeekColumnBinding);

            if (!(BindingOperations.GetBinding(this, CalendarHeightProperty) is Binding calendarHeightBinding))
            {
                calendarHeightBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("CalendarHeight"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(HeightProperty, calendarHeightBinding);

            if (!(BindingOperations.GetBinding(this, CalendarWidthProperty) is Binding calendarWidthBinding))
            {
                calendarWidthBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("CalendarWidth"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(WidthProperty, calendarWidthBinding);

            if (!(BindingOperations.GetBinding(this, SelectedDateProperty) is Binding selectedDateBinding))
            {
                selectedDateBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("SelectedDate"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.SelectedDateProperty, selectedDateBinding);

            if (!(BindingOperations.GetBinding(this, DisplayDateProperty) is Binding displayDateBinding))
            {
                displayDateBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("DisplayDate"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.DisplayDateProperty, displayDateBinding);

            if (!(BindingOperations.GetBinding(this, DisplayDateStartProperty) is Binding displayDateStartBinding))
            {
                displayDateStartBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("DisplayDateStart"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.DisplayDateStartProperty, displayDateStartBinding);

            if (!(BindingOperations.GetBinding(this, DisplayDateEndProperty) is Binding displayDateEndBinding))
            {
                displayDateEndBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("DisplayDateEnd"),
                    Mode = BindingMode.TwoWay,
                };
            }
            Calendar.SetBinding(CalendarPicker.Calendar.DisplayDateEndProperty, displayDateEndBinding);
        }

        /// <summary>
        /// Convert date to to text
        /// </summary>
        private void ValidateText()
        {
            TextBox textbox = (TextBox)FindElement("Part_DateTextBox");

            if (textbox != null)
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

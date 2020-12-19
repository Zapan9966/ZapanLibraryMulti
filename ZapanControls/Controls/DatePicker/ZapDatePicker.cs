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
    public class ZapDatePicker : Control, INotifyPropertyChanged
    {
        #region Fields
        private string FormatString = "{0:MMM dd, yyyy}";
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
            //Popup popCalendarPopup = (Popup)FindElement("Part_CalendarPopup");
            if (Popup != null)
            {
                if (CalendarPlacement != PlacementType.Left)
                {
                    double btnWidth = 0.0;
                    Button button = (Button)FindElement("Part_CalendarButton");
                    if (button != null)
                        btnWidth = button.ActualWidth;

                    Popup.Placement = PlacementMode.RelativePoint;
                    //popCalendarPopup.HorizontalOffset = -(ActualWidth + 4);
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

            // CalendarPlacement
            PropertyMetadata calendarPlacementMetadata = new PropertyMetadata
            {
                DefaultValue = PlacementType.Right
            };
            CalendarPlacementProperty = DependencyProperty.Register("CalendarPlacement", typeof(PlacementType), typeof(ZapDatePicker), calendarPlacementMetadata);

            // IsCheckable
            FrameworkPropertyMetadata isCheckablePropertyMetadata = new FrameworkPropertyMetadata
            {
                DefaultValue = false,
                PropertyChangedCallback = new PropertyChangedCallback(OnIsCheckableChanged), 
                AffectsRender = true
            };
            IsCheckableProperty = DependencyProperty.Register("IsCheckable", typeof(bool), typeof(ZapDatePicker), isCheckablePropertyMetadata);

            // IsCheckedProperty
            FrameworkPropertyMetadata isCheckedPropertyMetadata = new FrameworkPropertyMetadata
            {
                DefaultValue = false,
                PropertyChangedCallback = new PropertyChangedCallback(OnIsCheckedChanged), 
            };
            IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(ZapDatePicker), isCheckedPropertyMetadata);

            // DateFormat
            FrameworkPropertyMetadata dateFormatMetadata = new FrameworkPropertyMetadata
            {
                DefaultValue = DateFormatType.MMMddyyyy,
                PropertyChangedCallback = new PropertyChangedCallback(OnDateFormatChanged), 
                AffectsRender = true
            };
            DateFormatProperty = DependencyProperty.Register("DateFormat", typeof(DateFormatType), typeof(ZapDatePicker), dateFormatMetadata);

            // ButtonStyle
            FrameworkPropertyMetadata buttonStyleMetadata = new FrameworkPropertyMetadata
            {
                DefaultValue = ButtonType.Flat,
                PropertyChangedCallback = new PropertyChangedCallback(OnButtonStyleChanged),
                AffectsRender = true
            };
            ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(ButtonType), typeof(ZapDatePicker), buttonStyleMetadata);

            // DisplayDate
            PropertyMetadata displayDateMetadata = new PropertyMetadata
            {
                CoerceValueCallback = CoerceDateToBeInRange,
                DefaultValue = DateTime.Today,
                PropertyChangedCallback = new PropertyChangedCallback(OnDisplayDateChanged)
            };
            DisplayDateProperty = DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(ZapDatePicker), displayDateMetadata);

            // Text
            PropertyMetadata textMetadata = new PropertyMetadata
            {
                DefaultValue = DateTime.Today.ToString(),
                PropertyChangedCallback = new PropertyChangedCallback(OnTextChanged)
            };
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ZapDatePicker), textMetadata);

            // DisplayDateStart
            PropertyMetadata displayDateStartMetaData = new PropertyMetadata
            {
                CoerceValueCallback = CoerceDateToBeInRange,
                DefaultValue = new DateTime(1, 1, 1),
            };
            DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart", typeof(DateTime), typeof(ZapDatePicker), displayDateStartMetaData);

            // DisplayDateEnd
            PropertyMetadata displayDateEndMetaData = new PropertyMetadata
            {
                DefaultValue = new DateTime(2199, 1, 1),
                CoerceValueCallback = new CoerceValueCallback(CoerceDisplayDateEnd)
            };
            DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd", typeof(DateTime), typeof(ZapDatePicker), displayDateEndMetaData);

            // CalendarTheme
            FrameworkPropertyMetadata calendarThemeMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = "AeroNormal",
                CoerceValueCallback = new CoerceValueCallback(CoerceCalendarTheme),
                PropertyChangedCallback = new PropertyChangedCallback(OnThemeChanged),
                AffectsRender = true
            };
            CalendarThemeProperty = DependencyProperty.Register("CalendarTheme", typeof(string), typeof(ZapDatePicker), calendarThemeMetaData);

            // CalendarHeight
            FrameworkPropertyMetadata calendarHeightMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = (double)175,
                CoerceValueCallback = new CoerceValueCallback(CoerceCalendarSize),
                AffectsRender = true
            };
            CalendarHeightProperty = DependencyProperty.Register("CalendarHeight", typeof(double), typeof(ZapDatePicker), calendarHeightMetaData);

            // CalendarWidth
            FrameworkPropertyMetadata calendarWidthMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = (double)175,
                CoerceValueCallback = new CoerceValueCallback(CoerceCalendarSize),
                AffectsRender = true
            };
            CalendarWidthProperty = DependencyProperty.Register("CalendarWidth", typeof(double), typeof(ZapDatePicker), calendarWidthMetaData);

            // FooterVisibility
            FrameworkPropertyMetadata footerVisibilityMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = Visibility.Collapsed,
                AffectsRender = true
            };
            FooterVisibilityProperty = DependencyProperty.Register("FooterVisibility", typeof(Visibility), typeof(ZapDatePicker), footerVisibilityMetaData);

            // IsReadOnly
            PropertyMetadata isReadOnlyMetaData = new PropertyMetadata
            {
                DefaultValue = true,
                PropertyChangedCallback = new PropertyChangedCallback(OnReadOnlyChanged)
            };
            IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ZapDatePicker), isReadOnlyMetaData);

            // WeekColumnVisibility
            FrameworkPropertyMetadata weekColumnVisibilityMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = Visibility.Collapsed,
                AffectsRender = true
            };
            WeekColumnVisibilityProperty = DependencyProperty.Register("WeekColumnVisibility", typeof(Visibility), typeof(ZapDatePicker), weekColumnVisibilityMetaData);

            // ButtonBackgroundBrushProperty
            FrameworkPropertyMetadata buttonBackgroundBrushMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = null,
                PropertyChangedCallback = new PropertyChangedCallback(OnButtonBackgroundBrushChanged),
                AffectsRender = true
            };
            ButtonBackgroundBrushProperty = DependencyProperty.Register("ButtonBackgroundBrush", typeof(Brush), typeof(ZapDatePicker), buttonBackgroundBrushMetaData);

            // ButtonBorderBrushProperty
            FrameworkPropertyMetadata buttonBorderBrushPropertyMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = null,
                PropertyChangedCallback = new PropertyChangedCallback(OnButtonBorderBrushChanged),
                AffectsRender = true
            };
            ButtonBorderBrushProperty = DependencyProperty.Register("ButtonBorderBrush", typeof(Brush), typeof(ZapDatePicker), buttonBorderBrushPropertyMetaData);


            // SelectedDateChanged event registration
            SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZapDatePicker));
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Event raised when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="e">The arguments to pass</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        #endregion

        #endregion

        #region Properties

        #region ButtonStyle
        /// <summary>
        /// Gets/Sets the button appearence
        /// </summary>
        public static readonly DependencyProperty ButtonStyleProperty;

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
                        img.UriSource = new Uri("pack://application:,,,/ZapanControls;component/Controls/DatePicker/Images/Control_MonthCalendar.bmp", UriKind.Absolute);
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

        #region ButtonBackgroundBrush
        public static readonly DependencyProperty ButtonBackgroundBrushProperty;

        public Brush ButtonBackgroundBrush
        {
            get { return (Brush)GetValue(ButtonBackgroundBrushProperty); }
            set { SetValue(ButtonBackgroundBrushProperty, value); }
        }

        private static void OnButtonBackgroundBrushChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            
            ZapDatePicker dp = o as ZapDatePicker;
            Button button = (Button)dp.FindElement("Part_CalendarButton");
            Brush brush = (Brush)e.NewValue;

            if (button != null && brush != null)
                button.Background = brush;
        }
        #endregion

        #region ButtonBorderBrush
        public static readonly DependencyProperty ButtonBorderBrushProperty;

        public Brush ButtonBorderBrush
        {
            get { return (Brush)GetValue(ButtonBorderBrushProperty); }
            set { SetValue(ButtonBorderBrushProperty, value); }
        }

        private static void OnButtonBorderBrushChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
            Button button = (Button)dp.FindElement("Part_CalendarButton");
            Brush brush = (Brush)e.NewValue;

            if (button != null && brush != null)
                button.BorderBrush = brush;
        }
        #endregion

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

        #region CalendarHeight
        /// <summary>
        /// Gets/Sets calendar height
        /// </summary>
        public static readonly DependencyProperty CalendarHeightProperty;

        public double CalendarHeight
        {
            get { return (double)GetValue(CalendarHeightProperty); }
            set { SetValue(CalendarHeightProperty, value); }
        }

        private static object CoerceCalendarSize(DependencyObject d, object o)
        {
            ZapDatePicker pdp = d as ZapDatePicker;
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
        public static readonly DependencyProperty CalendarPlacementProperty;

        public PlacementType CalendarPlacement
        {
            get { return (PlacementType)GetValue(CalendarPlacementProperty); }
            set { SetValue(CalendarPlacementProperty, value); }
        }
        #endregion

        #region CalendarWidth
        /// <summary>Gets/Sets calendar width</summary>
        public static readonly DependencyProperty CalendarWidthProperty;

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
        public static readonly DependencyProperty CalendarThemeProperty;

        public string CalendarTheme
        {
            get { return (string)GetValue(CalendarThemeProperty); }
            set { SetValue(CalendarThemeProperty, value); }
        }

        private static object CoerceCalendarTheme(DependencyObject d, object o)
        {
            string value = (string)o;
            if (string.IsNullOrEmpty(value))
                return "AeroNormal";
            return o;
        }

        private static void OnThemeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;

            if (dp.Template != null)
            {
                Grid grid = (Grid)dp.FindElement("Part_CalendarGrid");
                if (grid != null)
                {
                    if (grid.Children[0] is CalendarPicker.Calendar cld)
                    {
                        // test against legit value
                        ArrayList themes = GetEnumArray(typeof(Themes));
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
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(ZapDatePicker), 
            new PropertyMetadata(DateTime.Today, new PropertyChangedCallback(OnSelectedDateChanged), CoerceDateToBeInRange));

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
        public static readonly DependencyProperty DateFormatProperty;

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
        public static readonly DependencyProperty DisplayDateProperty;

        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        private static void OnDisplayDateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
        }
        #endregion

        #region DisplayDateStart
        /// <summary>
        /// Gets/Sets the minimum date that can be displayed
        /// </summary>
        public static readonly DependencyProperty DisplayDateStartProperty;

        public DateTime DisplayDateStart
        {
            get { return (DateTime)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }
        #endregion

        #region DisplayDateEnd
        /// <summary>
        /// Gets/Sets the maximum date that is displayed, and can be selected
        /// </summary>
        public static readonly DependencyProperty DisplayDateEndProperty;

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
        public static readonly DependencyProperty FooterVisibilityProperty;

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
        public static readonly DependencyProperty IsCheckableProperty;

        public bool IsCheckable
        {
            get { return (bool)GetValue(IsCheckableProperty); }
            set { SetValue(IsCheckableProperty, value); }
        }

        private static void OnIsCheckableChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
            bool value = (bool)e.NewValue;
            CheckBox cb = (CheckBox)dp.FindElement("Part_DateCheckBox");

            if (cb != null)
                cb.Visibility = value == false ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion

        #region IsChecked
        /// <summary>OnIsCheckedChanged
        /// Gets/Sets checkbox control check state
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty;

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
        public static readonly DependencyProperty IsReadOnlyProperty;

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
                TextBox tb = (TextBox)dp.FindElement("Part_DateTextBox");
                if (tb != null)
                    tb.IsReadOnly = (bool)e.NewValue;
            }
        }
        #endregion

        #region Text
        /// <summary>
        /// Gets/Sets text displayed in textbox
        /// </summary>
        public static readonly DependencyProperty TextProperty;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ZapDatePicker dp = o as ZapDatePicker;
        }
        #endregion

        #region WeekColumnVisibility
        /// <summary>
        /// Gets/Sets the week column visibility
        /// </summary>
        public static readonly DependencyProperty WeekColumnVisibilityProperty;

        public Visibility WeekColumnVisibility
        {
            get { return (Visibility)GetValue(WeekColumnVisibilityProperty); }
            set { SetValue(WeekColumnVisibilityProperty, value); }
        }
        #endregion

        #endregion

        #region SelectedDateChangedEvent
        public static readonly RoutedEvent SelectedDateChangedEvent;

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

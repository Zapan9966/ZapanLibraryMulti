#region Author/About
/************************************************************************************
*  vhCalendar 1.5                                                                   *
*                                                                                   *
*  Created:     June 20, 2010                                                       *
*  Built on:    Vista/Win7                                                          *
*  Purpose:     Calendar Control                                                    *
*  Revision:    1.5d                                                                *
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

-Updates to v1.2-
-Fixed-
-Selection bug on multiselect (math error)
-Some adjustments made to xaml (I'll flesh it out in next rev)
-Added-
-SelectedDatesChanged event -returns collections of old and new dates
-PropertyChange event -notifications added to most properties
-DisplayModeChanged event -fires when view is changed
-IsTodayHighlighted property -highlight current date
-HeaderHeight property -adjust header height [18-30]

-Updates to v1.3-
-Fixed-
-'stutter' at end of scrolling animation
-Some minor discrepencies in vhCalendar.Generic.xaml
-Theme routines adjusted to work on a per control instance (what I wanted)
-Added-
-Example of applying a custom theme
-ResourceKeys for WeekColumn, Footer, and DayColumn Forecolor
-updated example code
-a lot of small tweaks to xaml

-Updates to v1.4-
-seperated style files for aero, luna and office
-updated graphics
-updated custom style example

-Updates to v1.5-
-rewrote most of the Calendar class
-reorganized code into seperate classes
-added blackout dates
-fixed some xaml issues
-added drag and drop facility
-fixed a math bug in MonthModeDateToRowColumn
-added IsAnimated property
-fixed decade title
-added header font properties
-rewrote DatePicker control
-added checkbox
-added button brush overrides
-added calendar placement
-updated example code
-fixed theme change bug
-fixed footer visibility bug
-fixed datepicker placement bug
-fixed multi select persist bug
-fixed readonly bug in datepicker
-fixed header font size/weight properties
-fixed margin in header content

Cheers,
John
steppenwolfe_2000@yahoo.com
*/
#endregion

#region Directives
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Linq;
using ZapanControls.Helpers;
using ZapanControls.Libraries;
using ZapanControls.Controls.Themes;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.ResourceKeys;
using ZapanControls.Controls.ControlEventArgs;
#endregion

namespace ZapanControls.Controls.CalendarPicker
{
    [DefaultEvent("SelectedDateChanged"),
    DefaultProperty("DisplayDate"),
    TemplatePart(Name = "Part_HeaderBorder", Type = typeof(Border)),
    TemplatePart(Name = "Part_TitleButton", Type = typeof(DateButton)),
    TemplatePart(Name = "Part_NextButton", Type = typeof(RepeatButton)),
    TemplatePart(Name = "Part_PreviousButton", Type = typeof(RepeatButton)),
    TemplatePart(Name = "Part_MonthContainer", Type = typeof(Grid)),
    TemplatePart(Name = "Part_DayGrid", Type = typeof(UniformGrid)),
    TemplatePart(Name = "Part_WeekGrid", Type = typeof(UniformGrid)),
    TemplatePart(Name = "Part_MonthGrid", Type = typeof(UniformGrid)),
    TemplatePart(Name = "Part_DecadeGrid", Type = typeof(UniformGrid)),
    TemplatePart(Name = "Part_YearGrid", Type = typeof(UniformGrid)),
    TemplatePart(Name = "Part_CurrentDatePanel", Type = typeof(StackPanel)),
    TemplatePart(Name = "Part_CurrentDateText", Type = typeof(TextBlock)),
    TemplatePart(Name = "Part_AnimationContainer", Type = typeof(Grid)),
    TemplatePart(Name = "Part_FooterContainer", Type = typeof(Grid))]
    public class Calendar : ThemableControl<CalendarPickerThemes>
    {
        #region Property Name Constants
        /// <summary>
        /// Property names as string constants
        /// </summary>
        private const string CurrentlySelectedDatePropName = "CurrentlySelectedDateString";
        private const string DisplayModePropName = "DisplayMode";
        private const string DisplayDateStartPropName = "DisplayDateStart";
        private const string FooterVisibilityPropName = "FooterVisibility";
        private const string TodayHighlightedPropName = "IsTodayHighlighted";
        //private const string SelectionModePropName = "SelectionMode";
        private const string IsRangeSelectionPropName = "IsRangeSelection";
        private const string ShowDaysOfAllMonthsPropName = "ShowDaysOfAllMonths";
        private const string WeekColumnVisibilityPropName = "WeekColumnVisibility";
        private const string HeaderHeightPropName = "HeaderHeight";
        private const string HeaderFontSizePropName = "HeaderFontSize";
        private const string HeaderFontWeightPropName = "HeaderFontWeight";
        private const string AllowDragPropName = "AllowDrag";
        private const string AdornDragPropName = "AdornDrag";
        private const string IsAnimatedPropName = "IsAnimated";
        #endregion

        #region MonthList Enum
        /// <summary>
        /// Day of week enumeration
        /// </summary>
        private enum DayOfWeek
        {
            Lundi = 1,
            Mardi,
            Mercredi,
            Jeudi,
            Vendredi,
            Samedi,
            Dimanche,
        }

        /// <summary>
        /// Month enumeration
        /// </summary>
        private enum MonthList
        {
            Janvier = 1,
            Février,
            Mars,
            Avril,
            Mai,
            Juin,
            Juillet,
            Août,
            Septembre,
            Octobre,
            Novembre,
            Décembre
        }
        #endregion

        #region Fields
        Point _dragStart = new Point();
        Point _currentPos = new Point();
        DragData _dragData;
        DispatcherTimer _dispatcherTimer;
        private readonly DateButton[,] _btnMonthMode = new DateButton[6, 7];
        private readonly DateButton[,] _btnYearMode = new DateButton[4, 3];
        private readonly DateButton[] _btnDecadeMode = new DateButton[10];
        private readonly BlackoutDatesCollection _blackoutDates;
        #endregion

        #region Private Properties
        private bool HasInitialized { get; set; }
        private bool IsDragging { get; set; }
        //private bool IsDragImage { get; set; }
        private bool IsAnimating { get; set; }
        private bool IsMoveForward { get; set; }
        private Window ParentWindow { get; set; }
        #endregion

        #region Internal Event Handlers
        /// <summary>
        /// Drag Timer callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                StopDragTimer();
        }

        /// <summary>
        /// Listen for escape key to cancel drag op
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentWindow_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
            {
                StopDragTimer();
                e.Action = DragAction.Cancel;
            }
        }

        /// <summary>
        /// Get drag feedback from parent window for adorn window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentWindow_PreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (_dragData.Adorner != null)
            {
                Point pt = new Point(_currentPos.X + 16, _currentPos.Y + 16);
                _dragData.Adorner.Offset = pt;
            }

            Mouse.SetCursor(Cursors.Arrow);
            e.UseDefaultCursors = false;
            e.Handled = true;
        }

        /// <summary>
        /// Used to get current mouse coords in adorned drag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentWindow_PreviewDragOver(object sender, DragEventArgs e)
        {
            _currentPos = e.GetPosition(this);
        }

        /// <summary>
        /// Store initial cursor position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetDragData();
            _dragStart = e.GetPosition(null);
        }

        /// <summary>
        /// Test for drag op and start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (AllowDrag)
            {
                // Get the current mouse position
                _currentPos = e.GetPosition(null);
                Vector diff = _dragStart - _currentPos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
                        Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    if (!IsDragging)
                    {
                        IsDragging = true;
                        DateButton button = sender as DateButton;
                        DateTime data = (DateTime)button.Tag;

                        if (AdornDrag)
                        {
                            _dragData = new DragData(button, data, CreateDragWindow(button));
                            if (_dragData.Parent != null && _dragData.Data != null)
                            {
                                AllowDrop = true;
                                AddDragEventHandlers();
                                DataObject dragData = new DataObject("DateTime", _dragData.Data);
                                DragDrop.DoDragDrop(_dragData.Parent, dragData, DragDropEffects.Copy);
                                StartDragTimer();
                            }
                        }
                        else
                        {
                            _dragData = new DragData(button, data, null);
                            if (_dragData.Parent != null && _dragData.Data != null)
                            {
                                DataObject dragData = new DataObject("DateTime", _dragData.Data);
                                DragDrop.DoDragDrop(_dragData.Parent, dragData, DragDropEffects.Copy);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fires when a DateButton is clicked in Decade view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecadeModeButton_Click(object sender, RoutedEventArgs e)
        {
            DateButton button = (DateButton)sender;

            if ((int)button.Tag > 0)
            {
                DisplayDate = new DateTime((int)button.Tag, 1, 1);
                DisplayMode = DisplayType.Year;
            }
        }

        /// <summary>
        /// Fires when when the margin animation has completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarginAnimation_Completed(object sender, EventArgs e)
        {
            Grid grdScroll = (Grid)FindElement("Part_ScrollGrid");

            if (grdScroll != null)
                grdScroll.Visibility = Visibility.Collapsed;

            IsAnimating = false;
        }

        /// <summary>
        /// Fires when a DateButton is clicked in the Month view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthModeButton_Click(object sender, RoutedEventArgs e)
        {
            DateButton button = (DateButton)sender;
            DateTime date = (DateTime)button.Tag;

            if (SelectionMode == SelectionType.Single)
            {
                ClearSelectedDates(false);

                if (BindingOperations.GetBinding(this, SelectedDateProperty) is Binding binding)
                {
                    VisualTreeHelpers.UpdateBinding(this, binding, date);
                }
                else
                {
                    SelectedDate = date;
                }
                button.IsSelected = true;
            }
            else if (SelectionMode == SelectionType.Multiple)
            {
                ObservableCollection<DateTime> oldDates = SelectedDates.Clone();
                if (SelectedDates.Contains(date))
                {
                    if (IsRangeSelection && date != SelectedDates.First() && date != SelectedDates.Last())
                    {
                        List<DateTime> futurDates = SelectedDates.Where(d => d > date).ToList();

                        for (int i = 0; i < futurDates.Count; i++)
                        {
                            date = futurDates[i];

                            // Récupération des coordonnées du bouton représentant la date
                            MonthModeDateToRowColumn(date, out int row, out int column);

                            // Désélection du bouton
                            _btnMonthMode[row, column].IsSelected = false;
                            SelectedDates.Remove(date);
                        }
                    }
                    else
                    {
                        SelectedDates.Remove(date);
                        button.IsSelected = false;
                    }
                }
                else
                {
                    SelectedDates.Add(date);
                    button.IsSelected = true;
                }

                if (IsRangeSelection)
                {
                    SelectRange();
                }
                else if (BindingOperations.GetBinding(this, SelectedDatesProperty) is Binding binding)
                {
                    VisualTreeHelpers.UpdateBinding(this, binding, new ObservableCollection<DateTime>(SelectedDates.OrderBy(d => d)));
                }
                else 
                {
                    SelectedDates = new ObservableCollection<DateTime>(SelectedDates.OrderBy(d => d));
                }
                OnSelectedDatesChanged(this, new DependencyPropertyChangedEventArgs(SelectedDatesProperty, oldDates, SelectedDates));
            }
            else if (SelectionMode == SelectionType.Week)
            {
                MonthModeDateToRowColumn(date, out int row, out int column);
                ObservableCollection<DateTime> oldDates = SelectedDates.Clone();

                for (int i = 0; i < 7; i++)
                {
                    date = (DateTime)_btnMonthMode[row, i].Tag;
                    if (SelectedDates.Contains(date))
                    {
                        SelectedDates.Remove(date);
                        _btnMonthMode[row, i].IsSelected = false;
                    }
                    else
                    {
                        SelectedDates.Add(date);
                        _btnMonthMode[row, i].IsSelected = true;
                    }
                }

                if (IsRangeSelection)
                {
                    SelectRange();
                }
                else if (BindingOperations.GetBinding(this, SelectedDatesProperty) is Binding binding)
                {
                    VisualTreeHelpers.UpdateBinding(this, binding, new ObservableCollection<DateTime>(SelectedDates.OrderBy(d => d)));
                }
                else
                {
                    SelectedDates = new ObservableCollection<DateTime>(SelectedDates.OrderBy(d => d));
                }
                OnSelectedDatesChanged(this, new DependencyPropertyChangedEventArgs(SelectedDatesProperty, oldDates, SelectedDates));
            }
        }

        private void SelectRange()
        {
            if (SelectedDates.Any())
            {
                // Tri chronologique des dates 
                IEnumerable<DateTime> orderdDates = SelectedDates.OrderBy(d => d);

                // Nombre de jours entre la date de début et de fin
                int days = Convert.ToInt32((orderdDates.Last() - orderdDates.First()).TotalDays);

                if (days != SelectedDates.Count)
                {
                    ClearSelectedDates(false);

                    ObservableCollection<DateTime> selectedDates = new ObservableCollection<DateTime>();

                    for (int i = 0; i <= days; i++)
                    {
                        DateTime date = orderdDates.First().AddDays(i);
                        selectedDates.Add(date);

                        if (date.Year == DisplayDate.Year && date.Month == DisplayDate.Month)
                        {
                            // Récupération des coordonnées du bouton représentant la date
                            MonthModeDateToRowColumn(date, out int row, out int column);

                            // Sélection du bouton
                            _btnMonthMode[row, column].IsSelected = true;
                        }
                    }

                    if (BindingOperations.GetBinding(this, SelectedDatesProperty) is Binding binding)
                    {
                        VisualTreeHelpers.UpdateBinding(this, binding, selectedDates);
                    }
                    else
                    {
                        SelectedDates = selectedDates;
                    }
                }
            }
        }

        /// <summary>
        /// Fires when when the next button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAnimating)
                return;

            int month = DisplayDate.Month;
            int year = DisplayDate.Year;

            try
            {
                DateTime newDisplayDate = DisplayDate;
                if (DisplayMode == DisplayType.Month)
                {
                    if (month == 12)
                        newDisplayDate = new DateTime(year + 1, 1, 1);
                    else
                        newDisplayDate = new DateTime(year, month + 1, 1);
                }
                else if (DisplayMode == DisplayType.Year)
                    newDisplayDate = new DateTime(DisplayDate.Year + 1, 1, 1);
                else if (DisplayMode == DisplayType.Decade)
                    newDisplayDate = new DateTime(year - year % 10 + 10, 1, 1);

                IsMoveForward = true;

                if (newDisplayDate >= DisplayDateStart && newDisplayDate <= DisplayDateEnd)
                    DisplayDate = newDisplayDate;
            }
            catch (ArgumentOutOfRangeException) { }
        }
        
        /// <summary>
        /// Fires when when the previous button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAnimating)
                return;

            int month = DisplayDate.Month;
            int year = DisplayDate.Year;

            try
            {
                DateTime newDisplayDate = DisplayDate;

                if (DisplayMode == DisplayType.Month)
                {
                    if (month == 1)
                        newDisplayDate = new DateTime(year - 1, 12, DateTime.DaysInMonth(year - 1, 12));
                    else
                        newDisplayDate = new DateTime(year, month - 1, DateTime.DaysInMonth(year, month - 1));
                }
                else if (DisplayMode == DisplayType.Year)
                    newDisplayDate = new DateTime(year - 1, 12, DateTime.DaysInMonth(year - 1, 12));
                else if (DisplayMode == DisplayType.Decade)
                    newDisplayDate = new DateTime(year - year % 10 - 1, 12, DateTime.DaysInMonth(year - year % 10 - 1, 12));

                IsMoveForward = false;

                if (newDisplayDate >= DisplayDateStart && newDisplayDate <= DisplayDateEnd)
                    DisplayDate = newDisplayDate;
            }
            catch (ArgumentOutOfRangeException) { }
        }

        /// <summary>
        /// Fires when when the display transition animation has completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StbCalenderTransition_Completed(object sender, EventArgs e)
        {
            IsAnimating = false;
        }

        /// <summary>
        /// Fires when when the title button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleButton_Click(object sender, RoutedEventArgs e)
        {
            if (DisplayMode == DisplayType.Month)
                DisplayMode = DisplayType.Year;
            else if (DisplayMode == DisplayType.Year)
                DisplayMode = DisplayType.Decade;
        }

        /// <summary>
        /// Fires when a DateButton is clicked in Year view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearModeButton_Click(object sender, RoutedEventArgs e)
        {
            DateButton button = (DateButton)sender;

            if ((int)button.Tag > 0)
            {
                int month = (int)button.Tag;
                DisplayDate = new DateTime(DisplayDate.Year, month, 1);
                DisplayMode = DisplayType.Month;
            }
        }
        #endregion

        #region ThemableControl implementation
        internal override string ThemeFolder => "CalendarPicker";

        public override void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            if (e.Source is Calendar c)
            {
                // reset display
                c.DisplayMode = DisplayType.Month;
                c.SetMonthMode();
            }

            // Control
            SetCurrentValue(ControlBorderBrushProperty, TryFindResource(CalendarResourceKeys.ControlBorderBrushKey));
            SetCurrentValue(ControlBackgroundProperty, TryFindResource(CalendarResourceKeys.ControlBackgroundKey));

            // Header
            SetCurrentValue(HeaderNormalForegroundProperty, TryFindResource(CalendarResourceKeys.HeaderNormalForegroundKey));
            SetCurrentValue(HeaderFocusedForegroundProperty, TryFindResource(CalendarResourceKeys.HeaderFocusedForegroundKey));
            SetCurrentValue(HeaderPressedForegroundProperty, TryFindResource(CalendarResourceKeys.HeaderPressedForegroundKey));
            SetCurrentValue(HeaderNormalBorderBrushProperty, TryFindResource(CalendarResourceKeys.HeaderNormalBorderBrushKey));
            SetCurrentValue(HeaderFocusedBorderBrushProperty, TryFindResource(CalendarResourceKeys.HeaderFocusedBorderBrushKey));
            SetCurrentValue(HeaderPressedBorderBrushProperty, TryFindResource(CalendarResourceKeys.HeaderPressedBorderBrushKey));
            SetCurrentValue(HeaderNormalBackgroundProperty, TryFindResource(CalendarResourceKeys.HeaderNormalBackgroundKey));
            SetCurrentValue(HeaderFocusedBackgroundProperty, TryFindResource(CalendarResourceKeys.HeaderFocusedBackgroundKey));
            SetCurrentValue(HeaderPressedBackgroundProperty, TryFindResource(CalendarResourceKeys.HeaderPressedBackgroundKey));

            // Navigation Buttons
            SetCurrentValue(ArrowBorderBrushProperty, TryFindResource(CalendarResourceKeys.ArrowBorderBrushKey));
            SetCurrentValue(ArrowNormalFillProperty, TryFindResource(CalendarResourceKeys.ArrowNormalFillKey));
            SetCurrentValue(ArrowFocusedFillProperty, TryFindResource(CalendarResourceKeys.ArrowFocusedFillKey));
            SetCurrentValue(ArrowPressedFillProperty, TryFindResource(CalendarResourceKeys.ArrowPressedFillKey));

            // Day Column
            SetCurrentValue(DayNamesForegroundProperty, TryFindResource(CalendarResourceKeys.DayNamesForegroundKey));
            SetCurrentValue(DayNamesBorderBrushProperty, TryFindResource(CalendarResourceKeys.DayNamesBorderBrushKey));
            SetCurrentValue(DayNamesBackgroundProperty, TryFindResource(CalendarResourceKeys.DayNamesBackgroundKey));

            // Week Column
            SetCurrentValue(WeekColumnForegroundProperty, TryFindResource(CalendarResourceKeys.WeekColumnForegroundKey));
            SetCurrentValue(WeekColumnBorderBrushProperty, TryFindResource(CalendarResourceKeys.WeekColumnBorderBrushKey));
            SetCurrentValue(WeekColumnBackgroundProperty, TryFindResource(CalendarResourceKeys.WeekColumnBackgroundKey));

            // Button Normal
            SetCurrentValue(ButtonNormalForegroundProperty, TryFindResource(CalendarResourceKeys.ButtonNormalForegroundKey));
            SetCurrentValue(ButtonNormalBorderBrushProperty, TryFindResource(CalendarResourceKeys.ButtonNormalBorderBrushKey));
            SetCurrentValue(ButtonNormalBackgroundProperty, TryFindResource(CalendarResourceKeys.ButtonNormalBackgroundKey));
            // Button Focused
            SetCurrentValue(ButtonFocusedForegroundProperty, TryFindResource(CalendarResourceKeys.ButtonFocusedForegroundKey));
            SetCurrentValue(ButtonFocusedBorderBrushProperty, TryFindResource(CalendarResourceKeys.ButtonFocusedBorderBrushKey));
            SetCurrentValue(ButtonFocusedBackgroundProperty, TryFindResource(CalendarResourceKeys.ButtonFocusedBackgroundKey));
            // Button Selected
            SetCurrentValue(ButtonSelectedForegroundProperty, TryFindResource(CalendarResourceKeys.ButtonSelectedForegroundKey));
            SetCurrentValue(ButtonSelectedBorderBrushProperty, TryFindResource(CalendarResourceKeys.ButtonSelectedBorderBrushKey));
            SetCurrentValue(ButtonSelectedBackgroundProperty, TryFindResource(CalendarResourceKeys.ButtonSelectedBackgroundKey));
            // Button Defaulted
            SetCurrentValue(ButtonDefaultedForegroundProperty, TryFindResource(CalendarResourceKeys.ButtonDefaultedForegroundKey));
            SetCurrentValue(ButtonDefaultedBorderBrushProperty, TryFindResource(CalendarResourceKeys.ButtonDefaultedBorderBrushKey));
            SetCurrentValue(ButtonDefaultedBackgroundProperty, TryFindResource(CalendarResourceKeys.ButtonDefaultedBackgroundKey));
            // Button Pressed
            SetCurrentValue(ButtonPressedForegroundProperty, TryFindResource(CalendarResourceKeys.ButtonPressedForegroundKey));
            SetCurrentValue(ButtonPressedBorderBrushProperty, TryFindResource(CalendarResourceKeys.ButtonPressedBorderBrushKey));
            SetCurrentValue(ButtonPressedBackgroundProperty, TryFindResource(CalendarResourceKeys.ButtonPressedBackgroundKey));
            // Button Disabled
            SetCurrentValue(ButtonTransparentProperty, TryFindResource(CalendarResourceKeys.ButtonTransparentKey));
            SetCurrentValue(ButtonDisabledForegroundProperty, TryFindResource(CalendarResourceKeys.ButtonDisabledForegroundKey));
            SetCurrentValue(ButtonDisabledBorderBrushProperty, TryFindResource(CalendarResourceKeys.ButtonDisabledBorderBrushKey));
            SetCurrentValue(ButtonDisabledBackgroundProperty, TryFindResource(CalendarResourceKeys.ButtonDisabledBackgroundKey));

            // Footer
            SetCurrentValue(FooterForegroundProperty, TryFindResource(CalendarResourceKeys.FooterForegroundKey));
            SetCurrentValue(FooterBorderBrushProperty, TryFindResource(CalendarResourceKeys.FooterBorderBrushKey));
            SetCurrentValue(FooterBackgroundProperty, TryFindResource(CalendarResourceKeys.FooterBackgroundKey));
        }
        #endregion

        #region Constructors
        public Calendar()
        {
            SelectedDates = new ObservableCollection<DateTime>();
            _blackoutDates = new BlackoutDatesCollection(this);
        }

        static Calendar()
        {
            // override style
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Applies template and initial bindings
        /// </summary>
        public override void OnApplyTemplate()
        {
            // apply the template
            base.OnApplyTemplate();
            // template has been loaded
            HasInitialized = true;
            // set minimum size constraints
            MinWidth = 150;
            MinHeight = 150;

            // adds events and bindings
            SetBindings();
            // initialize buttons for display views
            InitializeMonth();
            InitializeYear();
            InitializeDecade();

            // set the default display view
            UpdateCalendar();

            // add bindings
            Grid grdFooterContainer = (Grid)FindElement("Part_FooterContainer");
            // property to element bindings
            if (grdFooterContainer != null)
            {
                // bind the footer grid visibility to the property
                Binding bindIsFooterVisible = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("FooterVisibility"),
                    Mode = BindingMode.TwoWay
                };
                grdFooterContainer.SetBinding(VisibilityProperty, bindIsFooterVisible);
            }

            UniformGrid grdWeek = (UniformGrid)FindElement("Part_WeekGrid");
            if (grdWeek != null)
            {
                // bind the week column grid visibility to the property
                Binding bindIsWeekVisible = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("WeekColumnVisibility"),
                    Mode = BindingMode.TwoWay
                };
                grdWeek.SetBinding(VisibilityProperty, bindIsWeekVisible);
            }

            Border brdHeaderBorder = (Border)FindElement("Part_HeaderBorder");
            if (brdHeaderBorder != null)
            {
                // bind the footer grid visibility to the property
                Binding bindHeaderHeight = new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
                    Path = new PropertyPath("HeaderHeight"),
                    Mode = BindingMode.TwoWay
                };
                brdHeaderBorder.SetBinding(HeightProperty, bindHeaderHeight);
            }

            // store parent window
            if (!IsDesignTime)
                FindParentWindow();
        }

        /// <summary>
        /// Sets the Day Name and Display name strings based on overall size of control
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            UniformGrid grdDay = (UniformGrid)FindElement("Part_DayGrid");
            if (grdDay != null)
            {
                for (int i = 0; i < 7; i++)
                {
                    Label lbl = (Label)grdDay.Children[i];

                    if (availableSize.Width < 180)
                        lbl.Content = lbl.Tag.ToString().Substring(0, 2);
                    else if (availableSize.Width < 430)
                        lbl.Content = lbl.Tag.ToString().Substring(0, 3);
                    else
                        lbl.Content = lbl.Tag.ToString();
                }
            }
            return availableSize;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        private double GetDayStringLength(Size availableSize)
        {
            UniformGrid grdDay = (UniformGrid)FindElement("Part_DayGrid");

            if (grdDay != null)
            {
                Label lblMeasure = (Label)grdDay.Children[3];
                Size szCompare = new Size(availableSize.Width / 7, availableSize.Height);

                lblMeasure.Content = lblMeasure.Tag.ToString();
                lblMeasure.Measure(availableSize);

                if (lblMeasure.DesiredSize.Width < szCompare.Width)
                    return -1;

                lblMeasure.Content = lblMeasure.Tag.ToString().Substring(0, 3);
                lblMeasure.Measure(availableSize);
                double width = lblMeasure.DesiredSize.Width;

                return width < szCompare.Width ? 3 : 2;
            }
            return 0;
        }
        #endregion

        #region Properties
        #region AllowDrag
        /// <summary>
        /// Gets/Sets dragging capability
        /// </summary>
        public static readonly DependencyProperty AllowDragProperty = DependencyProperty.Register(
            "AllowDrag", typeof(bool), typeof(Calendar), new UIPropertyMetadata(false, new PropertyChangedCallback(OnAllowDragChanged)));

        public bool AllowDrag
        {
            get { return (bool)GetValue(AllowDragProperty); }
            set { SetValue(AllowDragProperty, value); }
        }

        private static void OnAllowDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(AllowDragPropName));
        }
        #endregion

        #region AdornDrag
        /// <summary>
        /// Gets/Sets drag usese adorned window
        /// </summary>
        public static readonly DependencyProperty AdornDragProperty = DependencyProperty.Register(
            "AdornDrag", typeof(bool), typeof(Calendar), 
            new UIPropertyMetadata(
                false, 
                new PropertyChangedCallback(OnAdornChanged),
                new CoerceValueCallback(CoerceAdornDrag)));

        public bool AdornDrag
        {
            get { return (bool)GetValue(AdornDragProperty); }
            set { SetValue(AdornDragProperty, value); }
        }

        private static object CoerceAdornDrag(DependencyObject d, object o)
        {
            Calendar vc = d as Calendar;

            if (vc.ParentWindow == null && !vc.IsDesignTime && vc.HasInitialized)
            {
                bool value = false;
                return value;
            }
            return o;
        }

        private static void OnAdornChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(AdornDragPropName));
        }
        #endregion

        #region BlackoutDates
        /// <summary>
        /// 
        /// </summary>
        public BlackoutDatesCollection BlackoutDates
        {
            get { return _blackoutDates; }
        }
        #endregion BlackoutDates

        #region DisplayDate
        /// <summary>
        /// Gets/Sets the date that is being displayed in the calendar
        /// </summary>
        public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register(
            "DisplayDate", typeof(DateTime), typeof(Calendar), 
            new UIPropertyMetadata(
                DateTime.Today,
                new PropertyChangedCallback(OnModeChanged),
                new CoerceValueCallback(CoerceDateToBeInRange)));

        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        private static object CoerceDateToBeInRange(DependencyObject d, object o)
        {
            Calendar vc = d as Calendar;
            DateTime value = (DateTime)o;

            if (value < vc.DisplayDateStart)
                return vc.DisplayDateStart;

            if (value > vc.DisplayDateEnd)
                return vc.DisplayDateEnd;

            return o;
        }
        #endregion

        #region DisplayMode
        /// <summary>
        /// Gets/Sets the calender display mode
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register(
            "DisplayMode", typeof(DisplayType), typeof(Calendar), new FrameworkPropertyMetadata(DisplayType.Month, new PropertyChangedCallback(OnDisplayModeChanged)));

        public DisplayType DisplayMode
        {
            get { return (DisplayType)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        private static void OnDisplayModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            OnModeChanged(d, new DependencyPropertyChangedEventArgs());

            //raise the DisplayModeChanged event
            DisplayModeChangedEventArgs args =
                new DisplayModeChangedEventArgs(DisplayModeChangedEvent)
                {
                    NewMode = (DisplayType)e.NewValue,
                    OldMode = (DisplayType)e.OldValue
                };
            vc.RaiseEvent(args);

            //raise the PropertyChanged event
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(DisplayModePropName));
        }

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            vc.SetCalendar();
        }

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public static readonly RoutedEvent DisplayModeChangedEvent = EventManager.RegisterRoutedEvent("DisplayModeChanged",
            RoutingStrategy.Bubble, typeof(DisplayModeChangedEventHandler), typeof(Calendar));

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public event DisplayModeChangedEventHandler DisplayModeChanged
        {
            add { AddHandler(DisplayModeChangedEvent, value); }
            remove { RemoveHandler(DisplayModeChangedEvent, value); }
        }
        #endregion

        #region DisplayDateStart
        /// <summary>
        /// Gets/Sets the minimum date that is displayed and selected
        /// </summary>
        public static readonly DependencyProperty DisplayDateStartProperty = DependencyProperty.Register(
            "DisplayDateStart", typeof(DateTime), typeof(Calendar), 
            new UIPropertyMetadata(
                new DateTime(1, 1, 1),
                new PropertyChangedCallback(OnDisplayDateStartChanged),
                new CoerceValueCallback(CoerceDisplayDateStart)));

        public DateTime DisplayDateStart
        {
            get { return (DateTime)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }

        private static void OnDisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;

            vc.CoerceValue(DisplayDateEndProperty);
            vc.CoerceValue(SelectedDateProperty);
            vc.CoerceValue(DisplayDateProperty);

            vc.RaisePropertyChanged(new PropertyChangedEventArgs(DisplayDateStartPropName));
            OnModeChanged(d, new DependencyPropertyChangedEventArgs());
        }

        private static object CoerceDisplayDateStart(DependencyObject d, object o)
        {
            //Calendar vc = d as Calendar;
            //DateTime value = (DateTime)o;
            return o;
        }
        #endregion

        #region DisplayDateEnd
        /// <summary>
        /// Gets/Sets the maximum date that is displayed and selected
        /// </summary>
        public static readonly DependencyProperty DisplayDateEndProperty = DependencyProperty.Register(
            "DisplayDateEnd", typeof(DateTime), typeof(Calendar),
            new UIPropertyMetadata(
                new DateTime(2199, 1, 1),
                new PropertyChangedCallback(OnDisplayDateEndChanged),
                new CoerceValueCallback(CoerceDisplayDateEnd)));

        public DateTime DisplayDateEnd
        {
            get { return (DateTime)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }

        private static void OnDisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;

            vc.CoerceValue(SelectedDateProperty);
            vc.CoerceValue(DisplayDateProperty);

            OnModeChanged(d, new DependencyPropertyChangedEventArgs());
        }

        private static object CoerceDisplayDateEnd(DependencyObject d, object o)
        {
            Calendar vc = d as Calendar;
            DateTime value = (DateTime)o;

            if (value < vc.DisplayDateStart)
                return vc.DisplayDateStart;

            return o;
        }
        #endregion

        #region FooterVisibility
        /// <summary>
        /// Gets/Sets the footer visibility
        /// </summary>
        public static readonly DependencyProperty FooterVisibilityProperty = DependencyProperty.Register(
            "FooterVisibility", typeof(Visibility), typeof(Calendar), new FrameworkPropertyMetadata(
                Visibility.Visible,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnFooterVisibilityChanged)));

        public Visibility FooterVisibility
        {
            get { return (Visibility)GetValue(FooterVisibilityProperty); }
            set { SetValue(FooterVisibilityProperty, value); }
        }

        private static void OnFooterVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            Grid grdFooterContainer = (Grid)vc.FindElement("Part_FooterContainer");

            if (grdFooterContainer != null)
            {
                if ((Visibility)e.NewValue == Visibility.Visible)
                {
                    grdFooterContainer.Visibility = Visibility.Visible;
                    vc.UpdateCalendar();
                }
                else
                {
                    grdFooterContainer.Visibility = Visibility.Collapsed;
                    vc.UpdateCalendar();
                }

                vc.RaisePropertyChanged(new PropertyChangedEventArgs(FooterVisibilityPropName));
            }
        }
        #endregion

        #region HeaderFontSize
        /// <summary>
        /// Gets/Sets the title button font size
        /// </summary>
        public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
            "HeaderFontSize", typeof(double), typeof(Calendar), new FrameworkPropertyMetadata(
                (double)12,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnHeaderFontSizeChanged),
                new CoerceValueCallback(CoerceHeaderFontSize)));

        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        private static object CoerceHeaderFontSize(DependencyObject d, object o)
        {
            //Calendar vc = d as Calendar;
            double value = (double)o;

            if (value > 16)
                return 16;

            if (value < 7)
                return 7;

            return o;
        }

        private static void OnHeaderFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            DateButton btnTitle = (DateButton)vc.FindElement("Part_TitleButton");

            if (btnTitle != null)
            {
                btnTitle.FontSize = (double)e.NewValue;
                vc.RaisePropertyChanged(new PropertyChangedEventArgs(HeaderFontSizePropName));
            }
        }
        #endregion

        #region HeaderFontWeight
        /// <summary>
        /// Gets/Sets the title button font weight
        /// </summary>
        public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register(
            "HeaderFontWeight", typeof(FontWeight), typeof(Calendar), new FrameworkPropertyMetadata(
                FontWeights.Bold, 
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnHeaderFontWeightChanged)));

        public FontWeight HeaderFontWeight
        {
            get { return (FontWeight)GetValue(HeaderFontWeightProperty); }
            set { SetValue(HeaderFontWeightProperty, value); }
        }

        private static void OnHeaderFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            DateButton btnTitle = (DateButton)vc.FindElement("Part_TitleButton");

            if (btnTitle != null)
            {
                btnTitle.FontWeight = (FontWeight)e.NewValue;
                vc.RaisePropertyChanged(new PropertyChangedEventArgs(HeaderFontWeightPropName));
            }
        }
        #endregion

        #region HeaderHeight
        /// <summary>
        /// Gets/Sets the header height
        /// </summary>
        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register(
            "HeaderHeight", typeof(double), typeof(Calendar), new FrameworkPropertyMetadata(
                (double)20,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnHeaderHeightChanged),
                new CoerceValueCallback(CoerceHeaderHeight)));

        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        private static object CoerceHeaderHeight(DependencyObject d, object o)
        {
            //Calendar vc = d as Calendar;
            double value = (double)o;

            if (value > 30)
                return 30;

            if (value < 18)
                return 18;

            return o;
        }

        private static void OnHeaderHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            Border brdHeaderBorder = (Border)vc.FindElement("Part_HeaderBorder");

            if (brdHeaderBorder != null)
            {
                brdHeaderBorder.Height = (double)e.NewValue;
                vc.RaisePropertyChanged(new PropertyChangedEventArgs(HeaderHeightPropName));
            }
        }
        #endregion

        #region IsAnimated
        /// <summary>
        /// Gets/Sets animations are used
        /// </summary>
        public static readonly DependencyProperty IsAnimatedProperty = DependencyProperty.Register(
            "IsAnimated", typeof(bool), typeof(Calendar), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnIsAnimatedChanged)));

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        private static void OnIsAnimatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(IsAnimatedPropName));
        }
        #endregion

        #region IsDesignTime
        /// <summary>
        /// Tests if control is in design enviroment
        /// </summary>
        public bool IsDesignTime
        {
            get { return DesignerProperties.GetIsInDesignMode(this); }
        }
        #endregion

        #region IsTodayHighlighted
        /// <summary>
        /// Gets/Sets current day is highlighted
        /// </summary>
        public static readonly DependencyProperty IsTodayHighlightedProperty = DependencyProperty.Register(
            "IsTodayHighlighted", typeof(bool), typeof(Calendar), new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnTodayHighlightedChanged)));

        public bool IsTodayHighlighted
        {
            get { return (bool)GetValue(IsTodayHighlightedProperty); }
            set { SetValue(IsTodayHighlightedProperty, value); }
        }

        private static void OnTodayHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;

            if (vc.DisplayMode == DisplayType.Month)
            {
                vc.SetMonthMode();
                vc.RaisePropertyChanged(new PropertyChangedEventArgs(TodayHighlightedPropName));
            }
        }
        #endregion

        #region SelectedDate
        /// <summary>
        /// Gets/Sets the currently viewed date
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
            "SelectedDate", typeof(DateTime), typeof(Calendar), new UIPropertyMetadata(DateTime.Now, new PropertyChangedCallback(OnSelectedDateChanged)));

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        static void OnSelectedDateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = (Calendar)obj;
            vc.OnDateChanged(vc.SelectedDate, (DateTime)e.OldValue);
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(CurrentlySelectedDatePropName));
        }

        private void OnDateChanged(DateTime newDate, DateTime oldDate)
        {
            SelectedDateChangedEventArgs args =
                new SelectedDateChangedEventArgs(SelectedDateChangedEvent)
                {
                    NewDate = newDate,
                    OldDate = oldDate
                };
            RaiseEvent(args);
        }

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged",
            RoutingStrategy.Bubble, typeof(SelectedDateChangedEventHandler), typeof(Calendar));

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public event SelectedDateChangedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }
        #endregion

        #region SelectedDates
        /// <summary>
        /// Gets/Sets a collection of selected dates
        /// </summary>
        public static readonly DependencyProperty SelectedDatesProperty = DependencyProperty.Register(
            "CurrentlySelectedDates", typeof(ObservableCollection<DateTime>),
                typeof(Calendar), new UIPropertyMetadata(null,
                    delegate (DependencyObject sender, DependencyPropertyChangedEventArgs e)
                    {
                        Calendar cld = (Calendar)sender;
                        if (e.NewValue is INotifyCollectionChanged collection)
                        {
                            collection.CollectionChanged += delegate 
                            { 
                                cld.RaisePropertyChanged(new PropertyChangedEventArgs(CurrentlySelectedDatePropName)); 
                            };
                        }
                        cld.RaisePropertyChanged(new PropertyChangedEventArgs(CurrentlySelectedDatePropName));
                    },
                    new CoerceValueCallback(CoerceDatesChanged)
            ));

        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set { SetValue(SelectedDatesProperty, value); }
        }

        static void OnSelectedDatesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = (Calendar)obj;
            vc.OnDatesChanged((ObservableCollection<DateTime>)e.NewValue, (ObservableCollection<DateTime>)e.OldValue);
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(CurrentlySelectedDatePropName));
        }

        private static object CoerceDatesChanged(DependencyObject d, object o)
        {
            //Calendar vc = d as Calendar;
            return o;
        }

        private void OnDatesChanged(ObservableCollection<DateTime> newDates, ObservableCollection<DateTime> oldDates)
        {
            SelectedDatesChangedEventArgs args = new SelectedDatesChangedEventArgs(SelectedDatesChangedEvent)
            {
                NewDates = newDates,
                OldDates = oldDates
            };
            RaiseEvent(args);
        }

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public static readonly RoutedEvent SelectedDatesChangedEvent = EventManager.RegisterRoutedEvent("SelectedDatesChanged",
            RoutingStrategy.Bubble, typeof(SelectedDatesChangedEventHandler), typeof(Calendar));

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public event SelectedDatesChangedEventHandler SelectedDatesChanged
        {
            add { AddHandler(SelectedDatesChangedEvent, value); }
            remove { RemoveHandler(SelectedDatesChangedEvent, value); }
        }
        /// <summary>
        /// Gets/Sets a string that represents the selected date
        /// </summary>
        public string CurrentlySelectedDateString
        {
            get
            {
                if (SelectionMode != SelectionType.Single)
                {
                    if (SelectedDates.Count > 1)
                    {
                        return string.Format("{0} - {1}", SelectedDates[0].ToShortDateString(),
                            SelectedDates[SelectedDates.Count - 1].ToShortDateString());
                    }
                    else if (SelectedDates.Count == 1)
                    {
                        return SelectedDates[0].ToLongDateString();
                    }
                }
                return SelectedDate.ToLongDateString();
            }
        }
        #endregion

        #region SelectionMode
        /// <summary>
        /// Gets/Sets the selection mode
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode", typeof(SelectionType), typeof(Calendar), new UIPropertyMetadata(
                SelectionType.Single,
                delegate (DependencyObject sender, DependencyPropertyChangedEventArgs e) { },
                CoerceSelectionMode));

        public SelectionType SelectionMode
        {
            get { return (SelectionType)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        private static object CoerceSelectionMode(DependencyObject d, object o)
        {
            Calendar vc = d as Calendar;
            vc.ClearSelectedDates(false);
            
            return o;
        }
        #endregion

        #region IsRangeSelection

        public static readonly DependencyProperty IsRangeSelectionProperty = DependencyProperty.Register(
            "IsRangeSelection", typeof(bool), typeof(Calendar), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsRangeSelectionChanged)));

        public bool IsRangeSelection
        {
            get { return (bool)GetValue(IsRangeSelectionProperty); }
            set { SetValue(IsRangeSelectionProperty, value); }
        }

        private static void OnIsRangeSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(IsRangeSelectionPropName));
        }

        #endregion

        #region ShowDaysOfAllMonths
        /// <summary>
        /// Gets/Sets days of all months written to grid
        /// </summary>
        public static readonly DependencyProperty ShowDaysOfAllMonthsProperty = DependencyProperty.Register(
            "ShowDaysOfAllMonths", typeof(bool), typeof(Calendar), new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnShowDaysOfAllMonthsChanged)));

        public bool ShowDaysOfAllMonths
        {
            get { return (bool)GetValue(ShowDaysOfAllMonthsProperty); }
            set { SetValue(ShowDaysOfAllMonthsProperty, value); }
        }

        private static void OnShowDaysOfAllMonthsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            Grid grdMonthContainer = (Grid)vc.FindElement("Part_MonthContainer");

            if (grdMonthContainer != null)
            {
                if (grdMonthContainer.Visibility == Visibility.Visible)
                    vc.SetMonthMode();

                vc.RaisePropertyChanged(new PropertyChangedEventArgs(ShowDaysOfAllMonthsPropName));
            }
        }
        #endregion

        #region WeekColumnVisibility
        /// <summary>
        /// Gets/Sets the week column visibility
        /// </summary>
        public static readonly DependencyProperty WeekColumnVisibilityProperty = DependencyProperty.Register(
            "WeekColumnVisibility", typeof(Visibility), typeof(Calendar), 
            new FrameworkPropertyMetadata(
                Visibility.Visible, 
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnWeekColumnVisibilityChanged)));

        public Visibility WeekColumnVisibility
        {
            get { return (Visibility)GetValue(WeekColumnVisibilityProperty); }
            set { SetValue(WeekColumnVisibilityProperty, value); }
        }

        private static void OnWeekColumnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Calendar vc = d as Calendar;
            UniformGrid grdWeek = (UniformGrid)vc.FindElement("Part_WeekGrid");

            if (grdWeek != null)
            {
                if ((Visibility)e.NewValue == Visibility.Visible)
                {
                    grdWeek.Visibility = Visibility.Visible;
                    vc.UpdateCalendar();
                }
                else
                {
                    grdWeek.Visibility = Visibility.Collapsed;
                    vc.UpdateCalendar();
                }
            }
            vc.RaisePropertyChanged(new PropertyChangedEventArgs(WeekColumnVisibilityPropName));
        }
        #endregion
        #endregion

        #region Theme Properties
        #region ControlBorderBrush
        public static readonly DependencyProperty ControlBorderBrushProperty = DependencyProperty.Register(
            "ControlBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ControlBorderBrush
        {
            get => (Brush)GetValue(ControlBorderBrushProperty);
            set => SetValue(ControlBorderBrushProperty, value);
        }
        #endregion

        #region ControlBackground
        public static readonly DependencyProperty ControlBackgroundProperty = DependencyProperty.Register(
            "ControlBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ControlBackground
        {
            get => (Brush)GetValue(ControlBackgroundProperty);
            set => SetValue(ControlBackgroundProperty, value);
        }
        #endregion

        #region Header
        #region HeaderNormalForeground
        public static readonly DependencyProperty HeaderNormalForegroundProperty = DependencyProperty.Register(
            "HeaderNormalForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderNormalForeground
        {
            get => (Brush)GetValue(HeaderNormalForegroundProperty);
            set => SetValue(HeaderNormalForegroundProperty, value);
        }
        #endregion

        #region HeaderFocusedForeground
        public static readonly DependencyProperty HeaderFocusedForegroundProperty = DependencyProperty.Register(
            "HeaderFocusedForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderFocusedForeground
        {
            get => (Brush)GetValue(HeaderFocusedForegroundProperty);
            set => SetValue(HeaderFocusedForegroundProperty, value);
        }
        #endregion

        #region HeaderPressedForeground
        public static readonly DependencyProperty HeaderPressedForegroundProperty = DependencyProperty.Register(
            "HeaderPressedForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderPressedForeground
        {
            get => (Brush)GetValue(HeaderPressedForegroundProperty);
            set => SetValue(HeaderPressedForegroundProperty, value);
        }
        #endregion

        #region HeaderNormalBorderBrush
        public static readonly DependencyProperty HeaderNormalBorderBrushProperty = DependencyProperty.Register(
            "HeaderNormalBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderNormalBorderBrush
        {
            get => (Brush)GetValue(HeaderNormalBorderBrushProperty);
            set => SetValue(HeaderNormalBorderBrushProperty, value);
        }
        #endregion

        #region HeaderFocusedBorderBrush
        public static readonly DependencyProperty HeaderFocusedBorderBrushProperty = DependencyProperty.Register(
            "HeaderFocusedBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderFocusedBorderBrush
        {
            get => (Brush)GetValue(HeaderFocusedBorderBrushProperty);
            set => SetValue(HeaderFocusedBorderBrushProperty, value);
        }
        #endregion

        #region HeaderPressedBorderBrush
        public static readonly DependencyProperty HeaderPressedBorderBrushProperty = DependencyProperty.Register(
            "HeaderPressedBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderPressedBorderBrush
        {
            get => (Brush)GetValue(HeaderPressedBorderBrushProperty);
            set => SetValue(HeaderPressedBorderBrushProperty, value);
        }
        #endregion

        #region HeaderNormalBackground
        public static readonly DependencyProperty HeaderNormalBackgroundProperty = DependencyProperty.Register(
            "HeaderNormalBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderNormalBackground
        {
            get => (Brush)GetValue(HeaderNormalBackgroundProperty);
            set => SetValue(HeaderNormalBackgroundProperty, value);
        }
        #endregion

        #region HeaderFocusedBackground
        public static readonly DependencyProperty HeaderFocusedBackgroundProperty = DependencyProperty.Register(
            "HeaderFocusedBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderFocusedBackground
        {
            get => (Brush)GetValue(HeaderFocusedBackgroundProperty);
            set => SetValue(HeaderFocusedBackgroundProperty, value);
        }
        #endregion

        #region HeaderPressedBackground
        public static readonly DependencyProperty HeaderPressedBackgroundProperty = DependencyProperty.Register(
            "HeaderPressedBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush HeaderPressedBackground
        {
            get => (Brush)GetValue(HeaderPressedBackgroundProperty);
            set => SetValue(HeaderPressedBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Navigation Buttons
        #region ArrowBorderBrush
        public static readonly DependencyProperty ArrowBorderBrushProperty = DependencyProperty.Register(
            "ArrowBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ArrowBorderBrush
        {
            get => (Brush)GetValue(ArrowBorderBrushProperty);
            set => SetValue(ArrowBorderBrushProperty, value);
        }
        #endregion

        #region ArrowNormalFill
        public static readonly DependencyProperty ArrowNormalFillProperty = DependencyProperty.Register(
            "ArrowNormalFill", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ArrowNormalFill
        {
            get => (Brush)GetValue(ArrowNormalFillProperty);
            set => SetValue(ArrowNormalFillProperty, value);
        }
        #endregion

        #region ArrowFocusedFill
        public static readonly DependencyProperty ArrowFocusedFillProperty = DependencyProperty.Register(
            "ArrowFocusedFill", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ArrowFocusedFill
        {
            get => (Brush)GetValue(ArrowFocusedFillProperty);
            set => SetValue(ArrowFocusedFillProperty, value);
        }
        #endregion

        #region ArrowPressedFill
        public static readonly DependencyProperty ArrowPressedFillProperty = DependencyProperty.Register(
            "ArrowPressedFill", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ArrowPressedFill
        {
            get => (Brush)GetValue(ArrowPressedFillProperty);
            set => SetValue(ArrowPressedFillProperty, value);
        }
        #endregion
        #endregion

        #region Day Column
        #region DayNamesForeground
        public static readonly DependencyProperty DayNamesForegroundProperty = DependencyProperty.Register(
            "DayNamesForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush DayNamesForeground
        {
            get => (Brush)GetValue(DayNamesForegroundProperty);
            set => SetValue(DayNamesForegroundProperty, value);
        }
        #endregion

        #region DayNamesBorderBrush
        public static readonly DependencyProperty DayNamesBorderBrushProperty = DependencyProperty.Register(
            "DayNamesBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush DayNamesBorderBrush
        {
            get => (Brush)GetValue(DayNamesBorderBrushProperty);
            set => SetValue(DayNamesBorderBrushProperty, value);
        }
        #endregion

        #region DayNamesBackground
        public static readonly DependencyProperty DayNamesBackgroundProperty = DependencyProperty.Register(
            "DayNamesBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush DayNamesBackground
        {
            get => (Brush)GetValue(DayNamesBackgroundProperty);
            set => SetValue(DayNamesBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Week Column
        #region WeekColumnForeground
        public static readonly DependencyProperty WeekColumnForegroundProperty = DependencyProperty.Register(
            "WeekColumnForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush WeekColumnForeground
        {
            get => (Brush)GetValue(WeekColumnForegroundProperty);
            set => SetValue(WeekColumnForegroundProperty, value);
        }
        #endregion

        #region WeekColumnBorderBrush
        public static readonly DependencyProperty WeekColumnBorderBrushProperty = DependencyProperty.Register(
            "WeekColumnBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush WeekColumnBorderBrush
        {
            get => (Brush)GetValue(WeekColumnBorderBrushProperty);
            set => SetValue(WeekColumnBorderBrushProperty, value);
        }
        #endregion

        #region WeekColumnBackground
        public static readonly DependencyProperty WeekColumnBackgroundProperty = DependencyProperty.Register(
            "WeekColumnBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush WeekColumnBackground
        {
            get => (Brush)GetValue(WeekColumnBackgroundProperty);
            set => SetValue(WeekColumnBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Button
        #region Normal
        #region ButtonNormalForeground
        public static readonly DependencyProperty ButtonNormalForegroundProperty = DependencyProperty.Register(
            "ButtonNormalForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonNormalForeground
        {
            get => (Brush)GetValue(ButtonNormalForegroundProperty);
            set => SetValue(ButtonNormalForegroundProperty, value);
        }
        #endregion

        #region ButtonNormalBorderBrush
        public static readonly DependencyProperty ButtonNormalBorderBrushProperty = DependencyProperty.Register(
            "ButtonNormalBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonNormalBorderBrush
        {
            get => (Brush)GetValue(ButtonNormalBorderBrushProperty);
            set => SetValue(ButtonNormalBorderBrushProperty, value);
        }
        #endregion

        #region ButtonNormalBackground
        public static readonly DependencyProperty ButtonNormalBackgroundProperty = DependencyProperty.Register(
            "ButtonNormalBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonNormalBackground
        {
            get => (Brush)GetValue(ButtonNormalBackgroundProperty);
            set => SetValue(ButtonNormalBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Focused
        #region ButtonFocusedForeground
        public static readonly DependencyProperty ButtonFocusedForegroundProperty = DependencyProperty.Register(
            "ButtonFocusedForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonFocusedForeground
        {
            get => (Brush)GetValue(ButtonFocusedForegroundProperty);
            set => SetValue(ButtonFocusedForegroundProperty, value);
        }
        #endregion

        #region ButtonFocusedBorderBrush
        public static readonly DependencyProperty ButtonFocusedBorderBrushProperty = DependencyProperty.Register(
            "ButtonFocusedBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonFocusedBorderBrush
        {
            get => (Brush)GetValue(ButtonFocusedBorderBrushProperty);
            set => SetValue(ButtonFocusedBorderBrushProperty, value);
        }
        #endregion

        #region ButtonFocusedBackground
        public static readonly DependencyProperty ButtonFocusedBackgroundProperty = DependencyProperty.Register(
            "ButtonFocusedBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonFocusedBackground
        {
            get => (Brush)GetValue(ButtonFocusedBackgroundProperty);
            set => SetValue(ButtonFocusedBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Selected
        #region ButtonSelectedForeground
        public static readonly DependencyProperty ButtonSelectedForegroundProperty = DependencyProperty.Register(
            "ButtonSelectedForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonSelectedForeground
        {
            get => (Brush)GetValue(ButtonSelectedForegroundProperty);
            set => SetValue(ButtonSelectedForegroundProperty, value);
        }
        #endregion

        #region ButtonSelectedBorderBrush
        public static readonly DependencyProperty ButtonSelectedBorderBrushProperty = DependencyProperty.Register(
            "ButtonSelectedBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonSelectedBorderBrush
        {
            get => (Brush)GetValue(ButtonSelectedBorderBrushProperty);
            set => SetValue(ButtonSelectedBorderBrushProperty, value);
        }
        #endregion

        #region ButtonSelectedBackground
        public static readonly DependencyProperty ButtonSelectedBackgroundProperty = DependencyProperty.Register(
            "ButtonSelectedBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonSelectedBackground
        {
            get => (Brush)GetValue(ButtonSelectedBackgroundProperty);
            set => SetValue(ButtonSelectedBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Defaulted
        #region ButtonDefaultedForeground
        public static readonly DependencyProperty ButtonDefaultedForegroundProperty = DependencyProperty.Register(
            "ButtonDefaultedForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonDefaultedForeground
        {
            get => (Brush)GetValue(ButtonDefaultedForegroundProperty);
            set => SetValue(ButtonDefaultedForegroundProperty, value);
        }
        #endregion

        #region ButtonDefaultedBorderBrush
        public static readonly DependencyProperty ButtonDefaultedBorderBrushProperty = DependencyProperty.Register(
            "ButtonDefaultedBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonDefaultedBorderBrush
        {
            get => (Brush)GetValue(ButtonDefaultedBorderBrushProperty);
            set => SetValue(ButtonDefaultedBorderBrushProperty, value);
        }
        #endregion

        #region ButtonDefaultedBackground
        public static readonly DependencyProperty ButtonDefaultedBackgroundProperty = DependencyProperty.Register(
            "ButtonDefaultedBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonDefaultedBackground
        {
            get => (Brush)GetValue(ButtonDefaultedBackgroundProperty);
            set => SetValue(ButtonDefaultedBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Pressed
        #region ButtonPressedForeground
        public static readonly DependencyProperty ButtonPressedForegroundProperty = DependencyProperty.Register(
            "ButtonPressedForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonPressedForeground
        {
            get => (Brush)GetValue(ButtonPressedForegroundProperty);
            set => SetValue(ButtonPressedForegroundProperty, value);
        }
        #endregion

        #region ButtonPressedBorderBrush
        public static readonly DependencyProperty ButtonPressedBorderBrushProperty = DependencyProperty.Register(
            "ButtonPressedBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonPressedBorderBrush
        {
            get => (Brush)GetValue(ButtonPressedBorderBrushProperty);
            set => SetValue(ButtonPressedBorderBrushProperty, value);
        }
        #endregion

        #region ButtonPressedBackground
        public static readonly DependencyProperty ButtonPressedBackgroundProperty = DependencyProperty.Register(
            "ButtonPressedBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonPressedBackground
        {
            get => (Brush)GetValue(ButtonPressedBackgroundProperty);
            set => SetValue(ButtonPressedBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Disabled
        #region ButtonTransparent
        public static readonly DependencyProperty ButtonTransparentProperty = DependencyProperty.Register(
            "ButtonTransparent", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonTransparent
        {
            get => (Brush)GetValue(ButtonTransparentProperty);
            set => SetValue(ButtonTransparentProperty, value);
        }
        #endregion

        #region ButtonDisabledForeground
        public static readonly DependencyProperty ButtonDisabledForegroundProperty = DependencyProperty.Register(
            "ButtonDisabledForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonDisabledForeground
        {
            get => (Brush)GetValue(ButtonDisabledForegroundProperty);
            set => SetValue(ButtonDisabledForegroundProperty, value);
        }
        #endregion

        #region ButtonDisabledBorderBrush
        public static readonly DependencyProperty ButtonDisabledBorderBrushProperty = DependencyProperty.Register(
            "ButtonDisabledBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonDisabledBorderBrush
        {
            get => (Brush)GetValue(ButtonDisabledBorderBrushProperty);
            set => SetValue(ButtonDisabledBorderBrushProperty, value);
        }
        #endregion

        #region ButtonDisabledBackground
        public static readonly DependencyProperty ButtonDisabledBackgroundProperty = DependencyProperty.Register(
            "ButtonDisabledBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ButtonDisabledBackground
        {
            get => (Brush)GetValue(ButtonDisabledBackgroundProperty);
            set => SetValue(ButtonDisabledBackgroundProperty, value);
        }
        #endregion
        #endregion
        #endregion

        #region Footer
        #region FooterForeground
        public static readonly DependencyProperty FooterForegroundProperty = DependencyProperty.Register(
            "FooterForeground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush FooterForeground
        {
            get => (Brush)GetValue(FooterForegroundProperty);
            set => SetValue(FooterForegroundProperty, value);
        }
        #endregion

        #region FooterBorderBrush
        public static readonly DependencyProperty FooterBorderBrushProperty = DependencyProperty.Register(
            "FooterBorderBrush", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush FooterBorderBrush
        {
            get => (Brush)GetValue(FooterBorderBrushProperty);
            set => SetValue(FooterBorderBrushProperty, value);
        }
        #endregion

        #region FooterBackground
        public static readonly DependencyProperty FooterBackgroundProperty = DependencyProperty.Register(
            "FooterBackground", typeof(Brush), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush FooterBackground
        {
            get => (Brush)GetValue(FooterBackgroundProperty);
            set => SetValue(FooterBackgroundProperty, value);
        }
        #endregion
        #endregion
        #endregion

        #region Control Methods
        /// <summary>
        /// Add handlers for adorned drag
        /// </summary>
        private void AddDragEventHandlers()
        {
            ParentWindow.PreviewDragOver += new DragEventHandler(ParentWindow_PreviewDragOver);
            ParentWindow.PreviewGiveFeedback += new GiveFeedbackEventHandler(ParentWindow_PreviewGiveFeedback);
            ParentWindow.PreviewQueryContinueDrag += new QueryContinueDragEventHandler(ParentWindow_PreviewQueryContinueDrag);
        }

        /// <summary>
        /// Capture an elements bitmap
        /// </summary>
        /// <param name="target">element</param>
        /// <param name="dpiX">screen dpi X</param>
        /// <param name="dpiY">screen dpi Y</param>
        /// <returns>bitmap</returns>
        private static BitmapSource CaptureScreen(Visual target, double dpiX, double dpiY)
        {
            if (target == null)
                return null;

            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                                            (int)(bounds.Height * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            return rtb;
        }

        /// <summary>
        /// Clear selected dates
        /// </summary>
        public void ClearSelectedDates(bool persist)
        {
            if (!persist)
                SelectedDates.Clear();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (_btnMonthMode[i, j] != null)
                        _btnMonthMode[i, j].IsSelected = false;
                }
            }
        }

        /// <summary>
        /// Create the adorned drag window
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private DragAdorner CreateDragWindow(DateButton button)
        {
            // get the screen image
            BitmapSource screen = CaptureScreen(button, 96, 96);
            // create an imagebrush
            ImageBrush img = new ImageBrush
            {
                ImageSource = screen
            };
            // create the dragadorner
            Size sz = new Size(button.ActualWidth - 4, button.ActualHeight - 4);
            DragAdorner dragWindow = new DragAdorner(this, sz, img)
            {
                // set opacity.		
                Opacity = 0.8,
                Visibility = Visibility.Visible
            };
            // add the adorner
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            layer.Add(dragWindow);

            return dragWindow;
        }

        /// <summary>
        /// Visual tree helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <returns></returns>
        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T t)
                    return t;

                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        /// <summary>
        /// Find element in the template
        /// </summary>
        private object FindElement(string name)
        {
            try 
            {
                if (HasInitialized)
                    return Template.FindName(name, this);
                else
                    return null;
            }
            catch 
            {
                return null; 
            }
        }

        /// <summary>
        /// Store parent window for adorned drag operation
        /// </summary>
        private void FindParentWindow()
        {
            try
            {
                Window window = FindAnchestor<Window>(this);
                if (window != null)
                    ParentWindow = window;
                else
                    AdornDrag = false;
            }
            catch
            {
                AdornDrag = false;
            }
        }

        /// <summary>
        /// Get the week number from DateTime
        /// </summary>
        public int GetWeekNumber(DateTime date)
        {
            System.DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= System.DayOfWeek.Monday && day <= System.DayOfWeek.Wednesday)
                date = date.AddDays(3);

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday);
        }

        /// <summary>
        /// Initialize button elements and add to grid
        /// </summary>
        private void InitializeDecade()
        {
            UniformGrid grdDecade = (UniformGrid)FindElement("Part_DecadeGrid");

            if (grdDecade != null)
            {
                for (int j = 0; j < 10; j++)
                {
                    var element = NewDayControl();
                    element.Click += new RoutedEventHandler(DecadeModeButton_Click);
                    element.Tag = j - 1;
                    _btnDecadeMode[j] = element;
                    _btnDecadeMode[j].FontSize = FontSize + 1;
                    _btnDecadeMode[j].Margin = new Thickness(12, 6, 12, 6);
                    grdDecade.Children.Add(element);
                }
            }
        }

        /// <summary>
        /// Initialize button elements and add to grid
        /// </summary>
        private void InitializeMonth()
        {
            UniformGrid grdMonth = (UniformGrid)FindElement("Part_MonthGrid");
            UniformGrid grdDay = (UniformGrid)FindElement("Part_DayGrid");
            UniformGrid grdWeek = (UniformGrid)FindElement("Part_WeekGrid");

            if (grdMonth != null && grdDay != null && grdWeek != null)
            {
                // initialize day buttons
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        var element = NewDayControl();
                        element.Content = string.Format("{0},{1}", i, j);
                        element.Click += new RoutedEventHandler(MonthModeButton_Click);
                        element.PreviewMouseDown += new MouseButtonEventHandler(Element_PreviewMouseDown);
                        element.PreviewMouseMove += new MouseEventHandler(Element_PreviewMouseMove);
                        grdMonth.Children.Add(element);
                        _btnMonthMode[i, j] = element;
                    }
                }

                // initialize days
                string[] dayOfWeeks = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
                for (int j = 0; j < 7; j++)
                {
                    var element = new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Top,
                        Padding = new Thickness(0),
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        FontStyle = FontStyle,
                        FontWeight = FontWeights.Medium,
                        Tag = dayOfWeeks[j],
                        Style = (Style)FindResource("DayNameStyle")
                    };
                    grdDay.Children.Add(element);
                }

                // initialize week numbers
                for (int i = 0; i < 6; i++)
                {
                    var element = new Label
                    {
                        Background = Brushes.Transparent,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        FlowDirection = FlowDirection.LeftToRight,
                        Padding = new Thickness(0),
                        Content = "",
                        MinWidth = 20,
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        FontStyle = FontStyle,
                        FontWeight = FontWeight,
                        Style = (Style)FindResource("WeekNumberStyle")
                    };
                    grdWeek.Children.Add(element);
                }
            }
        }

        /// <summary>
        /// Initialize button elements and add to grid
        /// </summary>
        private void InitializeYear()
        {
            UniformGrid grdYear = (UniformGrid)FindElement("Part_YearGrid");

            if (grdYear != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var element = NewDayControl();
                        element.Content = ((MonthList)j + i * 3 + 1).ToString();
                        element.Click += new RoutedEventHandler(YearModeButton_Click);
                        element.Tag = j + i * 3 + 1;
                        FontSize = 11;
                        _btnYearMode[i, j] = element;
                        _btnYearMode[i, j].FontSize = FontSize + 1;
                        _btnYearMode[i, j].Margin = new Thickness(8, 4, 8, 4);
                        grdYear.Children.Add(element);
                    }
                }
            }
        }

        /// <summary>
        /// Test for default date
        /// </summary>
        private void IsTodaysDate()
        {
            if (DisplayMode == DisplayType.Month)
            {
                MonthModeDateToRowColumn(DateTime.Today, out int r, out int c);
                if (IsTodayHighlighted)
                    _btnMonthMode[r, c].IsTodaysDate = true;
                else
                    _btnMonthMode[r, c].IsTodaysDate = false;
            }
        }

        /// <summary>
        /// List all days in grid
        /// </summary>
        /// <param name="month">month</param>
        /// <param name="year">year</param>
        private void ListDaysOfAllMonths(int month, int year)
        {
            DateTime firstDay = new DateTime(year, month, 1);
            int fstCol = firstDay.DayOfWeek != System.DayOfWeek.Sunday ? (int)firstDay.DayOfWeek - 1 : 6;
            int newMonth = month;
            int newYear = year;

            // adjust for year
            if (month == 1)
            {
                newYear--;
                newMonth = 12;
            }
            else
            {
                newMonth--;
            }
            int days = DateTime.DaysInMonth(newYear, newMonth);

            // previous days
            for (int d = fstCol - 1; d >= 0; d--)
            {
                DateTime date = new DateTime(newYear, newMonth, days);
                _btnMonthMode[0, d].Content = days.ToString();
                _btnMonthMode[0, d].Tag = date;
                _btnMonthMode[0, d].IsEnabled = false;
                days--;
            }

            // future days
            newMonth = month;
            if (month == 12)
            {
                year++;
                newMonth = 1;
            }
            else
            {
                newMonth++;
            }

            days = DateTime.DaysInMonth(year, month);
            int day = 1;
            for (int d = fstCol + days + 1; d <= 42; d++)
            {
                int c = (d - 1) % 7;
                int r = (d - 1) / 7;
                DateTime date = new DateTime(year, newMonth, day);
                _btnMonthMode[r, c].Content = day.ToString();
                _btnMonthMode[r, c].Tag = date;
                _btnMonthMode[r, c].IsEnabled = false;
                day++;
            }
        }

        /// <summary>
        /// List the week numbers
        /// </summary>
        private void ListWeekNumbers()
        {
            UniformGrid grdWeek = (UniformGrid)FindElement("Part_WeekGrid");
            if (grdWeek != null)
            {
                grdWeek.Visibility = Visibility.Visible;
                for (int i = 0; i < 6; i++)
                {
                    Label label = (Label)grdWeek.Children[i];
                    label.Content = "";

                    if (_btnMonthMode[i, 6].Tag != null)
                    {
                        DateTime date = (DateTime)_btnMonthMode[i, 6].Tag;
                        label.Content = GetWeekNumber(date).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Return relative position of date within grid
        /// </summary>
        private void MonthModeDateToRowColumn(DateTime date, out int r, out int c)
        {
            int year = date.Year;
            int month = date.Month;
            DateTime firstDay = new DateTime(year, month, 1);
            int fstCol = firstDay.DayOfWeek != System.DayOfWeek.Sunday ? (int)firstDay.DayOfWeek - 1 : 6;
            int day = date.Day - 1;

            r = (day + fstCol) / 7;
            c = (day + fstCol) % 7;
        }

        /// <summary>
        /// Create a DateButton with default properties
        /// </summary>
        /// <returns>DateButton</returns>
        private DateButton NewDayControl()
        {
            var element = new DateButton
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Thickness(0),
                Style = (Style)FindResource("InsideButtonsStyle"),
                Background = Brushes.Transparent,
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
            };
            return element;
        }

        /// <summary>
        /// Remove handlers for adorned drag
        /// </summary>
        private void RemoveDragEventHandlers()
        {
            ParentWindow.PreviewDragOver -= new DragEventHandler(ParentWindow_PreviewDragOver);
            ParentWindow.PreviewGiveFeedback -= new GiveFeedbackEventHandler(ParentWindow_PreviewGiveFeedback);
            ParentWindow.PreviewQueryContinueDrag -= new QueryContinueDragEventHandler(ParentWindow_PreviewQueryContinueDrag);
        }

        /// <summary>
        /// Reset Drag operation data
        /// </summary>
        private void ResetDragData()
        {
            IsDragging = false;
            _dragData.Data = null;
            _dragData.Parent = null;
            AllowDrop = false;
            if (_dragData.Adorner != null)
            {
                RemoveDragEventHandlers();
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                adornerLayer.Remove( _dragData.Adorner );
                _dragData.Adorner = null;
            }
        }

        /// <summary>
        /// Start the animation sequence
        /// </summary>
        private void RunMonthTransition()
        {
            UniformGrid grdMonth = (UniformGrid)FindElement("Part_MonthGrid");
            Grid scrollGrid = (Grid)FindElement("Part_ScrollGrid");

            if (grdMonth != null && scrollGrid != null)
            {
                // not first run
                if (grdMonth.ActualWidth > 0)
                {
                    IsAnimating = true;
                    int width = (int)grdMonth.ActualWidth;
                    int height = (int)grdMonth.ActualHeight;
                    scrollGrid.Visibility = Visibility.Visible;

                    // alternative method
                    //RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default); 
                    //bitmap.Render(grdMonth);
                    //scrollGrid.Background = new ImageBrush(bitmap);

                    // get bitmap for the current state
                    BitmapSource screen = CaptureScreen(grdMonth, 96, 96);
                    scrollGrid.Background = new ImageBrush(screen);
                    // reset month grid
                    SetMonthMode();

                    // two animations one for image, other for month grid
                    ThicknessAnimation marginAnimation = new ThicknessAnimation
                    {
                        Duration = TimeSpan.FromMilliseconds(1000)
                    };
                    marginAnimation.Completed += new EventHandler(MarginAnimation_Completed);

                    ThicknessAnimation marginAnimation2 = new ThicknessAnimation
                    {
                        Duration = TimeSpan.FromMilliseconds(1000)
                    };

                    // expected direction of flow
                    if (IsMoveForward)
                    {
                        grdMonth.Margin = new Thickness(width, 0, width, 0);
                        marginAnimation.From = new Thickness(0);
                        marginAnimation.To = new Thickness(-width, 0, width, 0);
                        marginAnimation2.From = new Thickness(width, 0, -width, 0);
                        marginAnimation2.To = new Thickness(0);
                    }
                    else
                    {
                        grdMonth.Margin = new Thickness(-width, 0, width, 0);
                        marginAnimation.From = new Thickness(0);
                        marginAnimation.To = new Thickness(width, 0, -width, 0);
                        marginAnimation2.From = new Thickness(-width, 0, width, 0);
                        marginAnimation2.To = new Thickness(0);
                    }
                    // launch animations
                    scrollGrid.BeginAnimation(MarginProperty, marginAnimation);
                    grdMonth.BeginAnimation(MarginProperty, marginAnimation2);
                }
                else
                {
                    // first pass
                    SetMonthMode();
                }
            }
        }

        /// <summary>
        /// Start the animation sequence
        /// </summary>
        private void RunYearTransition()
        {
            Calendar c = this;
            Grid grdAnimationContainer = (Grid)FindElement("Part_AnimationContainer");

            if (grdAnimationContainer != null)
            {
                IsAnimating = true;
                // width animation
                double width = grdAnimationContainer.ActualWidth;
                double height = grdAnimationContainer.ActualHeight;
                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    From = width * .5f,
                    To = width,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200))
                };

                // height animation
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = height * .5f,
                    To = height,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200))
                };

                // add width and height propertiy targets to animation
                Storyboard.SetTargetName(widthAnimation, grdAnimationContainer.Name);
                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));
                Storyboard.SetTargetName(heightAnimation, grdAnimationContainer.Name);
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

                Storyboard stbCalenderTransition = new Storyboard();
                // add to storyboard
                stbCalenderTransition.Children.Add(widthAnimation);
                stbCalenderTransition.Children.Add(heightAnimation);

                // resize grid
                grdAnimationContainer.Width = Width * .1f;
                grdAnimationContainer.Height = Height * .1f;
                stbCalenderTransition.Completed += new EventHandler(StbCalenderTransition_Completed);
                // run animation
                stbCalenderTransition.Begin(grdAnimationContainer);
            }
        }

        /// <summary>
        /// Add bindings and events to elements
        /// </summary>
        private void SetBindings()
        {
            // this.MouseMove += new MouseEventHandler(Calendar_MouseMove);
            DateButton btnTitle = (DateButton)FindElement("Part_TitleButton");
            if (btnTitle != null)
            {
                btnTitle.FontFamily = FontFamily;
                btnTitle.FontSize = HeaderFontSize;
                btnTitle.FontStyle = FontStyle;
                btnTitle.FontWeight = HeaderFontWeight;
                btnTitle.Click += new RoutedEventHandler(TitleButton_Click);
            }

            TextBlock txtCurrentDate = (TextBlock)FindElement("Part_CurrentDateText");
            if (txtCurrentDate != null)
            {
                txtCurrentDate.FontFamily = FontFamily;
                txtCurrentDate.FontSize = FontSize;
                txtCurrentDate.FontStyle = FontStyle;
                txtCurrentDate.FontWeight = FontWeights.DemiBold;
            }

            RepeatButton btnNext = (RepeatButton)FindElement("Part_NextButton");
            if (btnNext != null)
                btnNext.Click += new RoutedEventHandler(NextButton_Click);

            RepeatButton btnPrevious = (RepeatButton)FindElement("Part_PreviousButton");
            if (btnPrevious != null)
                btnPrevious.Click += new RoutedEventHandler(PreviousButton_Click);
        }

        /// <summary>
        /// Switch date display mode
        /// </summary>
        private void SetCalendar()
        {
            ClearSelectedDates(true);
            UpdateCalendar();
        }

        /// <summary>
        /// Sets display to decade mode
        /// </summary>
        private void SetDecadeMode()
        {
            Grid grdMonthContainer = (Grid)FindElement("Part_MonthContainer");
            UniformGrid grdDecade = (UniformGrid)FindElement("Part_DecadeGrid");
            UniformGrid grdYear = (UniformGrid)FindElement("Part_YearGrid");

            if (grdMonthContainer != null && grdDecade != null && grdYear != null)
            {
                grdMonthContainer.Visibility = grdYear.Visibility = Visibility.Collapsed;
                grdDecade.Visibility = Visibility.Visible;

                // run the animation
                if (IsAnimated)
                    RunYearTransition();

                int decade = DisplayDate.Year - DisplayDate.Year % 10;
                for (int i = 0; i < 10; i++)
                {
                    int y = i + decade;
                    if (y >= DisplayDateStart.Year && y <= DisplayDateEnd.Year)
                    {
                        _btnDecadeMode[i].Content = decade + i;
                        _btnDecadeMode[i].Tag = decade + i;
                        _btnDecadeMode[i].IsEnabled = true;

                    }
                    else
                    {
                        _btnDecadeMode[i].Content = "";
                        _btnDecadeMode[i].Tag = null;
                        _btnDecadeMode[i].IsEnabled = false;
                    }
                }
                DateButton btnTitle = (DateButton)FindElement("Part_TitleButton");
                if (btnTitle != null)
                    btnTitle.Content = decade.ToString() + "-" + (decade + 9).ToString();
            }
        }

        /// <summary>
        /// Sets display to month mode
        /// </summary>
        private void SetMonthMode()
        {
            Grid grdMonthContainer = (Grid)FindElement("Part_MonthContainer");
            UniformGrid grdDecade = (UniformGrid)FindElement("Part_DecadeGrid");
            UniformGrid grdYear = (UniformGrid)FindElement("Part_YearGrid");

            if (grdMonthContainer != null && grdDecade != null && grdYear != null)
            {
                grdDecade.Visibility = grdYear.Visibility = Visibility.Collapsed;
                if (grdMonthContainer.Visibility != Visibility.Visible)
                {
                    grdMonthContainer.Visibility = Visibility.Visible;
                    if (IsAnimated)
                        RunYearTransition();
                }

                int year = DisplayDate.Year;
                int month = DisplayDate.Month;
                int days = DateTime.DaysInMonth(year, month);
                DateTime firstDay = new DateTime(year, month, 1);
                int fstCol = firstDay.DayOfWeek != System.DayOfWeek.Sunday ? (int)firstDay.DayOfWeek - 1 : 6;

                // clear buttons
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        _btnMonthMode[i, j].Content = "";
                        _btnMonthMode[i, j].IsEnabled = false;
                        _btnMonthMode[i, j].IsTodaysDate = false;
                        _btnMonthMode[i, j].IsCurrentMonth = false;
                        _btnMonthMode[i, j].IsBlackOut = false;
                    }
                }
                // write day numbers
                for (int d = 1; d <= days; d++)
                {
                    DateTime date = new DateTime(year, month, d);
                    if (date >= DisplayDateStart && date <= DisplayDateEnd)
                    {
                        int column, row;
                        row = (d + fstCol - 1) / 7;
                        column = (d + fstCol - 1) % 7;
                        _btnMonthMode[row, column].Content = d.ToString();
                        _btnMonthMode[row, column].IsEnabled = true;
                        _btnMonthMode[row, column].IsCurrentMonth = true;
                        _btnMonthMode[row, column].Tag = date;
                        // restore selected date(s)
                        if (SelectionMode == SelectionType.Single)
                        {
                            if (date == SelectedDate)
                                _btnMonthMode[row, column].IsSelected = true;
                        }
                        else
                        {
                            if (SelectedDates.Contains(date))
                                _btnMonthMode[row, column].IsSelected = true;
                        }
                        if (_blackoutDates.ContainsAny(new DateRangeHelper(date)))
                        {
                            _btnMonthMode[row, column].IsBlackOut = true;
                            _btnMonthMode[row, column].IsEnabled = false;
                        }
                    }
                }

                // show all days
                if (ShowDaysOfAllMonths)
                    ListDaysOfAllMonths(month, year);

                if (WeekColumnVisibility == Visibility.Visible)
                    ListWeekNumbers();

                //footer
                TextBlock txtCurrentDate = (TextBlock)FindElement("Part_CurrentDateText");
                if (txtCurrentDate != null)
                    txtCurrentDate.Text = "Aujourd'hui: " + DateTime.Today.ToShortDateString();

                // header title
                DateButton btnTitle = (DateButton)FindElement("Part_TitleButton");
                if (btnTitle != null)
                    btnTitle.Content = ((MonthList)month).ToString() + " " + year.ToString();

                // current selected
                if (DisplayDate.Month == DateTime.Today.Month)
                    IsTodaysDate();

            }
        }

        /// <summary>
        /// Sets display to year mode
        /// </summary>
        private void SetYearMode()
        {
            Grid grdMonthContainer = (Grid)FindElement("Part_MonthContainer");
            UniformGrid grdDecade = (UniformGrid)FindElement("Part_DecadeGrid");
            UniformGrid grdYear = (UniformGrid)FindElement("Part_YearGrid");

            if (grdMonthContainer != null && grdDecade != null && grdYear != null)
            {
                grdMonthContainer.Visibility = grdDecade.Visibility = Visibility.Collapsed;
                grdYear.Visibility = Visibility.Visible;

                // run the animation
                if (IsAnimated)
                    RunYearTransition();

                DateButton btnTitle = (DateButton)FindElement("Part_TitleButton");
                if (btnTitle != null)
                    btnTitle.Content = DisplayDate.Year.ToString();

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int month = j + i * 3 + 1;
                        if (new DateTime(DisplayDate.Year, month, DateTime.DaysInMonth(DisplayDate.Year, month)) >= DisplayDateStart &&
                            new DateTime(DisplayDate.Year, month, 1) <= DisplayDateEnd)
                        {
                            _btnYearMode[i, j].Content = ((MonthList)month).ToString();
                            _btnYearMode[i, j].IsEnabled = true;
                        }
                        else
                        {
                            _btnYearMode[i, j].Content = "";
                            _btnYearMode[i, j].IsEnabled = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Drag timer used to ensure timely clsure of secondary window
        /// </summary>
        private void StartDragTimer()
        {
            if (_dispatcherTimer == null)
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
                _dispatcherTimer.Interval = new TimeSpan(1000);
            }
            _dispatcherTimer.Start();
        }

        /// <summary>
        /// Halt drag timer and reset
        /// </summary>
        private void StopDragTimer()
        {
            _dispatcherTimer.Stop();
            ResetDragData();
        }

        /// <summary>
        /// Refresh selected Date
        /// </summary>
        internal void RefreshSelected()
        {
            bool isAnmated = IsAnimated.Clone();
            IsAnimated = false;

            ClearSelectedDates(false);
            UpdateCalendar();

            IsAnimated = isAnmated;
        }

        /// <summary>
        /// Updates the calendar display
        /// </summary>
        internal void UpdateCalendar()
        {
            switch (DisplayMode)
            {
                case DisplayType.Month:
                    if (IsAnimating || IsDesignTime || !IsAnimated)
                        SetMonthMode();
                    else
                        RunMonthTransition();
                    break;
                case DisplayType.Year:
                    SetYearMode();
                    break;
                case DisplayType.Decade:
                    SetDecadeMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("The DisplayMode value is not in acceptable range");
            }
        }
        #endregion
    }
}
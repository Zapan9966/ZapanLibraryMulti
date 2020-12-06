using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ZapanControls.Controls.Calendar.Common;

namespace ZapanControls.Controls
{
    [TemplatePart(Name = ElementDay, Type = typeof(CalendarDay))]
    [TemplatePart(Name = ElementDayHeader, Type = typeof(CalendarDayHeader))]
    [TemplatePart(Name = ElementLedger, Type = typeof(CalendarLedger))]
    [TemplatePart(Name = ElementScrollViewer, Type = typeof(ScrollViewer))]
    public partial class ZapCalendar : Control
    {
        private const string ElementDay = "PART_Day";
        private const string ElementDayHeader = "PART_DayHeader";
        private const string ElementLedger = "PART_Ledger";
        private const string ElementScrollViewer = "PART_ScrollViewer";

        private CalendarLedger _ledger;
        private CalendarDayHeader _dayHeader;
        private ScrollViewer _scrollViewer;
        private CalendarDay _day;

        static ZapCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapCalendar), new FrameworkPropertyMetadata(typeof(ZapCalendar)));

            CommandManager.RegisterClassCommandBinding(typeof(ZapCalendar), new CommandBinding(NextDay, new ExecutedRoutedEventHandler(OnExecutedNextDay), new CanExecuteRoutedEventHandler(OnCanExecuteNextDay)));
            CommandManager.RegisterClassCommandBinding(typeof(ZapCalendar), new CommandBinding(PreviousDay, new ExecutedRoutedEventHandler(OnExecutedPreviousDay), new CanExecuteRoutedEventHandler(OnCanExecutePreviousDay)));
        }

        #region CalendarTimeslotItemStyle

        public static readonly DependencyProperty CalendarTimeslotItemStyleProperty = DependencyProperty.Register(
            "CalendarTimeslotItemStyle", typeof(Style), typeof(ZapCalendar));

        [Category("ZapCalendar")]
        //[Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Style CalendarTimeslotItemStyle
        {
            get { return (Style)GetValue(CalendarTimeslotItemStyleProperty); }
            set { SetValue(CalendarTimeslotItemStyleProperty, value); }
        }

        #endregion

        #region CalendarLedgerItemStyle

        public static readonly DependencyProperty CalendarLedgerItemStyleProperty = DependencyProperty.Register(
            "CalendarLedgerItemStyle", typeof(Style), typeof(ZapCalendar));

        [Category("ZapCalendar")]
        //[Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Style CalendarLedgerItemStyle
        {
            get { return (Style)GetValue(CalendarLedgerItemStyleProperty); }
            set { SetValue(CalendarLedgerItemStyleProperty, value); }
        }

        #endregion

        #region CalendarAppointmentItemStyle

        public static readonly DependencyProperty CalendarAppointmentItemStyleProperty = DependencyProperty.Register(
            "CalendarAppointmentItemStyle", typeof(Style), typeof(ZapCalendar));

        [Category("ZapCalendar")]
        //[Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Style CalendarAppointmentItemStyle
        {
            get { return (Style)GetValue(CalendarAppointmentItemStyleProperty); }
            set { SetValue(CalendarAppointmentItemStyleProperty, value); }
        }

        #endregion

        #region AddAppointment

        public static readonly RoutedEvent AddAppointmentEvent = CalendarTimeSlotItem.AddAppointmentEvent.AddOwner(typeof(CalendarDay));

        public event RoutedEventHandler AddAppointment
        {
            add { AddHandler(AddAppointmentEvent, value); }
            remove { RemoveHandler(AddAppointmentEvent, value); }
        }

        #endregion

        #region Appointments

        public static readonly DependencyProperty AppointmentsProperty = DependencyProperty.Register(
            "Appointments", typeof(IEnumerable<Appointment>), typeof(ZapCalendar),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAppointmentsChanged)));

        public IEnumerable<Appointment> Appointments
        {
            get { return (ObservableCollection<Appointment>)GetValue(AppointmentsProperty); }
            set { SetValue(AppointmentsProperty, value); }
        }

        private static void OnAppointmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZapCalendar)d).OnAppointmentsChanged(e);
        }

        protected virtual void OnAppointmentsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_day != null)
                _day.PopulateDay();

            if (Appointments is INotifyCollectionChanged appointments)
                appointments.CollectionChanged += new NotifyCollectionChangedEventHandler(Appointments_CollectionChanged);

            FilterAppointments(CurrentDate);
        }

        void Appointments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FilterAppointments(CurrentDate);
        }

        #endregion

        #region CurrentDate

        public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register(
            "CurrentDate", typeof(DateTime), typeof(ZapCalendar), new FrameworkPropertyMetadata(DateTime.Now, 
                FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnCurrentDateChanged)));

        [Category("ZapCalendar")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        private static void OnCurrentDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZapCalendar)d).OnCurrentDateChanged(e);
        }

        protected virtual void OnCurrentDateChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_day != null)
                _day.PopulateDay();

            FilterAppointments(CurrentDate);
        }

        #endregion             

        #region PeakTimeSlotBackground

        public static readonly DependencyProperty PeakTimeSlotBackgroundProperty = DependencyProperty.Register(
            "PeakTimeSlotBackground", typeof(Brush), typeof(ZapCalendar), new FrameworkPropertyMetadata(Brushes.White,
                new PropertyChangedCallback(OnPeakTimeSlotBackgroundChanged)));

        [Category("ZapCalendar")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Brush PeakTimeSlotBackground
        {
            get { return (Brush)GetValue(PeakTimeSlotBackgroundProperty); }
            set { SetValue(PeakTimeSlotBackgroundProperty, value); }
        }

        private static void OnPeakTimeSlotBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZapCalendar)d).OnPeakTimeSlotBackgroundChanged(e);
        }

        protected virtual void OnPeakTimeSlotBackgroundChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_day != null)
                _day.PopulateDay();
        }

        #endregion

        #region OffPeakTimeslotBackground

        public static readonly DependencyProperty OffPeakTimeSlotBackgroundProperty = DependencyProperty.Register(
            "OffPeakTimeSlotBackground", typeof(Brush), typeof(ZapCalendar), new FrameworkPropertyMetadata(Brushes.LightCyan,
                new PropertyChangedCallback(OnOffPeakTimeSlotBackgroundChanged)));

        [Category("ZapCalendar")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Brush OffPeakTimeSlotBackground
        {
            get { return (Brush)GetValue(OffPeakTimeSlotBackgroundProperty); }
            set { SetValue(OffPeakTimeSlotBackgroundProperty, value); }
        }

        private static void OnOffPeakTimeSlotBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ZapCalendar)d).OnOffPeakTimeSlotBackgroundChanged(e);
        }

        protected virtual void OnOffPeakTimeSlotBackgroundChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_day != null)
                _day.PopulateDay();
        }

        #endregion

        private void FilterAppointments(DateTime date)
        {
            if (_day != null)
                _day.ItemsSource = Appointments.ByDate(date);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            XmlLanguage language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(language));
            LanguageProperty.OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(language));

            _ledger = GetTemplateChild(ElementLedger) as CalendarLedger;
            if (_ledger != null)
                _ledger.Owner = this;

            _day = GetTemplateChild(ElementDay) as CalendarDay;
            if (_day != null)
                _day.Owner = this;

            _dayHeader = GetTemplateChild(ElementDayHeader) as CalendarDayHeader;
            if (_dayHeader != null)
                _dayHeader.Owner = this;

            _scrollViewer = GetTemplateChild(ElementScrollViewer) as ScrollViewer;
        }

        public void ScrollToHome()
        {
            if (_scrollViewer != null)
                _scrollViewer.ScrollToHome();
        }

        public void ScrollToOffset(double offset)
        {
            if (_scrollViewer != null)
                _scrollViewer.ScrollToHorizontalOffset(offset);
        }

        #region NextDay/PreviousDay

        public static readonly RoutedCommand NextDay = new RoutedCommand("NextDay", typeof(ZapCalendar));
        public static readonly RoutedCommand PreviousDay = new RoutedCommand("PreviousDay", typeof(ZapCalendar));

        private static void OnCanExecuteNextDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((ZapCalendar)sender).OnCanExecuteNextDay(e);
        }

        private static void OnExecutedNextDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((ZapCalendar)sender).OnExecutedNextDay(e);
        }

        protected virtual void OnCanExecuteNextDay(CanExecuteRoutedEventArgs e)
        {
            if (e != null)
            {
                e.CanExecute = true;
                e.Handled = false;
            }
        }

        protected virtual void OnExecutedNextDay(ExecutedRoutedEventArgs e)
        {
            CurrentDate += TimeSpan.FromDays(1);

            if (e != null)
                e.Handled = true;
        }

        private static void OnCanExecutePreviousDay(object sender, CanExecuteRoutedEventArgs e)
        {
            ((ZapCalendar)sender).OnCanExecutePreviousDay(e);
        }

        private static void OnExecutedPreviousDay(object sender, ExecutedRoutedEventArgs e)
        {
            ((ZapCalendar)sender).OnExecutedPreviousDay(e);
        }

        protected virtual void OnCanExecutePreviousDay(CanExecuteRoutedEventArgs e)
        {
            if (e != null)
            {
                e.CanExecute = true;
                e.Handled = false;
            }
        }

        protected virtual void OnExecutedPreviousDay(ExecutedRoutedEventArgs e)
        {
            CurrentDate -= TimeSpan.FromDays(1);

            if (e != null)
                e.Handled = true;
        }

        #endregion
    }
}

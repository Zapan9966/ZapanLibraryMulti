using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZapanControls.Controls
{
    [TemplatePart(Name = CalendarDay.ElementTimeslotItems, Type = typeof(StackPanel))]
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Cannot be static")]
    internal sealed class CalendarDay : ItemsControl
    {
        private const string ElementTimeslotItems = "PART_TimeslotItems";

        StackPanel _dayItems;

        static CalendarDay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarDay), new FrameworkPropertyMetadata(typeof(CalendarDay)));
        }

        public CalendarDay()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dayItems = GetTemplateChild(ElementTimeslotItems) as StackPanel;

            PopulateDay();
        }

        public void PopulateDay()
        {
            if (_dayItems != null)
            {
                _dayItems.Children.Clear();

                DateTime startTime = new DateTime(Owner.CurrentDate.Year, Owner.CurrentDate.Month, Owner.CurrentDate.Day, 0, 0, 0);
                for (int i = 0; i < 48; i++)
                {
                    CalendarTimeSlotItem timeslot = new CalendarTimeSlotItem
                    {
                        StartTime = startTime,
                        EndTime = startTime + TimeSpan.FromMinutes(30)
                    };

                    if (startTime.Hour >= 8 && startTime.Hour <= 17)
                        timeslot.SetBinding(BackgroundProperty, GetOwnerBinding("PeakTimeslotBackground"));
                    else
                        timeslot.SetBinding(BackgroundProperty, GetOwnerBinding("OffPeakTimeslotBackground"));

                    timeslot.SetBinding(StyleProperty, GetOwnerBinding("CalendarTimeslotItemStyle"));
                    _dayItems.Children.Add(timeslot);

                    startTime += TimeSpan.FromMinutes(30);
                }
            }
            if (Owner != null)
            {
                Owner.ScrollToHome();
            }
        }

        #region ItemsControl Container Override

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CalendarAppointmentItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is CalendarAppointmentItem);
        }

        #endregion

        public ZapCalendar Owner { get; set; }

        private Binding GetOwnerBinding(string propertyName)
        {
            return new Binding(propertyName) { Source = Owner };
        }
    }
}

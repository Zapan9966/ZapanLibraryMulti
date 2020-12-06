using System.Windows;
using System.Windows.Controls;

namespace ZapanControls.Controls
{
    [TemplateVisualState(Name = StateNormal, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StateMouseOver, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StateDisabled, GroupName = GroupCommon)]
    internal sealed class CalendarAppointmentItem : ContentControl
    {
        public const string StateNormal = "Normal";
        public const string StateMouseOver = "MouseOver";
        public const string StateDisabled = "Disabled";

        public const string GroupCommon = "CommonStates";

        static CalendarAppointmentItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarAppointmentItem), new FrameworkPropertyMetadata(typeof(CalendarAppointmentItem)));
        }

        #region StartTime/EndTime

        public static readonly DependencyProperty StartTimeProperty = TimeSlotPanel.StartTimeProperty.AddOwner(typeof(CalendarAppointmentItem));

        public bool StartTime
        {
            get { return (bool)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }

        public static readonly DependencyProperty EndTimeProperty = TimeSlotPanel.EndTimeProperty.AddOwner(typeof(CalendarAppointmentItem));

        public bool EndTime
        {
            get { return (bool)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }

        #endregion
    }
}

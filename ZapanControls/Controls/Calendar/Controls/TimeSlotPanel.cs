using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace ZapanControls.Controls
{
    internal sealed class TimeSlotPanel : Panel
    {
        #region StartTime

        /// <summary>
        /// StartTime Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty StartTimeProperty = DependencyProperty.RegisterAttached(
            "StartTime", typeof(DateTime), typeof(TimeSlotPanel), new FrameworkPropertyMetadata(DateTime.Now));

        /// <summary>
        /// Gets the StartTime property.  This dependency property indicates ....
        /// </summary>
        public static DateTime GetStartTime(DependencyObject d) 
            => (DateTime)d.GetValue(StartTimeProperty);

        /// <summary>
        /// Sets the StartTime property.  This dependency property indicates ....
        /// </summary>
        public static void SetStartTime(DependencyObject d, DateTime value) 
            => d.SetValue(StartTimeProperty, value);

        #endregion

        #region EndTime

        /// <summary>
        /// EndTime Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty EndTimeProperty = DependencyProperty.RegisterAttached(
            "EndTime", typeof(DateTime), typeof(TimeSlotPanel), new FrameworkPropertyMetadata(DateTime.Now));

        /// <summary>
        /// Gets the EndTime property.  This dependency property indicates ....
        /// </summary>
        public static DateTime GetEndTime(DependencyObject d) 
            => (DateTime)d.GetValue(EndTimeProperty);

        /// <summary>
        /// Sets the EndTime property.  This dependency property indicates ....
        /// </summary>
        public static void SetEndTime(DependencyObject d, DateTime value) 
            => d.SetValue(EndTimeProperty, value);

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (UIElement element in this.Children)
            {
                element.Measure(size);
            }

            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in Children)
            {
                Nullable<DateTime> startTime = element.GetValue(StartTimeProperty) as DateTime?;
                Nullable<DateTime> endTime = element.GetValue(EndTimeProperty) as DateTime?;

                double start_minutes = (startTime.Value.Hour * 60) + startTime.Value.Minute;
                double end_minutes = (endTime.Value.Hour * 60) + endTime.Value.Minute;
                double start_offset = (finalSize.Height / (24 * 60)) * start_minutes;
                double end_offset = (finalSize.Height / (24 * 60)) * end_minutes;

                double left = 0;
                double top = start_offset;
                double width = finalSize.Width;
                double height = (end_offset - start_offset);

                element.Arrange(new Rect(left, top, width, height));
            }

            return finalSize;
        }
    }
}

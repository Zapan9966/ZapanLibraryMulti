using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ZapanControls.Helpers;

namespace ZapanControls.Behaviours
{
    public static class ScrollAnimationBehavior
    {
        // Source : https://stackoverflow.com/questions/20731402/animated-smooth-scrolling-on-scrollviewer

        private static double intendedLocation = 0;

        #region Private ScrollViewer for ListBox

        private static ScrollViewer _listBoxScroller = new ScrollViewer();

        #endregion

        #region VerticalOffset Property

        private static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached(
            "VerticalOffset", typeof(double), typeof(ScrollAnimationBehavior), new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        public static void SetVerticalOffset(FrameworkElement target, double value) 
            => target?.SetValue(VerticalOffsetProperty, value);

        public static double GetVerticalOffset(FrameworkElement target) 
            => (double)(target?.GetValue(VerticalOffsetProperty) ?? 0);

        #endregion

        #region TimeDuration Property

        private static readonly DependencyProperty TimeDurationProperty = DependencyProperty.RegisterAttached(
            "TimeDuration", typeof(TimeSpan), typeof(ScrollAnimationBehavior), new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 0)));

        public static void SetTimeDuration(FrameworkElement target, TimeSpan value)
            => target?.SetValue(TimeDurationProperty, value);

        public static TimeSpan GetTimeDuration(FrameworkElement target)
            => (TimeSpan)(target?.GetValue(TimeDurationProperty) ?? TimeSpan.Zero);        

        #endregion

        #region PointsToScroll Property

        private static readonly DependencyProperty PointsToScrollProperty = DependencyProperty.RegisterAttached(
            "PointsToScroll", typeof(double), typeof(ScrollAnimationBehavior), new PropertyMetadata(0.0));

        public static void SetPointsToScroll(FrameworkElement target, double value) 
            => target?.SetValue(PointsToScrollProperty, value);

        public static double GetPointsToScroll(FrameworkElement target)
            => (double)(target?.GetValue(PointsToScrollProperty) ?? 0);

        #endregion

        #region OnVerticalOffset Changed

        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        #endregion

        #region IsEnabled Property

        private static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled", typeof(bool), typeof(ScrollAnimationBehavior), new UIPropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(FrameworkElement target, bool value) 
            => target?.SetValue(IsEnabledProperty, value);

        public static bool GetIsEnabled(FrameworkElement target)
            => (bool)(target?.GetValue(IsEnabledProperty) ?? false);

        #endregion

        #region OnIsEnabledChanged Changed

        private static void OnIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ScrollViewer scroller)
            {
                scroller.Loaded += new RoutedEventHandler(ScrollerLoaded);
            }
            else if (sender is ListBox listbox)
            {
                listbox.Loaded += new RoutedEventHandler(ListboxLoaded);
            }
        }

        #endregion

        #region AnimateScroll Helper

        private static void AnimateScroll(ScrollViewer scrollViewer, double ToValue)
        {
            scrollViewer.BeginAnimation(VerticalOffsetProperty, null);
            DoubleAnimation verticalAnimation = new DoubleAnimation
            {
                From = scrollViewer.VerticalOffset,
                To = ToValue,
                Duration = new Duration(GetTimeDuration(scrollViewer))
            };
            scrollViewer.BeginAnimation(VerticalOffsetProperty, verticalAnimation);
        }

        #endregion

        #region NormalizeScrollPos Helper

        private static double NormalizeScrollPos(ScrollViewer scroll, double scrollChange, Orientation o)
        {
            double returnValue = scrollChange;

            if (scrollChange < 0)
            {
                returnValue = 0;
            }

            if (o == Orientation.Vertical && scrollChange > scroll.ScrollableHeight)
            {
                returnValue = scroll.ScrollableHeight;
            }
            else if (o == Orientation.Horizontal && scrollChange > scroll.ScrollableWidth)
            {
                returnValue = scroll.ScrollableWidth;
            }

            return returnValue;
        }

        #endregion

        #region UpdateScrollPosition Helper

        private static void UpdateScrollPosition(object sender)
        {
            if (sender is ListBox listbox)
            {
                double scrollTo = 0;

                for (int i = 0; i < (listbox.SelectedIndex); i++)
                {
                    if (listbox.ItemContainerGenerator.ContainerFromItem(listbox.Items[i]) is ListBoxItem tempItem)
                    {
                        scrollTo += tempItem.ActualHeight;
                    }
                }

                AnimateScroll(_listBoxScroller, scrollTo);
            }
        }

        #endregion

        #region SetEventHandlersForScrollViewer Helper

        private static void SetEventHandlersForScrollViewer(ScrollViewer scroller)
        {
            scroller.PreviewMouseWheel += new MouseWheelEventHandler(ScrollViewerPreviewMouseWheel);
            scroller.PreviewKeyDown += new KeyEventHandler(ScrollViewerPreviewKeyDown);
            scroller.PreviewMouseLeftButtonUp += Scroller_PreviewMouseLeftButtonUp;
        }

        #endregion

        private static void Scroller_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            intendedLocation = ((ScrollViewer)sender).VerticalOffset;
        }

        #region scrollerLoaded Event Handler

        private static void ScrollerLoaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scroller = sender as ScrollViewer;

            SetEventHandlersForScrollViewer(scroller);
        }

        #endregion

        #region listboxLoaded Event Handler

        private static void ListboxLoaded(object sender, RoutedEventArgs e)
        {
            ListBox listbox = sender as ListBox;

            _listBoxScroller = VisualTreeHelpers.FindChild<ScrollViewer>(listbox);
            SetEventHandlersForScrollViewer(_listBoxScroller);

            SetTimeDuration(_listBoxScroller, new TimeSpan(0, 0, 0, 0, 200));
            SetPointsToScroll(_listBoxScroller, 16.0);

            listbox.SelectionChanged += new SelectionChangedEventHandler(ListBoxSelectionChanged);
            listbox.Loaded += new RoutedEventHandler(ListBoxLoaded);
            listbox.LayoutUpdated += new EventHandler(ListBoxLayoutUpdated);
        }

        #endregion

        #region ScrollViewerPreviewMouseWheel Event Handler

        private static void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double mouseWheelChange = (double)e.Delta;
            ScrollViewer scroller = (ScrollViewer)sender;
            double newVOffset = intendedLocation - (mouseWheelChange * 2);

            scroller.ScrollToVerticalOffset(intendedLocation);
            if (newVOffset < 0)
            {
                newVOffset = 0;
            }
            else if (newVOffset > scroller.ScrollableHeight)
            {
                newVOffset = scroller.ScrollableHeight;
            }

            AnimateScroll(scroller, newVOffset);
            intendedLocation = newVOffset;

            e.Handled = true;
        }

        #endregion

        #region ScrollViewerPreviewKeyDown Handler

        private static void ScrollViewerPreviewKeyDown(object sender, KeyEventArgs e)
        {
            ScrollViewer scroller = (ScrollViewer)sender;

            Key keyPressed = e.Key;
            double newVerticalPos = GetVerticalOffset(scroller);
            bool isKeyHandled = false;

            if (keyPressed == Key.Down)
            {
                newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos + GetPointsToScroll(scroller)), Orientation.Vertical);
                intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }
            else if (keyPressed == Key.PageDown)
            {
                newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos + scroller.ViewportHeight), Orientation.Vertical);
                intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }
            else if (keyPressed == Key.Up)
            {
                newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos - GetPointsToScroll(scroller)), Orientation.Vertical);
                intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }
            else if (keyPressed == Key.PageUp)
            {
                newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos - scroller.ViewportHeight), Orientation.Vertical);
                intendedLocation = newVerticalPos;
                isKeyHandled = true;
            }

            if (newVerticalPos != GetVerticalOffset(scroller))
            {
                intendedLocation = newVerticalPos;
                AnimateScroll(scroller, newVerticalPos);
            }

            e.Handled = isKeyHandled;
        }

        #endregion

        #region ListBox Event Handlers

        private static void ListBoxLayoutUpdated(object sender, EventArgs e)
            => UpdateScrollPosition(sender);

        private static void ListBoxLoaded(object sender, RoutedEventArgs e) 
            => UpdateScrollPosition(sender);

        private static void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e) 
            => UpdateScrollPosition(sender);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ZapanControls.Behaviours
{
    /// <summary>
    /// Static class used to attach to wpf control
    /// </summary>
    public static class GridViewColumnResize
    {
        #region Dependency Attached Properties

        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
            "Width", typeof(string), typeof(GridViewColumnResize), new PropertyMetadata(OnSetWidthCallback));

        public static string GetWidth(DependencyObject obj)
        {
            return (string)obj?.GetValue(WidthProperty);
        }

        public static void SetWidth(DependencyObject obj, string value)
        {
            obj?.SetValue(WidthProperty, value);
        }

        private static void OnSetWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is GridViewColumn element)
            {
                GridViewColumnResizeBehavior behavior = GetOrCreateBehavior(element);
                behavior.Width = e.NewValue as string;
            }
            else
            {
                Console.Error.WriteLine($"Error: Expected type GridViewColumn but found {dependencyObject.GetType().Name}");
            }
        }

        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached(
            "Margin", typeof(double), typeof(GridViewColumnResize), 
            new PropertyMetadata(OnSetMarginCallback));

        public static double GetMargin(DependencyObject obj)
        {
            return (double)obj?.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject obj, double value)
        {
            obj?.SetValue(MarginProperty, value);
        }

        private static void OnSetMarginCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is GridViewColumn element)
            {
                GridViewColumnResizeBehavior behavior = GetOrCreateBehavior(element);
                behavior.Margin = double.TryParse(e.NewValue as string, out double margin) ? margin : 2;
            }
            else
            {
                Console.Error.WriteLine($"Error: Expected type GridViewColumn but found {dependencyObject.GetType().Name}");
            }
        }

        public static readonly DependencyProperty GridViewColumnResizeBehaviorProperty = DependencyProperty.RegisterAttached(
            "GridViewColumnResizeBehavior", typeof(GridViewColumnResizeBehavior), typeof(GridViewColumnResize), null);

        private static GridViewColumnResizeBehavior GetOrCreateBehavior(GridViewColumn element)
        {
            if (!(element.GetValue(GridViewColumnResizeBehaviorProperty) is GridViewColumnResizeBehavior behavior))
            {
                behavior = new GridViewColumnResizeBehavior(element);
                element.SetValue(GridViewColumnResizeBehaviorProperty, behavior);
            }
            return behavior;
        }

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(
            "Enabled", typeof(bool), typeof(GridViewColumnResize), new PropertyMetadata(OnSetEnabledCallback));

        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj?.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj?.SetValue(EnabledProperty, value);
        }

        private static void OnSetEnabledCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is ListView element)
            {
                ListViewResizeBehavior behavior = GetOrCreateBehavior(element);
                behavior.Enabled = (bool)e.NewValue;
            }
            else
            {
                Console.Error.WriteLine($"Error: Expected type ListView but found {dependencyObject.GetType().Name}");
            }
        }

        public static readonly DependencyProperty ListViewResizeBehaviorProperty = DependencyProperty.RegisterAttached(
            "ListViewResizeBehaviorProperty", typeof(ListViewResizeBehavior), typeof(GridViewColumnResize), null);

        private static ListViewResizeBehavior GetOrCreateBehavior(ListView element)
        {
            if (!(element.GetValue(GridViewColumnResizeBehaviorProperty) is ListViewResizeBehavior behavior))
            {
                behavior = new ListViewResizeBehavior(element);
                element.SetValue(ListViewResizeBehaviorProperty, behavior);
            }

            return behavior;
        }

        #endregion

        #region Nested type: ListViewResizeBehavior

        /// <summary>
        /// ListViewResizeBehavior class that gets attached to the ListView control
        /// </summary>
        internal class ListViewResizeBehavior : IDisposable
        {
            private const long RefreshTime = Timeout.Infinite;
            private const long Delay = 250;

            private readonly ListView _element;
            private readonly Timer _timer;

            public bool Enabled { get; set; }

            public ListViewResizeBehavior(ListView element)
            {
                _element = element ?? throw new ArgumentNullException("element");
                element.Loaded += OnLoaded;

                // Action for resizing and re-enable the size lookup
                // This stops the columns from constantly resizing to improve performance
                Action resizeAndEnableSize = () =>
                {
                    Resize();
                    _element.SizeChanged += OnSizeChanged;
                };
                _timer = new Timer(x => Application.Current.Dispatcher.BeginInvoke(resizeAndEnableSize), null, Delay, RefreshTime);
            }

            private void OnLoaded(object sender, RoutedEventArgs e)
            {
                _element.SizeChanged += OnSizeChanged;
            }

            private void OnSizeChanged(object sender, SizeChangedEventArgs e)
            {
                if (e.WidthChanged)
                {
                    _element.SizeChanged -= OnSizeChanged;
                    _timer.Change(Delay, RefreshTime);
                }
            }

            private void Resize()
            {
                if (Enabled)
                {
                    double totalWidth = _element.ActualWidth;
                    if (_element.View is GridView gv)
                    {
                        double allowedSpace = totalWidth - GetAllocatedSpace(gv);
                        double totalPercentage = GridViewColumnResizeBehaviors(gv).Sum(x => x.Percentage);
                        foreach (GridViewColumnResizeBehavior behavior in GridViewColumnResizeBehaviors(gv))
                        {
                            behavior.SetWidth(allowedSpace - behavior.Margin, totalPercentage);
                        }
                    }
                }
            }

            private static IEnumerable<GridViewColumnResizeBehavior> GridViewColumnResizeBehaviors(GridView gv)
            {
                foreach (GridViewColumn t in gv.Columns)
                {
                    if (t.GetValue(GridViewColumnResizeBehaviorProperty) is GridViewColumnResizeBehavior gridViewColumnResizeBehavior)
                    {
                        yield return gridViewColumnResizeBehavior;
                    }
                }
            }

            private static double GetAllocatedSpace(GridView gv)
            {
                double totalWidth = 0;
                foreach (GridViewColumn t in gv.Columns)
                {
                    if (t.GetValue(GridViewColumnResizeBehaviorProperty) is GridViewColumnResizeBehavior gridViewColumnResizeBehavior)
                    {
                        if (gridViewColumnResizeBehavior.IsStatic)
                        {
                            totalWidth += gridViewColumnResizeBehavior.StaticWidth;
                        }
                    }
                    else
                    {
                        totalWidth += t.ActualWidth;
                    }
                }
                return totalWidth;
            }

            public void Dispose()
            {
                _timer?.Dispose();
            }
        }

        #endregion
    }

    #region Nested type: GridViewColumnResizeBehavior

    /// <summary>
    /// GridViewColumn class that gets attached to the GridViewColumn control
    /// </summary>
    public class GridViewColumnResizeBehavior
    {
        private readonly GridViewColumn _element;

        public GridViewColumnResizeBehavior(GridViewColumn element)
        {
            _element = element;
        }

        public string Width { get; set; }

        public double Margin { get; set; } = 2;

        public bool IsStatic
        {
            get => StaticWidth >= 0;
        }

        public double StaticWidth  => double.TryParse(Width, out double result) ? result : -1;
        public double Percentage => !IsStatic ? Mulitplier * 100 : 0;

        public double Mulitplier
        {
            get
            {
                if (Width == "*" || Width == "1*") return 1;
                if (Width.EndsWith("*"))
                {
                    if (double.TryParse(Width.Substring(0, Width.Length - 1), out double perc))
                    {
                        return perc;
                    }
                }
                return 1;
            }
        }

        public void SetWidth(double allowedSpace, double totalPercentage)
        {
            double width = IsStatic ? StaticWidth : allowedSpace * (Percentage / totalPercentage);

            if (width > 0)
            {
                _element.Width = width;
            }
        }
    }

    #endregion
}

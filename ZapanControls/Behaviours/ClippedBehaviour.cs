using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ZapanControls.Behaviours
{
    public sealed class ClippedBehaviour 
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached(
            "Source", typeof(object), typeof(ClippedBehaviour), new PropertyMetadata(OnSetSourceCallback));

        public static object GetSource(DependencyObject obj) => obj?.GetValue(SourceProperty);
        public static void SetSource(DependencyObject obj, object value) => obj?.SetValue(SourceProperty, value);

        private static void OnSetSourceCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is FrameworkElement element)
            {
                if (e.NewValue is FrameworkElement source)
                {
                    var border = new Border()
                    {
                        Background = Brushes.Black,
                        SnapsToDevicePixels = true,
                        UseLayoutRounding = true
                    };

                    border.SetBinding(Border.CornerRadiusProperty, new Binding
                    {
                        Mode = BindingMode.OneWay,
                        Path = new PropertyPath("CornerRadius"),
                        Source = source
                    });

                    border.SetBinding(FrameworkElement.HeightProperty, new Binding
                    {
                        Mode = BindingMode.OneWay,
                        Path = new PropertyPath("ActualHeight"),
                        Source = source
                    });

                    border.SetBinding(FrameworkElement.WidthProperty, new Binding
                    {
                        Mode = BindingMode.OneWay,
                        Path = new PropertyPath("ActualWidth"),
                        Source = source
                    });

                    element.OpacityMask = new VisualBrush(border);
                }
                else
                {
                    element.OpacityMask = null;
                }
            }
            else
            {
                Console.Error.WriteLine($"Error: Expected type FrameworkElement but found {dependencyObject.GetType().Name}");
            }
        }
    }
}

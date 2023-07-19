using System;
using System.Windows;
using System.Windows.Controls;

namespace ZapanControls.Controls
{
    public sealed class UniformWrapPanel : WrapPanel
    {
        #region Properties
        #region IsAutoUniform
        public static readonly DependencyProperty IsAutoUniformProperty = DependencyProperty.Register(
            "IsAutoUniform", typeof(bool), typeof(UniformWrapPanel),
            new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(IsAutoUniformChanged)));

        public bool IsAutoUniform
        {
            get => (bool)GetValue(IsAutoUniformProperty);
            set => SetValue(IsAutoUniformProperty, value);
        }

        private static void IsAutoUniformChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is UniformWrapPanel panel)
            {
                panel.InvalidateVisual();
            }
        }
        #endregion
        #endregion

        #region Overrides
        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children.Count > 0 && IsAutoUniform)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    //double totalWidth = availableSize.Width;
                    ItemWidth = 0.0;
                    foreach (UIElement el in Children)
                    {
                        el.Measure(availableSize);
                        Size next = el.DesiredSize;
                        if (!(double.IsInfinity(next.Width) || double.IsNaN(next.Width)))
                        {
                            ItemWidth = Math.Max(next.Width, ItemWidth);
                        }
                    }
                }
                else
                {
                    //double totalHeight = availableSize.Height;
                    ItemHeight = 0.0;
                    foreach (UIElement el in Children)
                    {
                        el.Measure(availableSize);
                        Size next = el.DesiredSize;
                        if (!(double.IsInfinity(next.Height) || double.IsNaN(next.Height)))
                        {
                            ItemHeight = Math.Max(next.Height, ItemHeight);
                        }
                    }
                }
            }
            return base.MeasureOverride(availableSize);
        }
        #endregion
    }
}

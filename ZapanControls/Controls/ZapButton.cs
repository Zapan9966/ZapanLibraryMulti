using System.Windows;
using System.Windows.Controls;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;

namespace ZapanControls.Controls
{
    public sealed class ZapButton : ZapButtonBase
    {
        #region Template Declarations
        public static TemplatePath Flat = new TemplatePath(ZapButtonTemplates.Flat, "/ZapanControls;component/Themes/ZapButton/Template.Flat.xaml");
        public static TemplatePath Glass = new TemplatePath(ZapButtonTemplates.Glass, "/ZapanControls;component/Themes/ZapButton/Template.Glass.xaml");
        public static TemplatePath Round = new TemplatePath(ZapButtonTemplates.Round, "/ZapanControls;component/Themes/ZapButton/Template.Round.xaml");
        #endregion

        #region Native Properties Changed
        #region Height
        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapButton zb)
            {
                if (zb.ZapTemplate == ZapButtonTemplates.Round.ToString())
                {
                    double height = (double)e.NewValue;
                    if (double.IsNaN(height) || double.IsInfinity(height))
                    {
                        zb.Height = zb.MinHeight;
                    }
                    else if (zb.Width != height)
                    {
                        zb.Width = height;
                    }
                }
            }
        }
        #endregion

        #region Width
        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapButton zb)
            {
                if (zb.ZapTemplate == ZapButtonTemplates.Round.ToString())
                {
                    double width = (double)e.NewValue;
                    if (double.IsNaN(width) || double.IsInfinity(width))
                    {
                        zb.Width = zb.MinWidth;
                    }
                    else if (zb.Height != width)
                    {
                        zb.Height = width;
                    }
                }
            }
        }
        #endregion
        #endregion

        #region Constructor
        static ZapButton()
        {
            WidthProperty.OverrideMetadata(typeof(ZapButton), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, OnWidthChanged));
            HeightProperty.OverrideMetadata(typeof(ZapButton), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, OnHeightChanged));
        }

        public ZapButton()
        {

        }
        #endregion

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ContentPresenter content = (ContentPresenter)Template.FindName("PART_Content", this);
            if (content != null)
            {
                content.SizeChanged += (s, e) =>
                {
                    if (ZapTemplate == "Round")
                    {
                        Width = content.ActualWidth + 4;
                    }
                };
            }
        }

        protected override void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            base.OnThemeChanged(sender, e);
        }
        #endregion
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZapanControls.Controls
{
    /// <summary>
    /// A control featuring a range of loading indicating animations.
    /// </summary>
    [TemplatePart(Name = "Border", Type = typeof(Border))]
    public class ZapLoadingIndicator : Control
    {
        private Border PART_Border;
        private static readonly ResourceDictionary _dict = new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/ZapanControls;component/Themes/ZapLoadingIndicator.xaml")
        };

        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="ZapLoadingIndicator.AccentColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(
            "AccentColor", typeof(Brush), typeof(ZapLoadingIndicator), new FrameworkPropertyMetadata(Brushes.Indigo));

        /// <summary>
        /// Get/set loading indicator color.
        /// </summary>
        public Brush AccentColor
        {
            get { return (Brush)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapLoadingIndicator.IsActive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ZapLoadingIndicator), new PropertyMetadata(true, (o, e) =>
            {
                ZapLoadingIndicator li = (ZapLoadingIndicator)o;

                if (li.PART_Border == null)
                    return;

                if ((bool)e.NewValue == false)
                {
                    VisualStateManager.GoToElementState(li.PART_Border, "Inactive", false);
                    li.PART_Border.Visibility = Visibility.Collapsed;
                }
                else
                {
                    VisualStateManager.GoToElementState(li.PART_Border, "Active", false);
                    li.PART_Border.Visibility = Visibility.Visible;

                    foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(li.PART_Border))
                    {
                        if (group.Name == "ActiveStates")
                        {
                            foreach (VisualState state in group.States)
                            {
                                if (state.Name == "Active")
                                    state.Storyboard.SetSpeedRatio(li.PART_Border, li.SpeedRatio);
                            }
                        }
                    }
                }
            }));

        /// <summary>
        /// Get/set whether the loading indicator is active.
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapLoadingIndicator.SpeedRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpeedRatioProperty =
            DependencyProperty.Register("SpeedRatio", typeof(double), typeof(ZapLoadingIndicator), new PropertyMetadata(1d, (o, e) =>
            {
                ZapLoadingIndicator li = (ZapLoadingIndicator)o;

                if (li.PART_Border == null || li.IsActive == false)
                    return;

                foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(li.PART_Border))
                {
                    if (group.Name == "ActiveStates")
                    {
                        foreach (VisualState state in group.States)
                        {
                            if (state.Name == "Active")
                                state.Storyboard.SetSpeedRatio(li.PART_Border, (double)e.NewValue);
                        }
                    }
                }
            }));

        /// <summary>
        /// Get/set the speed ratio of the animation.
        /// </summary>
        public double SpeedRatio
        {
            get { return (double)GetValue(SpeedRatioProperty); }
            set { SetValue(SpeedRatioProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ZapLoadingIndicator.IndicatorStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndicatorStyleProperty = DependencyProperty.Register(
            "IndicatorStyle", typeof(LoadingIndicatorStyle), typeof(ZapLoadingIndicator), new PropertyMetadata(LoadingIndicatorStyle.None, (o, e) =>
            {

                LoadingIndicatorStyle style = (LoadingIndicatorStyle)e.NewValue;
                ZapLoadingIndicator li = (ZapLoadingIndicator)o;

                switch (style)
                {
                    case LoadingIndicatorStyle.None:
                        li.Style = null;
                        break;
                    case LoadingIndicatorStyle.Arcs:
                        li.Style = (Style)_dict["ZapLoadingIndicatorArcsStyle"];
                        break;
                    case LoadingIndicatorStyle.ArcsRing:
                        li.Style = (Style)_dict["ZapLoadingIndicatorArcsRingStyle"];
                        break;
                    case LoadingIndicatorStyle.DoubleBounce:
                        li.Style = (Style)_dict["ZapLoadingIndicatorDoubleBounceStyle"];
                        break;
                    case LoadingIndicatorStyle.FlipPane:
                        li.Style = (Style)_dict["ZapLoadingIndicatorFlipPlaneStyle"];
                        break;
                    case LoadingIndicatorStyle.Pulse:
                        li.Style = (Style)_dict["ZapLoadingIndicatorPulseStyle"];
                        break;
                    case LoadingIndicatorStyle.Ring:
                        li.Style = (Style)_dict["ZapLoadingIndicatorRingStyle"];
                        break;
                    case LoadingIndicatorStyle.ThreeDots:
                        li.Style = (Style)_dict["ZapLoadingIndicatorThreeDotsStyle"];
                        break;
                    case LoadingIndicatorStyle.Wave:
                        li.Style = (Style)_dict["ZapLoadingIndicatorWaveStyle"];
                        break;
                }
            }));

        /// <summary>
        /// Get/set the loading indicator style.
        /// </summary>
        public LoadingIndicatorStyle IndicatorStyle
        {
            get { return (LoadingIndicatorStyle)GetValue(IndicatorStyleProperty); }
            set { SetValue(IndicatorStyleProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZapanControls.Controls.ZapLoadingIndicator"/> class.
        /// </summary>
        public ZapLoadingIndicator()
        { }

        #endregion

        #region Overrides

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Border = (Border)GetTemplateChild("PART_Border");

            if (PART_Border != null)
            {
                VisualStateManager.GoToElementState(PART_Border, (IsActive ? "Active" : "Inactive"), false);
                foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(PART_Border))
                {
                    if (group.Name == "ActiveStates")
                    {
                        foreach (VisualState state in group.States)
                        {
                            if (state.Name == "Active")
                                state.Storyboard.SetSpeedRatio(PART_Border, this.SpeedRatio);
                        }
                    }
                }
                PART_Border.Visibility = (IsActive ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        #endregion

    }

    #region Enums

    /// <summary>
    /// Loading Indicator styles 
    /// </summary>
    public enum LoadingIndicatorStyle
    {
        None = 0,
        Arcs = 1,
        ArcsRing = 2,
        DoubleBounce = 3,
        FlipPane = 4,
        Pulse = 5,
        Ring = 6,
        ThreeDots = 7,
        Wave = 8
    }

    #endregion
}

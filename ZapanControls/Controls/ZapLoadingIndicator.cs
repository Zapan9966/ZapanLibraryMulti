using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZapanControls.Controls.ControlEventArgs;
using ZapanControls.Controls.Primitives;
using ZapanControls.Controls.Templates;
using ZapanControls.Controls.Themes;
using ZapanControls.Interfaces;
using ZapanControls.Libraries;

namespace ZapanControls.Controls
{
    /// <summary>
    /// A control featuring a range of loading indicating animations.
    /// </summary>
    [TemplatePart(Name = "Border", Type = typeof(Border))]
    public class ZapLoadingIndicator : Control, ITemplate<ZapLoadingIndicatorTemplates>
    {
        #region Fields
        private Border PART_Border;
        private bool _hasInitialized;
        #endregion

        #region Template Declarations
        public static TemplatePath Arcs = new TemplatePath(ZapLoadingIndicatorTemplates.Arcs, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.Arcs.xaml");
        public static TemplatePath ArcsRing = new TemplatePath(ZapLoadingIndicatorTemplates.ArcsRing, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.ArcsRing.xaml");
        public static TemplatePath DoubleBounce = new TemplatePath(ZapLoadingIndicatorTemplates.DoubleBounce, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.DoubleBounce.xaml");
        public static TemplatePath FlipPane = new TemplatePath(ZapLoadingIndicatorTemplates.FlipPane, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.FlipPane.xaml");
        public static TemplatePath Pulse = new TemplatePath(ZapLoadingIndicatorTemplates.Pulse, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.Pulse.xaml");
        public static TemplatePath Ring = new TemplatePath(ZapLoadingIndicatorTemplates.Ring, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.Ring.xaml");
        public static TemplatePath ThreeDots = new TemplatePath(ZapLoadingIndicatorTemplates.ThreeDots, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.ThreeDots.xaml");
        public static TemplatePath Wave = new TemplatePath(ZapLoadingIndicatorTemplates.Wave, "/ZapanControls;component/Themes/ZapLoadingIndicator/Template.Wave.xaml");
        #endregion

        #region Properties
        #region AccentColor
        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(
            "AccentColor", typeof(Brush), typeof(ZapLoadingIndicator),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnAccentColorChanged));

        public Brush AccentColor { get => (Brush)GetValue(AccentColorProperty); set => SetValue(AccentColorProperty, value); }

        private static void OnAccentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(AccentColorProperty, e.NewValue);
        #endregion

        #region IsActive
        /// <summary>
        /// Identifies the <see cref="IsActive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            "IsActive", typeof(bool), typeof(ZapLoadingIndicator), new PropertyMetadata(true, OnIsActiveChanged));

        /// <summary>
        /// Get/set whether the loading indicator is active.
        /// </summary>
        public bool IsActive { get => (bool)GetValue(IsActiveProperty); set => SetValue(IsActiveProperty, value); }

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZapLoadingIndicator li)
            {
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
            }
        }
        #endregion

        #region SpeedRatio
        /// <summary>
        /// Identifies the <see cref="SpeedRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpeedRatioProperty = DependencyProperty.Register(
            "SpeedRatio", typeof(double), typeof(ZapLoadingIndicator), new PropertyMetadata(1d, OnSpeedRatioChanged));

        /// <summary>
        /// Get/set the speed ratio of the animation.
        /// </summary>
        public double SpeedRatio { get => (double)GetValue(SpeedRatioProperty); set => SetValue(SpeedRatioProperty, value); }

        private static void OnSpeedRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        { 
            d.SetValueCommon(SpeedRatioProperty, e.NewValue);

            if (d is ZapLoadingIndicator li)
            {
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
            }
        }
        #endregion
        #endregion

        #region Template Properties
        #region HasInitialized
        public bool HasInitialized { get => _hasInitialized; private set => Set(ref _hasInitialized, value); }
        #endregion

        #region TemplateDictionaries
        public Dictionary<string, ResourceDictionary> TemplateDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion

        #region ZapTemplate
        public static readonly DependencyProperty ZapTemplateProperty = DependencyProperty.Register(
            "ZapTemplate", typeof(ZapLoadingIndicatorTemplates), typeof(ZapLoadingIndicator),
            new FrameworkPropertyMetadata(ZapLoadingIndicatorTemplates.ThreeDots,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnZapTemplateChanged),
                new CoerceValueCallback(CoerceZapTemplateChange)));

        public ZapLoadingIndicatorTemplates ZapTemplate 
        { 
            get => (ZapLoadingIndicatorTemplates)GetValue(ZapTemplateProperty); 
            set => SetValue(ZapTemplateProperty, value); 
        }

        private static void OnZapTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) //=> d.TemplateChanged(e, TemplateChangedEvent);
        {
            // test args
            if (!(d is ZapLoadingIndicator li) || e == null)
                throw new ArgumentNullException("Invalid ZapTemplate property");

            ZapLoadingIndicatorTemplates curTemplateName = (ZapLoadingIndicatorTemplates)e.OldValue;
            string curRegisteredTemplateName = curTemplateName.ToString().TemplateRegistrationName(d.GetType());

            if (li.TemplateDictionaries.ContainsKey(curRegisteredTemplateName))
            {
                // remove current template
                ResourceDictionary curTemplateDictionary = li.TemplateDictionaries[curRegisteredTemplateName];
                li.Resources.MergedDictionaries.Remove(curTemplateDictionary);
            }

            // new template name
            ZapLoadingIndicatorTemplates newTemplateName = (ZapLoadingIndicatorTemplates)e.NewValue;
            string newRegisteredTemplateName = newTemplateName.ToString().TemplateRegistrationName(d.GetType());

            // add the resource
            if (!li.TemplateDictionaries.ContainsKey(newRegisteredTemplateName))
            {
                throw new ArgumentNullException("Invalid ZapTemplate property");
            }
            else
            {
                // add the dictionary
                ResourceDictionary newTemplateDictionary = li.TemplateDictionaries[newRegisteredTemplateName];
                li.Resources.MergedDictionaries.Add(newTemplateDictionary);

                if (li.HasInitialized)
                    li.OnApplyTemplate();

                // Raise theme successfully changed event
                li.RaiseEvent(new TemplateChangedEventArgs(TemplateChangedEvent, li, curRegisteredTemplateName, newRegisteredTemplateName));
            }

            li.RaisePropertyChanged(new PropertyChangedEventArgs("ZapTemplate"));
        }

        private static object CoerceZapTemplateChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion
        #endregion

        #region Theme Properties
        #region DefaultThemeProperties
        public Dictionary<DependencyProperty, object> DefaultThemeProperties { get; internal set; } = new Dictionary<DependencyProperty, object>();
        #endregion

        #region Theme
        /// <summary>
        /// Get/Sets the theme
        /// </summary>
        public static DependencyProperty ThemeProperty = DependencyProperty.Register(
            "Theme", typeof(string), typeof(ZapLoadingIndicator),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnThemeChanged),
                new CoerceValueCallback(CoerceThemeChange)));

        public string Theme { get => (string)GetValue(ThemeProperty); set => SetValue(ThemeProperty, value); }
        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.ThemeChanged(e, ThemeChangedEvent);

        private static object CoerceThemeChange(DependencyObject d, object o)
        {
            return o;
        }
        #endregion

        #region ThemeDictionaries
        public Dictionary<string, ResourceDictionary> ThemeDictionaries { get; internal set; } = new Dictionary<string, ResourceDictionary>();
        #endregion
        #endregion

        #region Native Properties Changed
        #region Background
        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BackgroundProperty, e.NewValue);
        #endregion

        #region BorderBrush
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BorderBrushProperty, e.NewValue);
        #endregion

        #region BorderThickness
        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValueCommon(BorderThicknessProperty, e.NewValue);
        #endregion
        #endregion

        #region Events
        #region TemplateChanged
        public static readonly RoutedEvent TemplateChangedEvent = EventManager.RegisterRoutedEvent(
            "TemplateChanged", RoutingStrategy.Bubble, typeof(ITemplate<ZapLoadingIndicatorTemplates>.TemplateChangedEventHandler), typeof(ZapLoadingIndicator));

        public event ITemplate<ZapLoadingIndicatorTemplates>.TemplateChangedEventHandler TemplateChanged 
        { 
            add => AddHandler(TemplateChangedEvent, value); 
            remove => RemoveHandler(TemplateChangedEvent, value); 
        }
        #endregion

        #region ThemeChanged
        public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
            "ThemeChanged", RoutingStrategy.Bubble, typeof(ITheme.ThemeChangedEventHandler), typeof(ZapLoadingIndicator));

        public event ITheme.ThemeChangedEventHandler ThemeChanged { add => AddHandler(ThemeChangedEvent, value); remove => RemoveHandler(ThemeChangedEvent, value); }

        protected virtual void OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            DefaultThemeProperties.Clear();

            this.SetThemePropertyDefault(AccentColorProperty, ResourceKeys.ZapLoadingIndicatorResourceKeys.AccentColorKey);
            this.SetThemePropertyDefault(BackgroundProperty, ResourceKeys.ZapLoadingIndicatorResourceKeys.BackgroundKey);
            this.SetThemePropertyDefault(BorderBrushProperty, ResourceKeys.ZapLoadingIndicatorResourceKeys.BorderBrushKey);
            this.SetThemePropertyDefault(BorderThicknessProperty, ResourceKeys.ZapLoadingIndicatorResourceKeys.BorderThicknessKey);
            this.SetThemePropertyDefault(SpeedRatioProperty, ResourceKeys.ZapLoadingIndicatorResourceKeys.SpeedRatioKey);
        }
        #endregion
        #endregion

        #region Constructors
        static ZapLoadingIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZapLoadingIndicator), new FrameworkPropertyMetadata(typeof(ZapLoadingIndicator)));

            FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;

            BackgroundProperty.OverrideMetadata(typeof(ZapLoadingIndicator), new FrameworkPropertyMetadata(null, options, OnBackgroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(ZapLoadingIndicator), new FrameworkPropertyMetadata(null, options, OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(ZapLoadingIndicator), new FrameworkPropertyMetadata(new Thickness(0), options, OnBorderThicknessChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZapLoadingIndicator"/> class.
        /// </summary>
        public ZapLoadingIndicator()
        {
            // Load Templates
            this.RegisterAttachedTemplates<ZapLoadingIndicatorTemplates>(typeof(ZapLoadingIndicator));
            this.RegisterAttachedTemplates<ZapLoadingIndicatorTemplates>(GetType());
            this.LoadDefaultTemplate<ZapLoadingIndicatorTemplates>(ZapTemplateProperty);

            // Load Themes
            ThemeChanged += OnThemeChanged;
            this.RegisterInternalThemes<ZapLoadingIndicatorThemes>();
            this.LoadDefaultTheme(ThemeProperty);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HasInitialized = true;

            PART_Border = (Border)GetTemplateChild("PART_Border");

            if (PART_Border != null)
            {
                VisualStateManager.GoToElementState(PART_Border, IsActive ? "Active" : "Inactive", false);
                foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(PART_Border))
                {
                    if (group.Name == "ActiveStates")
                    {
                        foreach (VisualState state in group.States)
                        {
                            if (state.Name == "Active")
                                state.Storyboard.SetSpeedRatio(PART_Border, SpeedRatio);
                        }
                    }
                }
                PART_Border.Visibility = IsActive ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        /// <summary>Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            return Set(propertyName, ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <typeparam name="T">The type of the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(Expression<Func<T>> propertyNameExpression, ref T oldValue, T newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <typeparam name="TProp">The type of the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<TClass, TProp>(Expression<Func<TClass, TProp>> propertyNameExpression, ref TProp oldValue, TProp newValue)
        {
            return Set(ExpressionUtilities.GetPropertyName(propertyNameExpression), ref oldValue, newValue);
        }

        /// <summary>Updates the property and raises the changed event, but only if the new value does not equal the old value. </summary>
        /// <param name="propertyName">The property name as lambda. </param>
        /// <param name="oldValue">A reference to the backing field of the property. </param>
        /// <param name="newValue">The new value. </param>
        /// <returns>True if the property has changed. </returns>
        public bool Set<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (Equals(oldValue, newValue))
                return false;

            oldValue = newValue;
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
            return true;
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyName">The property name. </param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        public void RaisePropertyChanged(Expression<Func<object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <param name="propertyNameExpression">The property name as lambda. </param>
        public void RaisePropertyChanged<TClass>(Expression<Func<TClass, object>> propertyNameExpression)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(ExpressionUtilities.GetPropertyName(propertyNameExpression)));
        }

        /// <summary>Raises the property changed event. </summary>
        /// <param name="args">The arguments. </param>
        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>Raises the property changed event for all properties (string.Empty). </summary>
        public void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(string.Empty));
        }
        #endregion
    }
}

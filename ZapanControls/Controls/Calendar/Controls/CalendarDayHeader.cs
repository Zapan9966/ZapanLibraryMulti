﻿using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ZapanControls.Converters;

namespace ZapanControls.Controls
{
    [TemplatePart(Name = ElementDayHeaderLabel, Type = typeof(TextBlock))]
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Cannot be static")]
    internal sealed class CalendarDayHeader : Control
    {
        private const string ElementDayHeaderLabel = "PART_DayHeaderLabel";
        TextBlock _dayHeaderLabel;

        public ZapCalendar Owner { get; set; }

        static CalendarDayHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarDayHeader), new FrameworkPropertyMetadata(typeof(CalendarDayHeader)));
        }

        private Binding GetOwnerBinding(string propertyName)
        {
            return new Binding(propertyName) { Source = this.Owner };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dayHeaderLabel = GetTemplateChild(ElementDayHeaderLabel) as TextBlock;

            PopulateHeader();
        }

        void PopulateHeader()
        {
            Binding binding = GetOwnerBinding("CurrentDate");
            binding.Converter = new TitledConverter();
            _dayHeaderLabel.SetBinding(TextBlock.TextProperty, binding);
        }
    }
}

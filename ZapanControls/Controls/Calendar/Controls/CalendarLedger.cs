using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZapanControls.Controls
{
    [TemplatePart(Name = ElementLedgerItems, Type = typeof(StackPanel))]
    internal sealed class CalendarLedger : Control
    {
        private const string ElementLedgerItems = "PART_LedgerItems";

        StackPanel _ledgerItems;

        static CalendarLedger()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarLedger), new FrameworkPropertyMetadata(typeof(CalendarLedger)));
        }

        #region CalendarLedgerItemStyle

        public static readonly DependencyProperty CalendarLedgerItemStyleProperty = ZapCalendar.CalendarLedgerItemStyleProperty.AddOwner(typeof(CalendarLedger));

        public Style CalendarLedgerItemStyle
        {
            get => (Style)GetValue(CalendarLedgerItemStyleProperty);
            set => SetValue(CalendarLedgerItemStyleProperty, value);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _ledgerItems = GetTemplateChild(ElementLedgerItems) as StackPanel;

            PopulateLedger();
        }

        private void PopulateLedger()
        {
            if (_ledgerItems != null)
            {
                for (int i = 0; i < 24; i++)
                {
                    CalendarLedgerItem item = new CalendarLedgerItem
                    {
                        TimeslotA = i.ToString(),
                        TimeslotB = "00"
                    };
                    item.SetBinding(StyleProperty, GetOwnerBinding("CalendarLedgerItemStyle"));
                    _ledgerItems.Children.Add(item);
                }
            }
        }

        public ZapCalendar Owner { get; set; }

        private Binding GetOwnerBinding(string propertyName) 
            => new Binding(propertyName) { Source = Owner };
    }
}

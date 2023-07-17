using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ZapanControls.Controls;

namespace ZapanControls.Automation.Peers
{
    public sealed class ZapProgressAutomationPeer : RangeBaseAutomationPeer, IRangeValueProvider
    {
        #region Constructor
        public ZapProgressAutomationPeer(ZapProgress owner) : base(owner)
        {

        }
        #endregion

        protected override string GetClassNameCore()
        {
            return "ZapProgress";
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ProgressBar;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            // Indeterminate ProgressBar should not support RangeValue pattern
            return patternInterface == PatternInterface.RangeValue && ((ZapProgress)Owner).IsIndeterminate
                ? null
                : base.GetPattern(patternInterface);
        }

        ///<summary>Indicates that the value can only be read, not modified.
        ///returns True if the control is read-only</summary>
        bool IRangeValueProvider.IsReadOnly => true;

        ///<summary>Value of a Large Change</summary>
        double IRangeValueProvider.LargeChange => double.NaN;

        ///<summary>Value of a Small Change</summary>
        double IRangeValueProvider.SmallChange => double.NaN;
    }
}

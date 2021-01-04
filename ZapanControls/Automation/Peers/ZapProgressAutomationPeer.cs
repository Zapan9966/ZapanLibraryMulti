using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ZapanControls.Controls;

namespace ZapanControls.Automation.Peers
{
    public class ZapProgressAutomationPeer : RangeBaseAutomationPeer, IRangeValueProvider
    {
        #region Constructor
        public ZapProgressAutomationPeer(ZapProgress owner) : base(owner)
        {

        }
        #endregion

        override protected string GetClassNameCore()
        {
            return "ZapProgress";
        }

        override protected AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ProgressBar;
        }

        override public object GetPattern(PatternInterface patternInterface)
        {
            // Indeterminate ProgressBar should not support RangeValue pattern
            if (patternInterface == PatternInterface.RangeValue && ((ZapProgress)Owner).IsIndeterminate)
                return null;

            return base.GetPattern(patternInterface);
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

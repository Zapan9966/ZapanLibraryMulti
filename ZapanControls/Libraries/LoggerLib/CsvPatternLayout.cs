using log4net.Core;
using log4net.Layout;
using System.IO;

namespace ZapanControls.Libraries.LoggerLib
{
    public sealed class CsvPatternLayout : PatternLayout
    {
        public override void ActivateOptions()
        {
            // register custom pattern tokens
            AddConverter("newfield", typeof(NewFieldConverter));
            AddConverter("endrow", typeof(EndRowConverter));
            base.ActivateOptions();
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            using (CsvTextWriter ctw = new CsvTextWriter(writer))
            {
                // write the starting quote for the first field
                ctw.WriteQuote();
                base.Format(ctw, loggingEvent);
            }
        }
    }
}

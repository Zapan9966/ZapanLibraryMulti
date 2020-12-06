using log4net.Util;
using System.IO;

namespace ZapanControls.Libraries.LoggerLib
{
    public sealed class EndRowConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            // write the ending quote for the last field
            if (writer is CsvTextWriter ctw)
            {
                ctw.WriteQuote();
                writer?.WriteLine();
            }
        }
    }
}

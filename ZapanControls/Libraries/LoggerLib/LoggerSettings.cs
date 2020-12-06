using static log4net.Appender.RollingFileAppender;

namespace ZapanControls.Libraries.LoggerLib
{
    public class LoggerSettings
    {
        public string Folder { get; set; }
        public string PatternHeader { get; set; } = "Date;Heure;Niveau;Message;Exception\r\n";
        public string PatternConversion { get; set; } = "%date{dd/MM/yyyy}%newfield%date{HH:mm:ss,fff}%newfield%level%newfield%message%newfield%exception%endrow";
        public RollingMode RollingMode { get; set; } = RollingMode.Date;
        public string DatePattern { get; set; } = @"yyyyMMdd'.log'";
    }
}

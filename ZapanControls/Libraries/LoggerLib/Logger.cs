using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using System.Text;
using static log4net.Appender.RollingFileAppender;

namespace ZapanControls.Libraries.LoggerLib
{
    public sealed class Logger
    {
        private readonly ILog _logger;

        public Logger(
            string logFolder,
            string fileName = "",
            string patternHeader = "Date;Heure;Niveau;Message;Exception\r\n",
            string patternConversion = "%date{dd/MM/yyyy}%newfield%date{HH:mm:ss,fff}%newfield%level%newfield%message%newfield%exception%endrow",
            RollingMode rollingMode = RollingMode.Date,
            int maxSizeRollBackups = 7,
            bool appendToFile = true,
            bool staticLogFileName = false,
            FileAppender.LockingModelBase lockingModel = null)
        {
            _logger = LogManager.GetLogger(typeof(Logger));

            CsvPatternLayout patternLayout = new CsvPatternLayout()
            {
                Header = patternHeader,
                ConversionPattern = patternConversion
            };
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender
            {
                Layout = patternLayout,
                File = logFolder,
                RollingStyle = rollingMode,
                DatePattern = string.IsNullOrEmpty(fileName) 
                    ? @"yyyyMMdd'.csv'" 
                    : $@"yyyyMMdd'_{fileName}.csv'",
                MaxSizeRollBackups = maxSizeRollBackups,
                AppendToFile = appendToFile,
                StaticLogFileName = staticLogFileName,
                Encoding = Encoding.UTF8,
                LockingModel = lockingModel ?? new FileAppender.MinimalLock()
            };
            roller.ActivateOptions();

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.AddAppender(roller);
            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }

        public void Info(string message, Exception ex = null)
            => _logger.Info(message, ex);

        public void Warn(string message, Exception ex = null)
            => _logger.Warn(message, ex);

        public void Error(string message, Exception ex = null)
            => _logger.Error(message, ex);

        public void Debug(string message, Exception ex = null)
            => _logger.Debug(message, ex);
    }
}

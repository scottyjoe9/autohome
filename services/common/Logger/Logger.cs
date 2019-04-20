using System;
using System.Linq;

namespace Logger
{
    internal class Logger : ILogger
    {
        private readonly NLog.ILogger _logger;

        internal Logger(NLog.ILogger logger)
        {
            _logger = logger;
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsWarningEnabled => _logger.IsWarnEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        public void ChangeLogLevel(string logLevel)
        {

        }

        public string LogLevel
        {
            get
            {
                var rules = NLog.LogManager.Configuration.LoggingRules;
                var rule = rules.FirstOrDefault(x => x.LoggerNamePattern == _logger.Name);
                return rule.Levels.Min().ToString();
            }

        }

        public void Debug(Action<LogAction> message)
        {
            if (IsDebugEnabled)
                message((m, args) => _logger.Debug(m, args));
        }

        public void Error(Action<LogAction> message)
        {
            if (IsErrorEnabled)
                message((m, args) => _logger.Error(m, args));
        }

        public void Fatal(Action<LogAction> message)
        {
            if (IsFatalEnabled)
                message((m, args) => _logger.Fatal(m, args));
        }

        public void Info(Action<LogAction> message)
        {
            if (IsInfoEnabled)
                message((m, args) => _logger.Info(m, args));
        }

        public void Trace(Action<LogAction> message)
        {
            if (IsTraceEnabled)
                message((m, args) => _logger.Trace(m, args));
        }

        public void Warning(Action<LogAction> message)
        {
            if (IsWarningEnabled)
                message((m, args) => _logger.Warn(m, args));
        }
    }
}

using System;
using System.Runtime.CompilerServices;

namespace Logger
{
    public class ClassLogger<TClass> : ClassLogger, IClassLogger<TClass>
    {
        public ClassLogger(ILogger logger)
            : base(logger, typeof(TClass).Name)
        {
        }
    }

    public class ClassLogger : IClassLogger
    {
        private readonly ILogger _logger;
        private readonly string _className;

        internal ClassLogger(ILogger logger, string className)
        {
            _logger = logger;
            _className = className;
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsWarningEnabled => _logger.IsWarningEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        public void ChangeLogLevel(string logLevel)
        {
            throw new NotImplementedException();
        }

        public void Debug(Action<LogAction> message, [CallerMemberName] string methodName = "N/A")
        {
            if (IsDebugEnabled)
                message((m, args) => {
                    _logger.Debug(l => l(CreateFullMessage(m, methodName), args));
                });
        }

        public void Error(Action<LogAction> message, [CallerMemberName] string methodName = "N/A")
        {
            if (IsErrorEnabled)
                message((m, args) => {
                    _logger.Error(l => l(CreateFullMessage(m, methodName), args));
                });
        }

        public void Fatal(Action<LogAction> message, [CallerMemberName] string methodName = "N/A")
        {
            if (IsFatalEnabled)
                message((m, args) => {
                    _logger.Fatal(l => l(CreateFullMessage(m, methodName), args));
                });
        }

        public void Info(Action<LogAction> message, [CallerMemberName] string methodName = "N/A")
        {
            if (IsInfoEnabled)
                message((m, args) => {
                    _logger.Info(l => l(CreateFullMessage(m, methodName), args));
                });
        }

        public void Trace(Action<LogAction> message, [CallerMemberName] string methodName = "N/A")
        {
            if (IsTraceEnabled)
                message((m, args) => {
                    _logger.Trace(l => l(CreateFullMessage(m, methodName), args));
                });
        }

        public void Warning(Action<LogAction> message, [CallerMemberName] string methodName = "N/A")
        {
            if (IsWarningEnabled)
                message((m, args) => {
                    _logger.Warning(l => l(CreateFullMessage(m, methodName), args));
                });
        }

        private string CreateFullMessage(string message, string methodName)
        {
            return $"{ _className}.{methodName}: {message}";
        }
    }
}

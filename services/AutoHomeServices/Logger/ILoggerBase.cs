using System;
namespace Logger
{
    public interface ILoggerBase
    {
        bool IsTraceEnabled { get; }

        bool IsDebugEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarningEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        void ChangeLogLevel(string logLevel);
    }
}

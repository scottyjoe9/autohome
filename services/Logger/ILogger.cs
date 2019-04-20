using System;
namespace Logger
{
    public delegate void LogAction(string message, params object[] args);

    public interface ILogger: ILoggerBase
    {
        void Trace(Action<LogAction> message);

        void Debug(Action<LogAction> message);

        void Info(Action<LogAction> message);

        void Warning(Action<LogAction> message);

        void Error(Action<LogAction> message);

        void Fatal(Action<LogAction> message);
    }
}

using System;
using System.Runtime.CompilerServices;

namespace Logger
{
    public interface IClassLogger<TClass> : IClassLogger { }

    public interface IClassLogger: ILoggerBase
    {
        void Trace(Action<LogAction> message, [CallerMemberName] string methodName = "N/A");

        void Debug(Action<LogAction> message,[CallerMemberName] string methodName = "N/A");

        void Info(Action<LogAction> message, [CallerMemberName] string methodName = "N/A");

        void Warning(Action<LogAction> message, [CallerMemberName] string methodName = "N/A");

        void Error(Action<LogAction> message, [CallerMemberName] string methodName = "N/A");

        void Fatal(Action<LogAction> message, [CallerMemberName] string methodName = "N/A");
    }
}

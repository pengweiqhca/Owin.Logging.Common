using System;
using System.Diagnostics;
using Common.Logging;
using Microsoft.Owin.Logging;

namespace Owin.Logging.Common
{
    public class CommonLoggingFactory : ILoggerFactory
    {
        protected readonly Func<String, ILog> m_CommonLogConstructor;
        protected readonly Func<ILog, TraceEventType, bool> m_IsLogEventEnabledFunc;
        protected readonly Action<ILog, TraceEventType, String, Exception> m_WriteLogEventFunc;

        public CommonLoggingFactory()
            : this(DefaultCreateLogger, DefaultIsLogEventEnabled, DefaultWriteLogEvent)
        {
        }

        public CommonLoggingFactory(Func<ILog, TraceEventType, bool> isLogEventEnabledFunc)
            : this(DefaultCreateLogger, isLogEventEnabledFunc, DefaultWriteLogEvent)
        {
        }

        public CommonLoggingFactory(Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
            : this(DefaultCreateLogger, DefaultIsLogEventEnabled, writeLogEventFunc)
        {
        }

        public CommonLoggingFactory(Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
            : this(DefaultCreateLogger, isLogEventEnabledFunc, writeLogEventFunc)
        {
        }

        public CommonLoggingFactory(Func<String, ILog> commonLogConstructor, Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, String, Exception> writeLogEventFunc)
        {
            if (commonLogConstructor == null)
            {
                throw new ArgumentNullException(nameof(commonLogConstructor));
            }
            if (isLogEventEnabledFunc == null)
            {
                throw new ArgumentNullException(nameof(isLogEventEnabledFunc));
            }
            if (writeLogEventFunc == null)
            {
                throw new ArgumentNullException(nameof(writeLogEventFunc));
            }

            m_CommonLogConstructor = commonLogConstructor;
            m_IsLogEventEnabledFunc = isLogEventEnabledFunc;
            m_WriteLogEventFunc = writeLogEventFunc;
        }

        public ILogger Create(String name)
        {
            return new CommonLogger(m_CommonLogConstructor(name), m_IsLogEventEnabledFunc, m_WriteLogEventFunc);
        }

        protected static ILog DefaultCreateLogger(String name)
        {
            return LogManager.GetLogger(name);
        }

        protected static bool DefaultIsLogEventEnabled(ILog commonLog, TraceEventType traceEventType)
        {
            switch (traceEventType)
            {
                case TraceEventType.Critical:
                    return commonLog.IsFatalEnabled;
                case TraceEventType.Error:
                    return commonLog.IsErrorEnabled;
                case TraceEventType.Warning:
                    return commonLog.IsWarnEnabled;
                case TraceEventType.Information:
                    return commonLog.IsInfoEnabled;
                case TraceEventType.Verbose:
                    return commonLog.IsTraceEnabled;
                case TraceEventType.Start:
                    return commonLog.IsDebugEnabled;
                case TraceEventType.Stop:
                    return commonLog.IsDebugEnabled;
                case TraceEventType.Suspend:
                    return commonLog.IsDebugEnabled;
                case TraceEventType.Resume:
                    return commonLog.IsDebugEnabled;
                case TraceEventType.Transfer:
                    return commonLog.IsDebugEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(traceEventType), traceEventType, "Unhandled EventType");
            }
        }

        protected static void DefaultWriteLogEvent(ILog commonLog, TraceEventType traceEventType, String message, Exception ex)
        {
            switch (traceEventType)
            {
                case TraceEventType.Critical:
                    commonLog.Fatal(message, ex);
                    break;
                case TraceEventType.Error:
                    commonLog.Error(message, ex);
                    break;
                case TraceEventType.Warning:
                    commonLog.Warn(message, ex);
                    break;
                case TraceEventType.Information:
                    commonLog.Info(message, ex);
                    break;
                case TraceEventType.Verbose:
                    commonLog.Trace(message, ex);
                    break;
                case TraceEventType.Start:
                    commonLog.Debug(message, ex);
                    break;
                case TraceEventType.Stop:
                    commonLog.Debug(message, ex);
                    break;
                case TraceEventType.Suspend:
                    commonLog.Debug(message, ex);
                    break;
                case TraceEventType.Resume:
                    commonLog.Debug(message, ex);
                    break;
                case TraceEventType.Transfer:
                    commonLog.Debug(message, ex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(traceEventType), traceEventType, "Unhandled EventType");
            }
        }
    }
}

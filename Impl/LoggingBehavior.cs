using UnityEngine;

namespace Dekuple
{
    /// <summary>
    /// A MonoBehaviour that has its own logger
    /// </summary>
    public class LoggingBehavior
        : MonoBehaviour
        , Flow.ILogger
    {
        public string LogPrefix { get => _log.LogPrefix; set => _log.LogPrefix = value; }
        public object LogSubject { get => _log.LogSubject; set => _log.LogSubject = value; }
        public bool ShowSource { get; set; }
        public bool ShowStack { get; set; }
        public int Verbosity { get => _log.Verbosity; set => _log.Verbosity = value; }

        private readonly Flow.Impl.Logger _log = new Flow.Impl.Logger("");

        protected LoggingBehavior()
        {
            _log.LogSubject = this;
            _log.ShowStack = Parameters.DefaultShowTraceStack;
            _log.ShowSource = Parameters.DefaultShowTraceSource;
        }

        public void Info(string fmt, params object[] args)
        {
            _log.Info(fmt, args);
        }

        public void Warn(string fmt, params object[] args)
        {
            _log.Warn(fmt, args);
        }

        public void Error(string fmt, params object[] args)
        {
            _log.Error(fmt, args);
        }

        public void Verbose(int level, string fmt, params object[] args)
        {
            _log.Verbose(level, fmt, args);
        }
    }
}

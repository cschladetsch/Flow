namespace Flow
{
    /// <summary>
    /// DOC
    /// </summary>
    /// <typeparam name="TLogger"></typeparam>
    public class LoggerFacade<TLogger>
        : ILogger where TLogger
            : class
            , ILogger
            , new()
    {
        public string LogPrefix
        {
            get => _log.LogPrefix;
            set => _log.LogPrefix = value;
        }

        public object LogSubject { get; set; }
        public bool ShowSource { get; set; }
        public bool ShowStack { get; set; }
        public int Verbosity { get; set; }

        private readonly TLogger _log = new TLogger();

        public LoggerFacade(string pre)
        {
            _log.LogPrefix = pre;
            _log.ShowStack = false;
            _log.ShowSource = true;
        }

        public void Info(string fmt, params object[] args)
            => _log.Info(fmt, args);

        public void Warn(string fmt, params object[] args)
            => _log.Warn(fmt, args);

        public void Error(string fmt, params object[] args)
            => _log.Error(fmt, args);

        public void Verbose(int level, string fmt, params object[] args)
            => _log.Verbose(level, fmt, args);
    }
}

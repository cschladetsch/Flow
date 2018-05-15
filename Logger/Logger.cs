using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Flow.Impl
{
    /// <summary>
    /// Log system used by Models.
    /// </summary>
    public class Logger : ILogger
    {
        #region Public Fields
        public string Prefix { get;set; }
        public int Verbosity { get; set; }
        public static string LogFileName;
        public static ELogLevel MaxLevel;
        public string Name { get; set; }
        #endregion

        #region Public Methods
        public Logger()
        {
        }
        public Logger(string name)
        {
            Prefix = name;
        }
        public static void Initialise()
        {
        }
        #endregion

        #region Protected Methods
        public void Info(string fmt, params object[] args)
        {
            Log(ELogLevel.Info, string.Format(fmt, args));
        }
        public void Warn(string fmt, params object[] args)
        {
            Log(ELogLevel.Warn, string.Format(fmt, args));
        }
        public void Error(string fmt, params object[] args)
        {
            Log(ELogLevel.Error, string.Format(fmt, args));
        }
        public void Verbose(int level, string fmt, params object[] args)
        {
            if (level > Verbosity)
                return;
            Log(ELogLevel.Verbose, string.Format(fmt, args));
        }
        #endregion

        #region Private
        private void Log(ELogLevel level, string text)
        {
            Action<string> log = Debug.Log;
            if (level == ELogLevel.None)
                level = ELogLevel.Error;
#if TRACE
            var entry = MakeEntry(level, text);
            Console.WriteLine(entry);
            Trace.WriteLine(entry);
#else
            // TODO: use bitmasks as intended
            switch (level)
            {
                case ELogLevel.Info:
                    log = Debug.Log;
                    break;
                case ELogLevel.Warn:
                    log = Debug.LogWarning;
                    break;
                case ELogLevel.Error:
                    log = Debug.LogError;
                    break;
                case ELogLevel.Verbose:
                    log = Debug.Log;
                    break;
            }
            log(MakeEntry(level, text));
#endif
        }
        private string MakeEntry(ELogLevel level, string text)
        {
            return $"{Prefix}: type:{GetType()} name: {Name}: t'{text}'";
        }
        #endregion

        #region Protected Fields
        protected ELogLevel _logLevel;
        #endregion
    }
}

using System;
using System.Diagnostics;
using System.Text;
using Debug = UnityEngine.Debug;

namespace Flow.Impl
{
    /// <summary>
    /// Log system used by Models.
    /// </summary>
    public class Logger : ILogger
    {
        #region Public Fields
        public string LogPrefix { get;set; }
        public object Subject { get; set; }
        public int Verbosity { get; set; }

        public static string LogFileName;
        public static ELogLevel MaxLevel;
        #endregion

        #region Public Methods
        public Logger()
        {
            Subject = this;
        }

        public Logger(string pre)
            : this()
        {
            LogPrefix = pre;
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
            text = text.Trim();
            var named = Subject as INamed;
            var name = named == null ? "" : named.Name;
            var dt = DateTime.Now - _startTime;
            var ms = dt.ToString(@"fff");
            var time = dt.ToString(@"\mm\:ss\:") + ms;
            var prefix = string.IsNullOrEmpty(LogPrefix) ? "" : $"{LogPrefix}: ";
            var from = string.IsNullOrEmpty(name) ? "" : $" {name}:";
            var gen = Subject as IGenerator;
            var step = gen == null ? "" : $"#{gen.StepNumber}/{gen.Kernel.StepNumber}: ";
            return $"> {prefix}{time} {step}{Subject.GetType()}{from}\n\t`{text}`";
        }
        #endregion

        #region Protected Fields
        protected ELogLevel _logLevel;

        #endregion

        private static DateTime _startTime = DateTime.Now;
    }
}

using System;

namespace Flow.Logger
{
    [Flags]
    public enum ELogEntryType
    {
        None = 0,       // effectively /dev/null. usefull to keep logging calls that produce nothing.
        Log = 1,        // use for common info
        Warn = 2,       // stuff that shouldn't happen but aren't expected to be harmful.
        Error = 4,      // something happened that is unexpected and probably harmful
        Fatal = 8,      // shit's fucked.
        Everything = Log | Warn | Error | Fatal,
    }

    /// <summary>
    /// Logger with multiple output streams, and chained loggers/
    /// Also has independant output streams <see cref="ELogEntryType"/> and Verbosity eLevel.
    /// 
    /// All logging methods have an override that provides a verbosity eLevel.
    /// 
    /// This allows you to independantly control the verbosity of each log entry type.
    /// </summary>
    public interface ILogger
    {
        ELogEntryType LogEntries { get; set; }
        int Verbosity { get; set; }
        string Name { get; set; }

        void Log(string fmt, params object[] args);
        void Warn(string fmt, params object[] args);
        void Error(string fmt, params object[] args);

        // A log entry with a verbosity > this.Verbosity will not be logged.
        void Log(int verbosity, string fmt, params object[] args);
        void Warn(int verbosity, string fmt, params object[] args);
        void Error(int verbosity, string fmt, params object[] args);

        void AddFile(string filename);
        void AddLogger(ILogger logger);
        void AddStream(System.IO.StreamWriter writer);

        void WriteEntry(ELogEntryType entryType, string fmt, params object[] args);

        void Close();
        void Flush();
    }
}

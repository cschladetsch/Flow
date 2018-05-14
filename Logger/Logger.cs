//#define UNITY

using System;
using System.IO;
using System.Collections.Generic;

namespace Flow.Logger
{
    public class Logger : ILogger
    {
        public ELogEntryType LogEntries { get; set; }
        public int Verbosity { get; set; }
        public string Name { get; set; }

        public Logger(ELogEntryType entryType, string name = "")
        {
            LogEntries = entryType;
            Name = name;
        }

        public void Log(string fmt, params object[] args)
        {
            WriteEntry(ELogEntryType.Log, fmt, args);
        }

        public void Warn(string fmt, params object[] args)
        {
            WriteEntry(ELogEntryType.Warn, fmt, args);
        }

        public void Error(string fmt, params object[] args)
        {
            WriteEntry(ELogEntryType.Error, fmt, args);
        }

        public void Log(int verbosity, string fmt, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(int verbosity, string fmt, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(int verbosity, string fmt, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WriteEntry(ELogEntryType entryType, string fmt, params object[] args)
        {
            if (ShouldWrite(entryType))
                AddEntry(DateTime.Now, entryType, MakeMessageText(fmt, args));

            foreach (var log in _logs)
                log.WriteEntry(entryType, fmt, args);
        }

        private bool ShouldWrite(ELogEntryType entryType)
        {
            return (LogEntries & entryType) == entryType;
        }

        public void AddStream(StreamWriter writer)
        {
            if (writer == null || !writer.BaseStream.CanWrite)
            {
                Error("Cannot add stream {0} for log writing", writer);
                return;
            }
            _streams.Add(writer);
        }

        public void AddFile(string name)
        {
            var file = new StreamWriter(name) { AutoFlush = true };
            if (!file.BaseStream.CanWrite)
            {
                Error("Cannot open log file '{0} for writing", name);
                return;
            }
            _streams.Add(file);
        }

        public void AddLogger(ILogger logger)
        {
            _logs.Add(logger);
        }

        public void Close()
        {
            Flush();
            foreach (var stream in _streams)
                stream.Close();
            _streams.Clear();
        }

        public void Flush()
        {
            foreach (var stream in _streams)
                stream.Flush();
        }

        #region Overridable for customisation from base implementation
        /// <summary>
        /// Make the body message text of the log entry
        /// </summary>
        protected virtual string MakeMessageText(string fmt, object[] args)
        {
            return string.Format(fmt, args);
        }

        /// <summary>
        /// Make the entire formatted entry text for a log entry
        /// </summary>
        protected virtual string MakeEntryText(DateTime dateTime, ELogEntryType entryType, string message)
        {
            var entry = string.Format("{0}: {1}: #{2}: {3}: {4}", entryType.ToString(), MakeTimeString(dateTime),
#if UNITY
                UnityEngine.Time.frameCount
#else
				0
#endif
                , Name, message);
            return entry;
        }

        protected virtual void AddEntry(DateTime dateTime, ELogEntryType entryType, string message)
        {
            var entry = MakeEntryText(dateTime, entryType, message);
            foreach (var stream in _streams)
            {
                stream.WriteLine(entry);
            }
        }

        protected virtual string MakeTimeString(DateTime dateTime)
        {
            return dateTime.ToShortTimeString();
        }
        #endregion

        private readonly List<StreamWriter> _streams = new List<StreamWriter>();
        private readonly List<ILogger> _logs = new List<ILogger>();
    }
}

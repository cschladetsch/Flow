using System;
using System.IO;
using System.Collections.Generic;

namespace Flow.Logger
{
	public class Logger : ILogger
	{
		public ELogLevel LogLevel { get; set; }

		public string Name { get; set; }

		public Logger(ELogLevel level, string name = "")
		{
			LogLevel = level;
			Name = name;
		}

		public void Log(string fmt, params object[] args)
		{
			WriteEntry(ELogLevel.Log, fmt, args);
		}

		public void Warn(string fmt, params object[] args)
		{
			WriteEntry(ELogLevel.Warn, fmt, args);
		}

		public void Error(string fmt, params object[] args)
		{
			WriteEntry(ELogLevel.Error, fmt, args);
		}

		public void WriteEntry(ELogLevel level, string fmt, params object[] args)
		{
			if (ShouldWrite(ELogLevel.Error))
				AddEntry(DateTime.Now, level, MakeMessageText(fmt, args));

			foreach (var log in _logs)
				log.WriteEntry(level, fmt, args);
		}

		private bool ShouldWrite(ELogLevel level)
		{
			return (LogLevel & level) == level;
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
			var file = new StreamWriter(name) {AutoFlush = true};
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
		protected virtual string MakeEntryText(DateTime dateTime, ELogLevel level, string message)
		{
			var entry = string.Format("{0}: {1}: #{2}: {3}: {4}", level.ToString(), MakeTimeString(dateTime),
				#if UNITY_EDITOR
				UnityEngine.Time.frameCount
				#else
				0
				#endif
				, Name, message);
			return entry;
		}

		protected virtual void AddEntry(DateTime dateTime, ELogLevel level, string message)
		{
			var entry = MakeEntryText(dateTime, level, message);
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
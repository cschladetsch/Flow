using System;

namespace Flow.Logger
{
	[Flags]
	public enum ELogLevel
	{
		None = 0,
		Log = 1,
		Warn = 2,
		Error = 4,
		Verbose = Log | Warn | Error,
	}

	/// <summary>
	/// Logger with multiple output streams, and chained loggers
	/// </summary>
	public interface ILogger
	{
		ELogLevel LogLevel { get; set; }
		string Name { get; set; }

		void Log(string fmt, params object[] args);
		void Warn(string fmt, params object[] args);
		void Error(string fmt, params object[] args);

		void AddStream(System.IO.StreamWriter writer);
		void AddFile(string filename);
		void AddLogger(ILogger logger);

		void WriteEntry(ELogLevel level, string fmt, params object[] args);

		void Close();
		void Flush();
	}
}

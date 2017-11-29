using System;

namespace Flow.Logger
{
	#if UNITY_EDITOR
	public class UnityLogger : Logger
	{
		public UnityLogger() : base(ELogLevel.Verbose)
		{
		}

		public UnityLogger(ELogLevel level, string name = "") : base(level, name)
		{
		}

		protected override void AddEntry(DateTime dateTime, ELogLevel level, string message)
		{
			switch (level)
			{
				case ELogLevel.None:
					break;
				case ELogLevel.Log:
					Write(dateTime, message, UnityEngine.Debug.Log);
					break;
				case ELogLevel.Warn:
					Write(dateTime, message, UnityEngine.Debug.LogWarning);
					break;
				case ELogLevel.Error:
					Write(dateTime, message, UnityEngine.Debug.LogError);
					break;
				case ELogLevel.Verbose:
					break;
			}
		}

		private void Write(DateTime dateTime, string message, Action<string> log)
		{
			log(string.Format("{0}: #{1}: {2}", dateTime.ToLongTimeString(), UnityEngine.Time.frameCount, message));
		}
	}
	#endif
}
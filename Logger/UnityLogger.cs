using System;
using UnityEngine;

namespace Flow.Logger
{
#if UNITY3D
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
					// TODO: have to use this if running as a unit test - but there is no way to tell?
					//Write(dateTime, message, (str) => UnityEngine.TestTools.LogAssert.Expect(LogType.Error, str));
					Write(dateTime, message, UnityEngine.Debug.LogError);
					break;
				case ELogLevel.Verbose:
					break;
			}
		}

		private void Write(DateTime dateTime, string message, Action<string> log)
		{
			log($"{dateTime.ToLongTimeString()}: #{UnityEngine.Time.frameCount}: {message}");
		}
	}
#endif
}
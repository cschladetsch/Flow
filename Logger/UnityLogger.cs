#if ASDASDASD//UNITY3D
using System;
using UnityEngine;

namespace Flow.Logger
{
	public class UnityLogger : Logger
	{
		public UnityLogger() : base(ELogEntryType.Everything)
		{
		}

		public UnityLogger(ELogEntryType entryType, string name = "") : base(entryType, name)
		{
		}

		protected override void AddEntry(DateTime dateTime, ELogEntryType entryType, string message)
		{
			switch (entryType)
			{
				case ELogEntryType.None:
					break;
				case ELogEntryType.Log:
					Write(dateTime, message, UnityEngine.Debug.Log);
					break;
				case ELogEntryType.Warn:
					Write(dateTime, message, UnityEngine.Debug.LogWarning);
					break;
				case ELogEntryType.Error:
					// TODO: have to use this if running as a unit test - but there is no way to tell?
					//Write(dateTime, message, (str) => UnityEngine.TestTools.LogAssert.Expect(LogType.Error, str));
					Write(dateTime, message, UnityEngine.Debug.LogError);
					break;
				case ELogEntryType.Everything:
					break;
			}
		}

		private void Write(DateTime dateTime, string message, Action<string> log)
		{
			log($"{dateTime.ToLongTimeString()}: #{UnityEngine.Time.frameCount}: {message}");
		}
	}
}
#endif

#if asdasdasd//DOTNET
using System;

namespace Flow.Logger
{
	public class ConsoleLogger : Impl.Logger
	{
		public ConsoleLogger() : base(ELogEntryType.Everything)
		{
		}

		public ConsoleLogger(ELogEntryType entryType, string name = "") : base(entryType, name)
		{
		}

		protected override void AddEntry(DateTime dateTime, ELogEntryType entryType, string message)
		{
			Console.WriteLine("{0}: {1}: {2}", MakeTimeString(dateTime), entryType, message);
		}
	}
}
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flow.Logger
{
	#if DOTNET
	public class ConsoleLogger : Logger
	{
		public ConsoleLogger() : base(ELogLevel.Verbose)
		{
		}

		public ConsoleLogger(ELogLevel level, string name = "") : base(level, name)
		{
		}

		protected override void AddEntry(DateTime dateTime, ELogLevel level, string message)
		{
			Console.WriteLine("{0}: {1}: {2}", MakeTimeString(dateTime), level, message);
		}
	}
	#endif
}

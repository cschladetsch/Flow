<<<<<<< HEAD
﻿#if asdasdasd//DOTNET
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
=======
﻿#if DOTNET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flow.Logger
{
	public class ConsoleLogger : Logger
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
>>>>>>> 2156678... Updated to .Net4.5
}
#endif

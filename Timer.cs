// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow.Impl
{
	internal class Timer : Periodic, ITimer
	{
		internal Timer(IKernel kernel, TimeSpan span)
			: base(kernel, span)
		{
			TimeEnds = kernel.Time.Now + span;
			Elapsed += TimedOutHandler;
		}

		private void TimedOutHandler(ITransient sender)
		{
			Kernel.Trace.Log("OneShotTimer completed {0}", Name);
			Complete();
		}

		public DateTime TimeEnds { get; private set; }
	}
}

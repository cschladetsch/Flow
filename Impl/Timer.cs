// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

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

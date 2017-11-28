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
			Elapsed += self => Complete();
		}

		public DateTime TimeEnds { get; }
	}
}

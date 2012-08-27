using System;

namespace Flow
{
	internal class Timer : PeriodicTimer, ITimer
	{
		public DateTime TimeEnds { get; private set; }

		internal Timer(IKernel kernel, TimeSpan span)
			: base(kernel, span)
		{
			TimeEnds = kernel.Time.Now + span;
			Elapsed += (self) => Delete();
		}
	}
}
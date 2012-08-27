using System;
using System.Collections.Generic;

namespace Flow
{
	internal class TimedFuture<T> : Future<T>, ITimedFuture<T>
	{
		public event TimedOutHandler TimedOut;

		internal TimedFuture(IKernel kernel, TimeSpan span)
		{
			Timer = kernel.Factory.NewTimer(span);
			Timer.Elapsed += HandleElapsed;
		}

		void HandleElapsed(ITransient sender)
		{
			if (!Exists)
				return;

			HasTimedOut = true;

			Delete();
		}
	}
}
// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow.Impl
{
	internal class TimedFuture<T> : Future<T>, ITimedFuture<T>
	{
		internal TimedFuture(IKernel k, TimeSpan span)
		{
			Timer = k.Factory.OneShotTimer(span);
			k.Root.Add(Timer);
			Timer.Elapsed += HandleElapsed;
		}

		public event TimedOutHandler TimedOut;

		public ITimer Timer { get; internal set; }

		public bool HasTimedOut { get; protected set; }

		private void HandleElapsed(ITransient sender)
		{
			if (!Active)
				return;

			if (TimedOut != null)
				TimedOut(this);

			HasTimedOut = true;

			Complete();
		}
	}
}

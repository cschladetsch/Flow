// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	internal class TimedFuture<T> : Future<T>, ITimedFuture<T>
	{
		internal TimedFuture(ITransient kernel, TimeSpan span)
		{
			Timer = kernel.Factory.NewTimer(span);
			Timer.Elapsed += HandleElapsed;
		}

		/// <inheritdoc />
		public event TimedOutHandler TimedOut;

		/// <inheritdoc />
		public ITimer Timer { get; internal set; }

		/// <inheritdoc />
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

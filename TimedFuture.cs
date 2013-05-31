// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	internal class TimedFuture<T> : Future<T>, ITimedFuture<T>
	{
		/// <inheritdoc />
		public event TimedOutHandler TimedOut;

		/// <inheritdoc />
		public ITimer Timer { get; internal set; }

		/// <inheritdoc />
		public bool HasTimedOut { get; protected set; }

		internal TimedFuture(ITransient parent, TimeSpan span)
		{
			Timer = parent.Factory.NewTimer(span);
			parent.Factory.Kernel.Root.Add(Timer);
			Timer.Elapsed += HandleElapsed;
		}

		void HandleElapsed(ITransient sender)
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

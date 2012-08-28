// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Collections.Generic;

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

		internal TimedFuture(IKernel kernel, TimeSpan span)
		{
			Timer = kernel.Factory.NewTimer(span);
			Timer.Elapsed += HandleElapsed;
		}

		void HandleElapsed(ITransient sender)
		{
			if (!Exists)
				return;

			if (TimedOut != null)
				TimedOut(this);

			HasTimedOut = true;

			Delete();
		}
	}
}
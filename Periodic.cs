// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <inheritdoc />
	internal class Periodic : Subroutine<bool>, IPeriodic
	{
		private DateTime _expires;

		internal Periodic(IKernel kernel, TimeSpan interval)
		{
			Interval = interval;
			TimeStarted = kernel.Time.Now;
			_expires = TimeStarted + Interval;
			Sub = StepTimer;
		}

		/// <inheritdoc />
		public event TransientHandler Elapsed;

		/// <inheritdoc />
		public DateTime TimeStarted { get; private set; }

		/// <inheritdoc />
		public TimeSpan Interval { get; set; }

		private bool StepTimer(IGenerator self)
		{
			if (Kernel.Time.Now < _expires)
				return true;

			if (Elapsed != null)
				Elapsed(this);

			_expires = Kernel.Time.Now + Interval;

			return true;
		}
	}
}

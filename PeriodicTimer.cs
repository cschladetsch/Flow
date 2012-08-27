// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
    class PeriodicTimer : Subroutine<bool>, IPeriodicTimer
	{
		public event TransientHandler Elapsed;

		public DateTime TimeStarted { get; set; }

		public TimeSpan Interval { get; set; }

		public PeriodicTimer(IKernel kernel, TimeSpan interval)
		{
			Interval = interval;
			TimeStarted = kernel.Time.Now;
			_expires = TimeStarted + Interval;
			Sub = StepTimer;
		}

		bool StepTimer(IGenerator self)
		{
			if (Kernel.Time.Now < _expires)
				return true;

			if (Elapsed != null)
				Elapsed(this);

			_expires = Kernel.Time.Now + Interval;

			return true;
		}

	    DateTime _expires;
	}
}

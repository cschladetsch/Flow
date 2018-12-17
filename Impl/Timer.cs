<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class Timer
        : Periodic
        , ITimer
    {
        internal Timer(IKernel kernel, TimeSpan span)
            : base(kernel, span)
        {
            TimeEnds = kernel.Time.Now + span;
            Elapsed += TimedOutHandler;
        }

        private void TimedOutHandler(ITransient sender)
        {
            Kernel.Log.Info("OneShotTimer completed {0}", Name);
            Complete();
        }

        public DateTime TimeEnds { get; private set; }
    }
=======
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
			Kernel.Log.Info("OneShotTimer completed {0}", Name);
			Complete();
		}

		public DateTime TimeEnds { get; private set; }
	}
>>>>>>> 2156678... Updated to .Net4.5
}

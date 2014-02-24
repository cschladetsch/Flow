// CJS. Nabbed from http://code.google.com/p/mono-soc-2008/source/browse/trunk/parallelfx/System.Threading/System.Threading/SpinWait.cs?r=554
using System;
using System.Threading;

namespace Flow
{
	public struct SpinWait
	{
		// The number of step until SpinOnce yield on multicore machine
		const int step = 20;
		static readonly bool isSingleCpu = (Environment.ProcessorCount == 1);

		int ntime;

		public void SpinOnce()
		{
			// On a single-CPU system, spinning does no good
			if (isSingleCpu)
			{
				Yield();
			}
			else
			{
				if (Interlocked.Increment(ref ntime) % step == 0)
				{
					Yield();
				}
				else
				{
					// Multi-CPU system might be hyper-threaded, let other thread run
					Thread.SpinWait(2 * (ntime + 1));
				}
			}
		}

		public void SpinUntil(Func<bool> predicate)
		{
			while (!predicate())
				SpinOnce();
		}

		static void Yield()
		{
			// Replace sched_yield by Thread.Sleep(0) which does almost the same thing
			// (going back in kernel mode and yielding) but avoid the branching and unmanaged bridge
			Thread.Sleep(0);
		}

		public void Reset()
		{
			ntime = 0;
		}

		public bool NextSpinWillYield
		{
			get
			{
				return isSingleCpu || ntime % step == 0;
			}
		}

		public int Count
		{
			get
			{
				return ntime;
			}
		}
	}
}
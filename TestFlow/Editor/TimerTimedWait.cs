using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	class TimerWaitTest : TestBase
	{
		[TestCase(0.5f)]
		[TestCase(0.2f)]
		public void TestBreak(float timeOut)
		{
			_root.Add(
				_flow.Parallel(
					_flow.While(() => true,
						_flow.OneShotTimer(TimeSpan.FromSeconds(timeOut), (self) => self.Kernel.BreakFlow())
					)
				)
			);

			var delta = RunKernel(2) - timeOut;
			Assert.IsTrue(Math.Abs(delta) < 0.1f);
		}

		[TestCase(0.5f, 1.0f, 0.5f)]
		[TestCase(1.5f, 1.0f, 1.0f)]
		public void TestTimedWait(float timerLen, float timeOut, float kernelRunTime)
		{
			_root.Add(
				_flow.Wait(
					_flow.Trigger(_flow.OneShotTimer(TimeSpan.FromSeconds(timerLen))),
					TimeSpan.FromSeconds(timeOut)
				),
				_flow.Break()
			);

			var delta = timerLen - RunKernel(kernelRunTime);
			Assert.IsTrue(Math.Abs(delta) < 0.1f);
		}
	}
}

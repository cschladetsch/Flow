using System;
using NUnit.Framework;

namespace Flow.Test
{
	public class TestTimers : TestBase
	{
		[TestCase(0.4f, 0.5f, true)]
		[TestCase(0.4f, 0.2f, false)]
		public void TestOneShot(float span, float runTime, bool shouldBeCompleted)
		{
			var k = _kernel;
			var timer = k.Factory.Timer(TimeSpan.FromSeconds(span));

			var elapsed = false;
			var when = DateTime.Now;

			timer.Elapsed += (sender) => 
			{
				elapsed = true;
				when = timer.Kernel.Time.Now; 
			};

			k.Root.Add(timer);
			var start = RunKernel(TimeSpan.FromSeconds(runTime));

			if (shouldBeCompleted) 
			{
				Assert.IsTrue(!timer.Active);
				Assert.IsTrue(elapsed);
				Assert.IsTrue(when > start);
			} 
			else 
			{
				Assert.IsFalse(!timer.Active);
				Assert.IsFalse(elapsed);
			}
		}

		[TestCase(0.1f, 0.25f, 2)]
		[TestCase(0.1f, 0.45f, 4)]
		[TestCase(0.1f, 0.0f, 0)]
		public void TestPeriodic(float interval, float runTime, int numElapsed)
		{
			var k = _kernel;
			var timer = k.Factory.PeriodicTimer(TimeSpan.FromSeconds(interval));

			int elapsed = 0;
			timer.Elapsed += (sender) => ++elapsed;
 
			k.Root.Add(timer);
			RunKernel(TimeSpan.FromSeconds(runTime));

			Assert.AreEqual(numElapsed, elapsed);
		}

		[TestCase(0.1f, 0.2f, true)]
		[TestCase(0.2f, 0.1f, false)]
		public void TestTimedFuture(float futureLifetime, float runTime, bool result)
		{
			var span = TimeSpan.FromSeconds(futureLifetime);
			var future = _flow.TimedFuture<int>(span);

			Assert.IsFalse(future.Available);

			_root.Add(future);
			RunKernel(TimeSpan.FromSeconds(runTime));

			Step();

			Assert.AreEqual(result, future.HasTimedOut);
		}

		private DateTime RunKernel(TimeSpan span)
		{
			var start = _kernel.Time.Now;
			var end = start + span;
			_kernel.Step();
			while (_kernel.Time.Now < end)
			{
				_kernel.Step();
			}
			return start;
		}
	}
}


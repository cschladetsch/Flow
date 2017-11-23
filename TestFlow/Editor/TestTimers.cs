using System;
using NUnit.Framework;

namespace Flow.Test
{
	public class TestTimers : TestBase
	{
		private float futureLifetime;

		[TestCase(0.4f, 0.5f, true)]
		[TestCase(0.4f, 0.2f, false)]
		public void TestOneShot(float span, float runTime, bool shouldBeCompleted)
		{
			var kernel = Create.Kernel();
			var timer = kernel.Factory.Timer(TimeSpan.FromSeconds(span));

			var elapsed = false;
			var when = DateTime.Now;

			timer.Elapsed += (sender) => 
			{
				elapsed = true;
				when = timer.Kernel.Time.Now; 
			};

			kernel.Root.Add(timer);
			var start = RunKernel(kernel, TimeSpan.FromSeconds(runTime));

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
		//[TestCase(0.1f, 0.45f, 4)]
		[TestCase(0.1f, 0.0f, 0)]
		public void TestPeriodic(float interval, float runTime, int numElapsed)
		{
			var kernel = Create.Kernel();
			var timer = kernel.Factory.PeriodicTimer(TimeSpan.FromSeconds(interval));

			int elapsed = 0;
			timer.Elapsed += (sender) => ++elapsed;
 
			kernel.Root.Add(timer);
			RunKernel(kernel, TimeSpan.FromSeconds(runTime));

			Assert.AreEqual(numElapsed, elapsed);
		}

		[Test]
		public void TestTimedFutureLate()
		{
			TestTimedFuture(0.1f, 0.2f, true);
		}

		[Test]
		public void TestTimedFutureEarly()
		{
			TestTimedFuture(0.2f, 0.1f, false);
		}

		//[TestCase(0.1f, 0.2f, true)]
		//[TestCase(0.2f, 0.1f, false)]
		public void TestTimedFuture(float futureLifetime, float runTime, bool result)
		{
			this.futureLifetime = futureLifetime;
			var future = _factory.TimedFuture<int>(TimeSpan.FromSeconds(futureLifetime));

			Assert.IsFalse(future.Available);

			_root.Add(future);
			_root.Add(_factory.WaitFor(TimeSpan.FromSeconds(futureLifetime*1.1f)));

			Step(3);
			RunKernel(_kernel, TimeSpan.FromSeconds(runTime));

			Assert.AreEqual(result, future.HasTimedOut);
		}

		DateTime RunKernel(IKernel kernel, TimeSpan span)
		{
			var start = kernel.Time.Now;
			var end = start + span;
			while (kernel.Time.Now < end)
			{
				kernel.Step();
			}
			kernel.Step();
			return start;
		}
	}
}


using System;
using NUnit.Framework;
using Flow;

namespace TestFlow
{
	[TestFixture()]
	public class TestTimers
	{
		[TestCase(0.4f, 0.5f, true)]
		[TestCase(0.4f, 0.2f, false)]
		public void TestOneShot(float span, float runTime, bool shouldBeCompleted)
		{
			var kernel = Create.NewKernel();
			var timer = kernel.Factory.NewTimer(TimeSpan.FromSeconds(span));

			var elapsed = false;
			DateTime when = DateTime.Now;

			timer.Elapsed += (sender) => 
			{
				elapsed = true;
				when = timer.Kernel.Time.Now; 
			};

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
			var kernel = Create.NewKernel();
			var timer = kernel.Factory.NewPeriodicTimer(TimeSpan.FromSeconds(interval));

			int elapsed = 0;
			timer.Elapsed += (sender) => ++elapsed;
 
			RunKernel(kernel, TimeSpan.FromSeconds(runTime));

			Assert.AreEqual(numElapsed, elapsed);
		}

		[TestCase(0.1f, 0.2f, true)]
		[TestCase(0.2f, 0.1f, false)]
		public void TestTimedFuture(float span, float runTime, bool result)
		{
			var kernel = Create.NewKernel();
			var future = kernel.Factory.NewTimedFuture<int>(TimeSpan.FromSeconds(span));

			Assert.IsFalse(future.Available);

			RunKernel(kernel, TimeSpan.FromSeconds(runTime));

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


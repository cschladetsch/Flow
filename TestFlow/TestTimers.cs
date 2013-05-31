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

			kernel.Root.Add(timer);
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
 
			kernel.Root.Add(timer);
			RunKernel(kernel, TimeSpan.FromSeconds(runTime));

			Assert.AreEqual(numElapsed, elapsed);
		}

		[TestCase(1f, 2f, true)]
		[TestCase(2f, 1f, false)]
		public void TestTimedFuture(float wait, float runTime, bool result)
		{
			var kernel = Create.NewKernel();
			var future = kernel.Factory.NewTimedFuture<int>(TimeSpan.FromSeconds(wait));

			Assert.IsFalse(future.Available);
			kernel.Root.Add(future);
			RunKernel(kernel, TimeSpan.FromSeconds(runTime*2));

			Assert.AreEqual(result, future.HasTimedOut);
		}

		[TestCase]
		public void TestTimedFuture2()
		{
			var wait = 2;
			var runTime = 1;
			var kernel = Create.NewKernel();
			var future = kernel.Factory.NewTimedFuture<int>(TimeSpan.FromSeconds(wait));

			Assert.IsFalse(future.Available);
			kernel.Root.Add(future);
			RunKernel(kernel, TimeSpan.FromSeconds(runTime * 1.5f));

			Assert.AreEqual(false, future.HasTimedOut);
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


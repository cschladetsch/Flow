using System;
using NUnit.Framework;
using Flow;

namespace TestFlow
{
	[TestFixture()]
	public class TestTimers
	{
		[Test()]
		public void TestOneShot()
		{
			var kernel = Global.NewKernel();
			var timer = kernel.Factory.NewTimer(TimeSpan.FromSeconds(0.4f));

			var elapsed = false;
			DateTime when;

			timer.Elapsed += (sender) => { elapsed = true; when = timer.Kernel.Time.Now; };

			var start = RunKernel(kernel, TimeSpan.FromSeconds(0.5f));

			Assert.IsTrue(!timer.Exists);
			Assert.IsTrue(elapsed);
			Assert.IsTrue(when > start);
		}

		[Test()]
		public void TestPeriodic()
		{
			var kernel = Global.NewKernel();
			var timer = kernel.Factory.NewPeriodicTimer(TimeSpan.FromSeconds(0.1f));

			int elapsed = 0;
			timer.Elapsed += (sender) => ++elapsed;
 
			RunKernel(kernel, TimeSpan.FromSeconds(0.35f));

			Assert.AreEqual(3, elapsed);
		}

		[Test()]
		public void TestTimedFuture()
		{
			var kernel = Global.NewKernel();
			var future = kernel.Factory.NewTimedFuture<int>(TimeSpan.FromSeconds(0.1f));

			RunKernel(kernel, TimeSpan.FromSeconds(0.25f));

			Assert.IsTrue(future.HasTimedOut);
			Assert.IsFalse(future.Available);
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


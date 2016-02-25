using System;
using System.Collections.Generic;
using NUnit.Framework;
using Flow;

namespace TestFlow
{
	[TestFixture()]
	public class TestSequence
	{
		[Test()]
		public void TestSequence1()
		{
			var kernel = Create.NewKernel();

			var total = 0;
			var last = 0;
			kernel.Factory.NewSequence(
				() => { Console.WriteLine(kernel.StepNumber); if (kernel.StepNumber == 0) { total++; last = kernel.StepNumber; } },
				() => { if (kernel.StepNumber > 2) { total++; last = kernel.StepNumber; } },
				() => { if (kernel.StepNumber > 3) { total++; last = kernel.StepNumber; } },
				() => { if (kernel.StepNumber > 4) { total++; last = kernel.StepNumber; } }
          	);

			Assert.AreEqual(0, total);
			kernel.Step();
			kernel.Step();
			Assert.AreEqual(1, total);
			kernel.Step();
			Assert.AreEqual(2, total);
			kernel.Step();
			Assert.AreEqual(3, total);
			kernel.Step();
			Assert.AreEqual(4, total);
		}

		[Test()]
		public void TestParallel1()
		{
			var kernel = Create.NewKernel();

			var total = 0;
			kernel.Factory.NewParallel(
				() => total++,
				() => total++,
				() => total++,
				() => total++
          	);

			kernel.Step();
			kernel.Step();

			Assert.AreEqual (4, total);
		}
	}
}

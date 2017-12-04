using System.Linq;
using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	public class TestBarrier : TestBase
	{
		[Test]
		public void BarrierTest()
		{
			var kernel = Create.Kernel();
            var flow = kernel.Factory;

			var barrier = flow.Barrier();
			var future1 = flow.Future<int>();
			var future2 = flow.Future<int>();

			barrier.Add(future1);
			barrier.Add(future2);

			kernel.Root.Add(barrier, future1, future2);
			kernel.Step();
			kernel.Step();

			Assert.AreEqual(3, kernel.Root.Contents.Count());

			Assert.IsTrue(barrier.Active);
			Assert.IsTrue(future1.Active);
			Assert.IsTrue(future2.Active);

			kernel.Step();

			future1.Value = 123;

			kernel.Step();

			Assert.IsTrue(barrier.Active);
			Assert.IsFalse(future1.Active);
			Assert.IsTrue(future2.Active);

			future2.Value = 456;

			kernel.Step();

			Assert.IsFalse(barrier.Active);
			Assert.IsFalse(future1.Active);
			Assert.IsFalse(future2.Active);

			Assert.AreEqual(123, future1.Value);
			Assert.AreEqual(456, future2.Value);
		}
	}
}

using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace Flow.Test
{
	public class TestKernel : TestBase
	{
		[Test]
		public void TestCoroutine()
		{
			var kernel = Create.Kernel();
			var coro = kernel.Factory.Coroutine<int>(Coro1);
			kernel.Root.Add(coro);

			Assert.IsTrue(coro.Active);
			Assert.IsTrue(coro.Running);

			// first coroutine is added after first step
			Assert.AreEqual(0, kernel.Root.Contents.Count());
			kernel.Step();
			Assert.AreEqual(1, kernel.Root.Contents.Count());

            kernel.Step();
            //Assert.AreEqual(1, coro.Value);
			
            kernel.Step();
            //Assert.AreEqual(2, coro.Value);

            kernel.Step();
            //Assert.AreEqual(3, coro.Value);
			
            kernel.Step();
            //Assert.AreEqual(3, coro.Value);

			Assert.IsFalse(coro.Active);
			Assert.IsFalse(coro.Running);
		}
		
		IEnumerator<int> Coro1(IGenerator self)
		{
			yield return 1;
			yield return 2;
			yield return 3;
		}

		[Test]
		public void TestSubroutine()
		{
		}

		int Sub1(IGenerator self, int num)
		{
			return num*self.StepNumber;
		}

		[Test()]
		public void TestFuture()
		{
			var kernel = Create.Kernel();
			var spawn = kernel.Factory;

			var future = spawn.Future<int>();
			var coro = spawn.Coroutine<bool, IFuture<int>>(Coro2, future);

			_futureSet = false;

			kernel.Root.Add(future, coro);
			kernel.Step();
			kernel.Step();

			Assert.IsFalse(_futureSet);

			Assert.IsFalse(future.Available);
			future.Value = 42;
			Assert.IsTrue(future.Available);
			Assert.AreEqual(42, future.Value);
			
			kernel.Step();

            //Assert.IsFalse(coro.Value);
			Assert.IsTrue(_futureSet);
		}

		bool _futureSet;

		IEnumerator<bool> Coro2(IGenerator self, IFuture<int> future)
		{
			Assert.IsFalse(!future.Active);

			Assert.IsFalse(future.Available);

			yield return self.ResumeAfter(future) != null;

			_futureSet = true;

			Assert.IsTrue(!future.Active);

			Assert.IsTrue(future.Available);

			Assert.AreEqual(future.Value, 42);

			yield return false;
		}

		[Test()]
		public void TestBarrier()
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

		[Test]
		public void TestTrigger()
		{
			var kernel = Create.Kernel();
			var factory = kernel.Factory;

			var trigger = factory.Trigger();
			var future1 = factory.Future<int>();
			var future2 = factory.Future<int>();

			trigger.Add(future1);
			trigger.Add(future2);

			kernel.Root.Add(trigger, future1, future2);
			kernel.Step();
			kernel.Step();

			Assert.IsTrue(trigger.Active);
			Assert.IsTrue(future1.Active);
			Assert.IsTrue(future2.Active);

			kernel.Step();

			future1.Value = 123;

			kernel.Step();

			Assert.IsFalse(trigger.Active);
			Assert.IsFalse(future1.Active);
			Assert.IsTrue(future2.Active);
		}
	}
}

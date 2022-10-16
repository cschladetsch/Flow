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
            return num * self.StepNumber;
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

    }
}

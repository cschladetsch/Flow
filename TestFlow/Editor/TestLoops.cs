namespace Flow.Test
{
    using System.Collections;
    using System.Linq;
    using NUnit.Framework;

    public class TestLoops
        : TestBase
    {
        private int _count = 0;

        [Test]
        public void TestDebugLog()
        {
            Root.Add(New.Log("Hello World"));
            Step(5);
        }

        [Test]
        public void TestCoro()
        {
            Root.Add(New.Coroutine(CountTo, 10).Named("Body"));

            _count = 0;
            Step(20);
            Assert.AreEqual(_count, 10);
        }

        private IEnumerator CountTo(IGenerator self, int max)
        {
            while (++_count != max)
                yield return 0;
        }

        [TestCase(5, 5, true)]
        [TestCase(3, 3, true)]
        [TestCase(5, 3, false)]
        [TestCase(3, 5, true)]
        [TestCase(0, 5, false)]
        public void TestWhile(int upper, int steps, bool shouldMatch)
        {
            _count = 0;
            Root.Add(
                New.While(() =>
                {
                    ++_count;
                    Print($"step={Kernel.StepNumber}, count={_count}");
                    return _count < upper;
                })
            );

            Step(steps);
            Print(_count);

            if (shouldMatch)
                Assert.AreEqual(upper, _count);
            else
                Assert.AreNotEqual(upper, _count);
        }

        [Test]
        public void TestNop()
        {
            _count = 0;

            New.Nop().AddTo(Root);
            Assert.IsTrue(!Root.Contents.Any());

            Step();
            Assert.IsTrue(Root.Contents.Any());

            Step();
            Assert.IsTrue(!Root.Contents.Any());
        }
    }
}

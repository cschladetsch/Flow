using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace Flow.Test
{
    public class TestLoops : TestBase
    {
        [Test]
        public void TestDebugLog()
        {
            Root.Add(New.Log("Hello World"));
            Step(5);
        }

        [Test]
        public void TestCoro()
        {
            var f = New;
            Root.Add(
                f.Coroutine(CountTo, 10)//.Named("Body")
            );

            count = 0;
            Step(20);
            Assert.AreEqual(count, 10);
        }

        IEnumerator CountTo(IGenerator self, int max)
        {
            while (++count != max)
            {
                yield return 0;
            }
        }

        [TestCase(5, 5, true)]
        [TestCase(3, 3, true)]
        [TestCase(5, 3, false)]
        [TestCase(3, 5, true)]
        [TestCase(0, 5, false)]
        public void TestWhile(int upper, int steps, bool shouldMatch)
        {
            var f = New;
            count = 0;
            Root.Add(
                f.While(() =>
                {
                    ++count;
                    Print($"step={Kernel.StepNumber}, count={count}");
                    return count < upper;
                })
            );

            Step(steps);
            Print(count);
            if (shouldMatch)
                Assert.AreEqual(upper, count);
            else
                Assert.AreNotEqual(upper, count);
        }

        [Test]
        public void TestNop()
        {
            var f = New;
            count = 0;
            f.Nop().AddTo(Root);
            Assert.IsTrue(!Root.Contents.Any());
            Step(1);
            Assert.IsTrue(Root.Contents.Any());
            Step(1);
            Assert.IsTrue(!Root.Contents.Any());
        }

        private int count = 0;
    }
}

using System.Collections;
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

        [Test]
        public void TestWhile()
        {
            var f = New;
            count = 0;
            Root.Add(
                f.While(
                    () => ++count < 5,
                    f.Do(() => PrintFmt("count={0}", count))
                )
            );

            Step(5);
            Print(count);
            Assert.AreEqual(5, count);
        }

        private int count = 0;
    }
}
